namespace DatingSim.Core.Narrative;

public sealed class SceneDef
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public int Act { get; init; } = 1;
    public IReadOnlyList<DialogueLine> Lines { get; init; } = [];
    public IReadOnlyList<ChoiceDef> Choices { get; init; } = [];
    public string? AutoNextSceneId { get; init; }
    public bool IsEndingScene { get; init; }
}
