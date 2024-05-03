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

public class DayTwentyPulsePropagation : DaySolver
{

    public long SolvePartOne(List<string> inputLines)
    {
        Dictionary<string, Module> modules;
        List<Module> startModules;
        ExtractModulesFromInput(inputLines, out modules, out startModules);

        var sentPulses = new Queue<Pulse>();
        long totalLowPulseCount = 0;
        long totalHighPulseCount = 0;

        for (int i = 0; i < 1000; i++)
        {
            totalLowPulseCount += startModules.Count + 1;
            startModules.ForEach(x => sentPulses.Enqueue(new Pulse(PulseType.Low, "", x.Label)));

            while (sentPulses.Count > 0)
            {
                Pulse currentPulse = sentPulses.Dequeue();

                if (modules.ContainsKey(currentPulse.DestinationModule))
                {
                    modules[currentPulse.DestinationModule].RecieveAndSendPulse(currentPulse, sentPulses, out int lowPulseSendCount, out int HighPulseSendCount);
                    totalLowPulseCount += lowPulseSendCount;
                    totalHighPulseCount += HighPulseSendCount;
                }
            }
        }
        Console.WriteLine($"result as big integer: {(BigInteger)totalLowPulseCount * (BigInteger)totalHighPulseCount}");

        return totalLowPulseCount * totalHighPulseCount;
    }

    public long SolvePartTwo(List<string> inputLines)
    {
        Dictionary<string, Module> modules;
        List<Module> startModules;
        ExtractModulesFromInput(inputLines, out modules, out startModules);

        var sentPulses = new Queue<Pulse>();
        long buttonPressCount = 0;
        
        while(true)
        {
            buttonPressCount++;

            if (buttonPressCount % 100000 == 0)
            {
                Console.WriteLine(buttonPressCount);
            }
            startModules.ForEach(x => sentPulses.Enqueue(new Pulse(PulseType.Low, "", x.Label)));

            while (sentPulses.Count > 0)
            {
                Pulse currentPulse = sentPulses.Dequeue();

                if (currentPulse.PulseType == PulseType.Low && new List<string>() { "pr", "bt", "fv", "rd" }.Contains(currentPulse.DestinationModule))
                {
                    Console.WriteLine($"[{buttonPressCount,6}]: {currentPulse.DestinationModule} recieved Low Pulse");
                }

                if (currentPulse.PulseType == PulseType.Low && currentPulse.DestinationModule == "rx")
                {
                    return buttonPressCount; // last tested: 3170500000
                }

                if (modules.ContainsKey(currentPulse.DestinationModule))
                {
                    modules[currentPulse.DestinationModule].RecieveAndSendPulse(currentPulse, sentPulses, out int lowPulseSendCount, out int HighPulseSendCount);
                }
            }
        }
    }

    private static void ExtractModulesFromInput(List<string> inputLines, out Dictionary<string, Module> modules, out List<Module> startModules)
    {
        List<Tuple<string, string>> senderRecieverModuleList = new List<Tuple<string, string>>();

        foreach (string inputLine in inputLines)
        {
            if (inputLine[0] != 'b')
            {
                string currentModule = inputLine.Substring(1, 2);
                List<string> outputModules = inputLine.RemoveChar(' ').Split('>', ',').ToList();
                outputModules.RemoveAt(0);
                foreach (string outputModule in outputModules)
                {
                    senderRecieverModuleList.Add(new Tuple<string, string>(currentModule, outputModule));
                }
            }
        }
        modules = new Dictionary<string, Module>();
        string broadcasterInputLine = "";

        foreach (string inputLine in inputLines)
        {
            List<string> outputModules = inputLine.RemoveChar(' ').Split('>', ',').ToList();
            outputModules.RemoveAt(0);

            if (inputLine[0] == '%')
            {
                modules[inputLine.Substring(1, 2)] = new FlipFlopModule(outputModules, inputLine.Substring(1, 2));
            }
            else if (inputLine[0] == '&')
            {
                List<string> inputModules = new List<string>();
                senderRecieverModuleList.FindAll(x => x.Item2 == inputLine.Substring(1, 2)).ForEach(x => inputModules.Add(x.Item1));

                modules[inputLine.Substring(1, 2)] = new ConjunctionModule(inputModules, outputModules, inputLine.Substring(1, 2));
            }
            else if (inputLine[0] == 'b')
            {
                broadcasterInputLine = inputLine;
            }
        }
        startModules = new List<Module>();
        List<string> broadcasterOutputModules = broadcasterInputLine.RemoveChar(' ').Split('>', ',').ToList();
        broadcasterOutputModules.RemoveAt(0);
        foreach(string broadcasterOutputModule in broadcasterOutputModules)
        {
            startModules.Add(modules[broadcasterOutputModule]);
        }
    }

    enum PulseType
    {
        High,
        Low
    }

    struct Pulse
    {
        public PulseType PulseType;
        public string SenderModule;
        public string DestinationModule;

        public Pulse(PulseType pulseType, string senderModule, string destinationModule)
        {
            this.PulseType = pulseType;
            this.SenderModule = senderModule;
            this.DestinationModule = destinationModule;
        }
    }

    abstract class Module
    {
        public string Label { get; protected set; }
        protected List<string> _outputModules;

        public Module(List<string> outputModules, string label)
        {
            _outputModules = outputModules;
            Label = label;
        }

        public abstract void RecieveAndSendPulse(Pulse currentPulse, Queue<Pulse> sentPulses, out int LowPulseSendCount, out int HighPulseSendCount);
    }

    class FlipFlopModule : Module
    {
        public bool IsOn { get; private set; }

        public FlipFlopModule(List<string> outputModules, string label) : base(outputModules, label)
        {
            IsOn = false;
        }

        public override void RecieveAndSendPulse(Pulse currentPulse, Queue<Pulse> sentPulses, out int LowPulseSendCount, out int HighPulseSendCount)
        {
            LowPulseSendCount = 0;
            HighPulseSendCount = 0;

            if (currentPulse.PulseType == PulseType.High)
            {
                return;
            }
            else
            {
                PulseType sendPulseType = IsOn ? PulseType.Low : PulseType.High;
                IsOn = !IsOn;
                foreach(string outputModule in _outputModules)
                {
                    sentPulses.Enqueue(new Pulse(sendPulseType, this.Label, outputModule));
                }
                if (sendPulseType == PulseType.Low)
                {
                    LowPulseSendCount += _outputModules.Count;
                }
                else
                {
                    HighPulseSendCount += _outputModules.Count;
                }
                //Console.Write(new string(sendPulseType == PulseType.Low ? '0' : '1', _outputModules.Count()));
            }
        }
    }

    class ConjunctionModule : Module
    {
        Dictionary<string, PulseType> inputModuleToPulseTypeLookup = new Dictionary<string, PulseType>();

        public ConjunctionModule(List<string> inputModules, List<string> outputModules, string label) : base(outputModules, label)
        {
            foreach(string inputModule in inputModules)
            {
                inputModuleToPulseTypeLookup.Add(inputModule, PulseType.Low);
            }
        }

        public override void RecieveAndSendPulse(Pulse currentPulse, Queue<Pulse> sentPulses, out int LowPulseSendCount, out int HighPulseSendCount)
        {
            LowPulseSendCount = 0;
            HighPulseSendCount = 0;

            inputModuleToPulseTypeLookup[currentPulse.SenderModule] = currentPulse.PulseType;

            PulseType sendPulseType;
            if (inputModuleToPulseTypeLookup.All(x => x.Value == PulseType.High))
            {
                sendPulseType = PulseType.Low;
            }
            else
            {
                sendPulseType = PulseType.High;
            }

            foreach (string outputModule in _outputModules)
            {
                sentPulses.Enqueue(new Pulse(sendPulseType, this.Label, outputModule));
            }
            if (sendPulseType == PulseType.Low)
            {
                LowPulseSendCount += _outputModules.Count;
            }
            else
            {
                HighPulseSendCount += _outputModules.Count;
            }
        }
    }
}
