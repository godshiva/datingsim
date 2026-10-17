using DatingSim.Core.Characters;
using Xunit;

namespace DatingSim.Tests;

public class CharacterDesignTests
{
    [Fact]
    public void All_romanceable_characters_are_twenty_one()
    {
        foreach (var character in CharacterRegistry.All)
        {
            Assert.Equal(21, character.Age);
        }
    }

    [Fact]
    public void Each_character_has_unique_aesthetic_and_sensitive_issue()
    {
        var aesthetics = CharacterRegistry.All.Select(c => c.Aesthetic).ToHashSet();
        var issues = CharacterRegistry.All.Select(c => c.SensitiveIssue).ToHashSet();

        Assert.Equal(4, aesthetics.Count);
        Assert.Equal(4, issues.Count);
    }

    [Fact]
    public void Exactly_one_character_is_documented_as_the_happy_route()
    {
        var happy = CharacterRegistry.All.Where(c => c.RoutePromise.Contains("happy", StringComparison.OrdinalIgnoreCase)).ToList();
        Assert.Single(happy);
        Assert.Equal(CharacterId.Rin, happy[0].Id);
    }
}
