namespace AdventOfCode.Lib;

public abstract class PuzzleBase(IEnumerable<string> puzzleInput) : IPuzzle
{
    protected readonly IEnumerable<string> Input = puzzleInput;
    public abstract string FirstSolution();
    public abstract string SecondSolution();
}