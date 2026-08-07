namespace DatingSim.Core.Narrative;

public sealed class ChoiceDef
{
    public required string Id { get; init; }
    public required string Text { get; init; }
    public required string NextSceneId { get; init; }
    public ChoiceEffect Effect { get; init; } = new();
    public IReadOnlyList<string> RequiresFlags { get; init; } = [];
    public IReadOnlyList<string> ForbidsFlags { get; init; } = [];
    public bool IsRare { get; init; }
    public string? Hint { get; init; }
}
