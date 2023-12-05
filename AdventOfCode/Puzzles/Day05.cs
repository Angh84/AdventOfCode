using AdventOfCode.Lib;
using System.Collections.Concurrent;


namespace AdventOfCode.Puzzles;

public class Day05(IEnumerable<string> puzzleInput) : PuzzleBase(puzzleInput)
{
    public override string FirstSolution()
    {
        var almanac = ParseInput();
        var lowestLocation = almanac.GetLowestLocation();
        return lowestLocation.ToString();
    }

    public override string SecondSolution()
    {
        var almanac = ParseInput();
        var lowestLocation = almanac.GetLowestRangeLocation();
        return lowestLocation.ToString();
    }

    private Almanac ParseInput()
    {
        var almanac = new Almanac();
        var groups = ExtractGroups();
        almanac.Seeds = ParseSeeds(groups);
        almanac.SeedToSoil = ParseMap("seed-to-soil", groups);
        almanac.SoilToFertilizer = ParseMap("soil-to-fertilizer", groups);
        almanac.FertilizerToWater = ParseMap("fertilizer-to-water", groups);
        almanac.WaterToLight = ParseMap("water-to-light", groups);
        almanac.LightToTemperature = ParseMap("light-to-temperature", groups);
        almanac.TemperatureToHumidity = ParseMap("temperature-to-humidity", groups);
        almanac.HumidityToLocation = ParseMap("humidity-to-location", groups);
        return almanac;
    }
    private Mapping ParseMap(string mapName, List<string> groups)
    {
        var mapGroup = groups.First(g => g.StartsWith(mapName));
        var map = new Mapping();

        var mapPart = mapGroup.Split(":")[1].Trim();
        var mapList = mapPart.Split(Environment.NewLine);
        foreach (var mapEntry in mapList)
        {
            var parts = mapEntry.Split(" ");
            var destination = long.Parse(parts[0]);
            var source = long.Parse(parts[1]);
            var length = long.Parse(parts[2]);
            var offset = destination - source;
            var currentEntry = new MapEntry(source, source + length, offset);
            map.Entries.Add(currentEntry);
        }
        return map;
    }
    private List<long> ParseSeeds(List<string> groups)
    {
        var seeds = new List<long>();
        var seedGroup = groups.First(g => g.StartsWith("seeds:"));
        var seedList = seedGroup.Split(":")[1].Trim();
        foreach (var seed in seedList.Split(" "))
        {
            seeds.Add(long.Parse(seed));
        }
        return seeds;
    }
    private List<String> ExtractGroups()
    {
        List<string> inputGroups = new();
        var currentGroup = "";
        foreach (var line in Input)
        {
            if (line == "")
            {
                inputGroups.Add(currentGroup);
                currentGroup = "";
            }
            else
            {
                currentGroup += (line + Environment.NewLine) ;
            }
        }
        if (currentGroup != "")
            inputGroups.Add(currentGroup);
        return inputGroups;
    }

}
public class MapEntry(long startRange, long endRange, long offset)
{
    public readonly long StartRange = startRange;
    public readonly long EndRange = endRange;
    public readonly long Offset = offset;
}
public class Mapping
{
    public readonly List<MapEntry> Entries = new();
    public long GetDestination(long source)
    {
        foreach (var entry in Entries)
        {
            if (source >= entry.StartRange && source <= entry.EndRange)
            {
                return source + entry.Offset;
            }
        }
        return source;
    }
}
public class Almanac 
{
    public List<long> Seeds = new();
    public Mapping SeedToSoil = new();
    public Mapping SoilToFertilizer = new();
    public Mapping FertilizerToWater = new ();
    public Mapping WaterToLight = new();
    public Mapping LightToTemperature = new();
    public Mapping TemperatureToHumidity = new();
    public Mapping HumidityToLocation = new();

    public long GetLowestRangeLocation()
    {
        var seedRanges = new List<(long, long)>();
        for (var i = 0; i < Seeds.Count; i += 2) 
        {
            seedRanges.Add((Seeds[i], Seeds[i + 1]));
        }
        var totalLowest = new ConcurrentBag<long>();
        Parallel.ForEach(seedRanges, seedRange =>
        {
            var localLowest = long.MaxValue;
            for (var i = seedRange.Item1; i <= seedRange.Item1 + seedRange.Item2; i++)
            {
                var location = GetLocation(i);
                if (location < localLowest) localLowest = location;
            }
            totalLowest.Add(localLowest);
        }); 
        return totalLowest.Min();
    }
    public long GetLowestLocation()
    {
        var locations = Seeds.Select(GetLocation).ToList();
        return locations.Min();
    }

    private long GetLocation(long seed)
    {
        var soil = SeedToSoil.GetDestination(seed);
        var fertilizer = SoilToFertilizer.GetDestination(soil);
        var water = FertilizerToWater.GetDestination(fertilizer);
        var light = WaterToLight.GetDestination(water);
        var temperature = LightToTemperature.GetDestination(light);
        var humidity = TemperatureToHumidity.GetDestination(temperature);
        var location = HumidityToLocation.GetDestination(humidity);
        return location;
    }
}
