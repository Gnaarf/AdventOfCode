using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DayEightHauntedWasteland : DaySolver, DayInitializer
{
    string instruction;
    Dictionary<string, Node> nodes = new Dictionary<string, Node>();

    public void Initialize(List<string> inputLines)
    {
        instruction = inputLines[0];

        for (int i = 2; i < inputLines.Count; ++i)
        {
            string[] lineSplit = inputLines[i].RemoveChar(' ', '(', ')').Split('=', ',');
            nodes.Add(lineSplit[0], new Node(lineSplit[0], lineSplit[1], lineSplit[2]));
        }
    }

    public long SolvePartOne(List<string> inputLines)
    {
        return GetPathLength("AAA", new Predicate<string>(x => x == "ZZZ"));
    }

    long GetPathLength(string startNode, Predicate<string> endNodeCondition)
    {
        long stepCount = 0;
        int instructionIndex = 0;
        string currentNode = startNode;

        while (!endNodeCondition(currentNode))
        {
            currentNode = instruction[instructionIndex % instruction.Length] == 'R' ? nodes[currentNode].Right : nodes[currentNode].Left;
            instructionIndex++;
            stepCount++;
        }
        return stepCount;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        List<string> currentNodes = new List<string>();

        foreach (var node in nodes)
        {
            if (node.Value.Label.EndsWith("A"))
            {
                currentNodes.Add(node.Value.Label);
            }
        }

        List<long> cycleLengths = new List<long>();

        for (int i = 0; i < currentNodes.Count; i++)
        {
            cycleLengths.Add(GetPathLength(currentNodes[i], new Predicate<string>(x => x.EndsWith("Z"))));
        }

        return LeastCommonMultiple(cycleLengths.ToArray());
    }

    struct Node
    {
        public string Label;
        public string Left;
        public string Right;

        public Node (string label, string left, string right)
        {
            Label = label;
            Left = left;
            Right = right;
        }
    }

    static long GreatestCommonFactor(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long LeastCommonMultiple(long a, long b)
    {
        return (a / GreatestCommonFactor(a, b)) * b;
    }

    static long LeastCommonMultiple(params long[] a)
    {
        long result = a[0];
        for(int i = 1; i < a.Length; i++)
        {
            result = LeastCommonMultiple(result, a[i]);
        }
        return result;
    }
}
