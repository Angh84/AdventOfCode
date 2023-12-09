using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day08: PuzzleBase
{
    private readonly List<char> instructions = [];
    private readonly Node? root;
    private readonly List<Node> roots;
    public Day08(IEnumerable<string> puzzleInput): base(puzzleInput)
    {
        var parseInstructions = true;
        List<string> mapStrings = [];
        foreach (var line in Input)
        {
            if (string.IsNullOrEmpty(line))
            {
                parseInstructions = false;
                continue;
            }
            if (parseInstructions)
            {
                foreach (var instruction in line)
                {
                    instructions.Add(instruction);
                }
            }
            else 
            {
                mapStrings.Add(line);
            } 
        }  
        root = ParseMap(mapStrings);
        roots = ParseMapSecond(mapStrings);
    }
    public override string FirstSolution()
    {
        var current = root;
        var numberOfSteps = 0;
        while (current!.Value != "ZZZ") 
        {
            var currentInstruction = instructions[numberOfSteps % (instructions.Count)];
            current = currentInstruction switch
            {
                'L' => current.Left,
                'R' => current.Right,
                _ => current
            };
            numberOfSteps++;
        }
           
        return numberOfSteps.ToString();
    }

    public override string SecondSolution()
    {
        var solutions = new List<long>();
        roots.ForEach(n => 
        {
            var numberOfSteps = 0;
            while (!n.Value.EndsWith('Z'))
            {
                var currentInstruction = instructions[numberOfSteps % (instructions.Count)];
                n = (currentInstruction switch
                {
                    'L' => n.Left,
                    'R' => n.Right,
                    _ => n
                })!;
                numberOfSteps++;
            }
            solutions.Add(numberOfSteps);
        });
        var lcm = LeastCommonMultiple(solutions);
        return lcm.ToString();
    }
    private long LeastCommonMultiple(List<long> numbers) 
    {
        var lcm = numbers[0];
        for (var i = 1; i < numbers.Count; i++)
        {
            lcm = LeastCommonMultiple(lcm, numbers[i]);
        }
        return lcm;
    }
    private long LeastCommonMultiple(long a, long b)
    {
        return a / GreatestCommonDivisor(a, b) * b;
    }
    private long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    private Node ParseMap(List<string> mapStrings) 
    {
        List<Node> nodes = [];
        Dictionary<string,(string,string)> mapEntries = new();
        mapStrings.ForEach(i => 
        {
            var value = i.Split('=')[0].Trim();
            var left = i.Split('(')[1].Trim().Replace(")", "").Split(',')[0].Trim();
            var right = i.Split('(')[1].Trim().Replace(")", "").Split(',')[1].Trim();
            mapEntries.Add(value, (left, right));
            nodes.Add(new(value));
        });
        nodes.ForEach(n =>
        {
            var entry = mapEntries[n.Value];
            n.Left = nodes.FirstOrDefault(i => i.Value == entry.Item1);
            n.Right = nodes.FirstOrDefault(i => i.Value == entry.Item2);
        });
        return nodes.First(node => node.Value == "AAA");
    }
    private List<Node> ParseMapSecond(List<string> mapStrings)
    {
        List<Node> nodes = [];
        Dictionary<string, (string, string)> mapEntries = new();
        mapStrings.ForEach(i =>
        {
            var value = i.Split('=')[0].Trim();
            var left = i.Split('(')[1].Trim().Replace(")", "").Split(',')[0].Trim();
            var right = i.Split('(')[1].Trim().Replace(")", "").Split(',')[1].Trim();
            mapEntries.Add(value, (left, right));
            nodes.Add(new(value));
        });
        nodes.ForEach(n =>
        {
            var entry = mapEntries[n.Value];
            n.Left = nodes.FirstOrDefault(i => i.Value == entry.Item1);
            n.Right = nodes.FirstOrDefault(i => i.Value == entry.Item2);
        });
        return nodes.Where(n => n.Value.EndsWith('A')).ToList();

    }
    public class Node(string value)
    {
        public string Value { get; } = value;
        public Node? Left { get; set; }
        public Node? Right { get; set; }
    }
}    
