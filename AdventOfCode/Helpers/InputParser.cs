namespace AdventOfCode.Helpers;

internal static class InputParser
{
    internal static List<string> getInput(string path)
    {
        return new(File.ReadAllLines(path));
        
    }
}