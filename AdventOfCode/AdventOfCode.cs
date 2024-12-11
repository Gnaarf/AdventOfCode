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
        DaySolver daySolver = new DayThreeMullItOver();

        List<string> inputLines = ConvertToListOfStrings("../../../" + daySolver.GetType().ToString() + ".txt");
        //inputLines = TestData();

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
        if (daySolver is DayInitializer) { Console.WriteLine($"Init.   : {"",20} ({timeInitialization}ms)"); }
        Console.WriteLine($"Part One: {resultPartOne,20} ({timePartOne}ms)");
        Console.WriteLine($"Part Two: {resultPartTwo,20} ({timePartTwo}ms)");

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
        string s = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        s = s.RemoveChar('\r');
        return s.Split('\n').ToList();
    }
}

