// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RetroBot;

public class BotBrain
{
    private readonly IEnumerable<DateTime> RetroDates;

    public BotBrain()
    {
        RetroDates =
            Enumerable.Range(0, 12)
            .Select(months => new DateTime(2023, 1, 26) + new TimeSpan(28 * months, 0, 0, 0));
    }

    private static string TwoDaysBeforeText
    {
        get
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Reminder: The next retrospective is on Thursday morning. Please send @Chris Preston ***by lunchtime Wednesday***:");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• What has gone well?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• Where is there an opportunity to improve?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Anything received after that time will roll over into the next retrospective. You can submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2");
            return messageBuilder.ToString();
        }
    }
    
    private static string OneDayBeforeText
    {
        get
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Reminder: The next retrospective is tomorrow morning. Please send @Chris Preston ***by lunchtime today***:");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• What has gone well?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• Where is there an opportunity to improve?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Anything received after that time will roll over into the next retrospective. You can submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2");
            return messageBuilder.ToString();
        }
    }

    public async Task Post(DateTime today, IPoster poster, ILogger logger)
    {
        var numberOfDaysToSprintReview = RetroDates.Select(d => d.Date - today.Date).Select(diff => diff.TotalDays).Min();

        switch (numberOfDaysToSprintReview)
        {
            case > 1 and <= 2:
                logger.LogInformation("Posting the 2 days before message");
                await poster.Post(TwoDaysBeforeText);
                break;
            case > 0 and <= 1:
                logger.LogInformation("Posting the 1 day before message");
                await poster.Post(OneDayBeforeText);
                break;
            default:
                logger.LogInformation($"It's a {today.DayOfWeek} so don't need to post anything today.");
                break;
        }
    }
}