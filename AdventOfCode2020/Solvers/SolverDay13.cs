using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay13 : ISolver
    {
        private int _earlierTimestamp;
        private readonly List<int> _exercise1BusLines = new List<int>();
        private readonly Dictionary<int,int> _exercise2BusLinesDelays = new Dictionary<int,int>();
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            _earlierTimestamp = int.Parse(splitContent[0]);
            var busLines = splitContent[1].Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < busLines.Length; i++ )
            {
                var time = busLines[i];
                if (time == "x")
                    continue;
                var busLine = int.Parse(time);
                _exercise1BusLines.Add(busLine);
                _exercise2BusLinesDelays[busLine] = i;
            }

        }

        public string SolveFirstProblem()
        {
            var bestDiff = int.MaxValue;
            var bestLine = -1;
            foreach (var busLine in _exercise1BusLines)
            {
                var diff = (((_earlierTimestamp / busLine) * (busLine)) + busLine) - _earlierTimestamp;
                //var diff = _earlierTimestamp % busLine;
                if (diff < bestDiff)
                {
                    bestDiff = diff;
                    bestLine = busLine;
                }
            }

            return (bestDiff * bestLine).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            // x % 7 == 0
            // x % 13 == (13-1)
            // x % 59 == (59-4)
            // ....

            double n = 1;
            foreach (var busLine in _exercise2BusLinesDelays.Keys)
            {
                n *= busLine;
            }

            BigInteger oneSolution = 0;
            foreach (var kvp in _exercise2BusLinesDelays)
            {
                double ni = kvp.Key;
                double invni = n / ni;

                var factor = -1;
                for (int j = 1; j <= ni; j++)
                {
                    if (Math.Abs((j * invni) % ni - 1) < double.Epsilon)
                    {
                        factor = j;
                        break;
                    }
                }
                if (factor == -1)
                    throw new Exception();

                BigInteger ei = new BigInteger(factor * invni);
                oneSolution +=  ei * new BigInteger(((ni - kvp.Value) % ni));
            }

            var res = oneSolution % new BigInteger(n);

            return res.ToString();
            /* non working brute force
            var maxBus = _exercise1BusLines.Max();
            var maxBusDelay = _exercise2BusLinesDelays[maxBus];

            double round = 35155718987;
            while (true)
            {
                double timestamp = maxBus * round - maxBusDelay;
                if (TimestampIsValid(timestamp))
                    return timestamp.ToString();
                round++;
            }*/
        }

        private bool TimestampIsValid(in double timestamp)
        {
            foreach (var kvp in _exercise2BusLinesDelays)
            {
                if (((timestamp + kvp.Value) % kvp.Key) != 0)
                    return false;
            }

            return true;
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
