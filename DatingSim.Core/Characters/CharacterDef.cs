namespace DatingSim.Core.Characters;

public sealed class CharacterDef
{
    public required CharacterId Id { get; init; }
    public required string Name { get; init; }
    public required string ShortName { get; init; }
    public required string Aesthetic { get; init; }
    public required string PublicPersona { get; init; }
    public required string SensitiveIssue { get; init; }
    public required string LiePattern { get; init; }
    public required string HiddenTruth { get; init; }
    public required string RoutePromise { get; init; }
    public required string AccentColor { get; init; }
    public int Age => 21;
}
