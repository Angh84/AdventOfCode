using AdventOfCode.Puzzles;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("config.json", false);
IConfiguration config = builder.Build();

var inputLocation = config.GetSection("Settings")["InputLocation"];
var day = new Day01(inputLocation);
Console.WriteLine(day.SolutionOne());
Console.WriteLine(day.SolutionTwo());