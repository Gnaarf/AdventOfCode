using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public static class Helper
{
    public static bool IsNumber(char c)
    {
        return c >= '0' && c <= '9';
    }
    public static bool IsNumber(char c, out int number)
    {
        number = c - '0';
        return c >= '0' && c <= '9';
    }

    public static string ConcatenateNumberFromAdjacentChars(string s, int index)
    {
        if (!IsNumber(s[index]))
        {
            throw new Exception($"string does not have a number at index {index}. (Input: \"{s}\"");
        }

        string result = s[index].ToString();

        int i = index - 1;
        while (i >= 0 && IsNumber(s[i]))
        {
            result = s[i] + result;
            i--;
        }
        i = index + 1;
        while (i < s.Length && IsNumber(s[i]))
        {
            result = result + s[i];
            i++;
        }
        return result;
    }

    public static List<int> ExtractNumbers(string s, params char[] speparator)
    {
        return ExtractNumbers(s, 0, speparator);
    }

    public static List<int> ExtractNumbers(string s, int startIndex, params char[] separator)
    {
        string[] splits = s.Substring(startIndex).Split(separator);
        List<int> seedIndices = new List<int>();
        for (int i = 0; i < splits.Length; i++)
        {
            if (splits[i] != "")
            {
                seedIndices.Add(int.Parse(splits[i]));
            }
        }

        return seedIndices;
    }

    public static string RemoveChar(this string str, params char[] c)
    {
        string result = "";
        for (int i = 0; i < str.Length; ++i)
        {
            if (!c.Contains(str[i]))
            {
                result += str[i];
            }
        }
        return result;
    }

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
       => self.Select((item, index) => (item, index));

    public static T Get<T>(this T[,] array, Vector2Int pos)
    {
        return array[pos.X, pos.Y];
    }
    public static void Set<T>(this T[,] array, Vector2Int pos, T newValue)
    {
        array[pos.X, pos.Y] = newValue;
    }
}
