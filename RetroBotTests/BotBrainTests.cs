using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using NSubstitute;

namespace RetroBotTests;

using RetroBot;

public class BotBrainTests
{
    private readonly IPoster poster = Substitute.For<IPoster>();
    private readonly ILogger log = Substitute.For<ILogger>();

    [Fact]
    public async Task ShouldPost48HourReminder_4WeeksIntoSprint_OnTuesday()
    {
        var testDate = new DateTime(2022, 09, 6, 08, 00, 00);
        await BotBrain.Post(testDate, poster, log);

        await poster.Received(1).Post(Arg.Is<string>(s => s.Contains("Thursday morning")));
    }
    
    [Fact]
    public async Task ShouldPost24HourReminder_4WeeksIntoSprint_OnWednesday()
    {
        var testDate = new DateTime(2022, 09, 7, 08, 00, 00);
        await BotBrain.Post(testDate, poster, log);

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
        var testDate = new DateTime(2022, 08, date, 08, 00, 00);
        await BotBrain.Post(testDate, poster, log);

        await poster.DidNotReceiveWithAnyArgs().Post(default);
    }
    
    [Fact]
    public async Task ShouldNotPostAnything_On28Sep2022()
    {
        var testDate = new DateTime(2022, 09, 28, 08, 00, 00);
        await BotBrain.Post(testDate, poster, log);

        await poster.DidNotReceiveWithAnyArgs().Post(default);
    }
    
    [Fact]
    public async Task ShouldNotPostAnything_On12Oct2022()
    {
        var testDate = new DateTime(2022, 10, 12, 08, 00, 00);
        await BotBrain.Post(testDate, poster, log);

        await poster.DidNotReceiveWithAnyArgs().Post(default);
   }
    
    [Fact]
    public async Task ShouldNotPostAnything_On26Oct2022()
    {
        var testDate = new DateTime(2022, 10, 26, 08, 00, 00);
        await BotBrain.Post(testDate, poster, log);

        await poster.DidNotReceiveWithAnyArgs().Post(default);
    }
}