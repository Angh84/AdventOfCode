using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Lib;

public class SolutionRunner(IConfiguration config, HttpClient client) : ISolutionRunner
{
    private readonly string inputLocation = config.GetSection("Settings")["inputPath"] ?? throw new ArgumentNullException(nameof(config), "InputPath must be provided in config");
    private readonly string session = config.GetSection("Settings")["session"] ?? throw new ArgumentNullException(nameof(config), "Session must be provided in config");
    private readonly int year = int.Parse(config.GetSection("Settings")["year"] ?? throw new ArgumentNullException(nameof(config), "Year must be provided in config"));
    public async Task RunSolution(int day)
    {
        if (day is < 1 or > 25)
            throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 25");
        
        var type = Type.GetType($"AdventOfCode.Puzzles.Day{day:D2}");
        if (type is null)
            throw new ArgumentException($"Day {day} has not been implemented yet");
        
        var input = await GetInput(day);
        var puzzle = Activator.CreateInstance(type, input) as IPuzzle;
        
        Console.WriteLine(puzzle?.FirstSolution() ?? "No solution found");
        Console.WriteLine(puzzle?.SecondSolution() ?? "No solution found");
    }
    private async Task<IEnumerable<string>> GetInput(int day)
    {
        if (day is < 1 or > 25)
        {
            throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 25");
        }
        var fileName = "Day" + day.ToString("D2") + ".txt";
        var fullPath = Path.Combine(inputLocation, fileName);
        if (!File.Exists(fullPath))
        {
            await DownloadInput(day);
        }
        return await File.ReadAllLinesAsync(Path.Combine(inputLocation, fileName));
    }
    private async Task DownloadInput(int day)
    {
        var earliestRequestTime = new DateTime(year, 12, day, 5, 0, 0);
        var currentTime = DateTime.UtcNow;
        if (currentTime < earliestRequestTime) throw new ArgumentException($"Input for day {day} is not available until {earliestRequestTime.ToLocalTime()}");
        
        var requestUri = $"https://adventofcode.com/{year}/day/{day}/input";
        
        client.DefaultRequestHeaders.Add("Cookie", $"session={session}");
        var response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var input = await response.Content.ReadAsStringAsync();
        var fileName = "Day" + day.ToString("D2") + ".txt";
        var fullPath = Path.Combine(inputLocation, fileName);
        await File.WriteAllTextAsync(fullPath, input);
    }
    
}