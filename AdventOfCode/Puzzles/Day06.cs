using AdventOfCode.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles;

public class Day06(IEnumerable<string> puzzleInput) : PuzzleBase(puzzleInput)
{
    public override string FirstSolution()
    {
        IEnumerable<Round> rounds = ParseInput();
        var sum = rounds.Select(i => i.BeatDistance().Count()).Aggregate((x,y) => x*y);

        return sum.ToString();
    }

    public override string SecondSolution()
    {
        var round = ParseInputAsSingleRound();
        var sum = round.BeatDistance().Count();
        return sum.ToString();
    }
    private IEnumerable<Round> ParseInput()
    {
        var result = new List<Round>();
        var timeRow = Input.ToList()[0].Split(":");
        var times = timeRow[1].Trim().Split(" ");
        var cleanedTimes = times.Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x)).ToList();
        var distanceRow = Input.ToList()[1].Split(":");
        var distances = distanceRow[1].Trim().Split(" ");
        var cleanedDistances = distances.Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x)).ToList();

        for (var i = 0; i < cleanedTimes.Count; i++)
        {
            result.Add(new Round(cleanedTimes[i], cleanedDistances[i]));
            
        }
        return result;
    }
    private Round ParseInputAsSingleRound() 
    {
  
        var timeRow = Input.ToList()[0].Split(":");
        var times = timeRow[1].Trim().Split(" ");
        var cleanedTimes = times.Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x)).ToList();
        var distanceRow = Input.ToList()[1].Split(":");
        var distances = distanceRow[1].Trim().Split(" ");
        var cleanedDistances = distances.Where(x => !string.IsNullOrEmpty(x)).Select(x => long.Parse(x)).ToList();

        var concatenatedTimes = cleanedTimes.Select(x => x.ToString()).Aggregate((x, y) => x + y);
        var concatenatedDistances = cleanedDistances.Select(x => x.ToString()).Aggregate((x, y) => x + y);
        return new Round(long.Parse(concatenatedTimes), long.Parse(concatenatedDistances));
    }
    private class Round(long time, long bestDistance) 
    {
        public IEnumerable<(long,long)> BeatDistance() 
        {
            var race = new Race(time);
            var possibleWays = race.PossibleWays();
            var waysBeatingRecord = possibleWays.Where(x => x.Item2 > bestDistance);
            return waysBeatingRecord;
        }

    }
    private class Race(long raceTime) 
    {
        public List<(long,long)> PossibleWays() 
        {
            var result = new List<(long,long)>();
            for (var timeLoading = 0; timeLoading <= raceTime; timeLoading++)
            {
                var timeRacing = raceTime - timeLoading;
                var speed = timeLoading;
                var distance = timeRacing * speed;
                result.Add((timeLoading, distance));
            }
            return result;
        }
    }
}

