using DatingSim.Core.Characters;
using DatingSim.Core.Endings;
using DatingSim.Core.Save;
using DatingSim.Core.State;
using Xunit;

namespace DatingSim.Tests;

public class SaveSerializerTests
{
    [Fact]
    public void Round_trip_preserves_progress_and_flags()
    {
        var state = GameState.CreateNew("Alex");
        state.Act = 2;
        state.CurrentSceneId = "act2_rin";
        state.AddAffection(CharacterId.Rin, 22);
        state.SetFlag(GameFlags.RinInsight);
        state.ChoiceHistory.Add("a1_go_rin");
        state.LockedRoute = CharacterId.None;

        var json = SaveSerializer.Serialize(state);
        var loaded = SaveSerializer.Deserialize(json);

        Assert.Equal("Alex", loaded.PlayerName);
        Assert.Equal(2, loaded.Act);
        Assert.Equal("act2_rin", loaded.CurrentSceneId);
        Assert.Equal(22, loaded.GetAffection(CharacterId.Rin));
        Assert.True(loaded.HasFlag(GameFlags.RinInsight));
        Assert.Contains("a1_go_rin", loaded.ChoiceHistory);
        Assert.Equal(EndingId.None, loaded.Ending);
    }
}
