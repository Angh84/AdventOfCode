using System.Drawing;
using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day10 : PuzzleBase
{
    //6697
    //423
    private readonly Node start;
    private readonly int mapWidth;
    private readonly int mapHeight;
    public Day10(IEnumerable<string> puzzleInput) : base(puzzleInput)
    {
        mapWidth = Input.First().Length;
        mapHeight = Input.Count();
        start = CreateMap();
    }

    public override string FirstSolution()
    {
        List<Node> visited = [];
        List<Node> toVisit = [ start ];
        while (toVisit.Count != 0)
        {
            var current = toVisit.OrderBy(n => n.Distance).First();
            toVisit.Remove(current);
            if (visited.Contains(current)) continue;
            if (current.Type == NodeTypes.Start) current.Distance = 0;
            else current.Distance = current.Previous!.Distance + 1;
            visited.Add(current);
            if (current.Top != null)
            {
                current.Top.Previous = current;
                toVisit.Add(current.Top);
            }
            if (current.Bottom != null)
            {
                current.Bottom.Previous = current;
                toVisit.Add(current.Bottom);
            }
            if (current.Left != null)
            {
                current.Left.Previous = current;
                toVisit.Add(current.Left);
            }
            if (current.Right != null)
            {
                current.Right.Previous = current;
                toVisit.Add(current.Right);
            }
        }
        return visited.Max(n => n.Distance).ToString();
    }

    public override string SecondSolution()
    {
        var map = new char[mapWidth, mapHeight];
        for (var y=0; y<mapHeight; y++)
        {
            for (var x=0; x<mapWidth; x++)
            {
                map[x, y] = '.';
            }
        }

        List<Node> nodesToCheck = [start];
        List<Node> visited = [];
        while (nodesToCheck.Count != 0)
        {
            var current = nodesToCheck.First();
            nodesToCheck.Remove(current);
            if (visited.Contains(current)) continue;
            visited.Add(current);
            if (current.Top!=null) nodesToCheck.Add(current.Top);
            if (current.Bottom!=null) nodesToCheck.Add(current.Bottom);
            if (current.Left!=null) nodesToCheck.Add(current.Left);
            if (current.Right!=null) nodesToCheck.Add(current.Right);

            var charToUse = current switch
            {
                { Bottom: not null, Right: not null } => '\u2554',
                { Bottom: not null, Left: not null } => '\u2557',
                { Top: not null, Right: not null } => '\u255A',
                { Top: not null, Left: not null } => '\u255D',
                { Top: not null, Bottom: not null } => '\u2551',
                { Left: not null, Right: not null } => '\u2550',
                _ => ' '
            };

            map[current.Coordinates.X, current.Coordinates.Y] = charToUse;  
        }

        const char bt = '\u2551';
        const char rt = '\u255A';
        const char lt = '\u255D';
        for (var y = 0; y < mapHeight; y++)
        {
            var inside = false;
            for (var x=0; x<mapWidth; x++)
            {
                var current = map[x, y];

                switch (current)
                {
                    case bt or rt or lt:
                        inside = !inside;
                        break;
                    case '.' when !inside:
                        map[x,y] = ' ';
                        break;
                }
            }
        }
        for (var y = 0; y < mapHeight; y++)
        {
            for (var x=0; x<mapWidth; x++)
            {
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }

        return map.Cast<char>().Count(c => c == '.').ToString();
    }
    private Node CreateMap()
    {
        var x = 0;
        var y = 0;
        List<Node> nodes = [];
        foreach (var line in Input)
        {
            foreach (var c in line)
            {
                nodes.Add(new(c, x, y, mapHeight, mapWidth));
                x++;
            }
            y++;
            x = 0;
        }
        
        nodes.ForEach(node =>
        {
            node.SetRelations(nodes);
        });
        return nodes.First(n => n.Type == NodeTypes.Start);
    }
    private class Node
    {
        private readonly char value;
        private readonly int x;
        private readonly int y;
        private readonly int mapHeight;
        private readonly int mapWidth;
        public NodeTypes Type { get; }
        public Node? Previous { get; set; }
        public int Distance { get; set; }
        public Node? Top { get; private set; }
        public Node? Bottom { get; private set; }
        public Node? Left { get; private set; }
        public Node? Right { get; private set; }
        public Point Coordinates => new(x, y);
        public Node(char value, int x, int y, int mapHeight, int mapWidth)
        {
            this.value = value;
            this.x = x;
            this.y = y;
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            Type = value switch
            {
                '.' => NodeTypes.Ground,
                '|' => NodeTypes.Pipe,
                '-' => NodeTypes.Pipe,
                'L' => NodeTypes.Pipe,
                'F' => NodeTypes.Pipe,
                'J' => NodeTypes.Pipe,
                '7' => NodeTypes.Pipe,
                'S' => NodeTypes.Start,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Boom something went wrong")
            };
        }
        public void SetRelations(List<Node> nodes)
        {
            switch (value)
            {
                case '.':
                    break;
                case '|':
                    SetTop(nodes);
                    SetBottom(nodes);
                    break;
                case '-':
                    SetLeft(nodes);
                    SetRight(nodes);
                    break;
                case 'L':
                    SetTop(nodes);
                    SetRight(nodes);    
                    break;
                case 'F':
                    SetBottom(nodes);
                    SetRight(nodes);
                    break;
                case 'J':
                    SetTop(nodes);
                    SetLeft(nodes);
                    break;
                case '7':
                    SetBottom(nodes);
                    SetLeft(nodes);
                    break;
                case 'S':
                    SetTop(nodes);
                    SetRight(nodes);
                    SetBottom(nodes);
                    SetLeft(nodes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SetTop(List<Node> nodes)
        {
            if(y == 0) return;
            var top = nodes.FirstOrDefault(n => n.x == x && n.y == y - 1);
            if (top == null) return;
            switch(top.value)
            {
                case '|':
                case 'F':
                case '7':
                case 'S':
                    Top = top;
                    break;
            }
        }
        private void SetBottom(IEnumerable<Node> nodes)
        {
            if(y == mapHeight-1) return;
            var bottom = nodes.FirstOrDefault(n => n.x == x && n.y == y + 1);
            if (bottom == null) return;
            switch(bottom.value)
            {
                case '|':
                case 'L':
                case 'J':
                case 'S':
                    Bottom = bottom;
                    break;
            }
        }
        private void SetLeft(List<Node> nodes)
        {
            if(x == 0) return;
            var left = nodes.FirstOrDefault(n => n.x == x - 1 && n.y == y);
            if (left == null) return;
            switch(left.value)
            {
                case '-':
                case 'L':
                case 'F':
                case 'S':
                    Left = left;
                    break;
            }
        }
        private void SetRight(List<Node> nodes)
        {
            if(x == mapWidth-1) return;
            var right = nodes.FirstOrDefault(n => n.x == x + 1 && n.y == y);
            if (right == null) return;
            switch(right.value)
            {
                case '-':
                case 'J':
                case '7':
                case 'S':
                    Right = right;
                    break;
            }
        }
    }
    
}

public enum NodeTypes
{
    Ground,
    Pipe,
    Start
}