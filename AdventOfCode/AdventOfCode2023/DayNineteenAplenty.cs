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

public class DayNineteenAplenty : DaySolver
{


    public long SolvePartOne(List<string> inputLines)
    {
        Dictionary<string, Workflow> labelToWorkflowLookup = new Dictionary<string, Workflow>();

        int lineIndex = 0;

        // px{a<2006:qkq,m>2090:A,rfg}
        // pv{a>1716:R,A}
        while (inputLines[lineIndex] != "")
        {
            //Console.WriteLine(inputLines[lineIndex]);
            string inputLine = inputLines[lineIndex];
            labelToWorkflowLookup[inputLine.Split('{')[0]] = new Workflow(inputLine);
            lineIndex++;
        }

        lineIndex++;
        //Console.WriteLine("-------------------------------------------------------------------");

        long result = 0;

        // {x=787,m=2655,a=1222,s=2876}
        // {x=1679,m=44,a=2067,s=496}
        while (lineIndex < inputLines.Count)
        {
            //Console.WriteLine(inputLines[lineIndex]);
            string inputLine = inputLines[lineIndex];
            MachinePart machinePart = new MachinePart(inputLine);
            string currentWorkflowLabel = "in";

            while (currentWorkflowLabel != "A" && currentWorkflowLabel != "R")
            {
                currentWorkflowLabel = labelToWorkflowLookup[currentWorkflowLabel].Evaluate(machinePart);
            }
            if (currentWorkflowLabel == "A")
            {
                result += machinePart.CategoryValues.Sum();
            }
            lineIndex++;
        }

        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        Dictionary<string, string> workflowLookup = new Dictionary<string, string>();

        int lineIndex = 0;

        // px{a<2006:qkq,m>2090:A,rfg}
        // pv{a>1716:R,A}
        while (inputLines[lineIndex] != "")
        {
            string[] splitLine = inputLines[lineIndex].Split('{', '}');
            workflowLookup[splitLine[0]] = splitLine[1];
            lineIndex++;
        }

        BigInteger result = CountAcceptedParts(workflowLookup, "in", new MachinePartRange(new Vector2Int(1, 4000), new Vector2Int(1, 4000), new Vector2Int(1, 4000), new Vector2Int(1, 4000)));
        Console.WriteLine($"result as BigInteger = {result}");

        return (long)result;
    }

    public class MachinePart
    {
        public List<int> CategoryValues;

        public MachinePart(string inputLine)
        {
            // {x=787,m=2655,a=1222,s=2876}
            string[] splitLine = inputLine.Split('=', ',', '}');

            CategoryValues = new List<int>
            {
                int.Parse(splitLine[1]),
                int.Parse(splitLine[3]),
                int.Parse(splitLine[5]),
                int.Parse(splitLine[7])
            };
        }

        public enum Category
        {
            X = 0,
            M = 1, 
            A = 2, 
            S = 3
        }
    }

    class Workflow
    {
        public string label;
        List<Tuple<Predicate<MachinePart>, string>> evaluationAndOutputList;

        public Workflow(string inputLine)
        {
            evaluationAndOutputList = new List<Tuple<Predicate<MachinePart>, string>>();

            // "rfg{s<537:gd,x>2440:R,A}"  OR  "qs{s>3448:A,lnx}"

            string[] labelAndRulesSplit = inputLine.Split('{', '}'); // "rfg", "s<537:gd,x>2440:R,A", ""
            label = labelAndRulesSplit[0];

            string[] rulesSplit = labelAndRulesSplit[1].Split(','); // "s<537:gd", "x>2440:R", "A"  OR  "lnx"

            foreach (string rule in rulesSplit) // "s<537:gd" OR "A"
            {
                if(rule.Length < 2 || (rule[1] != '<' && rule[1] != '>')) // "A"  OR  "lnx"
                {
                    evaluationAndOutputList.Add(new Tuple<Predicate<MachinePart>, string>(mPart => true, rule));
                }
                else // "s<537:gd"
                {
                    string outputLabel = rule.Split('<', '>', ':')[2];

                    MachinePart.Category ratedCategory = ConvertCharToCategory(rule[0]);
                    int value = int.Parse(rule.Split('<', '>', ':')[1]);

                    Predicate<MachinePart> machinePartEvaluation;
                    if (rule[1] == '<')
                    {
                        machinePartEvaluation = mPart => mPart.CategoryValues[(int)ratedCategory] < value;
                    }
                    else
                    {
                        machinePartEvaluation = mPart => mPart.CategoryValues[(int)ratedCategory] > value;
                    }
                    evaluationAndOutputList.Add(new Tuple<Predicate<MachinePart>, string>(machinePartEvaluation, outputLabel));
                }
            }
        }

        public string Evaluate(MachinePart machinePart)
        {
            foreach (Tuple<Predicate<MachinePart>, string> evaluationAndOutput in evaluationAndOutputList)
            {
                if (evaluationAndOutput.Item1(machinePart))
                {
                    return evaluationAndOutput.Item2;
                }
            }
            throw new Exception("not even the default rule triggered... that's bad...");
        }
    }

    public static MachinePart.Category ConvertCharToCategory(char c)
    {
        switch (c)
        {
            default:
            case 'x': return MachinePart.Category.X;
            case 'm': return MachinePart.Category.M;
            case 'a': return MachinePart.Category.A;
            case 's': return MachinePart.Category.S;
        }
    }

    BigInteger CountAcceptedParts(Dictionary<string, string> workflowLookup, string currentWorkflowLabel, MachinePartRange range)
    {
        if (currentWorkflowLabel == "A")
        {
            return range.GetAmountOfDifferentParts();
        }
        else if (currentWorkflowLabel == "R")
        {
            return 0;
        }

        //"a<2006:qkq,m>2090:A,rfg"
        string[] workflowSteps = workflowLookup[currentWorkflowLabel].Split(',');

        BigInteger result = 0;

        foreach (string workflowStep in workflowSteps)
        {
            if (workflowStep.Length > 1 && (workflowStep[1] == '<' || workflowStep[1] == '>'))
            {
                range.SplitRange(workflowStep, out MachinePartRange? satisfyingRange, out MachinePartRange? nonsatisfyingRange);
                if (satisfyingRange != null)
                {
                    result += CountAcceptedParts(workflowLookup, workflowStep.Split(':').Last(), satisfyingRange.Value);
                }
                if (nonsatisfyingRange != null)
                {
                    range = nonsatisfyingRange.Value;
                }
                else
                {
                    break;
                }
            }
            else if (workflowStep == "A")
            {
                result += range.GetAmountOfDifferentParts();
            }
            else if (workflowStep != "R")
            {
                result += CountAcceptedParts(workflowLookup, workflowStep, range);
            }
        }
        Console.WriteLine($"{currentWorkflowLabel}: current result = {result}, after finishing {workflowLookup[currentWorkflowLabel]}");
        return result;
    }

    struct MachinePartRange
    {
        public Vector2Int XtremelyCool;
        public Vector2Int Musical;
        public Vector2Int Aerodynamic;
        public Vector2Int Shiny;

        public MachinePartRange(Vector2Int x, Vector2Int m, Vector2Int a, Vector2Int s)
        {
            this.XtremelyCool = x;
            this.Musical = m;
            this.Aerodynamic = a;
            this.Shiny = s;
        }

        public BigInteger GetAmountOfDifferentParts()
        {
            return (BigInteger)(XtremelyCool.Y - XtremelyCool.X + 1) * (Musical.Y - Musical.X + 1) * (Aerodynamic.Y - Aerodynamic.X + 1) * (Shiny.Y - Shiny.X + 1);
        }

        public void SplitRange(string workflowStep, out MachinePartRange? satisfyingRange, out MachinePartRange? nonsatisfyingRange)
        {
            // "a<2006:qkq"
            char category = workflowStep[0];
            char comparer = workflowStep[1];
            int cutoffValue = int.Parse(workflowStep.Split('<', '>', ':')[1]);

            Vector2Int value;
            switch (category)
            {
                default:
                case 'x': value = XtremelyCool; break;
                case 'm': value = Musical; break;
                case 'a': value = Aerodynamic; break;
                case 's': value = Shiny; break;
            }

            if (comparer == '<')
            {
                if (cutoffValue <= value.X)
                {
                    satisfyingRange = null;
                    nonsatisfyingRange = this;
                }
                else if (value.Y < cutoffValue)
                {
                    satisfyingRange = this;
                    nonsatisfyingRange = null;
                }
                else
                {
                    MachinePartRange satisfyingCopyOfThis = this;
                    MachinePartRange nonsatisfyingCopyOfThis = this;
                    switch (category)
                    {
                        default:
                        case 'x':
                            satisfyingCopyOfThis.XtremelyCool = new Vector2Int(XtremelyCool.X, cutoffValue - 1);
                            nonsatisfyingCopyOfThis.XtremelyCool = new Vector2Int(cutoffValue, XtremelyCool.Y);
                            break;
                        case 'm':
                            satisfyingCopyOfThis.Musical = new Vector2Int(Musical.X, cutoffValue - 1);
                            nonsatisfyingCopyOfThis.Musical = new Vector2Int(cutoffValue, Musical.Y);
                            break;
                        case 'a':
                            satisfyingCopyOfThis.Aerodynamic = new Vector2Int(Aerodynamic.X, cutoffValue - 1);
                            nonsatisfyingCopyOfThis.Aerodynamic = new Vector2Int(cutoffValue, Aerodynamic.Y);
                            break;
                        case 's':
                            satisfyingCopyOfThis.Shiny = new Vector2Int(Shiny.X, cutoffValue - 1);
                            nonsatisfyingCopyOfThis.Shiny = new Vector2Int(cutoffValue, Shiny.Y);
                            break;
                    }
                    satisfyingRange = satisfyingCopyOfThis;
                    nonsatisfyingRange = nonsatisfyingCopyOfThis;
                }
            }
            else // comparer == '>'
            {
                if (value.Y <= cutoffValue)
                {
                    satisfyingRange = null;
                    nonsatisfyingRange = this;
                }
                else if (cutoffValue < value.X)
                {
                    satisfyingRange = this;
                    nonsatisfyingRange = null;
                }
                else
                {
                    MachinePartRange satisfyingCopyOfThis = this;
                    MachinePartRange nonsatisfyingCopyOfThis = this;
                    switch (category)
                    {
                        default:
                        case 'x':
                            satisfyingCopyOfThis.XtremelyCool = new Vector2Int(cutoffValue + 1, XtremelyCool.Y);
                            nonsatisfyingCopyOfThis.XtremelyCool = new Vector2Int(XtremelyCool.X, cutoffValue);
                            break;
                        case 'm':
                            satisfyingCopyOfThis.Musical = new Vector2Int(cutoffValue + 1, Musical.Y);
                            nonsatisfyingCopyOfThis.Musical = new Vector2Int(Musical.X, cutoffValue);
                            break;
                        case 'a':
                            satisfyingCopyOfThis.Aerodynamic = new Vector2Int(cutoffValue + 1, Aerodynamic.Y);
                            nonsatisfyingCopyOfThis.Aerodynamic = new Vector2Int(Aerodynamic.X, cutoffValue);
                            break;
                        case 's':
                            satisfyingCopyOfThis.Shiny = new Vector2Int(cutoffValue + 1, Shiny.Y);
                            nonsatisfyingCopyOfThis.Shiny = new Vector2Int(Shiny.X, cutoffValue);
                            break;
                    }
                    satisfyingRange = satisfyingCopyOfThis;
                    nonsatisfyingRange = nonsatisfyingCopyOfThis;
                }
            }
        }
    }
}
