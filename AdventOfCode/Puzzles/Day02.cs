using AdventOfCode.Helpers;

namespace AdventOfCode.Puzzles
{
    internal class Day02 : IDay
    {
        private readonly List<string> input;
        private readonly IEnumerable<Game> games;
        public Day02(string path)
        {
            var fileName = GetType().Name + ".txt";
            var inputFile = Path.Combine(path, fileName);
            input = InputParser.GetInput(inputFile);
            games = input.Select(ParseGame);
        }

        public string SolutionOne()
        {
            return games.Where(game => game.IsValid()).Sum(game => game.Id).ToString();
        }

        public string SolutionTwo()
        {
            return games.Select(game => game.Power()).Sum().ToString();
        }

        private Game ParseGame(string row)
        {
            var split = row.Split(':');
            var id = split[0].Split(' ');
            var newGame = new Game(int.Parse(id[1]));
            var rounds = split[1].Split(';');
            foreach (var round in rounds)
            {
                var red = 0;
                var blue = 0;
                var green = 0;
                
                var ballCombos = round.Split(',');
                foreach (var bc in ballCombos)
                {
                    var parts = bc.Trim().Split(' ');
                    var amount = int.Parse(parts[0]);
                    var color = parts[1];
                    switch (color)
                    {
                        case "red":
                            red = amount;
                            break;
                        case "blue":
                            blue = amount;
                            break;
                        case "green":
                            green = amount;
                            break;
                    }
                }
                newGame.AddRound(red, blue, green);
            }
            return newGame;
        }
    }

    internal class Game(int id)
    {
        public readonly int Id = id;
        private readonly List<Round> rounds = new();
        public bool IsValid() => rounds.All(r => r.IsValidRound(12,14,13));
        public void AddRound(int red, int blue, int green) => rounds.Add(new(red, blue, green));

        public int Power()
        {
            var r = 0;
            var g = 0;
            var b = 0;
            
            foreach (var round in rounds)
            {
                if (round.Red > r) r = round.Red;
                if (round.Blue > b) b = round.Blue;
                if (round.Green > g) g = round.Green;
            }
            return r * b * g;
        }
    }

    internal class Round(int red, int blue, int green)
    {
        public readonly int Red = red;
        public readonly int Blue = blue;
        public readonly int Green = green;
        
        public bool IsValidRound(int red,int blue, int green) => Red <= red && Blue <= blue && Green <= green;
    }
}
