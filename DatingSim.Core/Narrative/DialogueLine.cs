using DatingSim.Core.Characters;

namespace DatingSim.Core.Narrative;

public sealed class DialogueLine
{
    public required string Text { get; init; }
    public CharacterId Speaker { get; init; } = CharacterId.None;
    public string? SpeakerDisplayName { get; init; }

    public string ResolveSpeakerName()
    {
        if (!string.IsNullOrWhiteSpace(SpeakerDisplayName))
        {
            return SpeakerDisplayName!;
        }

        return Speaker == CharacterId.None
            ? string.Empty
            : CharacterRegistry.Get(Speaker).ShortName;
    }
}
