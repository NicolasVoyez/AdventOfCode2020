using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay10 : ISolver
    {
        private HashSet<int> Adapters = new HashSet<int>();
        private List<int> OrderedAdapters;
        private int builtIn;
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            int max = 0;
            foreach (var currentLine in splitContent)
            {
                var current = int.Parse(currentLine);
                if (current > max)
                    max = current;
                Adapters.Add(current);
            }

            Adapters.Add(0);
            Adapters.Add(max + 3);

            OrderedAdapters = Adapters.OrderBy(a => a).ToList();

            builtIn = max + 3;
        }

        public string SolveFirstProblem()
        {
            int diff1 = 0;
            int diff3 = 0;
            for (int i = 0; i < OrderedAdapters.Count - 1; i++)
            {
                var diff = OrderedAdapters[i + 1] - OrderedAdapters[i];
                if (diff == 1)
                    diff1++;
                else if (diff == 3)
                    diff3++;
            }

            return (diff1 * diff3).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            var paths = new double[builtIn +1];
            paths[0] = 1;
            if (Adapters.Contains(1))
                paths[1] = 1;

            for (int i = 2; i <= builtIn; i++)
            {
                if (!Adapters.Contains(i))
                    continue;

                for (int j = i - 1; j >= i - 3; j--)
                {
                    if (j >= 0)
                        paths[i] += paths[j];
                }
            }


            return paths[builtIn].ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
