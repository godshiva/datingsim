using DatingSim.Core.Narrative;
using DatingSim.Core.State;

namespace DatingSim.Scripts.App;

/// <summary>
/// In-memory session bridge between Godot UI scenes.
/// </summary>
public static class GameSession
{
    public static StoryEngine Engine { get; } = new();
    public static GameState? State { get; set; }

    public static void StartNew(string playerName = "You")
    {
        State = GameState.CreateNew(playerName);
    }

    public static void Continue(GameState state)
    {
        State = state;
    }

    public static void Clear() => State = null;
}
