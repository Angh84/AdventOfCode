using System.Diagnostics;
using AdventOfCode.Lib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();

var config = builder.Build();
var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(config)
    .AddHttpClient()
    .AddScoped<ISolutionRunner, SolutionRunner>()
    .BuildServiceProvider();
var ih = serviceProvider.GetService<ISolutionRunner>();
Debug.Assert(ih != null, nameof(ih) + " != null");

await ih.RunSolution(3);

