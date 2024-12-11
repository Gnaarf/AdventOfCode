using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Web.UI;

public class DayOneHistorianHysteria : DaySolver
{

    public long SolvePartOne(List<string> inputLines)
    {
        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();

        foreach (string line in inputLines)
        {
            List<int> numbers = Helper.ExtractNumbers(line, ' ');
            list1.Add(numbers[0]);
            list2.Add(numbers[1]);
        }

        list1.Sort();
        list2.Sort();

        int result = 0;

        for (int i = 0; i < list1.Count(); i++)
        {
            result += Math.Abs(list1[i] - list2[i]);
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();

        foreach (string line in inputLines)
        {
            List<int> numbers = Helper.ExtractNumbers(line, ' ');
            list1.Add(numbers[0]);
            list2.Add(numbers[1]);
        }

        list1.Sort();
        list2.Sort();

        int result = 0;
        int index1 = 0;
        int index2 = 0;
        List<int> similarityScores = new List<int>();

        while (index1 < list1.Count)
        {
            if (index1 > 0 && list1[index1 - 1] == list1[index1])
            {
                similarityScores.Add(similarityScores.Last());
            }
            else
            {
                while (list1[index1] > list2[index2])
                {
                    index2++;
                }
                int similarityScore = 0;
                while (list1[index1] == list2[index2])
                {
                    similarityScore += list1[index1];
                    index2++;
                }
                if (similarityScore > 0)
                {
                    similarityScores.Add(similarityScore);
                }
            }

            index1++;
        }

        return similarityScores.Sum();
    }
}
