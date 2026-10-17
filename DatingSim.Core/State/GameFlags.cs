namespace DatingSim.Core.State;

/// <summary>
/// Story flags set by choices. Names are stable for save compatibility and tests.
/// </summary>
public static class GameFlags
{
    public const string MetClub = "met_club";
    public const string Act1Complete = "act1_complete";
    public const string Act2Complete = "act2_complete";
    public const string Act3Complete = "act3_complete";
    public const string RouteLocked = "route_locked";

    public const string RinInsight = "rin_insight";
    public const string RinTrusted = "rin_trusted";
    public const string RinNotPity = "rin_not_pity";
    public const string RinSawKindness = "rin_saw_kindness";

    public const string MikaPushedEx = "mika_pushed_ex";
    public const string MikaFoundDrawerHint = "mika_drawer_hint";
    public const string MikaObsessed = "mika_obsessed";
    public const string TriedToLeaveMika = "tried_leave_mika";

    public const string AkariQuestionedRival = "akari_questioned_rival";
    public const string AkariCoveredForYou = "akari_covered";
    public const string AkariLeverage = "akari_leverage";

    public const string SoraMoneyLie = "sora_money_lie";
    public const string SoraInvitedCrime = "sora_invited_crime";
    public const string SoraRefusedCrime = "sora_refused_crime";
    public const string SoraAcceptedCrime = "sora_accepted_crime";

    public const string RareSecretMeeting = "rare_secret_meeting";
}
