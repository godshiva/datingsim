using DatingSim.Core.Endings;
using DatingSim.Core.Narrative;
using DatingSim.Core.State;
using Xunit;

namespace DatingSim.Tests;

public class PathDiscoverabilityTests
{
    private readonly StoryEngine _engine = new();

    [Fact]
    public void Every_scene_is_reachable_from_new_game()
    {
        var report = PathExplorer.Explore(_engine);

        Assert.Empty(report.UnreachableScenes);
        Assert.Contains(StoryBootstrap.OpeningSceneId, report.ReachableScenes);
    }

    [Fact]
    public void Every_authored_choice_is_discoverable()
    {
        var report = PathExplorer.Explore(_engine);

        Assert.Empty(report.UnreachableChoices);
    }

    [Fact]
    public void Every_ending_is_reachable()
    {
        var report = PathExplorer.Explore(_engine);
        var expected = Enum.GetValues<EndingId>().Where(e => e != EndingId.None).ToHashSet();

        Assert.Equal(expected, report.ReachableEndings.ToHashSet());
    }

    [Fact]
    public void Only_one_happy_ending_exists()
    {
        var happy = EndingCatalog.All.Where(e => e.IsHappy).Select(e => e.Id).ToList();

        Assert.Single(happy);
        Assert.Equal(EndingId.RinTrueLove, happy[0]);
        Assert.Equal(EndingId.RinTrueLove, EndingCatalog.OnlyHappyEnding);
    }

    [Fact]
    public void Happy_ending_requires_insight_flags_not_affection_alone()
    {
        var state = GameState.CreateNew();
        state.AddAffection(Core.Characters.CharacterId.Rin, 100);
        state.LockedRoute = Core.Characters.CharacterId.Rin;

        Assert.False(EndingResolver.CanGetRinTrueEnding(state));
        Assert.Equal(EndingId.LonelyNight, EndingResolver.Resolve(state));

        state.SetFlag(GameFlags.RinInsight);
        state.SetFlag(GameFlags.RinNotPity);
        state.SetFlag(GameFlags.RinSawKindness);

        Assert.True(EndingResolver.CanGetRinTrueEnding(state));
        Assert.Equal(EndingId.RinTrueLove, EndingResolver.Resolve(state));
    }

    [Fact]
    public void Canonical_happy_path_reaches_rin_true_love()
    {
        var state = GameState.CreateNew();
        Play(state,
            "a1_go_rin",
            "a1_rin_probe_family",
            "a1_rin_lie_see",
            "a1_to_act2",
            "a2_partner_rin",
            "a2_rin_quiet",
            "a2_to_act3",
            "a3_lock_rin",
            "a3_rin_true");

        Assert.Equal(EndingId.RinTrueLove, state.Ending);
        Assert.Equal("ending_rin", state.CurrentSceneId);
    }

    [Fact]
    public void Mika_route_always_ends_in_death()
    {
        var state = GameState.CreateNew();
        Play(state,
            "a1_go_mika",
            "a1_mika_flatter",
            "a1_to_act2",
            "a2_partner_mika",
            "a2_mika_promise",
            "a2_to_act3",
            "a3_lock_mika",
            "a3_mika_stay");

        Assert.Equal(EndingId.MikaDeath, state.Ending);
    }

    [Fact]
    public void Sora_route_ends_in_crime_whether_you_accept_or_refuse()
    {
        var accept = GameState.CreateNew();
        Play(accept,
            "a1_go_sora",
            "a1_sora_money",
            "a1_sora_lie_note",
            "a1_to_act2",
            "a2_partner_sora",
            "a2_sora_callout",
            "a2_sora_accept",
            "a2_to_act3",
            "a3_lock_sora",
            "a3_sora_go");
        Assert.Equal(EndingId.SoraCrime, accept.Ending);

        var refuse = GameState.CreateNew();
        Play(refuse,
            "a1_go_sora",
            "a1_sora_money",
            "a1_sora_lie_note",
            "a1_to_act2",
            "a2_partner_sora",
            "a2_sora_callout",
            "a2_sora_refuse",
            "a2_to_act3",
            "a3_lock_sora",
            "a3_sora_no");
        Assert.Equal(EndingId.SoraCrime, refuse.Ending);
    }

    private void Play(GameState state, params string[] choiceIds)
    {
        foreach (var choiceId in choiceIds)
        {
            _engine.ApplyChoice(state, choiceId);
        }
    }
}
