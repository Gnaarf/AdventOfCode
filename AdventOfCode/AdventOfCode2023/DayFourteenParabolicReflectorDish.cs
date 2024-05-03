using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class DayFourteenParabolicReflectorDish : DaySolver, DayInitializer
{
    char[,] mapForPartOne;
    char[,] mapForPartTwo;

    public void Initialize(List<string> inputLines)
    {
        mapForPartOne = new char[inputLines.Count, inputLines[0].Length];
        mapForPartTwo = new char[inputLines.Count, inputLines[0].Length];

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                mapForPartOne[x, y] = inputLines[y][x];
                mapForPartTwo[x, y] = inputLines[y][x];
            }
        }
    }

    public long SolvePartOne(List<string> inputLines)
    {
        RollAllRocks(Direction.Up, mapForPartOne);
        
        return GetTotalLoad(mapForPartOne);
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        List<char[,]> mapRecords = new List<char[,]>();
        int cycleStartIndex = -1;

        for(int i = 0; i < 100000; i++)
        {
            mapRecords.Add((char[,])(mapForPartTwo.Clone()));

            RollAllRocks(Direction.Up, mapForPartTwo);
            RollAllRocks(Direction.Left, mapForPartTwo);
            RollAllRocks(Direction.Down, mapForPartTwo);
            RollAllRocks(Direction.Right, mapForPartTwo);

            //Console.WriteLine($"Load after {i} cycles: {GetTotalLoad(mapForPartTwo)}");

            bool foundIdenticalMap = false;
            for(int j = i; j >= 0; j--)
            {
                if (AreMapsIdentical(mapRecords[j], mapForPartTwo))
                {
                    Console.WriteLine($"the current map ({i+1}) is identical to the recorded map ({j})");
                    cycleStartIndex = j;
                    foundIdenticalMap = true;
                    break;
                }
            }
            if (foundIdenticalMap)
            {
                break;
            }
        }

        int cycleLength = mapRecords.Count - cycleStartIndex;
        int index = cycleStartIndex + (1000000000 - cycleStartIndex) % cycleLength;

        Console.WriteLine($"cycle length = {cycleLength}");
        for(int i = cycleStartIndex; i < mapRecords.Count; i++)
        {
            Console.WriteLine($"Load after {i} cycles: {GetTotalLoad(mapRecords[i])}");
        }
        Console.WriteLine();
        Console.WriteLine($"Load after {mapRecords.Count} cycles: {GetTotalLoad(mapForPartTwo)}");
        Console.WriteLine($"converted index = {index}");


        return GetTotalLoad(mapRecords[index]);
    }

    bool AreMapsIdentical(char[,] map1, char[,] map2)
    {
        for (int y = 0; y < map1.GetLength(1); y++)
        {
            for (int x = 0; x < map1.GetLength(0); x++)
            {
                if (map1[x,y] != map2[x, y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    enum Direction
    {
        Up, Down, Left, Right
    }

    private void RollAllRocks(Direction direction, char[,] map)
    {
        Vector2Int startIndex;
        Vector2Int incrementDirection;
        Vector2Int rollDirection;

        switch (direction)
        {
            case Direction.Up:
                startIndex = new Vector2Int(0, 0);
                incrementDirection = new Vector2Int(1, 1);
                rollDirection = new Vector2Int(0, -1);
                break;
            case Direction.Down:
                startIndex = new Vector2Int(0, map.GetLength(1) - 1);
                incrementDirection = new Vector2Int(1, -1);
                rollDirection = new Vector2Int(0, 1);
                break;
            case Direction.Left:
                startIndex = new Vector2Int(0, 0);
                incrementDirection = new Vector2Int(1, 1);
                rollDirection = new Vector2Int(-1, 0);
                break;
            case Direction.Right:
            default:
                startIndex = new Vector2Int(map.GetLength(0) - 1, 0);
                incrementDirection = new Vector2Int(-1, 1);
                rollDirection = new Vector2Int(1, 0);
                break;
        }

        for (int y = startIndex.Y; y >= 0 && y < map.GetLength(1); y += incrementDirection.Y)
        {
            for (int x = startIndex.X; x >= 0 && x < map.GetLength(0); x += incrementDirection.X)
            {
                if (map[x, y] == 'O')
                {
                    Vector2Int target = new Vector2Int(x, y) + rollDirection;
                    while (target.X >= 0 && target.X < map.GetLength(0) && target.Y >= 0 && target.Y < map.GetLength(1) && map[target.X, target.Y] == '.')
                    {
                        target += rollDirection;
                    }
                    target -= rollDirection;

                    map[x, y] = '.';
                    map[target.X, target.Y] = 'O';
                }
            }
        }
    }

    private long GetTotalLoad(char[,] map)
    {
        long result = 0;

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                result += map[x, y] == 'O' ? map.GetLength(1) - y : 0;
            }
        }

        return result;
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
}
