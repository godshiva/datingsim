using DatingSim.Core.Characters;
using DatingSim.Core.Endings;

namespace DatingSim.Core.State;

public sealed class GameState
{
    public const int CurrentSaveVersion = 1;

    public int SaveVersion { get; set; } = CurrentSaveVersion;
    public string PlayerName { get; set; } = "You";
    public int Act { get; set; } = 1;
    public string CurrentSceneId { get; set; } = StoryBootstrap.TitleSceneId;
    public CharacterId LockedRoute { get; set; } = CharacterId.None;
    public EndingId Ending { get; set; } = EndingId.None;
    public Dictionary<CharacterId, int> Affection { get; set; } = CreateDefaultAffection();
    public HashSet<string> Flags { get; set; } = new(StringComparer.Ordinal);
    public List<string> ChoiceHistory { get; set; } = [];
    public DateTimeOffset LastSavedUtc { get; set; } = DateTimeOffset.UtcNow;

    public bool HasEnding => Ending != EndingId.None;

    public static GameState CreateNew(string playerName = "You")
    {
        return new GameState
        {
            PlayerName = string.IsNullOrWhiteSpace(playerName) ? "You" : playerName.Trim(),
            Act = 1,
            CurrentSceneId = StoryBootstrap.OpeningSceneId,
            Affection = CreateDefaultAffection(),
            Flags = new HashSet<string>(StringComparer.Ordinal),
            ChoiceHistory = [],
        };
    }

    public int GetAffection(CharacterId id) =>
        Affection.TryGetValue(id, out var value) ? value : 0;

    public void AddAffection(CharacterId id, int delta)
    {
        if (id == CharacterId.None || delta == 0)
        {
            return;
        }

        Affection[id] = Math.Clamp(GetAffection(id) + delta, 0, 100);
    }

    public bool HasFlag(string flag) => Flags.Contains(flag);

    public void SetFlag(string flag)
    {
        if (!string.IsNullOrWhiteSpace(flag))
        {
            Flags.Add(flag);
        }
    }

    public CharacterId HighestAffection(params CharacterId[] candidates)
    {
        var list = candidates.Length == 0
            ? new[] { CharacterId.Rin, CharacterId.Mika, CharacterId.Akari, CharacterId.Sora }
            : candidates;

        CharacterId best = CharacterId.None;
        var bestScore = int.MinValue;
        var tie = false;

        foreach (var id in list)
        {
            var score = GetAffection(id);
            if (score > bestScore)
            {
                best = id;
                bestScore = score;
                tie = false;
            }
            else if (score == bestScore)
            {
                tie = true;
            }
        }

        return tie || bestScore < 1 ? CharacterId.None : best;
    }

    public GameState Clone()
    {
        return new GameState
        {
            SaveVersion = SaveVersion,
            PlayerName = PlayerName,
            Act = Act,
            CurrentSceneId = CurrentSceneId,
            LockedRoute = LockedRoute,
            Ending = Ending,
            Affection = new Dictionary<CharacterId, int>(Affection),
            Flags = new HashSet<string>(Flags, StringComparer.Ordinal),
            ChoiceHistory = [.. ChoiceHistory],
            LastSavedUtc = LastSavedUtc,
        };
    }

    private static Dictionary<CharacterId, int> CreateDefaultAffection() =>
        new()
        {
            [CharacterId.Rin] = 0,
            [CharacterId.Mika] = 0,
            [CharacterId.Akari] = 0,
            [CharacterId.Sora] = 0,
        };
}

/// <summary>
/// Scene id constants shared by bootstrap and UI without pulling the full story graph.
/// </summary>
public static class StoryBootstrap
{
    public const string TitleSceneId = "title";
    public const string OpeningSceneId = "act1_arrival";
}
