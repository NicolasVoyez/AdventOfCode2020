using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay15 : ISolver
    {
        private int currentStep = 1;
        private int previousTold;
        private Dictionary<int, List<int>> calledAt = new Dictionary<int, List<int>>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var currentLine in splitContent)
            {
                var numbers = currentLine.Split(new string[] { "," }, StringSplitOptions.None).Select(int.Parse).ToList();
                for (; currentStep <= numbers.Count; currentStep++)
                {
                    previousTold = numbers[currentStep - 1];
                    calledAt[previousTold] = new List<int> { currentStep };
                }
            }
        }

        public string SolveFirstProblem()
        {
            return Solve(2020);
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            return Solve(30000000);
        }

        public string Solve(int maxBound)
        {
            for (; currentStep <= maxBound; currentStep++)
            {
                if (calledAt[previousTold].Count == 1)
                {
                    previousTold = 0;
                }
                else
                {
                    var previousOccurrences = calledAt[previousTold];
                    previousTold = previousOccurrences[previousOccurrences.Count - 1] -
                                   previousOccurrences[previousOccurrences.Count - 2];
                }

                if (calledAt.TryGetValue(previousTold, out var occurrences))
                    occurrences.Add(currentStep);
                else
                    calledAt[previousTold] = new List<int> { currentStep };
            }

            return previousTold.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
