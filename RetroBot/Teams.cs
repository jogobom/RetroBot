// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MessageCards;

namespace RetroBot;

internal class Teams : IPoster
{
    private readonly HttpClient client = new();

    public async Task Post(string message)
    {
        var webhookUrl = Environment.GetEnvironmentVariable("TEAMS_WEBHOOK");
        if (!string.IsNullOrWhiteSpace(webhookUrl))
        {
            await Post(message, webhookUrl);
        }
    }

    private async Task Post(string message, string hook)
    {
        var card = new MessageCard("Retrospectives")
        {
            Text = message
        };

        var content = new StringContent(card.ToJson(), Encoding.UTF8, "application/json");
        await client.PostAsync(hook, content);
    }
}