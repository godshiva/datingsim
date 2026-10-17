using DatingSim.Core.Characters;
using DatingSim.Core.Endings;

namespace DatingSim.Core.Narrative;

public sealed class ChoiceEffect
{
    public CharacterId AffectionTarget { get; init; } = CharacterId.None;
    public int AffectionDelta { get; init; }
    public IReadOnlyList<string> SetFlags { get; init; } = [];
    public CharacterId LockRoute { get; init; } = CharacterId.None;
    public EndingId SetEnding { get; init; } = EndingId.None;
    public int? SetAct { get; init; }
}
