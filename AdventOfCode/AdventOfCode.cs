using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

internal class AdventOfCode
{
    public static void Main()
    {
        List<string> inputLines = ConvertToListOfStrings("../../../DayTwentytwoSandSlabs.txt");
        inputLines = TestData();
        DaySolver daySolver = new DayTwentytwoSandSlabs();

        Stopwatch stopwatch = new Stopwatch();

        long resultPartOne, resultPartTwo;
        double timeInitialization = 0, timePartOne, timePartTwo;

        if (daySolver is DayInitializer)
        {
            stopwatch.Restart();
            (daySolver as DayInitializer).Initialize(inputLines);
            stopwatch.Stop();
            timeInitialization = stopwatch.Elapsed.TotalMilliseconds;
        }

        stopwatch.Restart();
        resultPartOne = daySolver.SolvePartOne(inputLines);
        stopwatch.Stop();
        timePartOne = stopwatch.Elapsed.TotalMilliseconds;

        stopwatch.Restart();
        resultPartTwo = daySolver.SolvePartTwo(inputLines);
        stopwatch.Stop();
        timePartTwo = stopwatch.Elapsed.TotalMilliseconds;

        Console.WriteLine("------------------");
        if (daySolver is DayInitializer) { Console.WriteLine($"Init.   : {"", 20} ({timeInitialization}ms)"); }
        Console.WriteLine($"Part One: {resultPartOne, 20} ({timePartOne}ms)");
        Console.WriteLine($"Part Two: {resultPartTwo, 20} ({timePartTwo}ms)");

        Console.ReadLine();
    }

    static List<string> ConvertToListOfStrings(string filePath)
    {
        StreamReader streamReader = File.OpenText(filePath);

        List<string> lines = new List<string>();

        string line = streamReader.ReadLine();

        while (line != null)
        {
            lines.Add(line);
            line = streamReader.ReadLine();
        }

        streamReader.Close();

        return lines;
    }

    static List<string> TestData()
    {
        string s = "1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9";
        s = s.RemoveChar('\r');
        return s.Split('\n').ToList();
    }
}

