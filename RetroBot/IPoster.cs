// Copyright © 2022 Waters Corporation. All Rights Reserved.

using System.Threading.Tasks;

namespace RetroBot;

public interface IPoster
{
    Task Post(string message);
}