using NSubstitute;

namespace RetroBotTests;

using RetroBot;

public class BotBrainTests
{
    private readonly DateTime lastRetrospectiveDate = new DateTime(2022, 08, 11);

    [Fact]
    public async Task ShouldPostInterimReminder_2WeeksIntoSprint_OnWednesday()
    {
        var poster = Substitute.For<IPoster>();

        var testDate = new DateTime(2022, 08, 24);
        await BotBrain.Post(testDate, lastRetrospectiveDate, poster);

        await poster.Received(1).Post(Arg.Is<string>(s => s.Contains("no longer have retrospectives")));
    }
    
    [Fact]
    public async Task ShouldPost48HourReminder_4WeeksIntoSprint_OnTuesday()
    {
        var poster = Substitute.For<IPoster>();
        
        var testDate = new DateTime(2022, 09, 6);
        await BotBrain.Post(testDate, lastRetrospectiveDate, poster);

        await poster.Received(1).Post(Arg.Is<string>(s => s.Contains("Thursday morning")));
    }
    
    [Fact]
    public async Task ShouldPost24HourReminder_4WeeksIntoSprint_OnWednesday()
    {
        var poster = Substitute.For<IPoster>();
        
        var testDate = new DateTime(2022, 09, 7);
        await BotBrain.Post(testDate, lastRetrospectiveDate, poster);

        await poster.Received(1).Post(Arg.Is<string>(s => s.Contains("tomorrow morning")));
    }

    [Theory]
    [InlineData(29)]
    [InlineData(25)]
    [InlineData(15)]
    [InlineData(6)]
    [InlineData(12)]
    public async Task ShouldNotPostAnything_AtAnyOtherTime(int date)
    {
        var poster = Substitute.For<IPoster>();
        
        var testDate = new DateTime(2022, 08, date);
        await BotBrain.Post(testDate, lastRetrospectiveDate, poster);

        await poster.DidNotReceive().Post(Arg.Any<string>());
    }
}