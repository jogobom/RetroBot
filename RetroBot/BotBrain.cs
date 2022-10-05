// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Text;
using System.Threading.Tasks;

namespace RetroBot;

public static class BotBrain
{
    private static string InterimReminderText
    {
        get
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Reminder: We no longer have retrospectives every two weeks but if you have any thoughts about:");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• What has gone well?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• Where is there an opportunity to improve?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("you can submit them to @Chris Preston for the next retrospective at any time. You can also submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2");
            return messageBuilder.ToString();
        }
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
            messageBuilder.AppendLine("Reminder: The next retrospective is tomorrow morning. Please send @Chris Preston **by lunchtime today***:");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• What has gone well?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("• Where is there an opportunity to improve?");
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Anything received after that time will roll over into the next retrospective. You can submit things anonymously if you prefer to do that, using this form: https://forms.office.com/r/AYdQ66SEU2");
            return messageBuilder.ToString();
        }
    }

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
                switch (numberOfWeeksIntoSprint)
                {
                    case 0:
                        Console.WriteLine("It's a Tuesday of the week that the sprint ends, so posting the 2 days before message");
                        await poster.Post(TwoDaysBeforeText);
                        break;
                    default:
                        Console.WriteLine("It's a Tuesday but not the week that the sprint ends, so don't need to post anything today");
                        break;
                }

                break;
            }
            case DayOfWeek.Wednesday:
            {
                switch (numberOfWeeksIntoSprint)
                {
                    case 0:
                        Console.WriteLine("It's a Wednesday of the week that the sprint ends, so posting the 1 day before message");
                        await poster.Post(OneDayBeforeText);
                        break;
                    case 2:
                        Console.WriteLine("It's a Wednesday of 2 weeks into the sprint, so posting the interim reminder");
                        await poster.Post(InterimReminderText);
                        break;
                    default:
                        Console.WriteLine($"It's a Wednesday but it's week {numberOfWeeksIntoSprint}, so don't need to post anything today");
                        break;
                }

                break;
            }
            default:
                Console.WriteLine($"It's a {today.DayOfWeek} so don't need to post anything today.");
                break;
        }
    }
}