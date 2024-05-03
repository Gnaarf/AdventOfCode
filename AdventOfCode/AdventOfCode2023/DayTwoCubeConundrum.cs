using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DayTwoCubeConundrum
{
    public static long Run()
    {
        StreamReader streamReader = File.OpenText("../../../DayTwoCubeConundrum.txt");

        string line = streamReader.ReadLine();
        long result = 0;

        while (line != null)
        {
            result += EvaluateLinePartTwo(line);

            line = streamReader.ReadLine();
        }

        streamReader.Close();

        return result;
    }

    static int EvaluateLine(string line)
    {
        string[] lineParts = line.Split(':');

        int gameIndex = int.Parse(lineParts[0].Substring(5));
        string[] subsets = lineParts[1].Split(';');

        int redCubeCount = 0, greenCubeCount = 0, blueCubeCount = 0;

        //Console.Write($"eval: Game {gameIndex}: ");

        foreach (string subset in subsets)
        {
            GetCubeCount(subset, out redCubeCount, out greenCubeCount, out blueCubeCount);

            //Console.Write($"{redCubeCount} red, {greenCubeCount} green, {blueCubeCount} blue; ");

            if (redCubeCount > 12 || greenCubeCount > 13 || blueCubeCount > 14)
            {
                return 0;
            }
        }

        //Console.WriteLine($"\norig: {line}");

        return gameIndex;
    }

    static int EvaluateLinePartTwo(string line)
    {
        string[] lineParts = line.Split(':');

        int gameIndex = int.Parse(lineParts[0].Substring(5));
        string[] subsets = lineParts[1].Split(';');

        int redCubeCount = 0, greenCubeCount = 0, blueCubeCount = 0;
        int maxRedCubeCount = 0, maxGreenCubeCount = 0, maxBlueCubeCount = 0;

        foreach (string subset in subsets)
        {
            GetCubeCount(subset, out redCubeCount, out greenCubeCount, out blueCubeCount);

            maxRedCubeCount = Math.Max(maxRedCubeCount, redCubeCount);
            maxGreenCubeCount = Math.Max(maxGreenCubeCount, greenCubeCount);
            maxBlueCubeCount = Math.Max(maxBlueCubeCount, blueCubeCount);
        }

        return maxRedCubeCount * maxGreenCubeCount * maxBlueCubeCount;
    }

    static void GetCubeCount(string subset, out int redCubeCount, out int greenCubeCount, out int blueCubeCount)
    {
        redCubeCount = 0;
        greenCubeCount = 0;
        blueCubeCount = 0;

        string digits = "";

        for (int i = 0; i < subset.Length; i++)
        {
            if (Helper.IsNumber(subset[i]))
            {
                digits += subset[i];
            }
            else if (digits.Length > 0 && subset[i] == ' ')
            {
                if (subset[i + 1] == 'r')
                {
                    redCubeCount = int.Parse(digits);
                    i += 4; // " red" has 4 chars
                }
                else if (subset[i + 1] == 'g')
                {
                    greenCubeCount = int.Parse(digits);
                    i += 6; // " green" has 6 chars
                }
                else if (subset[i + 1] == 'b')
                {
                    blueCubeCount = int.Parse(digits);
                    i += 5; // " blue" has 5 chars
                }
                digits = "";
            }
        }
    }
}
