namespace DatingSim.Core.Endings;

public static class EndingCatalog
{
    private static readonly IReadOnlyDictionary<EndingId, EndingDef> Endings =
        new Dictionary<EndingId, EndingDef>
        {
            [EndingId.RinTrueLove] = new EndingDef
            {
                Id = EndingId.RinTrueLove,
                Title = "Rain on the Annex Roof",
                Summary =
                    "Rin stops pretending she doesn't care. You stay — not as a project, not as a pity case. " +
                    "The only ending that isn't a knife with a ribbon on it.",
                IsHappy = true,
                IsRare = true,
                RouteCharacter = "Rin",
            },
            [EndingId.MikaDeath] = new EndingDef
            {
                Id = EndingId.MikaDeath,
                Title = "The Locked Drawer",
                Summary =
                    "Mika's smile never changes. The annex studio smells like turpentine and copper. " +
                    "You learn too late what happened to the person in her sketches.",
                IsHappy = false,
                IsRare = false,
                RouteCharacter = "Mika",
            },
            [EndingId.AkariRuin] = new EndingDef
            {
                Id = EndingId.AkariRuin,
                Title = "Minutes of the Meeting",
                Summary =
                    "Akari thanks you for 'protecting the club.' By morning your name is on every disciplinary form, " +
                    "and she is still everyone's favorite president.",
                IsHappy = false,
                IsRare = false,
                RouteCharacter = "Akari",
            },
            [EndingId.SoraCrime] = new EndingDef
            {
                Id = EndingId.SoraCrime,
                Title = "Errand After Midnight",
                Summary =
                    "Sora calls it a game until the campus police call it theft. " +
                    "Whether you helped or refused, the evidence points at you — and she is already laughing somewhere else.",
                IsHappy = false,
                IsRare = false,
                RouteCharacter = "Sora",
            },
            [EndingId.LonelyNight] = new EndingDef
            {
                Id = EndingId.LonelyNight,
                Title = "Lights Out in the Annex",
                Summary =
                    "You never chose anyone clearly enough. The club dissolves around you. " +
                    "No one kills you. No one saves you. You just leave.",
                IsHappy = false,
                IsRare = true,
                RouteCharacter = "None",
            },
        };

    public static IEnumerable<EndingDef> All => Endings.Values;

    public static EndingDef Get(EndingId id) => Endings[id];

    public static EndingId OnlyHappyEnding => EndingId.RinTrueLove;
}
