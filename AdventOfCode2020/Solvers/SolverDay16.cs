using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay16 : ISolver
    {
        private List<int> _myTicket;
        private List<List<int>> _nearbyTickets = new List<List<int>>();
        private List<Rule> _rules = new List<Rule>();

        class Rule
        {
            public string Name { get; }
            public int Min1 { get; }
            public int Max1 { get; }
            public int Min2 { get; }
            public int Max2 { get; }

            public Rule(string name, int min1, int max1, int min2, int max2)
            {
                Name = name;
                Min1 = min1;
                Max1 = max1;
                Min2 = min2;
                Max2 = max2;
            }

            public bool Validate(int value)
            {
                return (value >= Min1 && value <= Max1) ||
                       (value >= Min2 && value <= Max2);
            }

            public override string ToString()
            {
                return Name;
            }
        }

        public void InitInput(string content)
        {
            int toValidate = 0;
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var currentLine in splitContent)
            {
                if (currentLine == "your ticket:" || currentLine == "nearby tickets:")
                {
                    toValidate++;
                    continue;
                }

                switch (toValidate)
                {
                    case 0:
                        _rules.Add(CreateRule(currentLine));
                        break;
                    case 1:
                        _myTicket = CreateTicket(currentLine);
                        break;
                    case 2:
                        _nearbyTickets.Add(CreateTicket(currentLine));
                        break;
                }
            }
        }

        private List<int> CreateTicket(string currentLine)
        {
            return currentLine.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }

        private Rule CreateRule(string currentLine)
        {
            var split = currentLine.Split(new[] { ": ", "-", " or " }, StringSplitOptions.RemoveEmptyEntries);
            return new Rule(split[0], int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]), int.Parse(split[4]));
        }

        public string SolveFirstProblem()
        {
            int ticketScanningErrorRate = 0;
            foreach (var nearbyTicket in _nearbyTickets)
            {
                foreach (var number in nearbyTicket)
                {
                    if (_rules.All(r => !r.Validate(number)))
                        ticketScanningErrorRate += number;
                }
            }

            return ticketScanningErrorRate.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {

            var keptTickets = new List<List<int>> { _myTicket };

            // keep only valid
            foreach (var nearbyTicket in _nearbyTickets)
            {
                bool keep = true;
                foreach (var number in nearbyTicket)
                {
                    if (_rules.All(r => !r.Validate(number)))
                    {
                        keep = false;
                        break;
                    }
                }
                if (keep)
                    keptTickets.Add(nearbyTicket);
            }

            Dictionary<Rule, List<int>> validRanges = new Dictionary<Rule, List<int>>();

            foreach (var rule in _rules)
            {
                var valid = new List<int>();
                for (int i = 0; i < _myTicket.Count; i++)
                {
                    if (keptTickets.All(t => rule.Validate(t[i])))
                        valid.Add(i);
                }

                validRanges[rule] = valid;
            }

            bool hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                foreach (var kvp in validRanges)
                {
                    if (kvp.Value.Count == 1)
                    {
                        foreach (var kvp2 in validRanges)
                        {
                            if (kvp.Key == kvp2.Key)
                                continue;
                            if (kvp2.Value.Remove(kvp.Value[0]))
                                hasChanges = true;
                        }
                    }
                }
            }
            /*
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (var kvp in validRanges)
            {
                sb.Append(kvp.Key.Name);
                sb.Append(" : ");
                sb.Append(string.Join(",", kvp.Value));
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());*/

            BigInteger res = 1;
            foreach (var kvp in validRanges)
            {
                if (kvp.Key.Name.ToLower().Contains("departure"))
                    res *= _myTicket[kvp.Value[0]];
            }

            return res.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
