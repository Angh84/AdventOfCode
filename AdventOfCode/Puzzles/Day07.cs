using AdventOfCode.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles
{
    public class Day07(IEnumerable<string> puzzleInput) : PuzzleBase(puzzleInput)
    {
        public override string FirstSolution()
        {
            var hands = ParseHands();
            hands.Sort();
            var sum = 0;
            for (var i=0; i< hands.Count; i++)
            {
                sum += hands[i].Bid * (i + 1);
            }
            return sum.ToString();
        }

        public override string SecondSolution()
        {
            var hands = ParseHandsWithJokers();
            hands.Sort();
            var sum = 0;
            for (var i = 0; i < hands.Count; i++)
            {
                sum += hands[i].Bid * (i + 1);
            }
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
            foreach (char c in cardString)
            {
                if (c == 'T')
                {
                    hand.Cards.Add(10);
                }
                else if (c == 'J')
                {
                    hand.Cards.Add(11);
                }
                else if (c == 'Q')
                {
                    hand.Cards.Add(12);
                }
                else if (c == 'K')
                {
                    hand.Cards.Add(13);
                }
                else if (c == 'A')
                {
                    hand.Cards.Add(14);
                }
                else
                {
                    hand.Cards.Add(int.Parse(c.ToString()));
                }
            }
            return hand;
        }
        private HandWithJoker ParseHandWithJoker(string handString)
        {
            var parts = handString.Split(' ');
            var hand = new HandWithJoker(int.Parse(parts[1]));
            var cardString = parts[0];
            foreach (char c in cardString)
            {
                if (c == 'T')
                {
                    hand.Cards.Add(10);
                }
                else if (c == 'J')
                {
                    hand.Cards.Add(1);
                }
                else if (c == 'Q')
                {
                    hand.Cards.Add(12);
                }
                else if (c == 'K')
                {
                    hand.Cards.Add(13);
                }
                else if (c == 'A')
                {
                    hand.Cards.Add(14);
                }
                else
                {
                    hand.Cards.Add(int.Parse(c.ToString()));
                }
            }
            return hand;
        }
        private class Hand(int bid) : IComparable<Hand>
        {
            public List<int> Cards { get; set; } = [];
            public int Bid { get; } = bid;

            public int CompareTo(Hand? other)
            {
                if (other == null)
                {
                    return 1;
                }
                else
                {
                    if (this.Type() > other.Type())
                    {
                        return 1;
                    }
                    else if (this.Type() < other.Type())
                    {
                        return -1;
                    }
                    else
                    {
                        for (var i = 0; i < this.Cards.Count; i++)
                        {
                            if (this.Cards[i] > other.Cards[i])
                            {
                                return 1;
                            }
                            else if (this.Cards[i] < other.Cards[i])
                            {
                                return -1;
                            }
                        }
                    }
                }
                return 0;
            }

            public HandType Type ()
            {
                if (Cards.Distinct().Count() == 1)
                {
                    return HandType.FiveOfAKind;
                }
                else if (Cards.Distinct().Count() == 2)
                {
                    if (Cards.GroupBy(x => x).Any(x => x.Count() == 4))
                    {
                        return HandType.FourOfAKind;
                    }
                    else
                    {
                        return HandType.FullHouse;
                    }
                }
                else if (Cards.Distinct().Count() == 3)
                {
                    if (Cards.GroupBy(x => x).Any(x => x.Count() == 3))
                    {
                        return HandType.ThreeOfAKind;
                    }
                    else
                    {
                        return HandType.TwoPair;
                    }
                }
                else if (Cards.Distinct().Count() == 4)
                {
                    return HandType.Pair;
                }
                else
                {
                    return HandType.HighCard;
                }
            }
        }
        private class HandWithJoker(int bid): IComparable<HandWithJoker> 
        {
            public List<int> Cards { get; set; } = [];
            public int Bid { get; } = bid;
            public int CompareTo(HandWithJoker? other)
            {
                if (other == null)
                {
                    return 1;
                }
                else
                {
                    if (this.Type() > other.Type())
                    {
                        return 1;
                    }
                    else if (this.Type() < other.Type())
                    {
                        return -1;
                    }
                    else
                    {
                        for (var i = 0; i < this.Cards.Count; i++)
                        {
                            if (this.Cards[i] > other.Cards[i])
                            {
                                return 1;
                            }
                            else if (this.Cards[i] < other.Cards[i])
                            {
                                return -1;
                            }
                        }
                    }
                }
                return 0;
            }

            public HandType Type()
            {
                int jokerCount = Cards.Count(card => card == 1);
                var groups = Cards.Where(card => card != 1)
                                      .GroupBy(card => card)
                                      .OrderByDescending(group => group.Count()).ToList();

                if (groups.Count == 0) // we got 5 jokers
                {
                    return HandType.FiveOfAKind;
                }

                var uniqueCardCounts = groups.Select(group => group.Count() + jokerCount).ToList();
                jokerCount -= (uniqueCardCounts[0] - groups[0].Count());

                if (uniqueCardCounts.Count == 1)
                {
                    return HandType.FiveOfAKind;
                }

                if (uniqueCardCounts.Count == 2)
                {
                    if (uniqueCardCounts.Contains(4) || uniqueCardCounts.Contains(4 - jokerCount))
                    {
                        return HandType.FourOfAKind;
                    }
                    else
                    {
                        return HandType.FullHouse;
                    }
                }

                if (uniqueCardCounts.Count == 3)
                {
                    if (uniqueCardCounts.Contains(3) || uniqueCardCounts.Contains(3 - jokerCount))
                    {
                        return HandType.ThreeOfAKind;
                    }
                    else
                    {
                        return HandType.TwoPair;
                    }
                }

                if (uniqueCardCounts.Count == 4)
                {
                    return HandType.Pair;
                }

                return HandType.HighCard;
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
    
}
