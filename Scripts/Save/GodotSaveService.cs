using DatingSim.Core.Save;
using DatingSim.Core.State;
using Godot;

namespace DatingSim.Scripts.Save;

public static class GodotSaveService
{
    public const string SlotPath = "user://after_hours_slot1.json";

    public static bool HasSave() => Godot.FileAccess.FileExists(SlotPath);

    public static void Save(GameState state)
    {
        state.LastSavedUtc = DateTimeOffset.UtcNow;
        var json = SaveSerializer.Serialize(state);
        using var file = Godot.FileAccess.Open(SlotPath, Godot.FileAccess.ModeFlags.Write);
        if (file is null)
        {
            throw new InvalidOperationException($"Could not open save slot: {Godot.FileAccess.GetOpenError()}");
        }

        file.StoreString(json);
    }

    public static GameState Load()
    {
        if (!HasSave())
        {
            throw new InvalidOperationException("No save file found.");
        }

        using var file = Godot.FileAccess.Open(SlotPath, Godot.FileAccess.ModeFlags.Read);
        if (file is null)
        {
            throw new InvalidOperationException($"Could not read save slot: {Godot.FileAccess.GetOpenError()}");
        }

        return SaveSerializer.Deserialize(file.GetAsText());
    }

    public static void Delete()
    {
        if (HasSave())
        {
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(SlotPath));
        }
    }
}
