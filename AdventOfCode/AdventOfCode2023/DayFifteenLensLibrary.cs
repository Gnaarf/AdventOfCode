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

public class DayFifteenLensLibrary : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        string[] splitStrings = inputLines[0].Split(',');

        foreach(string sequence in splitStrings)
        {
            result += GetHASH(sequence);
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        List<List<KeyValuePair<string, int>>> hashMap = new List<List<KeyValuePair<string, int>>>(256);

        for (int i = 0; i < 256; i++)
        {
            hashMap.Add(new List<KeyValuePair<string, int>>());
        }

        string[] splitStrings = inputLines[0].Split(',');

        foreach (string command in splitStrings)
        {
            if (command.Last() == '-')
            {
                string key = command.Substring(0, command.Length - 1);
                List<KeyValuePair<string, int>> hashMapSubList = hashMap[GetHASH(key)];
                int index = hashMapSubList.FindIndex(x => x.Key == key);
                if (index > -1)
                {
                    hashMapSubList.RemoveAt(hashMapSubList.FindIndex(x => x.Key == key));
                }
            }
            else
            {
                string key = command.Substring(0, command.Length - 2);
                int value = command.Last() - '0';

                List<KeyValuePair<string, int>> hashMapSubList = hashMap[GetHASH(key)];

                int index = hashMapSubList.FindIndex(x => x.Key == key);
                if (index > -1)
                {
                    hashMapSubList[index] = new KeyValuePair<string, int> (key, value);
                }
                else
                {
                    hashMapSubList.Add(new KeyValuePair<string, int>(key, value));
                }
            }
        }

        long result = 0;

        for (int i = 0; i < hashMap.Count; i++)
        {
            for (int j = 0; j < hashMap[i].Count; j++)
            {
                result += (i + 1) * (j + 1) * hashMap[i][j].Value;
            }
        }

        return result;
    }

    private static int GetHASH(string key)
    {
        int splitResult = 0;
        for (int i = 0; i < key.Length; i++)
        {
            splitResult = (splitResult + (int)key[i]) * 17 % 256;
        }
        return splitResult;
    }
}
