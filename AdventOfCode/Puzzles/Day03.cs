using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day03 : PuzzleBase
{
    private readonly List<EnginePart> engineParts;
    private readonly List<PartNumber> partNumbers;

    public Day03(IEnumerable<string> puzzleInput) : base(puzzleInput)
    {
        engineParts = FindEngineParts();
        partNumbers = FindPartNumbers();
    }
    private List<EnginePart> FindEngineParts()
    {
        List<EnginePart> result = new();
        for (var y = 0; y < Input.Count(); y++)
        {
            var row = Input.ElementAt(y);
            
            for (var x = 0; x < row.Length; x++)
            {
                var charToCheck = row[x];
                if (charToCheck == '.' || char.IsDigit(charToCheck)) continue;
                result.Add(new(x, y, charToCheck));
            }
        }
        return result;
    }
    private List<PartNumber> FindPartNumbers()
    {
        List<PartNumber> result = new();

        for (var y = 0; y < Input.Count(); y++)
        {
            var row = Input.ElementAt(y);
            var currentNumber = string.Empty;
            var startPos = 0;
            for (var x = 0; x < row.Length; x++)
            {
                var charToCheck = row[x];
                if (char.IsDigit(charToCheck))
                {
                    if (currentNumber == string.Empty) startPos = x;
                    currentNumber += charToCheck;
                }
                else
                {
                    if (currentNumber == string.Empty) continue;
                    result.Add(new(int.Parse(currentNumber), startPos, y, currentNumber.Length));
                    currentNumber = string.Empty;
                }
            }

            if (currentNumber != string.Empty)
                result.Add(new(int.Parse(currentNumber), startPos, y, currentNumber.Length));
        }

        return result;
    }
    public override string FirstSolution()
    {
        engineParts.ForEach(HandlePartNumbers);

        return partNumbers
            .Where(p => p.Added)
            .Sum(p => p.Number)
            .ToString();
    }
    public override string SecondSolution()
    {
        var possibleGears = engineParts.Where(p => p.EngineChar == '*');
        var gearRatios = new List<int>();

        foreach (var gear in possibleGears)
        {
            var surroundingParts = GetSurroundingParts(gear);
            if (surroundingParts.Count == 2)
                gearRatios.Add(surroundingParts[0].Number * surroundingParts[1].Number);
        }

        return gearRatios.Sum().ToString();
    }
    private void HandlePartNumbers(EnginePart enginePart)
    {
        partNumbers
            .Where(p =>!p.Added && IsInProximity(p, enginePart))
            .ToList()
            .ForEach(p=>p.Added = true);
    }

    private bool IsInProximity(PartNumber partNumber, EnginePart enginePart)
    {
        var yAxisSame = enginePart.YPos == partNumber.YPos;
        var isOneAbove = enginePart.YPos >= 1 && enginePart.YPos - 1 == partNumber.YPos;
        var isOneBelow = enginePart.YPos < Input.Count() && enginePart.YPos + 1 == partNumber.YPos;
        
        var xNearSameY = partNumber.XStart == enginePart.XPos + 1 || partNumber.XEnd == enginePart.XPos - 1;
        var xNearbyDiffYAxis = IsXInProximity(partNumber, enginePart);
        return (yAxisSame && xNearSameY) || ((isOneAbove || isOneBelow) && xNearbyDiffYAxis);
    }
    private bool IsXInProximity(PartNumber p, EnginePart e)
    {
        return (p.XStart >= e.XPos - 1 && p.XStart <= e.XPos + 1) ||
               (p.XEnd >= e.XPos - 1 && p.XEnd <= e.XPos + 1);
    }
    private List<PartNumber> GetSurroundingParts(EnginePart gear)
    {
        var partBefore = partNumbers.FirstOrDefault(p => p.YPos == gear.YPos && p.XEnd == gear.XPos - 1);
        var partAfter = partNumbers.FirstOrDefault(p => p.YPos == gear.YPos && p.XStart == gear.XPos + 1);
        var partsAbove = GetHorizontalParts(gear, -1);
        var partsBelow = GetHorizontalParts(gear, 1);

        var surroundingParts = partsAbove.Concat(partsBelow).ToList();
        if (partBefore != null)
            surroundingParts.Add(partBefore);
        if (partAfter != null)
            surroundingParts.Add(partAfter);

        return surroundingParts;
    }
    private IEnumerable<PartNumber> GetHorizontalParts(EnginePart gear, int offset)
    {
        return partNumbers.Where(p =>
            p.YPos == gear.YPos + offset && ((p.XStart >= gear.XPos - 1 && p.XStart <= gear.XPos + 1) ||
                                             (p.XEnd >= gear.XPos - 1 && p.XEnd <= gear.XPos + 1)));
    }
}
public class PartNumber(int number, int xStart, int yStart, int length)
{
    public readonly int Number = number;
    public readonly int XStart = xStart;
    public readonly int YPos = yStart;
    public bool Added;
    public int XEnd => XStart + length - 1;
}
public class EnginePart(int x, int y, char engineChar)
{
    public readonly char EngineChar = engineChar;
    public readonly int XPos = x;
    public readonly int YPos = y;
}