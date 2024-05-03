using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class DayThreeRucksackReorganization : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        string firstHalf, secondHalf;

        foreach (string inputLine in inputLines)
        {
            firstHalf = inputLine.Remove(inputLine.Length / 2);
            secondHalf = inputLine.Substring(inputLine.Length / 2);

            foreach(char c in firstHalf)
            {
                if (secondHalf.Contains(c))
                {
                    result += char.IsLower(c) ? (c - 'a' + 1) : (c - 'A' + 27);
                    break;
                }
            }

        }
        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        List<string> group = new List<string>();

        foreach (string inputLine in inputLines)
        {
            group.Add(inputLine);

            if (group.Count == 3)
            {
                foreach (char c in group[0])
                {
                    if (group[1].Contains(c) && group[2].Contains(c))
                    {
                        result += char.IsLower(c) ? (c - 'a' + 1) : (c - 'A' + 27);
                        break;
                    }
                }
                group.Clear();
            }
        }
        return result;
    }
}
