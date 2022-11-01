// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RetroBot;

public static class BotBrain
{
    private static readonly List<DateTime> RetroDates = new()
    {
        new DateTime(2022, 9, 8),
        new DateTime(2022, 10, 6),
        new DateTime(2022, 11, 3),
        new DateTime(2022, 12, 1),
    };

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

    public static async Task Post(DateTime today, IPoster poster, ILogger logger)
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