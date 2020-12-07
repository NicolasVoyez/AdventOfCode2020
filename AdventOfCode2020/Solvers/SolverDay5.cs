using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay5 : ISolver
    {
        class PlanePosition
        {
            private readonly string _planePosition;
            private int _row;
            private int _col;
            public int SeatNumber => _col + (_row*8);

            public PlanePosition(string planePosition)
            {
                _planePosition = planePosition;
                _row = GetRow();
                _col = GetColumn();
            }

            private int GetColumn()
            {
                int minBound = 0;
                int maxBound = 7;
                for (int i = 0; i < 3; i++)
                {
                    if (_planePosition[_planePosition.Length  - 1- i] == 'L')
                        maxBound = (int)Math.Floor(((double)minBound + maxBound) / 2);
                    else
                        minBound = (int)Math.Ceiling(((double)minBound + maxBound) / 2);
                }

                return  minBound;
            }

            private int GetRow()
            {
                int minBound = 0;
                int maxBound = 127;
                for (int i = 0; i < 7; i++)
                {
                    if (_planePosition[i] == 'F')
                        maxBound = (int)Math.Floor(((double)minBound + maxBound) / 2);
                    else
                        minBound = (int)Math.Ceiling(((double)minBound + maxBound) / 2);
                }

                return minBound;
            }
        }

        private List<PlanePosition> _planePositions = new List<PlanePosition>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var currentLine in splitContent)
            {
                _planePositions.Add(new PlanePosition(currentLine));
            }
        }

        public string SolveFirstProblem()
        {
            return _planePositions.Max(p => p.SeatNumber).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            HashSet<int> existingSeats = new HashSet<int>();
            foreach (var position in _planePositions)
            {
                existingSeats.Add(position.SeatNumber);
            }
            /*
            var max = _planePositions.Max(p => p.SeatNumber);
            for (int pos = 0; pos < max; pos++ )
            {
                var existBefore = existingSeats.Contains(pos - 1);
                var existAfter = existingSeats.Contains(pos + 1);
                if (existBefore && existAfter)
                    return pos.ToString();
            }*/

            List<(int,int)> res = new List<(int, int)>();
            var maxDist = 1000;
            var ordered = existingSeats.OrderBy(x => x).ToList();
            for (int i = 0; i < ordered.Count - 1; i++)
            {
                var curr = ordered[i];
                var next = ordered[i +1];
                var dist = next - curr;
                if (dist < maxDist && dist > 1)
                {
                    res = new List<(int, int)>{(curr, next)};
                    maxDist = dist;
                }
                else if (dist == maxDist)
                {
                    res.Add((curr, next));
                }
            }

            return res.Aggregate("Between", (text, tuple) => text + $" ({tuple.Item1} and {tuple.Item2}),");
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
