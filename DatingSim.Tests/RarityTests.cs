using DatingSim.Core.Endings;
using DatingSim.Core.Narrative;
using DatingSim.Core.State;
using Xunit;

namespace DatingSim.Tests;

public class RarityTests
{
    [Fact]
    public void Rare_choices_are_marked_and_reachable()
    {
        var engine = new StoryEngine();
        var rareChoices = engine.Scenes.Values
            .SelectMany(s => s.Choices)
            .Where(c => c.IsRare)
            .Select(c => c.Id)
            .ToHashSet(StringComparer.Ordinal);

        Assert.Contains("a1_rare_listen", rareChoices);
        Assert.Contains("a3_rin_true", rareChoices);
        Assert.Contains("a3_lock_none", rareChoices);

        var report = PathExplorer.Explore(engine);
        foreach (var id in rareChoices)
        {
            Assert.Contains(id, report.ReachableChoices);
        }
    }

    [Fact]
    public void Happy_ending_has_fewer_terminal_states_than_common_bad_ends()
    {
        var report = PathExplorer.Explore();

        Assert.True(report.EndingVisitCounts[EndingId.RinTrueLove] > 0);
        Assert.True(report.EndingVisitCounts[EndingId.MikaDeath] > 0);
        Assert.True(
            report.EndingVisitCounts[EndingId.RinTrueLove] < report.EndingVisitCounts[EndingId.MikaDeath],
            $"Expected fewer RinTrueLove states ({report.EndingVisitCounts[EndingId.RinTrueLove]}) than MikaDeath ({report.EndingVisitCounts[EndingId.MikaDeath]}).");
        Assert.True(
            report.EndingVisitCounts[EndingId.RinTrueLove] < report.EndingVisitCounts[EndingId.AkariRuin]);
    }

    [Fact]
    public void Happy_ending_is_rarer_than_mika_death_in_random_play()
    {
        var tallies = PathExplorer.MonteCarloEndings(runs: 20000, seed: 42);

        Assert.True(tallies[EndingId.MikaDeath] > 0, "MikaDeath should appear in random play.");
        Assert.True(
            tallies[EndingId.RinTrueLove] < tallies[EndingId.MikaDeath],
            $"Expected RinTrueLove ({tallies[EndingId.RinTrueLove]}) < MikaDeath ({tallies[EndingId.MikaDeath]}).");
    }

    [Fact]
    public void Rare_secret_meeting_appears_less_often_than_common_opening_picks()
    {
        var report = PathExplorer.Explore();

        var rareVisits = report.ChoiceVisitCounts.GetValueOrDefault("a1_rare_listen");
        var commonVisits = report.ChoiceVisitCounts.GetValueOrDefault("a1_go_mika");

        Assert.True(rareVisits > 0);
        Assert.True(commonVisits > 0);
        // In exhaustive BFS, visit counts reflect distinct state fan-out; rare opening should not dominate.
        Assert.True(rareVisits <= commonVisits);
    }

    [Fact]
    public void Ending_catalog_marks_happy_and_lonely_as_rare()
    {
        Assert.True(EndingCatalog.Get(EndingId.RinTrueLove).IsRare);
        Assert.True(EndingCatalog.Get(EndingId.LonelyNight).IsRare);
        Assert.False(EndingCatalog.Get(EndingId.MikaDeath).IsRare);
        Assert.False(EndingCatalog.Get(EndingId.AkariRuin).IsRare);
        Assert.False(EndingCatalog.Get(EndingId.SoraCrime).IsRare);
    }

    [Fact]
    public void Stairwell_leverage_branch_requires_rare_flag()
    {
        var engine = new StoryEngine();
        var state = GameState.CreateNew();
        engine.ApplyChoice(state, "a1_go_akari");
        engine.ApplyChoice(state, "a1_akari_loyal");
        engine.ApplyChoice(state, "a1_to_act2");
        engine.ApplyChoice(state, "a2_partner_akari");

        var available = engine.GetAvailableChoices(state).Select(c => c.Id).ToHashSet();
        Assert.DoesNotContain("a2_akari_leverage", available);

        state.SetFlag(GameFlags.RareSecretMeeting);
        available = engine.GetAvailableChoices(state).Select(c => c.Id).ToHashSet();
        Assert.Contains("a2_akari_leverage", available);
    }
}
