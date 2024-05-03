using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DayNineMirageMaintenance : DaySolver, DayInitializer
{
    List<List<int>> numbersLines = new List<List<int>>();

    public void Initialize(List<string> inputLines)
    {
        for(int i = 0; i < inputLines.Count; i++)
        {
            numbersLines.Add(Helper.ExtractNumbers(inputLines[i], ' '));
        }
    }

    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        for (int i = 0; i < numbersLines.Count; i++)
        {
            List<List<int>> differencesList = new List<List<int>>();

            differencesList.Add(numbersLines[i]);
            List<int> differences = numbersLines[i];

            while(!differences.All(x => x == 0))
            {
                differences = GetDifferences(differences);
                differencesList.Add(differences);
            }

            differencesList[differencesList.Count - 1].Add(0);

            for(int j = differencesList.Count - 2; j >= 0; j--)
            {
                int newEntry = differencesList[j].Last() + differencesList[j + 1].Last();
                differencesList[j].Add(newEntry);
            }
            result += differencesList[0].Last();
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        for (int i = 0; i < numbersLines.Count; i++)
        {
            List<List<int>> differencesList = new List<List<int>>();

            differencesList.Add(numbersLines[i]);
            List<int> differences = numbersLines[i];

            while(!differences.All(x => x == 0))
            {
                differences = GetDifferences(differences);
                differencesList.Add(differences);
            }

            differencesList[differencesList.Count - 1].Add(0);

            for(int j = differencesList.Count - 2; j >= 0; j--)
            {
                int newEntry = differencesList[j].First() - differencesList[j + 1].First();
                differencesList[j].Insert(0, newEntry);
            }
            result += differencesList[0].First();
        }

        return result;
    
    }

    List<int> GetDifferences(List<int> numbers)
    {
        List<int> differences = new List<int>();

        for(int i  = 1; i < numbers.Count; i++)
        {
            differences.Add(numbers[i] - numbers[i - 1]);
        }
        return differences;
    }
}
