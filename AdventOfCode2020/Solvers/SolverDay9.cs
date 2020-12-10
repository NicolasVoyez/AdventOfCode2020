using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay9 : ISolver
    {
        private List<double> input = new List<double>();
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            foreach (var currentLine in splitContent)
            {
                input.Add(double.Parse(currentLine));
            }
        }

        private int _foundIndex = 0;
        public string SolveFirstProblem()
        {
            for (int i = 25; i < input.Count; i++)
            {
                var curr = input[i];
                var found = false;
                for (int j = i - 25; j < i - 1; j++)
                {
                    for (int k = i - 24; k < i; k++)
                    {
                        if (input[j] + input[k] == curr)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }

                if (!found)
                {
                    _foundIndex = i;
                    return curr.ToString();
                }
            }

            return "error";
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            (int from, int to) = GetIndexesForEx2(firstProblemSolution);

            if (from == -1)
                return "Error";

            var items = input.Skip(from).Take(to - from);
            var min = items.Min();
            var max = items.Max();
            return (min + max).ToString();
        }

        private (int t, int j) GetIndexesForEx2(string firstProblemSolution)
        {
            var toFind = double.Parse(firstProblemSolution);
            for (int i = 0; i < _foundIndex; i++)
            {
                var sum = input[i];
                for (int j = i + 1; j < _foundIndex; j++)
                {
                    sum += input[j];
                    if (sum == toFind)
                    {
                        return (i,j);
                    }

                    if (sum > toFind)
                        break;
                }
            }

            return (-1,-1);
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
