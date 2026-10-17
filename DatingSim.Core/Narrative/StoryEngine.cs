using DatingSim.Core.Characters;
using DatingSim.Core.Endings;
using DatingSim.Core.State;

namespace DatingSim.Core.Narrative;

public sealed class StoryEngine
{
    private readonly IReadOnlyDictionary<string, SceneDef> _scenes;

    public StoryEngine(IReadOnlyDictionary<string, SceneDef>? scenes = null)
    {
        _scenes = scenes ?? StoryCatalog.Build();
    }

    public IReadOnlyDictionary<string, SceneDef> Scenes => _scenes;

    public SceneDef GetScene(string sceneId)
    {
        if (!_scenes.TryGetValue(sceneId, out var scene))
        {
            throw new KeyNotFoundException($"Unknown scene '{sceneId}'.");
        }

        return scene;
    }

    public IReadOnlyList<ChoiceDef> GetAvailableChoices(GameState state, SceneDef? scene = null)
    {
        scene ??= GetScene(state.CurrentSceneId);
        return scene.Choices
            .Where(choice => IsChoiceAvailable(state, choice))
            .ToList();
    }

    public static bool IsChoiceAvailable(GameState state, ChoiceDef choice)
    {
        foreach (var flag in choice.RequiresFlags)
        {
            if (!state.HasFlag(flag))
            {
                return false;
            }
        }

        foreach (var flag in choice.ForbidsFlags)
        {
            if (state.HasFlag(flag))
            {
                return false;
            }
        }

        return true;
    }

    public SceneDef ApplyChoice(GameState state, string choiceId)
    {
        var scene = GetScene(state.CurrentSceneId);
        var choice = scene.Choices.FirstOrDefault(c => c.Id == choiceId)
            ?? throw new InvalidOperationException($"Choice '{choiceId}' is not on scene '{scene.Id}'.");

        if (!IsChoiceAvailable(state, choice))
        {
            throw new InvalidOperationException($"Choice '{choiceId}' is not available in the current state.");
        }

        ApplyEffect(state, choice.Effect);
        state.ChoiceHistory.Add(choice.Id);
        state.CurrentSceneId = choice.NextSceneId;

        var next = GetScene(state.CurrentSceneId);
        state.Act = next.Act;

        if (next.IsEndingScene && state.Ending == EndingId.None)
        {
            state.Ending = ResolveEndingFromScene(next.Id);
        }

        return next;
    }

    public SceneDef AdvanceIfAuto(GameState state)
    {
        var scene = GetScene(state.CurrentSceneId);
        if (string.IsNullOrWhiteSpace(scene.AutoNextSceneId) || scene.Choices.Count > 0)
        {
            return scene;
        }

        state.CurrentSceneId = scene.AutoNextSceneId!;
        var next = GetScene(state.CurrentSceneId);
        state.Act = next.Act;
        return next;
    }

    public EndingId ResolveEnding(GameState state)
    {
        if (state.Ending != EndingId.None)
        {
            return state.Ending;
        }

        return EndingResolver.Resolve(state);
    }

    public static void ApplyEffect(GameState state, ChoiceEffect effect)
    {
        state.AddAffection(effect.AffectionTarget, effect.AffectionDelta);

        foreach (var flag in effect.SetFlags)
        {
            state.SetFlag(flag);
        }

        if (effect.LockRoute != CharacterId.None)
        {
            state.LockedRoute = effect.LockRoute;
            state.SetFlag(GameFlags.RouteLocked);
        }

        if (effect.SetEnding != EndingId.None)
        {
            state.Ending = effect.SetEnding;
        }

        if (effect.SetAct is int act)
        {
            state.Act = act;
        }
    }

    private static EndingId ResolveEndingFromScene(string sceneId) => sceneId switch
    {
        "ending_rin" => EndingId.RinTrueLove,
        "ending_mika" => EndingId.MikaDeath,
        "ending_akari" => EndingId.AkariRuin,
        "ending_sora" => EndingId.SoraCrime,
        "ending_lonely" => EndingId.LonelyNight,
        _ => EndingId.None,
    };
}
