using Xunit;
using Moq;
using task04;

namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }
    
    [Fact]
    public void Fighter_ShouldHaveLessFirePowerThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }
    
    [Fact]
    public void Cruiser_MoveForward_ShouldNotThrow()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
    }
    
    [Fact]
    public void Fighter_Rotate_ShouldNotThrow()
    {
        var fighter = new Fighter();
        fighter.Rotate(45);
    }
    
    [Fact]
    public void BothShips_ShouldImplementISpaceship()
    {
        ISpaceship fighter = new Fighter();
        ISpaceship cruiser = new Cruiser();
    }
}