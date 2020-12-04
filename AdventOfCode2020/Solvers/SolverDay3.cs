using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    public class SolverDay3 : ISolver
    {
        /// <summary>
        /// Y, X
        /// </summary>
        public bool[,] _field;

        private int maxY;
        private int maxX;

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            maxY = splitContent.Length;
            maxX = splitContent[0].Length;
            _field = new bool[maxY, maxX];
            for (int y = 0; y < maxY; y++)
            {
                var line = splitContent[y];
                for (int x = 0; x < maxX; x++)
                {
                    _field[y, x] = line[x] == '#';
                }
            }
        }

        public string SolveFirstProblem()
        {
            var count = GetTreesInSlope(3,1);

            return count.ToString();
        }

        private double GetTreesInSlope(int slopeX, int slopeY)
        {
            double count = 0;
            var x = 0;
            for (int y = 0; y*slopeY < maxY; y++)
            {
                if (_field[y*slopeY, (y * slopeX) % maxX])
                    count++;
            }

            return count;
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            return (GetTreesInSlope(1, 1) *
                    GetTreesInSlope(3, 1) *
                    GetTreesInSlope(5, 1) *
                    GetTreesInSlope(7, 1) *
                    GetTreesInSlope(1, 2)).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
