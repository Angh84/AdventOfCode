﻿using AdventOfCode.Puzzles;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();
IConfiguration config = builder.Build();


var inputLocation = config.GetSection("Settings")["inputPath"];
var day = new Day01(inputLocation);
Console.WriteLine(day.SolutionOne());
Console.WriteLine(day.SolutionTwo());