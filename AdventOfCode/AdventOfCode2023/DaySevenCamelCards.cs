using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DaySevenCamelCards : DaySolver, DayInitializer
{
    public static Dictionary<char, int> CardToIndexLookup = new Dictionary<char, int>();

    public void Initialize(List<string> inputLines)
    {
        SetupCardToIndexLookup();
    }

    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        List<HandInfo> handInfos = new List<HandInfo>();

        for(int i = 0; i < inputLines.Count; i++)
        {
            HandInfo handInfo = new HandInfo();

            string[] inputSplit = inputLines[i].Split(' ');
            
            handInfo.cards = inputSplit[0];
            handInfo.handType = DeternmineHandType(inputSplit[0]);
            handInfo.bid = int.Parse(inputSplit[1]);

            handInfos.Add(handInfo);
        }

        handInfos.Sort();

        for(int i = 0; i < handInfos.Count; i++)
        {
            result += (i + 1) * handInfos[i].bid;
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        for(int i = 0; i < inputLines.Count; i++)
        {
            inputLines[i] = inputLines[i].Replace('J', '*'); ;
        }

        return SolvePartOne(inputLines);
    }

    void SetupCardToIndexLookup()
    {
        CardToIndexLookup.Clear();
        CardToIndexLookup.Add('*', -1);
        CardToIndexLookup.Add('2', 0);
        CardToIndexLookup.Add('3', 1);
        CardToIndexLookup.Add('4', 2);
        CardToIndexLookup.Add('5', 3);
        CardToIndexLookup.Add('6', 4);
        CardToIndexLookup.Add('7', 5);
        CardToIndexLookup.Add('8', 6);
        CardToIndexLookup.Add('9', 7);
        CardToIndexLookup.Add('T', 8);
        CardToIndexLookup.Add('J', 9);
        CardToIndexLookup.Add('Q', 10);
        CardToIndexLookup.Add('K', 11);
        CardToIndexLookup.Add('A', 12);
    }

    enum HandType
    {
        HighCard = 0,
        OnePair = 1,
        TwoPairs = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6,
    }

    struct HandInfo : IComparable<HandInfo>
    {
        public string cards;
        public HandType handType;
        public int bid;

        public int CompareTo(HandInfo other)
        {
            if (this.handType != other.handType)
            {
                return this.handType > other.handType ? 1 : -1;
            }
            else
            {
                for (int i = 0; i < this.cards.Length; i++)
                {
                    if (this.cards[i] != other.cards[i])
                    {
                        return CardToIndexLookup[this.cards[i]] > CardToIndexLookup[other.cards[i]] ? 1 : -1;
                    }
                }
            }
            return 0;
        }

        public override string ToString()
        {
            return $"{cards} ({handType}), {bid}";
        }
    }

    HandType DeternmineHandType(string hand)
    {
        int[] cardCounts = new int[13];
        int jokerCount = 0;

        foreach (char card in hand)
        {
            if (card == '*')
            {
                jokerCount++;
            }
            else
            {
                cardCounts[CardToIndexLookup[card]]++;
            }
        }

        int[] tupleHistogram = new int[5];
        int highestTupleIndex = 0;

        foreach (int cardCount in cardCounts)
        {
            if (cardCount > 0)
            {
                tupleHistogram[cardCount - 1]++;
                highestTupleIndex = Math.Max(highestTupleIndex, cardCount - 1);
            }
        }

        if (jokerCount > 0)
        {
            tupleHistogram[highestTupleIndex]--;
            tupleHistogram[Math.Min(highestTupleIndex + jokerCount, 4)]++;
        }

        if (tupleHistogram[0] == 5)
        {
            return HandType.HighCard;
        }
        else if (tupleHistogram[0] == 3 /*&& sameCardCount[1] == 2*/)
        {
            return HandType.OnePair;
        }
        else if (tupleHistogram[1] == 2)
        {
            return HandType.TwoPairs;
        }
        else if (tupleHistogram[2] == 1)
        {
            if (tupleHistogram[0] == 2)
            {
                return HandType.ThreeOfAKind;
            }
            else
            {
                return HandType.FullHouse;
            }
        }
        else if (tupleHistogram[3] == 1)
        {
            return HandType.FourOfAKind;
        }
        else
        {
            return HandType.FiveOfAKind;
        }
    }
}
