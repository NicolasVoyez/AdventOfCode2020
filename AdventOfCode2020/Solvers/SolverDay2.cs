using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    public class SolverDay2 : ISolver
    {
        private readonly List<InputDay2> _inputs = new List<InputDay2>();

        public void InitInput(string content)
        {
            foreach (var line in content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries))
            {
                var values = line.Split(new[] {": ", " ", "-"}, StringSplitOptions.RemoveEmptyEntries);
                _inputs.Add(new InputDay2(int.Parse(values[0]), int.Parse(values[1]), values[2][0], values[3]));
            }
        }

        public string SolveFirstProblem()
        {
            int count = 0;
            foreach (var input in _inputs)
            {
                if (input.IsValidEx1)
                    count++;
            }

            return count.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            int count = 0;
            foreach (var input in _inputs)
            {
                if (input.IsValidEx2)
                    count++;
            }

            return count.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }

    public struct InputDay2
    {
        public int LowerBound { get; }
        public int UpperBound { get; }
        public int Letter { get; }
        public string Password { get; }

        public bool IsValidEx1
        {
            get
            {
                int count = 0;
                foreach (var letter in Password)
                {
                    if (letter == Letter)
                        count++;
                    if (count > UpperBound) return false;
                }

                return count >= LowerBound;
            }
        }
        public bool IsValidEx2 => Password[LowerBound - 1] == Letter ^ Password[UpperBound - 1] == Letter;

        public InputDay2(int lowerBound, int upperBound, int letter, string password)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Letter = letter;
            Password = password;
        }
    }
}
