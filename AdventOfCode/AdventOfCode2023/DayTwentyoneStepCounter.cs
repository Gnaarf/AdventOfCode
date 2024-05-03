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
using System.Web;

public class DayTwentyoneStepCounter : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        char[,] map = ExtractMap(inputLines, out Vector2Int startPosition);
        FloodFill(map, 64, startPosition, out int reachableByEvenNumberSteps, out int reachableByOddNumberSteps);
        return reachableByEvenNumberSteps;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        var map = ExtractMap(inputLines, out var startPos);

        int totalStepCount = 26501365;
        int expanse = (totalStepCount - map.GetLength(0) / 2) / map.GetLength(0);

        List<Tuple<MapPart, int, Vector2Int>> floodFillSetups = new List<Tuple<MapPart, int, Vector2Int>>()
        {
            new Tuple<MapPart, int, Vector2Int>(MapPart.Middle, map.GetLength(0) / 2, startPos),
            new Tuple<MapPart, int, Vector2Int>(MapPart.TopLeft, map.GetLength(0) / 2 - 1, new Vector2Int(0 , 0)),
            new Tuple<MapPart, int, Vector2Int>(MapPart.TopRight, map.GetLength(0) / 2 - 1, new Vector2Int(map.GetLength(0) - 1, 0)),
            new Tuple<MapPart, int, Vector2Int>(MapPart.BottomLeft, map.GetLength(0) / 2 - 1, new Vector2Int(0, map.GetLength(1) - 1)),
            new Tuple<MapPart, int, Vector2Int>(MapPart.BottomRight, map.GetLength(0) / 2 - 1, new Vector2Int(map.GetLength(0) - 1, map.GetLength(1) - 1)),
            new Tuple<MapPart, int, Vector2Int>(MapPart.All, map.GetLength(0) - 1, startPos),
        };

        Dictionary<MapPart, int> reachableByEvenNumberSteps = new Dictionary<MapPart, int>();
        Dictionary<MapPart, int> reachableByOddNumberSteps = new Dictionary<MapPart, int>();

        foreach(var setup in floodFillSetups)
        {
            FloodFill(map, setup.Item2, setup.Item3, out int reachEven, out int reachOdd);
            reachableByEvenNumberSteps[setup.Item1] = reachEven;
            reachableByOddNumberSteps[setup.Item1] = reachOdd;
        }

        Console.WriteLine($"all: {reachableByOddNumberSteps[MapPart.All]}, sum: {reachableByOddNumberSteps[MapPart.Middle] + reachableByOddNumberSteps[MapPart.TopLeft] + reachableByOddNumberSteps[MapPart.TopRight] + reachableByOddNumberSteps[MapPart.BottomLeft] + reachableByOddNumberSteps[MapPart.BottomRight]}");
        Console.WriteLine($"all: {reachableByEvenNumberSteps[MapPart.All]}, sum: {reachableByEvenNumberSteps[MapPart.Middle] + reachableByEvenNumberSteps[MapPart.TopLeft] + reachableByEvenNumberSteps[MapPart.TopRight] + reachableByEvenNumberSteps[MapPart.BottomLeft] + reachableByEvenNumberSteps[MapPart.BottomRight]}");

        BigInteger lightgreen =
            (BigInteger)expanse * expanse * reachableByEvenNumberSteps[MapPart.All];
        BigInteger darkgreen =
            (BigInteger)(expanse - 1) * (expanse - 1) * reachableByOddNumberSteps[MapPart.All];
        BigInteger darkBlue =
            ((BigInteger)reachableByOddNumberSteps[MapPart.Middle] * (4 * expanse)) +
            ((BigInteger)reachableByOddNumberSteps[MapPart.TopLeft] * (3 * expanse - 1)) +
            ((BigInteger)reachableByOddNumberSteps[MapPart.TopRight] * (3 * expanse - 1)) +
            ((BigInteger)reachableByOddNumberSteps[MapPart.BottomLeft] * (3 * expanse - 1)) +
            ((BigInteger)reachableByOddNumberSteps[MapPart.BottomRight] * (3 * expanse - 1));
        BigInteger lightBlue =
            ((BigInteger)reachableByEvenNumberSteps[MapPart.TopLeft] * expanse) +
            ((BigInteger)reachableByEvenNumberSteps[MapPart.TopRight] * expanse) +
            ((BigInteger)reachableByEvenNumberSteps[MapPart.BottomLeft] * expanse) +
            ((BigInteger)reachableByEvenNumberSteps[MapPart.BottomRight] * expanse);

        BigInteger result = lightgreen + darkgreen + darkBlue + lightBlue;

        Console.WriteLine($"result as BigInteger: {result}");

        return (long)result;
    }

    private void FloodFill(char[,] map, int stepCount, Vector2Int startPosition, out int reachableByEvenNumberSteps, out int reachableByOddNumberSteps)
    {
        var currentPositions = new List<Vector2Int>() { startPosition };
        reachableByEvenNumberSteps = 1;
        reachableByOddNumberSteps = 0;
        int[,] reachedStepCount = new int[map.GetLength(0), map.GetLength(1)];

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                reachedStepCount[x, y] = -1;
            }
        }

        reachedStepCount.Set(startPosition, 0);

        List<Vector2Int> allDirection = new List<Vector2Int>() { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right };

        for (int i = 1; i <= stepCount; i++)
        {
            var previousPositions = new List<Vector2Int>(currentPositions);
            currentPositions.Clear();

            foreach (var previousPosition in previousPositions)
            {
                foreach (var direction in allDirection)
                {
                    Vector2Int newPos = previousPosition + direction;
                    if (newPos.X >= 0 && newPos.Y >= 0 && newPos.X < map.GetLength(0) && newPos.Y < map.GetLength(1) &&
                        map.Get(newPos) != '#' && reachedStepCount.Get(newPos) == -1)
                    {
                        reachedStepCount.Set(newPos, i);
                        currentPositions.Add(newPos);
                        if (i % 2 == 0)
                        {
                            reachableByEvenNumberSteps++;
                        }
                        else
                        {
                            reachableByOddNumberSteps++;
                        }
                    }
                }
            }
        }
    }

    enum MapPart
    {
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight,
        Middle,
        All
    }

    char[,] ExtractMap(List<string> inputLines, out Vector2Int startPos)
    {
        char[,] map = new char[inputLines[0].Length, inputLines.Count];
        startPos = Vector2Int.Zero;

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x= 0; x < inputLines[y].Length; x++)
            {
                map[x,y] = inputLines[y][x];
                if(map[x, y] == 'S')
                {
                    startPos = new Vector2Int(x, y);
                }
            }
        }
        return map;
    }
    enum Direction
    {
        Up, Down, Left, Right
    }

}
