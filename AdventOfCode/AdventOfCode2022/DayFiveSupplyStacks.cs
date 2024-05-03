using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Web.ModelBinding;

public class DayFiveSupplyStacks : DaySolver
{
    public void ExtractStacksAndInstructions(List<string> inputLines, out string[] stacks, out List<string> instructions)
    {
        stacks = Enumerable.Repeat("", inputLines[0].Length / 4 + 1).ToArray();

        int lineIndex = 0;

        while (inputLines[lineIndex] != "")
        {
            for (int i = 0; i < stacks.Length; i++)
            {
                char currentChar = inputLines[lineIndex][i * 4 + 1];
                if (currentChar != ' ')
                {
                    stacks[i] = inputLines[lineIndex][i * 4 + 1] + stacks[i];
                }
            }
            lineIndex++;
        }

        instructions = inputLines.GetRange(lineIndex + 1, inputLines.Count - lineIndex - 1);
    }

    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        string[] stacks;
        List<string> instructions;

        ExtractStacksAndInstructions(inputLines, out stacks, out instructions);

        int from, to, amount;
        string[] instructionSplits;

        foreach (string instruction in instructions)
        {
            instructionSplits = instruction.Split(' ');

            amount = int.Parse(instructionSplits[1]);
            from = int.Parse(instructionSplits[3]) - 1;
            to = int.Parse(instructionSplits[5]) - 1;

            string fromString = stacks[from];
            stacks[to] = stacks[to] + new string(fromString.Substring(fromString.Length - amount).Reverse().ToArray());
            stacks[from] = fromString.Remove(fromString.Length - amount);

        }

        Console.WriteLine("--- End Configuration (Part 1) ---");
        WriteLines(stacks);
        Console.WriteLine();

        return result;
    }

    void WriteLines(string[] strings)
    {
        foreach (string s in strings)
        {
            Console.WriteLine(s);
        }
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        string[] stacks;
        List<string> instructions;

        ExtractStacksAndInstructions(inputLines, out stacks, out instructions);

        int from, to, amount;
        string[] instructionSplits;

        foreach (string instruction in instructions)
        {
            instructionSplits = instruction.Split(' ');

            amount = int.Parse(instructionSplits[1]);
            from = int.Parse(instructionSplits[3]) - 1;
            to = int.Parse(instructionSplits[5]) - 1;

            string fromString = stacks[from];
            stacks[to] = stacks[to] + fromString.Substring(fromString.Length - amount);
            stacks[from] = fromString.Remove(fromString.Length - amount);
        }

        Console.WriteLine("--- End Configuration (Part 2) ---");
        WriteLines(stacks);
        Console.WriteLine();

        return result;
    }
}