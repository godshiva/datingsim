namespace DatingSim.Core.Characters;

public static class CharacterRegistry
{
    private static readonly IReadOnlyDictionary<CharacterId, CharacterDef> Characters =
        new Dictionary<CharacterId, CharacterDef>
        {
            [CharacterId.Rin] = new CharacterDef
            {
                Id = CharacterId.Rin,
                Name = "Rin Okada",
                ShortName = "Rin",
                Aesthetic = "Ink-black bob, charcoal coat, cool silver jewelry — night-rain palette.",
                PublicPersona = "Blunt, dismissive, allergic to small talk. Looks like she would rather be anywhere else.",
                SensitiveIssue = "Being pitied, abandoned, or treated like an emotional dumping ground.",
                LiePattern = "If pressed about family or why she joined the club, she invents a tidy 'I like quiet rooms' story.",
                HiddenTruth = "The only genuinely decent person. Her coldness is armor; she quietly covers for people who never notice.",
                RoutePromise = "Only happy ending — if you see through the armor without forcing her open.",
                AccentColor = "#6B7C93",
            },
            [CharacterId.Mika] = new CharacterDef
            {
                Id = CharacterId.Mika,
                Name = "Mika Hayashi",
                ShortName = "Mika",
                Aesthetic = "Long dark hair, lavender cardigan, watercolor stains on her sleeves — soft studio light.",
                PublicPersona = "Shy, gentle, offers to sketch you before she finishes a sentence.",
                SensitiveIssue = "Her last relationship and anything about the locked drawer in the annex studio.",
                LiePattern = "Claims her ex 'moved away for grad school' and that the drawer is 'just old supplies.'",
                HiddenTruth = "Obsessive. If you romance her fully and then pull away — or find what she hid — she kills you.",
                RoutePromise = "Death ending.",
                AccentColor = "#9B8BB8",
            },
            [CharacterId.Akari] = new CharacterDef
            {
                Id = CharacterId.Akari,
                Name = "Akari Fujimoto",
                ShortName = "Akari",
                Aesthetic = "Coral ribbon, sunlit cardigan, perfect posture — campus-brochure warmth.",
                PublicPersona = "Club president. Charismatic, helpful, insists the After Hours Club is 'a little family.'",
                SensitiveIssue = "Her scholarship, perfect record, and why a rival member vanished from the roster.",
                LiePattern = "Frames every scandal as 'a misunderstanding we already resolved as a club.'",
                HiddenTruth = "A social climber who will torch your reputation to keep her image immaculate.",
                RoutePromise = "Betrayal / social ruin ending.",
                AccentColor = "#E07A5F",
            },
            [CharacterId.Sora] = new CharacterDef
            {
                Id = CharacterId.Sora,
                Name = "Sora Watanabe",
                ShortName = "Sora",
                Aesthetic = "Mint hoodie, pink clips, twin tails, snack wrappers in every pocket — candy-store chaos.",
                PublicPersona = "Bubbly, clingy, best-friends-you-in fifteen minutes.",
                SensitiveIssue = "Money, her 'side hustle,' and late-night campus errands.",
                LiePattern = "Says she tutors rich kids / sells handmade charms. Changes the subject with snacks and nicknames.",
                HiddenTruth = "Runs a theft ring. Wants you as an accomplice — refuse, and she frames you.",
                RoutePromise = "Crime / framed ending.",
                AccentColor = "#7BC6A4",
            },
        };

    public static IEnumerable<CharacterDef> All => Characters.Values;

    public static CharacterDef Get(CharacterId id) => Characters[id];

    public static bool TryGet(CharacterId id, out CharacterDef def) => Characters.TryGetValue(id, out def!);
}
