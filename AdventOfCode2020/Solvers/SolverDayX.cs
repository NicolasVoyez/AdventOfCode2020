using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDayX : ISolver
    {
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            foreach (var currentLine in splitContent)
            {
            }
        }

        public string SolveFirstProblem()
        {
            throw new NotImplementedException();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            throw new NotImplementedException();
        }

        public bool Question2CodeIsDone { get; } = false;
    }
}
