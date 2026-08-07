namespace DatingSim.Core.Endings;

public sealed class EndingDef
{
    public required EndingId Id { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required bool IsHappy { get; init; }
    public required bool IsRare { get; init; }
    public required string RouteCharacter { get; init; }
}
