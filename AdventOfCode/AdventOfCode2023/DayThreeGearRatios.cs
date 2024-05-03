using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DayThreeGearRatios
{
    public static long Run()
    {
        StreamReader streamReader = File.OpenText("../../../DayThreeGearRatios.txt");

        string line = streamReader.ReadLine();

        List<string> input = new List<string>();

        while (line != null)
        {
            input.Add(line);

            line = streamReader.ReadLine();
        }

        streamReader.Close();

        //long result = EvaluatePartOne(input);
        long result = EvaluatePartTwo(input);

        return result;
    }

    static long EvaluatePartOne(List<string> inputLines)
    {
        bool[,] isTagged = new bool[inputLines.Count, inputLines[0].Length];

        TagAdjacentFields(inputLines, isTagged, x => !Helper.IsNumber(x) && x != '.');
       
        PrintColorCodedInput(inputLines, isTagged);

        char currentSymbol;
        string currentNumber;
        bool isCurrentNumberTagged;
        long result = 0;

        for (int y = 0; y < inputLines.Count; y++)
        {
            isCurrentNumberTagged = false;
            currentSymbol = '.';
            currentNumber = "";

            for (int x = 0; x < inputLines[y].Length; x++)
            {
                currentSymbol = inputLines[y][x];

                if (Helper.IsNumber(currentSymbol))
                {
                    currentNumber += currentSymbol;
                    isCurrentNumberTagged |= isTagged[y, x];
                }
                else
                {
                    if (currentNumber != "" && isCurrentNumberTagged)
                    {
                        result += long.Parse(currentNumber);
                    }

                    isCurrentNumberTagged = false;
                    currentNumber = "";
                }
            }
            if (currentNumber != "" && isCurrentNumberTagged)
            {
                result += long.Parse(currentNumber);
            }
        }

        return result;
    }

    private static void TagAdjacentFields(List<string> inputLines, bool[,] isTagged, Predicate<char> tagPredicate)
    {
        char currentSymbol;

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                currentSymbol = inputLines[y][x];

                if (tagPredicate(currentSymbol))
                {
                    TagAdjacentFields(isTagged, x, y);
                }
            }
        }
    }

    private static void PrintColorCodedInput(List<string> inputLines, bool[,] isTagged)
    {
        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                if (isTagged[y, x])
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write(inputLines[y][x]);
            }
            Console.WriteLine();
        }
    }

    static void TagAdjacentFields(bool[,] tagArray, int x, int y)
    {
        TagFieldIfIndexIsValid(tagArray, x - 1, y - 1);
        TagFieldIfIndexIsValid(tagArray, x, y - 1);
        TagFieldIfIndexIsValid(tagArray, x + 1, y - 1);

        TagFieldIfIndexIsValid(tagArray, x - 1, y);
        TagFieldIfIndexIsValid(tagArray, x, y);
        TagFieldIfIndexIsValid(tagArray, x + 1, y);

        TagFieldIfIndexIsValid(tagArray, x - 1, y + 1);
        TagFieldIfIndexIsValid(tagArray, x, y + 1);
        TagFieldIfIndexIsValid(tagArray, x + 1, y + 1);
    }

    static void TagFieldIfIndexIsValid(bool[,] tagArray, int x, int y)
    {
        if (IsIndexValid(x, y, tagArray.GetLength(0), tagArray.GetLength(1)))
        {
            tagArray[y, x] = true;
        }
    }

    static bool IsIndexValid(int x, int y, int xDim, int yDim)
    {
        return x >= 0 && x < xDim && y >= 0 && y < yDim;
    }

    static long EvaluatePartTwo(List<string> inputLines)
    {
        char currentSymbol;
        long result = 0;

        for (int y = 0; y < inputLines.Count; y++)
        {
            Console.Write($"now at line {y}:");

            for (int x = 0; x < inputLines[y].Length; x++)
            {
                Console.Write(inputLines[y][x]);

                currentSymbol = inputLines[y][x];

                if(currentSymbol == '*')
                {
                    result += DoGearStuff(inputLines, x, y);
                }
            }
            Console.WriteLine();
        }
        return result;
    }

    static int DoGearStuff(List<string> inputLines, int gearX, int gearY)
    {
        List<Vector2Int> numberIndices = new List<Vector2Int>();

        for (int y = gearY - 1; y <= gearY + 1; y++)
        {
            for (int x = gearX - 1; x <= gearX + 1; x++)
            {
                if (IsIndexValid(x, y, inputLines[y].Length, inputLines.Count) && Helper.IsNumber(inputLines[y][x]))
                {
                    numberIndices.Add(new Vector2Int(x, y));
                }
            }
        }

        foreach (var v1 in numberIndices)
        {
            foreach (var v2 in numberIndices)
            {
                if (v1.Y != v2.Y ||
                    (v1.Y == v2.Y && !Helper.IsNumber( inputLines[(int)v1.Y][(int)(v1.X + v2.X) / 2])))
                {
                    int number1 = int.Parse(Helper.ConcatenateNumberFromAdjacentChars(inputLines[(int)v1.Y], (int)v1.X));
                    int number2 = int.Parse(Helper.ConcatenateNumberFromAdjacentChars(inputLines[(int)v2.Y], (int)v2.X));

                    return number1 * number2;
                }
            }
        }

        return 0;
    }
}
