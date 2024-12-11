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

public class DayFourCeresSearch : DaySolver
{

    public long SolvePartOne(List<string> inputLines)
    {
        string[] targetWords = new string[] { "SAMX", "XMAS" };
        int targetWordLength = 4;
        Vector2Int[] directions = new Vector2Int[] { Vector2Int.Right + Vector2Int.Up, Vector2Int.Right, Vector2Int.Right + Vector2Int.Down, Vector2Int.Down };
        //Vector2Int[] directions = new Vector2Int[]{ Vector2Int.Right + Vector2Int.Up, Vector2Int.Right, Vector2Int.Right + Vector2Int.Down, Vector2Int.Down, Vector2Int.Left + Vector2Int.Down, Vector2Int.Left, Vector2Int.Left + Vector2Int.Up, Vector2Int.Up};

        long result = 0;

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[0].Length; x++)
            {
                int hits = 0;
                foreach(Vector2Int direction in directions)
                {
                    if (0 <= x + (targetWordLength - 1) * direction.X && x + (targetWordLength - 1) * direction.X < inputLines[y].Length &&
                        0 <= y + (targetWordLength - 1) * direction.Y && y + (targetWordLength - 1) * direction.Y < inputLines.Count)
                    {
                        string word = "";
                        for(int i = 0; i < 4; i++)
                        {
                            word += inputLines[y + i * direction.Y].ElementAt(x + i * direction.X);
                        }
                        if (targetWords.Contains(word))
                        {
                            hits++;
                            result++;
                        }
                    }
                }
                //Console.BackgroundColor = hits == 0 ? ConsoleColor.Black : hits == 1 ? ConsoleColor.DarkGray : hits == 2 ? ConsoleColor.DarkYellow : hits >= 3 ? ConsoleColor.DarkMagenta : ConsoleColor.DarkRed;
                //Console.Write(inputLines[y][x]);
            }
            //Console.WriteLine();
        }
        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;
        string[] targetWords = new string[] { "MAS", "SAM" };

        for (int y = 1; y < inputLines.Count - 1; y++)
        {
            for (int x = 1; x < inputLines[0].Length - 1; x++)
            {

                if (inputLines[y][x] == 'A')
                {
                    string diag1 = inputLines[y - 1][x - 1] + "A" + inputLines[y + 1][x + 1];
                    string diag2 = inputLines[y - 1][x + 1] + "A" + inputLines[y + 1][x - 1];
                    if(targetWords.Contains(diag1) && targetWords.Contains(diag2))
                    {
                        result++;
                    }
                }
            }
        }
        return result;
    }
}
