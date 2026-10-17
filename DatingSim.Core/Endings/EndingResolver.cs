using DatingSim.Core.Characters;
using DatingSim.Core.State;

namespace DatingSim.Core.Endings;

public static class EndingResolver
{
    /// <summary>
    /// Resolves the finale when the story reaches a route-lock checkpoint without an explicit ending yet.
    /// Happy path requires Rin route + insight flags — not affection alone.
    /// </summary>
    public static EndingId Resolve(GameState state)
    {
        if (state.Ending != EndingId.None)
        {
            return state.Ending;
        }

        var route = state.LockedRoute != CharacterId.None
            ? state.LockedRoute
            : state.HighestAffection();

        return route switch
        {
            CharacterId.Rin when CanGetRinTrueEnding(state) => EndingId.RinTrueLove,
            CharacterId.Rin => EndingId.LonelyNight,
            CharacterId.Mika => EndingId.MikaDeath,
            CharacterId.Akari => EndingId.AkariRuin,
            CharacterId.Sora => EndingId.SoraCrime,
            _ => EndingId.LonelyNight,
        };
    }

    public static bool CanGetRinTrueEnding(GameState state) =>
        state.HasFlag(GameFlags.RinInsight)
        && state.HasFlag(GameFlags.RinNotPity)
        && state.HasFlag(GameFlags.RinSawKindness)
        && state.GetAffection(CharacterId.Rin) >= 40;
}
