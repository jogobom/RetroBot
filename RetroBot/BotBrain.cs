// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Threading.Tasks;

namespace RetroBot;

public static class BotBrain
{
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

    private static int NumberOfWeeksSinceReferenceDate(DateTime today, DateTime referenceDate)
    {
        return (int)Math.Ceiling((today - referenceDate).Days / 7.0);
    }
    
    public static async Task Post(DateTime today, DateTime referenceDate, IPoster poster)
    {
        var numberOfWeeksIntoSprint = NumberOfWeeksSinceReferenceDate(today, referenceDate) % 4;
        
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (today.DayOfWeek)
        {
            case DayOfWeek.Tuesday:
            {
                if (numberOfWeeksIntoSprint == 0)
                {
                    await poster.Post(TwoDaysBeforeText);
                }
                break;
            }
            case DayOfWeek.Wednesday:
            {
                switch (numberOfWeeksIntoSprint)
                {
                    case 0:
                        await poster.Post(OneDayBeforeText);
                        break;
                    case 2:
                        await poster.Post(InterimReminderText);
                        break;
                }
                break;
            }
        }
    }
}