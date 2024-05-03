using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

public class DaySeventeenClumsyCrucible : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        Map map = new Map(inputLines);

        Vector2Int destinationPosition = map.GetDimensions() - Vector2Int.One;
        Heap<Path> paths = new Heap<Path>();
        List<Path> shortestPaths = new List<Path>();

        paths.Insert(new Path(Vector2Int.Zero, new List<Direction>(), 0));

        while (shortestPaths.Count < 3)
        {
            Path currentPath = paths.Poll();

            if (currentPath.Position == destinationPosition)
            {
                    shortestPaths.Add(currentPath);
            }
            else
            {
                foreach(Direction direction in currentPath.GetNextPossibleDirectionsForPartOne())
                {
                    Vector2Int nextPosition = GetAdjacentPosition(currentPath.Position, direction);

                    if(nextPosition.X >= 0 &&  nextPosition.Y >= 0 && nextPosition.X < inputLines[0].Length && nextPosition.Y < inputLines.Count)
                    {
                        int newTotalCost = currentPath.TotalCost + map.GetTile(nextPosition).Cost;
                        Path newPath = new Path(nextPosition, new List<Direction>(currentPath.DirectionHistory) { direction }, newTotalCost);

                        map.UpdateShortestPath(nextPosition, newPath, out bool shortestPathWasUpdated);

                        if (shortestPathWasUpdated)
                        {
                            paths.Insert(newPath);
                        }
                    }
                }
            }
        }

        return shortestPaths[0].TotalCost;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        Map map = new Map(inputLines);
        
        Vector2Int destinationPosition = map.GetDimensions() - Vector2Int.One;
        Heap<Path> paths = new Heap<Path>();
        List<Path> shortestPaths = new List<Path>();

        paths.Insert(new Path(Vector2Int.Zero, new List<Direction>(), 0));

        while (shortestPaths.Count < 3)
        {
            Path currentPath = paths.Poll();

            //if(currentPath.Position == new Vector2Int(10,0))
            //{
            //    Console.SetCursorPosition(0, 0);
            //    Console.WriteLine("current path cost = " + currentPath.TotalCost);
            //    map.DrawPath(currentPath.DirectionHistory);
            //}

            if (currentPath.Position == destinationPosition)
            {
                shortestPaths.Add(currentPath);
            }
            else
            {
                foreach (Direction direction in currentPath.GetNextPossibleDirectionsForPartTwo())
                {
                    Vector2Int nextPosition = GetAdjacentPosition(currentPath.Position, direction);

                    if (nextPosition.X >= 0 && nextPosition.Y >= 0 && nextPosition.X < inputLines[0].Length && nextPosition.Y < inputLines.Count)
                    {
                        int newTotalCost = currentPath.TotalCost + map.GetTile(nextPosition).Cost;
                        Path newPath = new Path(nextPosition, new List<Direction>(currentPath.DirectionHistory) { direction }, newTotalCost);

                        map.UpdateShortestPath(nextPosition, newPath, out bool shortestPathWasUpdated);

                        if (shortestPathWasUpdated)
                        {
                            paths.Insert(newPath);
                        }
                    }
                }
            }
        }

        return shortestPaths[0].TotalCost;
    }

    static Vector2Int GetAdjacentPosition(Vector2Int position, Direction direction)
    {
        switch (direction)
        {
            default:
            case Direction.Left: return position + Vector2Int.Left;
            case Direction.Right: return position + Vector2Int.Right;
            case Direction.Up: return position + Vector2Int.Up;
            case Direction.Down: return position + Vector2Int.Down;
        }
    }

    public enum Direction
    {
        Left, Right, Up, Down
    }

    class Map
    {
        Tile[,] tiles;

        public Map(List<string> inputLines)
        {
            tiles = new Tile[inputLines[0].Length, inputLines.Count];

            for (int y = 0; y < inputLines.Count; y++)
            {
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    tiles[x, y] = new Tile(inputLines[y][x] - '0');
                }
            }
        }

        public Vector2Int GetDimensions()
        {
            return new Vector2Int(tiles.GetLength(0), tiles.GetLength(1));
        }

        public Tile GetTile(Vector2Int position)
        {
            return tiles[position.X, position.Y];
        }

        public void UpdateShortestPath(Vector2Int position, Path newPath, out bool shortestPathWasUpdated)
        {
            Tile tile = GetTile(position);
            shortestPathWasUpdated = false;

            Vector2Int streakStartPosition = position;
            int index = newPath.DirectionHistory.Count - 1;
            while (index >= 0 && newPath.DirectionHistory[index] == newPath.DirectionHistory.Last())
            {
                streakStartPosition = GetAdjacentPosition(streakStartPosition, newPath.DirectionHistory.Last().GetInverseDirection());
                index--;
            }

            bool c = tile.ShortestPathsCosts.ContainsKey(streakStartPosition);


            if (!tile.ShortestPathsCosts.ContainsKey(streakStartPosition) || tile.ShortestPathsCosts[streakStartPosition] > newPath.TotalCost)
            {
                tile.ShortestPathsCosts[streakStartPosition] = newPath.TotalCost;
                shortestPathWasUpdated = true;
            }
        }

        public class Tile
        {
            public int Cost;
            public Dictionary<Vector2Int, int> ShortestPathsCosts = new Dictionary<Vector2Int, int>();

            public Tile(int cost)
            {
                this.Cost = cost;
            }
        }

       public void DrawPath(List<Direction> directions)
        {
            List<Vector2Int> path = new List<Vector2Int>() { Vector2Int.Zero };

            foreach (Direction direction in directions)
            {
                path.Add(GetAdjacentPosition(path.Last(), direction));
            }

            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                for (int x = 0; x < tiles.GetLength(0); x++)
                {
                    Console.BackgroundColor = path.Any(vec => vec == new Vector2Int(x, y)) ? ConsoleColor.DarkRed : ConsoleColor.Black;
                    Console.Write(tiles[x,y].Cost);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    class Path : IComparable<Path>, IComparable
    {
        public Vector2Int Position;
        public List<Direction> DirectionHistory;
        public int TotalCost;

        public Path(Vector2Int position, List<Direction> directionHistory, int totalCost)
        {
            this.Position = position;
            this.DirectionHistory = directionHistory;
            this.TotalCost = totalCost;
        }

        public List<Direction> GetNextPossibleDirectionsForPartOne()
        {
            if (DirectionHistory.Count == 0)
            {
                return new List<Direction>() { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
            }
            else
            {
                List<Direction> possibleDirections = DirectionHistory.Last().GetPerpendicularDirections();

                if (DirectionHistory.Count < 3 ||
                   DirectionHistory[DirectionHistory.Count - 1] != DirectionHistory[DirectionHistory.Count - 2] ||
                   DirectionHistory[DirectionHistory.Count - 1] != DirectionHistory[DirectionHistory.Count - 3])
                {
                    possibleDirections.Add(DirectionHistory[DirectionHistory.Count - 1]);
                }
                return possibleDirections;
            }
        }

        public List<Direction> GetNextPossibleDirectionsForPartTwo()
        {
            if (DirectionHistory.Count == 0)
            {
                return new List<Direction>() { Direction.Left, Direction.Right, Direction.Up, Direction.Down };
            }
            else
            {
                int lastConsecutiveMovementCount = 0;
                int index = DirectionHistory.Count - 1;

                while (index >= 0 && DirectionHistory[index] == DirectionHistory.Last())
                {
                    lastConsecutiveMovementCount++;
                    index--;
                }

                if (lastConsecutiveMovementCount < 4)
                {
                    return new List<Direction>() { DirectionHistory.Last() };
                }
                else if(lastConsecutiveMovementCount < 10)
                {
                    return new List<Direction>(DirectionHistory.Last().GetPerpendicularDirections()) { DirectionHistory.Last() };
                }
                else
                {
                    return DirectionHistory.Last().GetPerpendicularDirections();
                }
            }
        }

        public int CompareTo(Path other)
        {
            if (TotalCost != other.TotalCost)
            {
                return other.TotalCost - TotalCost;
            }
            else if(DirectionHistory.Count != other.DirectionHistory.Count)
            {
                return other.DirectionHistory.Count - DirectionHistory.Count;
            }
            else
            {
                return GetHashCode() - other.GetHashCode();
            }
        }

        public int CompareTo(object obj)
        {
            if(obj is Path)
            {
                return CompareTo((Path)obj);
            }
            return GetHashCode() - obj.GetHashCode();
        }
    }
}

static class DirectionExtension
{
    public static List<DaySeventeenClumsyCrucible.Direction> GetPerpendicularDirections(this DaySeventeenClumsyCrucible.Direction direction)
    {
        switch (direction)
        {
            case DaySeventeenClumsyCrucible.Direction.Left:
                return new List<DaySeventeenClumsyCrucible.Direction>() { DaySeventeenClumsyCrucible.Direction.Down, DaySeventeenClumsyCrucible.Direction.Up };
            case DaySeventeenClumsyCrucible.Direction.Right:
                return new List<DaySeventeenClumsyCrucible.Direction>() { DaySeventeenClumsyCrucible.Direction.Up, DaySeventeenClumsyCrucible.Direction.Down };
            case DaySeventeenClumsyCrucible.Direction.Up:
                return new List<DaySeventeenClumsyCrucible.Direction>() { DaySeventeenClumsyCrucible.Direction.Left, DaySeventeenClumsyCrucible.Direction.Right };
            case DaySeventeenClumsyCrucible.Direction.Down:
                return new List<DaySeventeenClumsyCrucible.Direction>() { DaySeventeenClumsyCrucible.Direction.Right, DaySeventeenClumsyCrucible.Direction.Left };
            default:
                return new List<DaySeventeenClumsyCrucible.Direction>();
        }
    }
    public static DaySeventeenClumsyCrucible.Direction GetInverseDirection(this DaySeventeenClumsyCrucible.Direction direction)
    {
        switch (direction)
        {
            default:
            case DaySeventeenClumsyCrucible.Direction.Left:
                return DaySeventeenClumsyCrucible.Direction.Right;
            case DaySeventeenClumsyCrucible.Direction.Right:
                return DaySeventeenClumsyCrucible.Direction.Left;
            case DaySeventeenClumsyCrucible.Direction.Up:
                return DaySeventeenClumsyCrucible.Direction.Down;
            case DaySeventeenClumsyCrucible.Direction.Down:
                return DaySeventeenClumsyCrucible.Direction.Up;
        }
    }
}

