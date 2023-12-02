using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day01(IEnumerable<string> puzzleInput) : PuzzleBase(puzzleInput)
{
    public override string FirstSolution()
    {
        var sum = 0;
        foreach (var item in Input)
        {
            var numbers = new List<int>();
            foreach (var character in item)
            {
                if(int.TryParse(character.ToString(), out var number))
                {
                    numbers.Add(number);
                }
            }
   
            var digitString = numbers[0].ToString() ;
            if(numbers.Count >1)
                digitString += numbers[^1].ToString();
            else
                digitString += numbers[0].ToString();
            sum += int.Parse(digitString);
        }
        return sum.ToString();
    }

    public override string SecondSolution()
    {
        var sum = Input.Sum(GetSumOfDigits);
        return sum.ToString();
    }

    private int GetSumOfDigits(string row)
    {
        var inputDictionary = new Dictionary<string, int>
        {
            { "zero", 0 },
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
            { "four", 4 },
            { "five", 5 },
            { "six", 6 },
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 }
        };
        List<int> digits = new();

        for (var i = 0; i < row.Length; i++)
        {
            if (int.TryParse(row[i].ToString(), out var number))
            {
                digits.Add(number);
            }
            else 
            {
                var digit = inputDictionary.FirstOrDefault(entry 
                    => row[i..].StartsWith(entry.Key));

                if (!digit.Equals(default(KeyValuePair<string, int>)))
                {
                    digits.Add(digit.Value);
                }
            }
        }
        var digitString = digits[0].ToString();
        if (digits.Count > 1)
            digitString += digits[^1].ToString();
        else
            digitString += digits[0].ToString();
        return int.Parse(digitString);
            
    }
        
}