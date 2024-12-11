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

public class DayThreeMullItOver : DaySolver
{

    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;
        string validPrefix = "mul(";

        foreach (string line in inputLines)
        {
            for (int i = 0; i < line.Length - 3; i++)
            {
                if (line.Substring(i, 4) == validPrefix)
                {
                    i += 4;
                    int lengthToClosingBracket = line.Substring(i).IndexOf(')') + 1;
                    string[] substrings = line.Substring(i, lengthToClosingBracket - 1).Split(',');
                    if (substrings.Length == 2)
                    {
                        try
                        {
                            int[] numbers = new int[] { int.Parse(substrings[0]), int.Parse(substrings[1]) };
                            result += numbers[0] * numbers[1];
                            i += lengthToClosingBracket - 1;
                        }
                        catch (Exception e) { }
                    }
                }
            }

        }
        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;
        string mulPrefix = "mul(";
        bool isActive = true;

        foreach (string line in inputLines)
        {
            for (int i = 0; i < line.Length - 3; i++)
            {
                if (line.Substring(i, 4) == "do()")
                {
                    isActive = true;
                    i += 3;
                }
                else if (i < line.Length - 6 && line.Substring(i, 7) == "don't()")
                {
                    isActive = false;
                    i += 6;
                }
                else if (isActive && line.Substring(i, 4) == mulPrefix)
                {
                    i += 4;
                    int lengthToClosingBracket = line.Substring(i).IndexOf(')') + 1;
                    string[] substrings = line.Substring(i, lengthToClosingBracket - 1).Split(',');
                    if (substrings.Length == 2)
                    {
                        try
                        {
                            int[] numbers = new int[] { int.Parse(substrings[0]), int.Parse(substrings[1]) };
                            result += numbers[0] * numbers[1];
                            i += lengthToClosingBracket - 1;
                        }
                        catch (Exception e) { }
                    }
                }
            }

        }
        return result;
    }
}
