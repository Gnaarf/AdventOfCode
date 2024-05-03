using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class DayOneCalorieCounting : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        int maxCalories = 0;
        int currentCalories = 0;

        for(int i = 0; i < inputLines.Count; i++)
        {
            if (inputLines[i] == "")
            {
                maxCalories = Math.Max(maxCalories, currentCalories);
                currentCalories = 0;
            }
            else
            {
                currentCalories += int.Parse(inputLines[i]);
            }
        }

        return maxCalories;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        List<int> maxCalories = new List<int>() { 0, 0, 0};
        int currentCalories = 0;

        for (int i = 0; i < inputLines.Count; i++)
        {
            if (inputLines[i] == "")
            {
                maxCalories.Add(currentCalories);
                maxCalories.Sort();
                maxCalories.RemoveAt(0);
                currentCalories = 0;
            }
            else
            {
                currentCalories += int.Parse(inputLines[i]);
            }
        }

        return maxCalories[0] + maxCalories[1] + maxCalories[2];
    }
}
