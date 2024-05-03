using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

public class DayTwoRockPaperScissors : DaySolver
{
    public long SolvePartOne(List<string> inputLines)
    {
        long result = 0;

        foreach (string inputLine in inputLines)
        {
            Outcome outcome = GetGameOutcome(inputLine[0], inputLine[2]);
            result += GetPointsXYZMoves(outcome, inputLine[2]);
            //Console.WriteLine($"{inputLine} --- {outcome}, outcome Points: {(int)outcome}, input Points: {inputLine[2] - 'W'}, total Points: {GetPoints(outcome, inputLine[2])}");
        }
        return result;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        long result = 0;

        foreach (string inputLine in inputLines)
        {
            Outcome outcome = (Outcome)((inputLine[2] - 'X') * 3);
            char yourMove = ChooseMove(inputLine[0], outcome);
            result += GetPointsABCMoves(outcome, yourMove);
            //Console.WriteLine($"{inputLine} --- {outcome}, your Move: {yourMove}, {(int) outcome} + {yourMove - 'A' + 1} = {GetPointsABCMoves(outcome, yourMove)}");
        }
        return result;
    }

    Outcome GetGameOutcome(char opponentMove, char yourMove)
    {
        int difference = ((yourMove - 'X') - (opponentMove - 'A') + 3) % 3;

        return (Outcome)(difference * 3);
    }

    char ChooseMove(char opponentMove, Outcome outcome)
    {
        int difference = ((int)outcome / 3) - 1;
        return (char)((opponentMove - 'A' + difference + 3) % 3 + 'A');
    }

    int GetPointsXYZMoves(Outcome outcome, char yourMove)
    {
        return (int)outcome + yourMove - 'X' + 1;
    }

    int GetPointsABCMoves(Outcome outcome, char yourMove)
    {
        return (int)outcome + yourMove - 'A' + 1;
    }

    enum Outcome
    {
        Win = 6,
        Loss = 0,
        Draw = 3
    }
}
