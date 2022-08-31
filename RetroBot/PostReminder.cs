// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace RetroBot;

public static class PostReminder
{
    private static readonly DateTime ReferenceRetroDate = new(2022, 8, 9);
    
    private const string InterimReminderText = @"Reminder: We no longer have retrospectives every two weeks but if you have any thoughts about:
• What has gone well?
• Where is there an opportunity to improve?
you can submit them to @Chris Preston for the next retrospective at any time. You can also submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2";
    
    private const string TwoDaysBeforeText = @"Reminder: The next retrospective is on Thursday morning. Please send @Chris Preston *_by lunchtime Wednesday_*:
• What has gone well?
• Where is there an opportunity to improve?
Anything received after that time will roll over into the next retrospective. You can submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2";
    
    private const string OneDayBeforeText = @"Reminder: The next retrospective is tomorrow morning. Please send @Chris Preston *_by lunchtime today_*:
• What has gone well?
• Where is there an opportunity to improve?
Anything received after that time will roll over into the next retrospective. You can submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2";
    
// 0 8-10 * * 1-5 - the testing one
// 0 9 * * 2-3 - the true one
    
    [FunctionName("PostReminder")]
    public static async Task RunAsync([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

        var today = DateTime.Today;
        var numberOfWeeksIntoSprint = NumberOfTestDaysSinceReferenceDate(today) % 4;
        
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (today.DayOfWeek)
        {
            case DayOfWeek.Thursday:
            {
                if (numberOfWeeksIntoSprint == 0)
                {
                    await Teams.Post(TwoDaysBeforeText);
                }

                break;
            }
            case DayOfWeek.Friday:
            {
                switch (numberOfWeeksIntoSprint)
                {
                    case 0:
                        await Teams.Post(OneDayBeforeText);
                        break;
                    case 2:
                        await Teams.Post(InterimReminderText);
                        break;
                }

                break;
            }
        }
    }

    // private static int NumberOfWeeksSinceReferenceDate(DateTime today)
    // {
    //     return (today - ReferenceRetroDate).Days / 7;
    // }
    private static int NumberOfTestDaysSinceReferenceDate(DateTime today)
    {
        return (today - ReferenceRetroDate).Days / 3;
    }
}