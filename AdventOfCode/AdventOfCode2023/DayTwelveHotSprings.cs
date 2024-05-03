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

public class DayTwelveHotSprings : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        string[] lineSplit;
        string springRecord;
        List<int> sequenceRecord;

        long possibilities;

        //int index = 0;
        foreach (string inputLine in inputLines)
        {
            lineSplit = inputLine.Split(' ');
            springRecord = lineSplit[0];
            sequenceRecord = Helper.ExtractNumbers(lineSplit[1], ',');
            
            possibilities = CountSequenceFits(springRecord, sequenceRecord, false);

            result += possibilities;

            //Console.WriteLine();
            //Console.WriteLine($"{inputLine} --- line ({index++}) end: {possibilities} => result: {result}");
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        string[] lineSplit;
        string springRecord;
        List<int> sequenceRecord;

        long possibilities;

        //int index = 0;
        foreach (string inputLine in inputLines)
        {
            lineSplit = inputLine.Split(' ');
            springRecord = lineSplit[0] + "?" + lineSplit[0] + "?" + lineSplit[0] + "?" + lineSplit[0] + "?" + lineSplit[0];
            sequenceRecord = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                sequenceRecord.AddRange(Helper.ExtractNumbers(lineSplit[1], ','));
            }

            possibilities = CountSequenceFits(springRecord, sequenceRecord, false);

            result += possibilities;

            //Console.WriteLine();
            //string sequenceRecordAsString = sequenceRecord.ConvertAll(x => x + ", ").Aggregate((x, y) => x + y);
            //Console.WriteLine($"{inputLine, 32} --- line ({index++}) end: {possibilities} => result: {result}");
        }

        return result;
    }

    struct SpringData
    {
        public static Dictionary<string, long> sequenceFitLookup = new Dictionary<string, long>();
        public static string originalSpringRecord;
        public static List<int> originalSequenceRecord;

        public string springRecord;
        public string recreatedSpringRecord;
        public List<int> sequenceRecord;

        public string Key { get { return springRecord + (sequenceRecord.Count > 0 ? sequenceRecord.ConvertAll(x => " " + x).Aggregate((x, y) => x + y) : ""); } }

        public bool TryGetCachedValue(out long value)
        {
            return SpringData.sequenceFitLookup.TryGetValue(Key, out value);
        }

        public void CacheValue(long value)
        {
            SpringData.sequenceFitLookup.Add(Key, value);
        }
    }

    public long CountSequenceFits(string springRecord, List<int> sequenceRecord, bool enableDebugOutput)
    {
        SpringData.sequenceFitLookup.Clear();
        SpringData.originalSpringRecord = springRecord;
        SpringData.originalSequenceRecord = new List<int>(sequenceRecord);

        SpringData springData = new SpringData()
        {
            springRecord = springRecord,
            recreatedSpringRecord = "",
            sequenceRecord = new List<int>(sequenceRecord)
        };

        long result = CountSequenceFitsRecursion(springData, enableDebugOutput); ;
        if (enableDebugOutput) { Console.WriteLine(); }
        return result;
    }

    long CountSequenceFitsRecursion(SpringData data, bool enableDebugOutput)
    {
        if (enableDebugOutput)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(data.recreatedSpringRecord);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{data.springRecord}");
            Console.Write(data.sequenceRecord.Count > 0 ? data.sequenceRecord.ConvertAll(x => " " + x).Aggregate((x, y) => x + y) : "");
        }

        if(data.TryGetCachedValue(out long cachedValue))
        {
            return cachedValue;
        }

        int minSequenceLength = data.sequenceRecord.Sum() + data.sequenceRecord.Count - 1;
        if(data.springRecord.Length < minSequenceLength)
        {
            return 0;
        }
        else if(data.sequenceRecord.Count == 0)
        {

            if (enableDebugOutput && !data.springRecord.Contains('#')) { Console.Write(" -- found one!"); }
            return data.springRecord.Contains('#') ? 0 : 1;
        }
        else if(data.springRecord.Length == minSequenceLength)
        {
            if (enableDebugOutput && DoesRecreatedRecordFit(data.springRecord, ConstructMinimalSpringRecordFromSequenceRecord(data.sequenceRecord))) { Console.Write(" -- found one!"); }
            return DoesRecreatedRecordFit(data.springRecord, ConstructMinimalSpringRecordFromSequenceRecord(data.sequenceRecord)) ? 1 : 0;
        }
        else
        {
            long result = 0;

            // case: skip the next '?' or sequence of dots
            if (data.springRecord[0] != '#') 
            {
                int leadingDotsCount = data.springRecord.IndexOfAny(new[] { '#', '?' });

                SpringData newDataForSkipCase = new SpringData()
                {
                    springRecord = data.springRecord.Substring(Math.Max(1, leadingDotsCount)),
                    recreatedSpringRecord = data.recreatedSpringRecord + new string('.', Math.Max(1, leadingDotsCount)),
                    sequenceRecord = new List<int>(data.sequenceRecord)
                };
                result += CountSequenceFitsRecursion(newDataForSkipCase, enableDebugOutput);
            }

            // case: lock the first sequence
            char charAtPosOneAfterSequence = data.springRecord[data.sequenceRecord[0]];
            if (data.springRecord.Substring(0, data.sequenceRecord[0]).All(x => x == '?' || x == '#') && (charAtPosOneAfterSequence == '.' || charAtPosOneAfterSequence == '?'))
            {
                SpringData newDataForLockCase = new SpringData()
                {
                    springRecord = data.springRecord.Substring(data.sequenceRecord[0] + 1),
                    recreatedSpringRecord = data.recreatedSpringRecord + new string('#', data.sequenceRecord[0]) + '.',
                    sequenceRecord = data.sequenceRecord.GetRange(1, data.sequenceRecord.Count - 1)
                };
                result += CountSequenceFitsRecursion(newDataForLockCase, enableDebugOutput);
            }

            if(!data.TryGetCachedValue(out long tmp))
            {
                data.CacheValue(result);
            }

            return result;
        }
    }

    string ConstructMinimalSpringRecordFromSequenceRecord(List<int> sequenceRecord)
    {
        return string.Concat(sequenceRecord.ConvertAll(x => new string('#', x) + '.')).TrimEnd('.');
    }

    public bool DoesRecreatedRecordFit(string springRecord, string recreatedRecord)
    {
        if (springRecord.Length != recreatedRecord.Length)
        {
            return false;
        }

        for (int i = 0; i < springRecord.Length; i++)
        {
            if (springRecord[i] != '?' && springRecord[i] != recreatedRecord[i])
            {
                return false;
            }
        }

        return true;
    }
}
