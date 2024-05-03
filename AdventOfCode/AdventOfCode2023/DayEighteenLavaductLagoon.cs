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

public class DayEighteenLavaductLagoon : DaySolver
{
    delegate int DistanceDecoder(string inputLine);
    delegate Vector2Long DirectionDecoder(string inputLine);

    public long SolvePartOne(List<string> inputLines)
    {
        SortedDictionary<long, List<VerticalLineSegment>> segmentsListsDict = ConvertToSegmentList
            (inputLines, 
            GetDistanceFromInputLineForPartOne, 
            GetDirectionFromInputLineForPartOne, 
            out Vector2Long min, 
            out Vector2Long max);

        Console.WriteLine($"min = {min}, max = {max}");

        BigInteger result = CountTilesWithinBorders(segmentsListsDict, min, max);

        Console.WriteLine($"result as BigInteger: {result}");

        return (long)result;
    }


    public long SolvePartTwo(List<string> inputLines)
    {
        SortedDictionary<long, List<VerticalLineSegment>> segmentsListsDict = ConvertToSegmentList
            (inputLines,
            GetDistanceFromInputLineForPartTwo,
            GetDirectionFromInputLineForPartTwo,
            out Vector2Long min,
            out Vector2Long max);

        Console.WriteLine($"min = {min}, max = {max}");

        BigInteger result = CountTilesWithinBorders(segmentsListsDict, min, max);

        Console.WriteLine($"result as BigInteger: {result}");

        return (long)result;
    }

    class VerticalLineSegment
    {
        public long X;
        public long MinY;
        public long MaxY;

        public VerticalLineSegment(long x, long minY, long maxY)
        {
            this.X = x;
            this.MinY = minY;
            this.MaxY = maxY;
        }

        public bool Intersects(long y)
        {
            return MinY <= y && MaxY >= y;
        }
    }

    SortedDictionary<long, List<VerticalLineSegment>> ConvertToSegmentList(List<string> inputLines, DistanceDecoder distanceDecoder, DirectionDecoder directionDecoder, out Vector2Long min, out Vector2Long max)
    {
        var segments = new SortedDictionary<long, List<VerticalLineSegment>>();

        Vector2Long position = new Vector2Long(0, 0);
        min = position;
        max = position;

        foreach (string inputLine in inputLines)
        {
            int distance = distanceDecoder(inputLine);
            Vector2Long direction = directionDecoder(inputLine);

            Vector2Long nextPosition = position + distance * direction;

            if (direction == Vector2Long.Up || direction == Vector2Long.Down)
            {
                var newSegment = new VerticalLineSegment(position.X, Math.Min(position.Y, nextPosition.Y), Math.Max(position.Y, nextPosition.Y));
                if (segments.ContainsKey(position.X))
                {
                    segments[position.X].Add(newSegment);
                }
                else
                {
                    segments[position.X] = new List<VerticalLineSegment> { newSegment };
                }
            }

            min = new Vector2Long(Math.Min(min.X, nextPosition.X), Math.Min(min.Y, nextPosition.Y));
            max = new Vector2Long(Math.Max(max.X, nextPosition.X), Math.Max(max.Y, nextPosition.Y));

            position = nextPosition;
        }

        return segments;
    }

    BigInteger CountTilesWithinBorders(SortedDictionary<long, List<VerticalLineSegment>> segmentsListsDictionary, Vector2Long min, Vector2Long max)
    {
        BigInteger result = 0;

        VerticalLineSegment lastIntersectedLineSegment;
        VerticalLineSegment currentLineSegment;
        bool isInside;
        bool isOnBorder;

        for (long y = min.Y; y <= max.Y; y++)
        {
            //Console.Write($"{result,5}:");

            isInside = false;
            isOnBorder = false;
            lastIntersectedLineSegment = null;

            foreach(KeyValuePair<long, List<VerticalLineSegment>> segmentsList in segmentsListsDictionary)
            {
                currentLineSegment = segmentsList.Value.Find(seg => seg.Intersects(y));

                if(currentLineSegment == null)
                {
                    continue;
                }

                if(isInside || isOnBorder)
                {
                    result += currentLineSegment.X - lastIntersectedLineSegment.X;
                    //Console.BackgroundColor = ConsoleColor.DarkRed;
                    //Console.Write(new string('o', (int)(currentLineSegment.X - lastIntersectedLineSegment.X - 1)));
                }
                else // isOutside
                {
                    result += 1;
                    //Console.BackgroundColor = ConsoleColor.Black;
                    //Console.Write(new string(' ', (int)(lastIntersectedLineSegment == null ? currentLineSegment.X - min.X :  currentLineSegment.X - lastIntersectedLineSegment.X - 1)));
                }
                //Console.BackgroundColor = ConsoleColor.DarkGray;
                //Console.Write("#");

                if (y != currentLineSegment.MinY && y != currentLineSegment.MaxY)
                {
                    isInside = !isInside;
                }
                else
                {
                    if (isOnBorder && 
                        ((lastIntersectedLineSegment.MaxY == y && currentLineSegment.MinY == y) || (lastIntersectedLineSegment.MinY == y && currentLineSegment.MaxY == y)))
                    {
                        isInside = !isInside;
                    }
                    isOnBorder = !isOnBorder;
                }

                lastIntersectedLineSegment = currentLineSegment;
            }
            //Console.BackgroundColor = ConsoleColor.Black;
            //Console.Write('.');
            //Console.WriteLine();
        }

        return result;
    }

    public enum Direction
    {
        Left, Right, Up, Down
    }

    int GetDistanceFromInputLineForPartOne(string inputLine)
    {
        return int.Parse(inputLine.Split(' ')[1]);
    }

    Vector2Long GetDirectionFromInputLineForPartOne(string inputLine)
    {
        string[] split = inputLine.Split(' ');

        switch (split[0])
        {
            default:
            case "L": return Vector2Long.Left;
            case "R": return Vector2Long.Right;
            case "U": return Vector2Long.Up;
            case "D": return Vector2Long.Down;
        }
    }

    int GetDistanceFromInputLineForPartTwo(string inputLine)
    {
        return int.Parse(inputLine.Split(' ')[2].Substring(2,5),System.Globalization.NumberStyles.HexNumber );
    }

    Vector2Long GetDirectionFromInputLineForPartTwo(string inputLine)
    {
        char codedDirection = inputLine.Split(' ')[2][7];

        switch (codedDirection)
        {
            default:
            case '2': return Vector2Long.Left;
            case '0': return Vector2Long.Right;
            case '3': return Vector2Long.Up;
            case '1': return Vector2Long.Down;
        }
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
