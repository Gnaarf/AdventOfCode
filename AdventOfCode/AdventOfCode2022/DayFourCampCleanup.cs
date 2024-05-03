using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class DayFourCampCleanup : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;
        string[] lineSplit;

        foreach (string inputLine in inputLines)
        {
            lineSplit = inputLine.Split('-', ',');

            if ((int.Parse(lineSplit[0]) <= int.Parse(lineSplit[2]) && int.Parse(lineSplit[1]) >= int.Parse(lineSplit[3])) ||
                (int.Parse(lineSplit[0]) >= int.Parse(lineSplit[2]) && int.Parse(lineSplit[1]) <= int.Parse(lineSplit[3])))
            {
                result++;
            }
        }
        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;
        string[] lineSplit;

        foreach (string inputLine in inputLines)
        {
            lineSplit = inputLine.Split('-', ',');

            if (Contains(int.Parse(lineSplit[0]), int.Parse(lineSplit[1]), int.Parse(lineSplit[2]), int.Parse(lineSplit[3])) ||
                Contains(int.Parse(lineSplit[2]), int.Parse(lineSplit[3]), int.Parse(lineSplit[0]), int.Parse(lineSplit[1])))
            {
                result++;
            }
        }
        return result;
    }

    bool Contains(int lowerBound, int upperBound, params int[] values)
    {
        return values.Any(x => lowerBound <= x && upperBound >= x);
    }
}
