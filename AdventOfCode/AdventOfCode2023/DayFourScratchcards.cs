using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DayFourScratchcards : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        foreach(string inputLine in inputLines)
        {
            result += EvaluateLinePartOne(inputLine);
        }

        return result;
    }

    static long EvaluateLinePartOne(string inputLine)
    {
        ScratchCard scratchCard = new ScratchCard(inputLine);

        //Console.WriteLine(inputLine + " ... matches: " + scratchCard.numberOfMatches);

        return scratchCard.numberOfMatches == 0 ? 0 : (long)Math.Pow(2, scratchCard.numberOfMatches - 1);
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        int[] cardCounts = new int[inputLines.Count]; // initialized with 0s

        for(int i = 0; i < inputLines.Count; i++)
        {
            cardCounts[i]++; // add one for the original card we have

            ScratchCard scratchCard = new ScratchCard(inputLines[i]);

            for(int j = 1; j <= scratchCard.numberOfMatches; j++)
            {
                cardCounts[i + j] += cardCounts[i];
            }

            result += cardCounts[i];
        }

        return result;
    }

    struct ScratchCard
    {
        public int gameIndex;
        public List<int> winningNumbers;
        public List<int> numbersYouHave;

        public int numberOfMatches;

        public ScratchCard(string inputLine)
        {
            string[] inputLineSplit = inputLine.Split(':', '|');

            gameIndex = int.Parse(inputLineSplit[0].Substring(5));

            winningNumbers = IntParseAllNumbers(inputLineSplit[1].Split(' '));
            numbersYouHave = IntParseAllNumbers(inputLineSplit[2].Split(' '));
            
            numberOfMatches = 0;

            foreach (int number in numbersYouHave)
            {
                if (winningNumbers.Contains(number))
                {
                    numberOfMatches++;
                }
            }
        }

        private static List<int> IntParseAllNumbers(string[] strings)
        {
            List<int> parsedNumbers = new List<int>();

            foreach (string s in strings)
            {
                if (s != "")
                {
                    parsedNumbers.Add(int.Parse(s));
                }
            }

            return parsedNumbers;
        }
    }
}
