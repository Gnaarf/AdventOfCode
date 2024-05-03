using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class DayTenPipeMaze : DaySolver, DayInitializer
{
    Vector2Int _startPosition;
    char[,] _pipes;

    public void Initialize(List<string> inputLines)
    {
        _pipes = new char[inputLines[0].Length, inputLines.Count];

        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                _pipes[x, y] = inputLines[y][x];

                if (inputLines[y][x] == 'S')
                {
                    _startPosition = new Vector2Int(x, y);
                }
            }
        }
    }

    public long SolvePartOne(List<string> inputLines)
    {
        long pathLength;

        GetPath(out pathLength);

        return pathLength / 2;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        bool[,] isPathPipe = GetPath();

        bool isInside;
        bool[,] insideMap = new bool[_pipes.GetLength(0), _pipes.GetLength(1)];

        for (int y = 0; y < inputLines.Count; y++)
        {
            isInside = false;

            for (int x = 0; x < inputLines[y].Length; x++)
            {
                if (isPathPipe[x, y])
                {
                    if (_pipes[x, y] == '|')
                    {
                        isInside = !isInside;
                    }
                    else
                    {
                        char startPipe = _pipes[x, y];
                        do
                        {
                            x++;
                        } while (_pipes[x, y] == '-');

                        if (HasConnectionTo(startPipe, Direction.North) && HasConnectionTo(_pipes[x, y], Direction.South) ||
                            HasConnectionTo(startPipe, Direction.South) && HasConnectionTo(_pipes[x, y], Direction.North))
                        {
                            isInside = !isInside;
                        }
                    }
                }
                else
                {
                    if (isInside)
                    {
                        insideMap[x, y] = true;
                        result++;
                    }
                }
            }
        }

        //for (int y = 0; y < _pipes.GetLength(1); y++)
        //{
        //    for (int x = 0; x < _pipes.GetLength(0); x++)
        //    {
        //        Console.BackgroundColor = insideMap[x, y] ? ConsoleColor.DarkBlue : isPathPipe[x,y] ? ConsoleColor.DarkRed : ConsoleColor.Black;
        //        Console.Write(_pipes[x, y]);
        //    }
        //    Console.WriteLine();
        //}

        return result;
    }

    private bool[,] GetPath()
    {
        long tmp;
        return GetPath(out tmp);
    }

    private bool[,] GetPath(out long pathLength)
    {
        Direction direction = Direction.North;

        foreach (Direction d in Enum.GetValues(typeof(Direction)))
        {
            if (HasConnectionTo(GetPipe(GetNeighbor(_startPosition, d)), Inverse(d)))
            {
                direction = d;
                break;
            }
        }

        Vector2Int position = _startPosition;
        pathLength = 0;
        bool[,] isOnPipePath = new bool[_pipes.GetLength(0), _pipes.GetLength(1)];

        do
        {
            isOnPipePath[position.X, position.Y] = true;
            position = GetNeighbor(position, direction);
            direction = GetOtherDirection(GetPipe(position), Inverse(direction));
            pathLength++;
        } while (position != _startPosition);

        //DebugDrawAndColor(isOnPipePath);
        return isOnPipePath;
    }

    enum Direction
    {
        North,
        East,
        South,
        West,
    }

    Direction Inverse(Direction direction)
    {
        switch (direction)
        {
            case Direction.North: return Direction.South;
            case Direction.East: return Direction.West;
            case Direction.South: return Direction.North;
            case Direction.West: return Direction.East;
            default: return Direction.North;
        }
    }

    char GetPipe(Vector2Int pos)
    {
        return _pipes[pos.X, pos.Y];
    }

    Vector2Int GetNeighbor(Vector2Int pos, Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return new Vector2Int(pos.X, pos.Y - 1);
            case Direction.East: return new Vector2Int(pos.X + 1, pos.Y);
            case Direction.South: return new Vector2Int(pos.X, pos.Y + 1);
            case Direction.West: return new Vector2Int(pos.X - 1, pos.Y);
            default: return pos;
        }
    }

    Direction GetOtherDirection(char pipe, Direction incomingDirection)
    {
        foreach (Direction d in Enum.GetValues(typeof(Direction)))
        {
            if (HasConnectionTo(pipe, d) && d != incomingDirection)
            {
                return d;
            }
        }
        return Direction.North;
    }

    bool HasConnectionTo(char pipe, Direction dir)
    {
        switch (dir)
        {
            case Direction.North: return pipe == '|' || pipe == 'L' || pipe == 'J';
            case Direction.East: return pipe == '-' || pipe == 'L' || pipe == 'F';
            case Direction.South: return pipe == '|' || pipe == '7' || pipe == 'F';
            case Direction.West: return pipe == '-' || pipe == 'J' || pipe == '7';
            default: return false;
        }
    }

    public void DebugDrawAndColor(bool[,] coloredTiles)
    {
        for (int y = 0; y < _pipes.GetLength(1); y++)
        {
            for (int x = 0; x < _pipes.GetLength(0); x++)
            {
                Console.BackgroundColor = coloredTiles[x, y] ? ConsoleColor.Blue : ConsoleColor.Black;
                Console.Write(_pipes[x, y]);
            }
            Console.WriteLine();
        }
    }
}
