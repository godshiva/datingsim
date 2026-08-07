using DatingSim.Core.Endings;
using DatingSim.Core.State;

namespace DatingSim.Core.Narrative;

public sealed record PathReachabilityReport(
    IReadOnlyCollection<string> ReachableScenes,
    IReadOnlyCollection<string> UnreachableScenes,
    IReadOnlyCollection<EndingId> ReachableEndings,
    IReadOnlyCollection<string> ReachableChoices,
    IReadOnlyCollection<string> UnreachableChoices,
    IReadOnlyDictionary<string, int> ChoiceVisitCounts,
    IReadOnlyDictionary<EndingId, int> EndingVisitCounts);

/// <summary>
/// Exhaustive BFS over choice outcomes for discoverability analysis.
/// State key includes scene + flags + locked route + ending (affection omitted; endings are flag/route driven).
/// </summary>
public static class PathExplorer
{
    public static PathReachabilityReport Explore(StoryEngine? engine = null, int maxStates = 200_000)
    {
        engine ??= new StoryEngine();
        var scenes = engine.Scenes;

        var reachableScenes = new HashSet<string>(StringComparer.Ordinal);
        var reachableChoices = new HashSet<string>(StringComparer.Ordinal);
        var reachableEndings = new HashSet<EndingId>();
        var choiceVisits = new Dictionary<string, int>(StringComparer.Ordinal);
        var endingVisits = new Dictionary<EndingId, int>();

        var queue = new Queue<GameState>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        var start = GameState.CreateNew("Tester");
        queue.Enqueue(start);
        seen.Add(StateKey(start));

        while (queue.Count > 0)
        {
            if (seen.Count > maxStates)
            {
                throw new InvalidOperationException($"Path exploration exceeded {maxStates} unique states.");
            }

            var state = queue.Dequeue();
            reachableScenes.Add(state.CurrentSceneId);

            if (state.HasEnding)
            {
                reachableEndings.Add(state.Ending);
                endingVisits[state.Ending] = endingVisits.GetValueOrDefault(state.Ending) + 1;
                continue;
            }

            var scene = engine.GetScene(state.CurrentSceneId);
            if (scene.IsEndingScene)
            {
                var ending = engine.ResolveEnding(state);
                if (ending != EndingId.None)
                {
                    state.Ending = ending;
                    reachableEndings.Add(ending);
                    endingVisits[ending] = endingVisits.GetValueOrDefault(ending) + 1;
                }

                continue;
            }

            var choices = engine.GetAvailableChoices(state, scene);
            if (choices.Count == 0)
            {
                continue;
            }

            foreach (var choice in choices)
            {
                reachableChoices.Add(choice.Id);
                choiceVisits[choice.Id] = choiceVisits.GetValueOrDefault(choice.Id) + 1;

                var next = state.Clone();
                engine.ApplyChoice(next, choice.Id);

                var key = StateKey(next);
                if (seen.Add(key))
                {
                    queue.Enqueue(next);
                }
            }
        }

        var allChoices = scenes.Values.SelectMany(s => s.Choices).Select(c => c.Id).ToHashSet(StringComparer.Ordinal);
        var unreachableScenes = scenes.Keys.Where(id => !reachableScenes.Contains(id)).OrderBy(x => x).ToList();
        var unreachableChoices = allChoices.Where(id => !reachableChoices.Contains(id)).OrderBy(x => x).ToList();

        return new PathReachabilityReport(
            reachableScenes.OrderBy(x => x).ToList(),
            unreachableScenes,
            reachableEndings.OrderBy(x => x).ToList(),
            reachableChoices.OrderBy(x => x).ToList(),
            unreachableChoices,
            choiceVisits,
            endingVisits);
    }

    /// <summary>
    /// Monte Carlo playthroughs choosing uniformly among available choices.
    /// </summary>
    public static Dictionary<EndingId, int> MonteCarloEndings(
        int runs,
        int? seed = null,
        StoryEngine? engine = null)
    {
        engine ??= new StoryEngine();
        var rng = seed is int s ? new Random(s) : new Random();
        var tallies = Enum.GetValues<EndingId>()
            .Where(e => e != EndingId.None)
            .ToDictionary(e => e, _ => 0);

        for (var i = 0; i < runs; i++)
        {
            var state = GameState.CreateNew("Rng");
            var guard = 0;
            while (!state.HasEnding && guard++ < 200)
            {
                var scene = engine.GetScene(state.CurrentSceneId);
                if (scene.IsEndingScene)
                {
                    state.Ending = engine.ResolveEnding(state);
                    break;
                }

                var choices = engine.GetAvailableChoices(state, scene);
                if (choices.Count == 0)
                {
                    break;
                }

                var pick = choices[rng.Next(choices.Count)];
                engine.ApplyChoice(state, pick.Id);
            }

            if (state.HasEnding)
            {
                tallies[state.Ending] = tallies.GetValueOrDefault(state.Ending) + 1;
            }
        }

        return tallies;
    }

    private static string StateKey(GameState state)
    {
        var flags = string.Join(',', state.Flags.OrderBy(f => f, StringComparer.Ordinal));
        return $"{state.CurrentSceneId}|{state.LockedRoute}|{state.Ending}|{flags}";
    }
}
