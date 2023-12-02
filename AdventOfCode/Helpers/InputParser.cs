namespace AdventOfCode.Helpers;

internal static class InputParser
{
    internal static List<string> GetInput(string path)
    {
        return new(File.ReadAllLines(path));
        
    }
}