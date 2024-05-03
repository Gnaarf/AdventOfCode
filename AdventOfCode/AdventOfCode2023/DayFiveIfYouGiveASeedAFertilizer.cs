using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DayFiveIfYouGiveASeedAFertilizer : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        List<uint> seedIndices = ExtractSeedIndicesPartOne(inputLines[0]);

        List<Map> maps = SetupMaps(inputLines);

        long result = long.MaxValue;
        
        foreach(uint index in seedIndices)
        {
            result = Math.Min(result, RunIndexThroughMaps(index, maps));
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        string[] firstInputLineSplits = inputLines[0].Split(' ');

        List<Map> maps = SetupMaps(inputLines);

        uint result = uint.MaxValue;

        for (uint i = 1; i < firstInputLineSplits.Length; i += 2)
        {
            uint startValue = uint.Parse(firstInputLineSplits[i]);
            uint range = uint.Parse(firstInputLineSplits[i + 1]);

            Console.WriteLine($"starting batch nr {i / 2 + 1} with {firstInputLineSplits[i + 1]} entries");

            for (uint r = 0; r < range; r++)
            {
                if (r % 1000000 == 0) { Console.WriteLine("now at entry " + r); }

                result = Math.Min(result, RunIndexThroughMaps(startValue + r, maps));
            }
        }

        return result;
    }

    private uint RunIndexThroughMaps(uint index, List<Map> maps)
    {
        for (int i = 0; i < maps.Count; i++)
        {
            index = maps[i].GetMappedValue(index);
        }
        return index;
    }

    private List<Map> SetupMaps(List<string> inputLines)
    {
        List<MapInterval> mapIntervals = new List<MapInterval>();
        List<Map> maps = new List<Map>();

        for (int currentLineIndex = 3; currentLineIndex < inputLines.Count; currentLineIndex++)
        {
            if (inputLines[currentLineIndex] == "")
            {
                maps.Add(new Map(mapIntervals));
                mapIntervals = new List<MapInterval>();
                currentLineIndex++; // skip the line that says "xxx-to-yyy map"
            }
            else
            {
                string[] mapValueStrings = inputLines[currentLineIndex].Split(' ');
                mapIntervals.Add(new MapInterval(uint.Parse(mapValueStrings[1]), uint.Parse(mapValueStrings[0]), uint.Parse(mapValueStrings[2])));
            }
        }

        return maps;
    }

    private List<uint> ExtractSeedIndicesPartOne(string inputLine)
    {
        string[] firstInputLineSplits = inputLine.Split(' ');
        List<uint> seedIndices = new List<uint>();
        for (uint i = 1; i < firstInputLineSplits.Length; i++)
        {
            seedIndices.Add(uint.Parse(firstInputLineSplits[i]));
        }
        return seedIndices;
    }

    class Map
    {
        List<MapInterval> _mapIntervals;

        public Map(List<MapInterval> mapIntervals) 
        {
            _mapIntervals = mapIntervals.ToList();
        }

        public uint GetMappedValue(uint value)
        {
            MapInterval mapInterval = _mapIntervals.Find(x => x.IsInRange(value));

            return mapInterval != null ? mapInterval.GetMappedValue(value) : value;
        }
    }

    class MapInterval
    {
        uint _sourceRangeStart;
        uint _destinationRangeStart; 
        uint _rangeLength;

        public MapInterval(uint sourceRangeStart, uint destinationRangeStart, uint rangeLength)
        {
            _sourceRangeStart = sourceRangeStart;
            _destinationRangeStart = destinationRangeStart;
            _rangeLength = rangeLength;
        }

        public bool IsInRange(uint value)
        {
            return _sourceRangeStart <= value && value < _sourceRangeStart + _rangeLength;
        }

        public uint GetMappedValue(uint value)
        {
            return value + _destinationRangeStart - _sourceRangeStart;
        }

    }
}
