﻿namespace PokemonBot.Models;

public class VersionSettings(string versionNumber)
{
    public string VersionNumber { get; set; } = versionNumber;
}