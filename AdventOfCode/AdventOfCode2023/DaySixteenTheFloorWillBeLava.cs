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

public class DaySixteenTheFloorWillBeLava : DaySolver, DayInitializer
{
    char[,] mirrorsMap;
    char[,] mapForPartTwo;

    public void Initialize(List<string> inputLines)
    {
        mirrorsMap = new char[inputLines.Count, inputLines[0].Length];
        mapForPartTwo = new char[inputLines.Count, inputLines[0].Length];

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                mirrorsMap[x, y] = inputLines[y][x];
                mapForPartTwo[x, y] = inputLines[y][x];
            }
        }
    }

    public long SolvePartOne(List<string> inputLines)
    {
        return CountEnergizedTiles(new Vector2Int(-1, 0), Direction.Right);
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        for (int x = 0; x < mirrorsMap.GetLength(0); x++)
        {
            result = Math.Max(result, CountEnergizedTiles(new Vector2Int(x, -1), Direction.Down));
            result = Math.Max(result, CountEnergizedTiles(new Vector2Int(x, mirrorsMap.GetLength(1)), Direction.Up));
        }

        for (int y = 0; y < mirrorsMap.GetLength(1); y++)
        {
            result = Math.Max(result, CountEnergizedTiles(new Vector2Int(-1, y), Direction.Right));
            result = Math.Max(result, CountEnergizedTiles(new Vector2Int(mirrorsMap.GetLength(0), y), Direction.Left));
        }

        return result;
    }

    long CountEnergizedTiles(Vector2Int startPosition, Direction startDirection)
    {
        List<Direction>[,] beamsMap = new List<Direction>[mirrorsMap.GetLength(0), mirrorsMap.GetLength(1)];

        for (int y = 0; y < beamsMap.GetLength(1); y++)
        {
            for (int x = 0; x < beamsMap.GetLength(0); x++)
            {
                beamsMap[x, y] = new List<Direction>();
            }
        }

        Stack<Tuple<Vector2Int, Direction>> beamHeads = new Stack<Tuple<Vector2Int, Direction>>();

        beamHeads.Push(new Tuple<Vector2Int, Direction>(startPosition, startDirection));

        while(beamHeads.Count > 0)
        {
            var currentBeamHead = beamHeads.Pop();

            Direction currentDir = currentBeamHead.Item2;
            Vector2Int nextPos = GetNextPositionInDirection(currentBeamHead.Item1, currentBeamHead.Item2);

            if(nextPos.X >= 0 && nextPos.Y >= 0 && nextPos.X < mirrorsMap.GetLength(0) && nextPos.Y < mirrorsMap.GetLength(1))
            {
                List<Direction> nextDiretions = GetOutgoingBeamDirections(currentDir, mirrorsMap[nextPos.X, nextPos.Y]);

                foreach(var dir in nextDiretions)
                {
                    if (!beamsMap[nextPos.X, nextPos.Y].Contains(dir))
                    {
                        beamsMap[nextPos.X, nextPos.Y].Add(dir);
                        beamHeads.Push(new Tuple<Vector2Int, Direction>(nextPos, dir));
                    }
                }
            }
        }

        long result = 0;

        foreach (var beamsOnTile in  beamsMap)
        {
            if(beamsOnTile.Count > 0)
            {
                result++;
            }
        }

        return result;
    }

    enum Direction
    {
        Up, Down, Left, Right
    }

    Vector2Int GetNextPositionInDirection(Vector2Int currentPosition, Direction direction)
    {
        switch(direction)
        {
            case Direction.Left: return currentPosition + Vector2Int.Left;
            case Direction.Right: return currentPosition + Vector2Int.Right;
            case Direction.Up: return currentPosition + Vector2Int.Up;
            case Direction.Down: return currentPosition + Vector2Int.Down;
            default: return currentPosition;
        }
    }

    List<Direction> GetOutgoingBeamDirections(Direction incomingBeamDirection, char mirror)
    {
        List<Direction> outGoingBeamDirectios = new List<Direction>();

        if(mirror == '.')
        {
            outGoingBeamDirectios.Add(incomingBeamDirection);
        }
        else if (mirror == '/')
        {
            switch (incomingBeamDirection)
            {
                case Direction.Left:
                    outGoingBeamDirectios.Add(Direction.Down);
                    break;
                case Direction.Right:
                    outGoingBeamDirectios.Add(Direction.Up);
                    break;
                case Direction.Up:
                    outGoingBeamDirectios.Add(Direction.Right);
                    break;
                case Direction.Down:
                    outGoingBeamDirectios.Add(Direction.Left);
                    break;
            }
        }
        else if (mirror == '\\')
        {
            switch (incomingBeamDirection)
            {
                case Direction.Left:
                    outGoingBeamDirectios.Add(Direction.Up);
                    break;
                case Direction.Right:
                    outGoingBeamDirectios.Add(Direction.Down);
                    break;
                case Direction.Up:
                    outGoingBeamDirectios.Add(Direction.Left);
                    break;
                case Direction.Down:
                    outGoingBeamDirectios.Add(Direction.Right);
                    break;
            }
        }
        else if (mirror == '-')
        {
            switch (incomingBeamDirection)
            {
                case Direction.Left:
                case Direction.Right:
                    outGoingBeamDirectios.Add(incomingBeamDirection);
                    break;
                case Direction.Up:
                case Direction.Down:
                    outGoingBeamDirectios.Add(Direction.Left);
                    outGoingBeamDirectios.Add(Direction.Right);
                    break;
            }
        }
        else if (mirror == '|')
        {
            switch (incomingBeamDirection)
            {
                case Direction.Left:
                case Direction.Right:
                    outGoingBeamDirectios.Add(Direction.Up);
                    outGoingBeamDirectios.Add(Direction.Down);
                    break;
                case Direction.Up:
                case Direction.Down:
                    outGoingBeamDirectios.Add(incomingBeamDirection);
                    break;
            }
        }

        return outGoingBeamDirectios;
    }
}
