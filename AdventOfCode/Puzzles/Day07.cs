using AdventOfCode.Lib;

namespace AdventOfCode.Puzzles;

public class Day07(IEnumerable<string> puzzleInput) : PuzzleBase(puzzleInput)
{
    public override string FirstSolution()
    {
        var hands = ParseHands();
        hands.Sort();
        var sum = hands.Select((t, i) => t.Bid * (i + 1)).Sum();
        return sum.ToString();
    }
    public override string SecondSolution()
    {
        var hands = ParseHandsWithJokers();
        hands.Sort();
        var sum = hands.Select((t, i) => t.Bid * (i + 1)).Sum();
        return sum.ToString();
    }

    private List<Hand> ParseHands() 
    {
        var result = new List<Hand>();
        Input.ToList().ForEach(x => result.Add(ParseHand(x)));
        return result;
    }
    private List<HandWithJoker> ParseHandsWithJokers()
    {
        var result = new List<HandWithJoker>();
        Input.ToList().ForEach(x => result.Add(ParseHandWithJoker(x)));
        return result;
    }
    private Hand ParseHand(string handString)
    {
        var parts = handString.Split(' ');
        var hand = new Hand(int.Parse(parts[1]));
        var cardString = parts[0];
        foreach (var c in cardString)
        {
            switch (c)
            {
                case 'T':
                    hand.Cards.Add(10);
                    break;
                case 'J':
                    hand.Cards.Add(11);
                    break;
                case 'Q':
                    hand.Cards.Add(12);
                    break;
                case 'K':
                    hand.Cards.Add(13);
                    break;
                case 'A':
                    hand.Cards.Add(14);
                    break;
                default:
                    hand.Cards.Add(int.Parse(c.ToString()));
                    break;
            }
        }
        return hand;
    }
    private HandWithJoker ParseHandWithJoker(string handString)
    {
        var parts = handString.Split(' ');
        var hand = new HandWithJoker(int.Parse(parts[1]));
        var cardString = parts[0];
        foreach (var c in cardString)
        {
            switch (c)
            {
                case 'T':
                    hand.Cards.Add(10);
                    break;
                case 'J':
                    hand.Cards.Add(1);
                    break;
                case 'Q':
                    hand.Cards.Add(12);
                    break;
                case 'K':
                    hand.Cards.Add(13);
                    break;
                case 'A':
                    hand.Cards.Add(14);
                    break;
                default:
                    hand.Cards.Add(int.Parse(c.ToString()));
                    break;
            }
        }
        return hand;
    }
    private class Hand(int bid) : IComparable<Hand>
    {
        public List<int> Cards { get; } = [];
        public int Bid { get; } = bid;

        public int CompareTo(Hand? other)
        {
            if (other == null) return 1;
            if (Type() > other.Type()) return 1;
            if (Type() < other.Type()) return -1;
            
            for (var i = 0; i < Cards.Count; i++)
            {
                if (Cards[i] > other.Cards[i]) return 1;
                if (Cards[i] < other.Cards[i]) return -1;
            }
            return 0;
        }

        public HandType Type ()
        {
            return Cards.Distinct().Count() switch
            {
                1 => HandType.FiveOfAKind,
                2 => Cards.GroupBy(x => x).Any(x => x.Count() == 4) 
                    ? HandType.FourOfAKind 
                    : HandType.FullHouse,
                3 => Cards.GroupBy(x => x).Any(x => x.Count() == 3) 
                    ? HandType.ThreeOfAKind 
                    : HandType.TwoPair,
                4 => HandType.Pair,
                _ => HandType.HighCard
            };
        }
    }
    private class HandWithJoker(int bid): IComparable<HandWithJoker> 
    {
        public List<int> Cards { get; } = [];
        public int Bid { get; } = bid;
        public int CompareTo(HandWithJoker? other)
        {
            if (other == null) return 1;
            if (Type() > other.Type())return 1;
            if (Type() < other.Type())return -1;
            for (var i = 0; i < Cards.Count; i++)
            {
                if (Cards[i] > other.Cards[i]) return 1;
                if (Cards[i] < other.Cards[i]) return -1;
            }
            return 0;
        }

        public HandType Type()
        {
            var jokerCount = Cards.Count(card => card == 1);
            var groups = Cards.Where(card => card != 1)
                                  .GroupBy(card => card)
                                  .OrderByDescending(group => group.Count()).ToList();

            if (groups.Count == 0) 
            {
                return HandType.FiveOfAKind;
            }

            var uniqueCardCounts = groups.Select(group => group.Count() + jokerCount).ToList();
            jokerCount -= uniqueCardCounts[0] - groups[0].Count();

            return uniqueCardCounts.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 when uniqueCardCounts.Contains(4) || uniqueCardCounts.Contains(4 - jokerCount) =>
                    HandType.FourOfAKind,
                2 => HandType.FullHouse,
                3 when uniqueCardCounts.Contains(3) || uniqueCardCounts.Contains(3 - jokerCount) => HandType
                    .ThreeOfAKind,
                3 => HandType.TwoPair,
                4 => HandType.Pair,
                _ => HandType.HighCard
            };
        }
    }
    private enum HandType
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        Pair = 2,
        HighCard = 1,
    }
}

