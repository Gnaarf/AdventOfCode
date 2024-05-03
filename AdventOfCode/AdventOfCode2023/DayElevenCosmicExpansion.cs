using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class DayElevenCosmicExpansion : DaySolver, DayInitializer
{
    List<int> _emptyRowIndices;
    List<int> _emptyColumnIndices;

    public void Initialize(List<string> inputLines)
    {
        _emptyRowIndices = new List<int>();

        for (int i = 0; i < inputLines.Count; i++)
        {
            if (inputLines[i].All(x => x == '.'))
            {
                _emptyRowIndices.Add(i);
            }
        }

        _emptyColumnIndices = new List<int>();
        bool containsGalaxy;

        for (int columnIndex = 0; columnIndex < inputLines[0].Length; columnIndex++)
        {
            containsGalaxy = false;
            for (int rowIndex = 0; rowIndex < inputLines.Count; rowIndex++)
            {
                if (inputLines[rowIndex][columnIndex] != '.')
                {
                    containsGalaxy = true;
                    break;
                }
            }
            if (!containsGalaxy)
            {
                _emptyColumnIndices.Add(columnIndex);
            }
        }
    }

    public long SolvePartOne(List<string> inputLines)
    {
        //DebugDrawGalaxyAndHighlightEmptyLinesAndRows(inputLines);

        List<Vector2Int> galaxies = GetAllGalaxies(inputLines);

        long totalPathLength, emptyGalaxyCrossingCount;
        CountTotalPathLengthAndGalaxieCrossings(galaxies, out totalPathLength, out emptyGalaxyCrossingCount);

        return totalPathLength + emptyGalaxyCrossingCount;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        List<Vector2Int> galaxies = GetAllGalaxies(inputLines);

        long totalPathLength, emptyGalaxyCrossingCount;
        CountTotalPathLengthAndGalaxieCrossings(galaxies, out totalPathLength, out emptyGalaxyCrossingCount);

        return totalPathLength + emptyGalaxyCrossingCount * 999999;
    }

    private void CountTotalPathLengthAndGalaxieCrossings(List<Vector2Int> galaxies, out long totalPathLength, out long totalEmptyGalaxyCrossingCount)
    {
        totalPathLength = 0;
        totalEmptyGalaxyCrossingCount = 0;
        long tmpPathLenght, tmpEmptyGalaxyCrossingCount;

        for (int i = 0; i < galaxies.Count - 1; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                CountPathLenghtAndGalaxyCrossings(galaxies[i], galaxies[j], out tmpPathLenght, out tmpEmptyGalaxyCrossingCount);
                totalPathLength += tmpPathLenght;
                totalEmptyGalaxyCrossingCount += tmpEmptyGalaxyCrossingCount;
            }
        }
    }

    private void CountPathLenghtAndGalaxyCrossings(Vector2Int g1, Vector2Int g2, out long pathLength, out long emptyGalaxyCrossingCount)
    {
        pathLength = Math.Abs(g2.Y - g1.Y) + Math.Abs(g2.X - g1.X);
        emptyGalaxyCrossingCount = 0;

        for (int x = Math.Min(g1.X, g2.X) + 1; x < Math.Max(g1.X, g2.X); x++)
        {
            if (_emptyColumnIndices.Contains(x)) { emptyGalaxyCrossingCount++; }
        }

        for (int y = Math.Min(g1.Y, g2.Y) + 1; y < Math.Max(g1.Y, g2.Y); y++)
        {
            if (_emptyRowIndices.Contains(y)) { emptyGalaxyCrossingCount++; }
        }
    }

    private static List<Vector2Int> GetAllGalaxies(List<string> inputLines)
    {
        List<Vector2Int> galaxies = new List<Vector2Int>();

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                if (inputLines[y][x] == '#')
                {
                    galaxies.Add(new Vector2Int(x, y));
                }
            }
        }

        return galaxies;
    }

    private void DebugDrawGalaxyAndHighlightEmptyLinesAndRows(List<string> inputLines)
    {
        for (int i = 0; i < inputLines.Count; i++)
        {
            string s = inputLines[i];
            if (inputLines[i].All(x => x == '.'))
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(inputLines[i]);
            }
            else
            {
                for (int j = 0; j < inputLines[i].Length; j++)
                {
                    Console.BackgroundColor = _emptyColumnIndices.Contains(j) ? ConsoleColor.Magenta : ConsoleColor.Black;
                    Console.Write(inputLines[i][j]);
                }
                Console.WriteLine();
            }
        }
    }
}
