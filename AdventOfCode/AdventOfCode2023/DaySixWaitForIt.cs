using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DaySixWaitForIt : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        List<int> intTimes = Helper.ExtractNumbers(inputLines[0], 12, ' ');
        List<int> intDistances = Helper.ExtractNumbers(inputLines[1], 12, ' ');

        List<long> times = new List<long>();
        List<long> distances = new List<long>();

        for (int i = 0; i < intTimes.Count; i++)
        {
            times.Add(intTimes[i]);
            distances.Add(intDistances[i]);
        }

        return SolveWithBinarySearch(times, distances);
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        int time = int.Parse(inputLines[0].Substring(12).RemoveChar(' '));
        long distance = long.Parse(inputLines[1].Substring(12).RemoveChar(' '));

        return SolveWithBinarySearch(new List<long>() { time }, new List<long>() { distance });
    }

    private static long Solve(List<long> times, List<long> distances)
    {
        /* closed formula: 
         * t... load time
         * T... total time
         * d... distance
         * 
         * d = t * (T-t)
         */

        long result = 1;
        long solutionCount;
        long firstSolutionIndex;

        for (int i = 0; i < times.Count; i++)
        {
            firstSolutionIndex = 0;

            while ((firstSolutionIndex * (times[i] - firstSolutionIndex) <= distances[i]))
            {
                firstSolutionIndex++;
            }

            solutionCount = (times[i] - 1) - 2 * (firstSolutionIndex - 1);

            //Console.WriteLine($"T={times[i]}, D={distances[i]}: solutionCount = {solutionCount}");
            result *= solutionCount;
        }

        return result;
    }

    private static long SolveWithBinarySearch(List<long> times, List<long> distances)
    {
        long result = 1;
        long solutionCount;

        for (int i = 0; i < times.Count; i++)
        {
            int lowerBound = 0;
            int upperBound = (int)times[i] / 2;
            int searchIndex = upperBound / 2;

            while (upperBound - lowerBound > 1)
            {
                searchIndex = (lowerBound + upperBound) / 2;
                if (searchIndex * (times[i] - searchIndex) <= distances[i])
                {
                    lowerBound = searchIndex;
                }
                else
                {
                    upperBound = searchIndex;
                }
            }

            //searchIndex += searchIndex * (times[i] - searchIndex) <= distances[i] ? 1 : 0;

            solutionCount = (times[i] - 1) - 2 * (searchIndex - 1);

            //Console.WriteLine($"T={times[i]}, D={distances[i]}: solutionCount = {solutionCount}");
            result *= solutionCount;
        }

        return result;
    }

}
