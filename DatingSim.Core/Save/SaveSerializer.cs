using System.Text.Json;
using System.Text.Json.Serialization;
using DatingSim.Core.Characters;
using DatingSim.Core.Endings;
using DatingSim.Core.State;

namespace DatingSim.Core.Save;

public static class SaveSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static string Serialize(GameState state)
    {
        var dto = ToDto(state);
        return JsonSerializer.Serialize(dto, Options);
    }

    public static GameState Deserialize(string json)
    {
        var dto = JsonSerializer.Deserialize<SaveGameDto>(json, Options)
            ?? throw new InvalidOperationException("Save file was empty or invalid.");
        return FromDto(dto);
    }

    public static SaveGameDto ToDto(GameState state) =>
        new()
        {
            SaveVersion = state.SaveVersion,
            PlayerName = state.PlayerName,
            Act = state.Act,
            CurrentSceneId = state.CurrentSceneId,
            LockedRoute = state.LockedRoute.ToString(),
            Ending = state.Ending.ToString(),
            Affection = state.Affection.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value),
            Flags = state.Flags.OrderBy(f => f, StringComparer.Ordinal).ToList(),
            ChoiceHistory = [.. state.ChoiceHistory],
            LastSavedUtc = state.LastSavedUtc,
        };

    public static GameState FromDto(SaveGameDto dto)
    {
        if (dto.SaveVersion > GameState.CurrentSaveVersion)
        {
            throw new InvalidOperationException(
                $"Save version {dto.SaveVersion} is newer than supported {GameState.CurrentSaveVersion}.");
        }

        var state = new GameState
        {
            SaveVersion = GameState.CurrentSaveVersion,
            PlayerName = string.IsNullOrWhiteSpace(dto.PlayerName) ? "You" : dto.PlayerName,
            Act = dto.Act <= 0 ? 1 : dto.Act,
            CurrentSceneId = string.IsNullOrWhiteSpace(dto.CurrentSceneId)
                ? StoryBootstrap.OpeningSceneId
                : dto.CurrentSceneId,
            LockedRoute = ParseEnum(dto.LockedRoute, CharacterId.None),
            Ending = ParseEnum(dto.Ending, EndingId.None),
            Flags = new HashSet<string>(dto.Flags ?? [], StringComparer.Ordinal),
            ChoiceHistory = dto.ChoiceHistory?.ToList() ?? [],
            LastSavedUtc = dto.LastSavedUtc == default ? DateTimeOffset.UtcNow : dto.LastSavedUtc,
        };

        foreach (var id in new[] { CharacterId.Rin, CharacterId.Mika, CharacterId.Akari, CharacterId.Sora })
        {
            state.Affection[id] = 0;
        }

        if (dto.Affection != null)
        {
            foreach (var (key, value) in dto.Affection)
            {
                if (Enum.TryParse<CharacterId>(key, ignoreCase: true, out var id) && id != CharacterId.None)
                {
                    state.Affection[id] = Math.Clamp(value, 0, 100);
                }
            }
        }

        return state;
    }

    private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback)
        where TEnum : struct, Enum =>
        Enum.TryParse<TEnum>(value, ignoreCase: true, out var parsed) ? parsed : fallback;
}
