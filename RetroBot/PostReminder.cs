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

    [FunctionName("PostReminder")]
    public static async Task RunAsync([TimerTrigger("0 0 9 * * 2-3")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

        var today = DateTime.Today;

        await BotBrain.Post(today, ReferenceRetroDate, new Teams());
    }
}