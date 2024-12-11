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

public class DayTwoRedNosedReports : DaySolver
{

    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        foreach (string line in inputLines)
        {
            List<int> reportSequence = Helper.ExtractNumbers(line);
            if (IsReportSequenceSafe(reportSequence))
            {
                result++;
            }
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        foreach (string line in inputLines)
        {
            List<int> reportSequence = Helper.ExtractNumbers(line);
            List<List<int>> reportSequenceList = new List<List<int>>();
            reportSequenceList.Add(reportSequence);
            for (int i = 0; i < reportSequence.Count; i++)
            {
                List<int> reportSequenceWithOneSequenceMissing = new List<int>(reportSequence);
                reportSequenceWithOneSequenceMissing.RemoveAt(i);
                reportSequenceList.Add(reportSequenceWithOneSequenceMissing);
            }

            foreach (List<int> modifiedReportSequence in reportSequenceList)
            {
                if (IsReportSequenceSafe(modifiedReportSequence))
                {
                    result++;
                    break;
                }
            }
        }

        return result;
    }

    private bool IsReportSequenceSafe(List<int> reports)
    {
        bool isIncreasing = reports[0] < reports[1];
        bool isSafe = true;
        for (int i = 1; i < reports.Count; i++)
        {
            int difference = reports[i - 1] - reports[i];
            if (difference == 0 ||
                Math.Abs(difference) > 3 ||
                (isIncreasing && difference > 0) ||
                (!isIncreasing && difference < 0))
            {
                isSafe = false;
                break;
            }
        }

        return isSafe;
    }

}
