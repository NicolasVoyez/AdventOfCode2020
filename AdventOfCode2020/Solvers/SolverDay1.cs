using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    public class SolverDay1 : ISolver
    {
        private List<int> Input;
        public void InitInput(string content)
        {
            Input = content.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }

        public string SolveFirstProblem()
        {
            for (int i = 0; i < Input.Count; i++)
            {
                var element1 = Input[i];
                if (element1 > 2020) continue; 
                for (int j = 0; j < Input.Count; j++)
                {
                    var element2 = Input[j];
                    if (element1 + element2 == 2020)
                    {
                        return (element1 * element2).ToString();
                    }
                }
            }

            return "";
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            for (int i = 0; i < Input.Count; i++)
            {
                var element1 = Input[i];
                if (element1 > 2020) continue;
                for (int j = 0; j < Input.Count; j++)
                {
                    var element2 = Input[j];
                    var sum12 = element1 + element2;
                    if (sum12 > 2020) continue;
                    for (int k = 0; k < Input.Count; k++)
                    {
                        var element3 = Input[k];
                        if (sum12 + element3 == 2020)
                        {
                            return (element1 * element2 * element3).ToString();
                        }
                    }
                }
            }

            return "";
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
