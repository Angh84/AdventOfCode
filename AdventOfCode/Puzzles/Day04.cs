using System.Globalization;
using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day04(IEnumerable<string> puzzleInput) : PuzzleBase(puzzleInput)
{
    public override string FirstSolution()
    {
        var cards = ParseCards();
        var sum = 0;
        foreach (var card in cards)
        {
            var points = card.GetPoints();
            if (points.Trim() == "") continue;
            sum += int.Parse(card.GetPoints());
        }

        return sum.ToString();
    }

    public override string SecondSolution()
    {
        var cards = ParseCards();
        ProcessCards(cards);
        var sum = cards.Sum(i => i.Copies);
        return sum.ToString();
    }

    private void ProcessCards(IEnumerable<Card> cards)
    {
        foreach (var card in cards) ProcessCard(card, cards);
    }

    private void ProcessCard(Card card, IEnumerable<Card> cards)
    {
        var matches = card.GetNumberOfMatches();
        var currentCopies = card.Copies;
        if (matches == 0) return;
        var firstId = card.Id + 1;
        var lastId = card.Id + matches;
        var cardsToIncrease = cards.Where(c => c.Id >= firstId && c.Id <= lastId);
        foreach (var cardToIncrease in cardsToIncrease) cardToIncrease.AddCopies(currentCopies);
    }


    private IEnumerable<Card> ParseCards()
    {
        var cards = new List<Card>();

        foreach (var line in Input)
        {
            var lineParts = line.Split(":");
            var idPart = lineParts[0].Trim();
            var temp = idPart.Split(' ');
            var lastPart = temp[^1];
            var id = int.Parse(lastPart);
            var numbersPart = lineParts[1].Trim();
            var lists = numbersPart.Split('|');
            var winningNumbersPart = lists[0].Trim();
            var myNumbersPart = lists[1].Trim();
            var winningList = winningNumbersPart.Split(' ');
            var myNumbersList = myNumbersPart.Split(' ');
            var wList = new List<int>();
            winningList.ToList().ForEach(i =>
            {
                var numStr = i.Trim();
                if (numStr == "") return;
                var num = int.Parse(numStr);
                wList.Add(num);
            });
            var cList = new List<int>();
            myNumbersList.ToList().ForEach(i =>
            {
                var numStr = i.Trim();
                if (numStr == "") return;
                var num = int.Parse(numStr);
                cList.Add(num);
            });
            var card = new Card(id, cList, wList);
            cards.Add(card);
        }

        return cards;
    }
}

public class Card(int id, IEnumerable<int> numbers, IEnumerable<int> winningNumbers)
{
    public int Copies = 1;
    public readonly int Id = id;

    public string GetPoints()
    {
        var numberOfMatches = GetNumberOfMatches();
        var points = numberOfMatches > 0 ? 1 * Math.Pow(2, numberOfMatches - 1) : 0;
        return points.ToString(CultureInfo.InvariantCulture);
    }

    public void AddCopies(int amount)
    {
        Copies += amount;
    }

    public int GetNumberOfMatches()
    {
        var numberOfMatches = numbers.Intersect(winningNumbers).Count();
        return numberOfMatches;
    }
}