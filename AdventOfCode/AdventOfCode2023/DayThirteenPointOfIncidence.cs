using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

public class DayThirteenPointOfIncidence : DaySolver, DayInitializer
{
    List<char[,]> mirrorMaps = new List<char[,]>();

    public void Initialize(List<string> inputLines)
    {
        List<string> currentInputBatch = new List<string>();

        foreach(string inputLine in inputLines)
        {
            if (inputLine != "")
            {
                currentInputBatch.Add(inputLine);
            }
            else
            {
                char[,] mirrorMap = new char[currentInputBatch[0].Length, currentInputBatch.Count];
                for (int y = 0; y < currentInputBatch.Count; y++)
                {
                    for (int x = 0; x < currentInputBatch[y].Length; x++)
                    {
                        mirrorMap[x,y] = currentInputBatch[y][x];
                    }
                }
                mirrorMaps.Add(mirrorMap);
                currentInputBatch.Clear();
            }
        }
        Console.WriteLine($"total map count = {mirrorMaps.Count}");
    }

    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        foreach(var mirrorMap in mirrorMaps)
        {
            if(HasHorizontalMirrorLine(mirrorMap, out int rowsAboveMirrorLineCount))
            {
                result += 100 * rowsAboveMirrorLineCount;
            }
            else if(HasHorizontalMirrorLine(Transpose(mirrorMap), out int columnsLeftOfMirrorLine))
            {
                result += columnsLeftOfMirrorLine;
            }
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        foreach (var mirrorMap in mirrorMaps)
        {
            Console.WriteLine();
            DebugDrawMap(mirrorMap);

            if (HasHorizontalMirrorLineOnFixedMirror(mirrorMap, out int rowsAboveMirrorLineCount))
            {
                Console.WriteLine($"After fixing, there is a mirror line between row {rowsAboveMirrorLineCount} and {rowsAboveMirrorLineCount + 1}");
                result += 100 * rowsAboveMirrorLineCount;
            }
            else if (HasHorizontalMirrorLineOnFixedMirror(Transpose(mirrorMap), out int columnsLeftOfMirrorLine))
            {
                Console.WriteLine($"After fixing, there is a mirror line between column {columnsLeftOfMirrorLine} and {columnsLeftOfMirrorLine + 1}");
                result += columnsLeftOfMirrorLine;
            }
        }

        return result;
    }

    bool HasHorizontalMirrorLine(char[,] mirror, out int rowsAboveMirrorLineCount)
    {
        for (int mirrorLineLocation = 1; mirrorLineLocation < mirror.GetLength(1); mirrorLineLocation++)
        {
            int maxOffset = Math.Min(mirrorLineLocation, mirror.GetLength(1) - mirrorLineLocation);
            bool isMirrorLine = true;

            for (int offset = 1; offset <= maxOffset; offset++)
            {
                if (!AreLinesIdentical(mirror, mirrorLineLocation - offset, mirrorLineLocation + (offset - 1)))
                {
                    isMirrorLine = false;
                    break;
                }
            }
            if (isMirrorLine)
            {
                rowsAboveMirrorLineCount = mirrorLineLocation;
                return true;
            }
        }

        rowsAboveMirrorLineCount = -1;
        return false;
    }

    bool HasHorizontalMirrorLineOnFixedMirror(char[,] mirrorWithSmudge, out int rowsAboveMirrorLineCount)
    {
        for (int mirrorLineLocation = 1; mirrorLineLocation < mirrorWithSmudge.GetLength(1); mirrorLineLocation++)
        {
            int differenceCount = 0;
            int maxOffset = Math.Min(mirrorLineLocation, mirrorWithSmudge.GetLength(1) - mirrorLineLocation);

            for (int offset = 1; offset <= maxOffset; offset++)
            {
                differenceCount += CountDifferencesInLines(mirrorWithSmudge, mirrorLineLocation - offset, mirrorLineLocation + (offset - 1));
                if(differenceCount > 1) 
                {
                    break;
                }
            }
            if (differenceCount == 1)
            {
                rowsAboveMirrorLineCount = mirrorLineLocation;
                return true;
            }
        }

        rowsAboveMirrorLineCount = -1;
        return false;
    }

    void DebugDrawMap(char[,] map)
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                Console.Write(map[x, y]);
            }
            Console.WriteLine();
        }
    }

    bool AreLinesIdentical(char[,] map, int y1, int y2)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            if (map[x, y1] != map[x, y2])
            {
                return false;
            }
        }
        return true;
    }

    int CountDifferencesInLines(char[,] map, int y1, int y2)
    {
        int differenceCounter = 0;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            if (map[x, y1] != map[x, y2])
            {
                differenceCounter++;
            }
        }
        return differenceCounter;
    }

    T[,] Transpose<T>(T[,] matrix)
    {
        T[,] transposedMatrix = new T[matrix.GetLength(1), matrix.GetLength(0)];

        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                transposedMatrix[y, x] = matrix[x, y];
            }
        }
        return transposedMatrix;
    }
}
