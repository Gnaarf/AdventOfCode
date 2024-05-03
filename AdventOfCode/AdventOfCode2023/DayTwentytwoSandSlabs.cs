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
using System.Dynamic;

public class DayTwentytwoSandSlabs : DaySolver, DayInitializer
{
    public void Initialize(List<string> inputLines)
    {
    }

    public long SolvePartOne(List<string> inputLines)
    {
        Map map = new Map(inputLines);
        
        map.MoveAllSlabsDown();

        return map.CountSafelyDisintegratableSlabs() ;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        Map map = new Map(inputLines);

        return -1;
    }

    class Map
    {
        List<Slab> _slabs = new List<Slab>();
        HashSet<Slab> _movedSlabs = new HashSet<Slab>();

        Slab[,,] Space;

        public Map(List<string> inputLines)
        {
            List<int> min = new List<int>() { int.MaxValue, int.MaxValue, int.MaxValue };
            List<int> max = new List<int>() { int.MinValue, int.MinValue, int.MinValue };

            foreach (string line in inputLines)
            {
                string[] lineParts = line.Split(',', '~');

                Vector3Int startPoint = new Vector3Int(int.Parse(lineParts[0]), int.Parse(lineParts[1]), int.Parse(lineParts[2]));
                Vector3Int endPoint = new Vector3Int(int.Parse(lineParts[3]), int.Parse(lineParts[4]), int.Parse(lineParts[5]));

                Slab slab = new Slab(startPoint, endPoint);
                _slabs.Add(slab);

                min[0] = Math.Min(Math.Min(min[0], int.Parse(lineParts[0])), int.Parse(lineParts[3]));
                min[1] = Math.Min(Math.Min(min[1], int.Parse(lineParts[1])), int.Parse(lineParts[4]));
                min[2] = Math.Min(Math.Min(min[2], int.Parse(lineParts[2])), int.Parse(lineParts[5]));

                max[0] = Math.Max(Math.Max(max[0], int.Parse(lineParts[0])), int.Parse(lineParts[3]));
                max[1] = Math.Max(Math.Max(max[1], int.Parse(lineParts[1])), int.Parse(lineParts[4]));
                max[2] = Math.Max(Math.Max(max[2], int.Parse(lineParts[2])), int.Parse(lineParts[5]));
            }

            _slabs.Sort((s1, s2) => s1.StartPoint.Z - s2.StartPoint.Z);

            Console.WriteLine($"min: ({min[0]}, {min[1]}, {min[2]})");
            Console.WriteLine($"max: ({max[0]}, {max[1]}, {max[2]})");

            Space = new Slab[max[0]+ 1 , max[1] + 1, max[2] + 1];

            foreach(Slab slab in _slabs)
            {
                InsertSlab(slab);
            }
        }

        void InsertSlab(Slab slab)
        {
            foreach (Vector3Int slabVoxel in slab.GetAllVoxels())
            {
                Space[slabVoxel.X, slabVoxel.Y, slabVoxel.Z] = slab;
            }
        }

        void RemoveSlab(Slab slab)
        {
            foreach (Vector3Int slabVoxel in slab.GetAllVoxels())
            {
                if (Space[slabVoxel.X, slabVoxel.Y, slabVoxel.Z] == slab)
                {
                    Space[slabVoxel.X, slabVoxel.Y, slabVoxel.Z] = null;
                }
            }
            _movedSlabs.Add(slab);
        }

        void MoveSlab(Slab slab, Vector3Int move)
        {
            if (move != Vector3Int.Zero)
            {
                return;
            }
            RemoveSlab(slab);
            slab.StartPoint += move;
            slab.EndPoint += move;
            InsertSlab(slab);
            _movedSlabs.Add(slab);
        }

        void ResetSlab(Slab slab)
        {
            MoveSlab(slab, slab.InitialStartPoint - slab.StartPoint);
        }

        void ResetAllSlabs()
        {
            foreach (Slab slab in _movedSlabs)
            {
                ResetSlab(slab);
            }
        }

        bool IsEmptyBelow(Slab slab)
        {
            foreach (Vector3Int belowVoxel in slab.GetAllVoxelsBelow())
            {
                if (belowVoxel.Z < 0 || Space[belowVoxel.X, belowVoxel.Y, belowVoxel.Z] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public void MoveAllSlabsDown()
        {
            MoveAllSlabsDown(out int tmp);
            return;
        }

        public void MoveAllSlabsDown(out int movedSlabsCount)
        {
            movedSlabsCount = 0;
            foreach(Slab slab in _slabs)
            {
                if(IsEmptyBelow(slab))
                {
                    movedSlabsCount++;
                    RemoveSlab(slab);
                    do
                    {
                        slab.StartPoint += Vector3Int.Down;
                        slab.EndPoint += Vector3Int.Down;
                    } while (IsEmptyBelow(slab));
                    InsertSlab(slab);
                }
            }
        }

        public int CountSafelyDisintegratableSlabs()
        {
            int result = 0;

            foreach(Slab slab in _slabs)
            {
                Console.WriteLine("current slab: " + slab.ToString() + ", " + slab.GetHashCode());

                List<Vector3Int> voxelsAbove = slab.GetAllVoxelsAbove();
                RemoveSlab(slab);

                List<Slab> slabsAbove = new List<Slab>();
                bool isSafeToDisintegrate = true;

                foreach(Vector3Int voxelAbove in voxelsAbove)
                {
                    Slab slabAbove = Space[voxelAbove.X, voxelAbove.Y, voxelAbove.Z];

                    if (slabAbove != null && !slabsAbove.Contains(slabAbove))
                    {
                        slabsAbove.Add(slabAbove);

                        if(IsEmptyBelow(slabAbove))
                        {
                            isSafeToDisintegrate = false;
                            break;
                        }
                    }
                }
                result += isSafeToDisintegrate ? 1 : 0;
                if (isSafeToDisintegrate) Console.WriteLine("                                       ==> is safe");

                InsertSlab(slab);
            }
            return result;
        }

        public long CountFallingSlabsWhenIndividuallyRemovingEverySingleSlab()
        {
            long result = 0;

            return result;
        }
    }

    class Slab
    {
        public Vector3Int StartPoint, EndPoint;
        public readonly Vector3Int InitialStartPoint, InitialEndPoint;
        Vector3Int _startToEndVector;

        public Slab(Vector3Int startPoint, Vector3Int endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;

            if (endPoint.X < startPoint.X || endPoint.Y < startPoint.Y || endPoint.Z < startPoint.Z)
            {
                this.StartPoint = endPoint;
                this.EndPoint = startPoint;
            }

            InitialStartPoint = this.StartPoint;
            InitialEndPoint = this.EndPoint;

            _startToEndVector = (endPoint - startPoint);
            if (_startToEndVector != Vector3Int.Zero)
            {
                _startToEndVector /= _startToEndVector.Length;
            }
        }

        public string ToString()
        {
            return $"Slab[{StartPoint}, {EndPoint}]";
        }

        public List<Vector3Int> GetAllVoxels()
        {
            List<Vector3Int> result = new List<Vector3Int>();
            Vector3Int currentPoint = StartPoint;

            while (currentPoint != EndPoint)
            {
                result.Add(currentPoint);
                currentPoint += _startToEndVector;
            }
            result.Add(EndPoint);

            return result;
        }
        public List<Vector3Int> GetAllVoxelsBelow()
        {
            if (StartPoint.X == EndPoint.X && StartPoint.Y == EndPoint.Y)
            {
                return new List<Vector3Int>() { StartPoint + Vector3Int.Down };
            }

            StartPoint += Vector3Int.Down;
            EndPoint += Vector3Int.Down;

            List<Vector3Int> result = GetAllVoxels();

            StartPoint += Vector3Int.Up;
            EndPoint += Vector3Int.Up;

            return result;
        }

        public List<Vector3Int> GetAllVoxelsAbove()
        {
            if (StartPoint.X == EndPoint.X && StartPoint.Y == EndPoint.Y)
            {
                return new List<Vector3Int>() { EndPoint + Vector3Int.Up };
            }

            StartPoint += Vector3Int.Up;
            EndPoint += Vector3Int.Up;

            List<Vector3Int> result = GetAllVoxels();

            StartPoint += Vector3Int.Down;
            EndPoint += Vector3Int.Down;

            return result;
        }

    }
}
