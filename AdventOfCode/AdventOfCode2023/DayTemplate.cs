using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface DaySolver
{
    long SolvePartOne(List<string> inputLines);

    long SolvePartTwo(List<string> inputLines);
}

public interface DayInitializer
{
    void Initialize(List<string> inputLines);
}

