using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;
public class Day09 : PuzzleBase
{
    private readonly List<DataSet> dataSets = [];
    public Day09(IEnumerable<string> puzzleInput) : base(puzzleInput)
    {
        foreach (var row in Input)
        {
            dataSets.Add(ParseRow(row));
        }
        foreach (var dataSet in dataSets)
        {
            dataSet.GenerateSequences();
        }
    }
    public override string FirstSolution()
    {
        var sumOfGeneratedSequences = dataSets.Sum(ds => ds.GetNextNumber());
        return sumOfGeneratedSequences.ToString();
    }

    public override string SecondSolution()
    {
        var sumOfGeneratedSequences = dataSets.Sum(ds => ds.GetPreviousNumber());
        return sumOfGeneratedSequences.ToString();
    }
    private DataSet ParseRow(string row)
    {
        var dataSet = new DataSet();
        dataSet.Sequences.Add(row.Split(' ').Select(int.Parse).ToList());
        return dataSet;
    }
    private class DataSet
    {
        public List<List<int>> Sequences { get;  } = [];

        public void GenerateSequences()
        {
            while (GenerateNextSequence())
            {
            }
        }   
        private bool GenerateNextSequence() 
        {
            var currentSequence = Sequences.Last();
            if (currentSequence.All(n => n == 0)) return false;
            
            var nextSequence = new List<int>();

            for (var i = 0; i < currentSequence.Count-1; i++)
            {
                var current = currentSequence[i];
                var next = currentSequence[i + 1];
                var diff = next - current;
                nextSequence.Add(diff);
            }
            Sequences.Add(nextSequence);
            return true;
        }

        public int GetNextNumber()
        {
            Sequences.Last().Add(0);
            for (var i = Sequences.Count-2; i >= 0; i--)
            {
                var currentSequence = Sequences[i];
                var previousSequence = Sequences[i + 1];
                var lastDiff = previousSequence[^1];
                var nextNumber = currentSequence[^1] + lastDiff;
                currentSequence.Add(nextNumber);
            }
            return Sequences.First()[^1];
        }
        public int GetPreviousNumber()
        {
            Sequences.Last().Insert(0, 0);
            for (var i = Sequences.Count - 2; i >= 0; i--)
            {
                var currentSequence = Sequences[i];
                var previousSequence = Sequences[i + 1];
                var lastDiff = previousSequence[0];
                var nextNumber = currentSequence[0] - lastDiff;
                currentSequence.Insert(0,nextNumber);
            }
            return Sequences.First()[0];
        }
    }
}
