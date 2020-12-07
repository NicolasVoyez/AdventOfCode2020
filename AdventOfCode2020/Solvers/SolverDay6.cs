using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay6 : ISolver
    {
        private List<HashSet<char>> answeredByAny = new List<HashSet<char>>();
        private List<HashSet<char>> answeredByAll = new List<HashSet<char>>();
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            var currentAnyGroup = new HashSet<char>();
            var currentAllGroup = new HashSet<char>();
            bool first = true;
            foreach (var currentLine in splitContent)
            {
                if (string.IsNullOrEmpty(currentLine))
                {
                    answeredByAny.Add(currentAnyGroup);
                    answeredByAll.Add(currentAllGroup);
                    currentAnyGroup = new HashSet<char>();
                    currentAllGroup = new HashSet<char>();
                    first = true;
                }
                else
                {

                    foreach (var c in currentLine)
                    {
                        currentAnyGroup.Add(c);
                        if (first)
                            currentAllGroup.Add(c);

                    }

                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        foreach (var c in currentAllGroup.ToList())
                        {
                            if (!currentLine.Contains(c))
                                currentAllGroup.Remove(c);
                        }
                    }
                }
            }
        }

        public string SolveFirstProblem()
        {
            return answeredByAny.Sum(x => x.Count).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            return answeredByAll.Sum(x => x.Count).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
