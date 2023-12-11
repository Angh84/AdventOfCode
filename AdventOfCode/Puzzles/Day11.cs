using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day11: PuzzleBase
{
    private readonly char[,] galaxyMap;
    public Day11(IEnumerable<string> puzzleInput) : base(puzzleInput)
    {
        var height = Input.Count();
        var width = Input.First().Length;
        galaxyMap = new char[width, height];
        var y = 0;
        foreach (var line in Input)
        {
            var x = 0;
            foreach (var c in line)
            {
                galaxyMap[x, y] = c;
                x++;
            }

            y++;
        }
    }
    public override string FirstSolution()
    {
        var distance = CalculateDistance(2);
        return distance;
    }

    public override string SecondSolution()
    {
        var distance = CalculateDistance(1000000);
        return distance;
    }

    private string CalculateDistance(long expansion)
    {
        List<int> lengths = [];
        for (var r = 0; r< galaxyMap.Rank; r++)  lengths.Add(galaxyMap.GetLength(r));
        List<int> cols = [];
        List<int> rows = [];
        List<(int, int)> galaxies = [];
        for (var x = 0; x < lengths[0]; x++)
        {
            for (var y = 0; y < lengths[1]; y++)
            {
                if (galaxyMap[x, y] != '#') continue;
                galaxies.Add((x,y));
                cols.Add(x);
                rows.Add(y);
            }
        }

        cols = cols.Distinct().ToList();
        rows = rows.Distinct().ToList();
        var allCols = Enumerable.Range(0, lengths[0]).ToList();
        var allRows = Enumerable.Range(0, lengths[1]).ToList();
        var emptyCols = allCols.Where(i => !cols.Contains(i)).ToList();
        var emptyRows = allRows.Where(i => !rows.Contains(i)).ToList();

        long sum = 0;
        for (var gi1 = 0; gi1 < galaxies.Count; gi1++)
        {
            for (var gi2 = gi1 + 1; gi2 < galaxies.Count; gi2++)
            {
                var g1 = galaxies[gi1];
                var g2 = galaxies[gi2];
                var baseDistance = int.Abs(g2.Item1 - g1.Item1) + int.Abs(g2.Item2 - g1.Item2);

                var emptyColsPassed =
                    emptyCols.Count(i => i > int.Min(g1.Item1, g2.Item1) && i < int.Max(g1.Item1, g2.Item1));
                var emptyRowsPassed =
                    emptyRows.Count(i => i > int.Min(g1.Item2, g2.Item2) && i < int.Max(g1.Item2, g2.Item2));
                var totalDistance = baseDistance + (emptyColsPassed + emptyRowsPassed)*(expansion-1);
                sum += totalDistance;
            }
        }
        return sum.ToString();
    }
}