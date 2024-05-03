using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class DayOneTrebuchet
{
    public static int Run()
    {
        return RunPartOne();
        //return RunPartTwo();
    }

    public static int RunPartOne()
    {
        StreamReader streamReader = File.OpenText("../../../DayOneTrebuchet.txt");

        string line = streamReader.ReadLine();
        int result = 0;

        while (line != null)
        {
            char firstNumber = line.First(Helper.IsNumber);
            char lastNumber = line.Last(Helper.IsNumber);

            result += int.Parse(firstNumber.ToString() + lastNumber.ToString());

            Console.WriteLine( $"+ {firstNumber.ToString() + lastNumber.ToString()} = {result}     -      {line}");

            line = streamReader.ReadLine();
        }

        streamReader.Close();

        return result;
    }

    static string[] numbers = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    static string[] numberStrings = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

    public static int RunPartTwo()
    {
        StreamReader streamReader = File.OpenText("../../../DayOneTrebuchet.txt");

        string line = streamReader.ReadLine();
        int result = 0;

        while (line != null)
        {
            int firstNumber = GetFirstNumber(line);
            int lastNumber = GetLastNumber(line);

            result += int.Parse(firstNumber.ToString() + lastNumber.ToString());

            Console.WriteLine($"+ {firstNumber.ToString() + lastNumber.ToString()} = {result}     -      {line}");

            line = streamReader.ReadLine();
        }

        streamReader.Close();

        return result;
    }

    static int GetFirstNumber(string line)
    {
        for (int i = 0; i < line.Length; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                if (DoesLineHaveNumberAtIndex(line, i, numbers[j - 1]) || DoesLineHaveNumberAtIndex(line, i, numberStrings[j - 1]))
                {
                    return j;
                }
            }
        }
        return -1;
    }

    static int GetLastNumber(string line)
    {
        for (int i = line.Length - 1; i >= 0; i--)
        {
            for (int j = 1; j < 10; j++)
            {
                if (DoesLineHaveNumberAtIndex(line, i, numbers[j - 1]) || DoesLineHaveNumberAtIndex(line, i, numberStrings[j - 1]))
                {
                    return j;
                }
            }
        }
        return -1;
    }

    static bool DoesLineHaveNumberAtIndex(string line, int index, string numberString)
    {
        // this code is much nicer than what I wrote :3
        return line.Substring(index).StartsWith(numberString);

        //if (line.Length - index < numberString.Length)
        //    return false;

        //bool isIdentical = true;

        //for(int i = 0; i < numberString.Length && index + i < line.Length; i++)
        //{
        //    isIdentical &= (line[index + i] == numberString[i]);
        //}

        //return isIdentical;
    }
}
