// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        var values = new Dictionary<string, string>
        {
            {"@type", "MessageCard"},
            {"@context", "https://schema.org/extensions"},
            {"themeColor", "ff00ff"},
            {"text", message },
        };

        var content = new StringContent(JsonSerializer.Serialize(values), Encoding.UTF8, "application/json");
        await client.PostAsync(hook, content);
    }
}