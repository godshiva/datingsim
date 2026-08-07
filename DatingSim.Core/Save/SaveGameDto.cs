using DatingSim.Core.Characters;
using DatingSim.Core.Endings;

namespace DatingSim.Core.Save;

public sealed class SaveGameDto
{
    public int SaveVersion { get; set; }
    public string PlayerName { get; set; } = "You";
    public int Act { get; set; }
    public string CurrentSceneId { get; set; } = string.Empty;
    public string LockedRoute { get; set; } = nameof(CharacterId.None);
    public string Ending { get; set; } = nameof(EndingId.None);
    public Dictionary<string, int> Affection { get; set; } = new();
    public List<string> Flags { get; set; } = [];
    public List<string> ChoiceHistory { get; set; } = [];
    public DateTimeOffset LastSavedUtc { get; set; }
}
