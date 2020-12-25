using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay24 : ISolver
    {
        class Hex
        {
            public int X { get; }
            public int Y { get; }
            public bool IsBlack { get; set; }

            /// <summary>
            /// Note : X and Y are multiples of 10
            /// </summary>
            public Hex(int x, int y, bool isBlack = false)
            {
                X = x;
                Y = y;
                IsBlack = isBlack;
            }

            public Hex E => new Hex(X + 10, Y);
            public Hex SE => new Hex(X + 5, Y + 10);
            public Hex SW => new Hex(X - 5, Y + 10);
            public Hex W => new Hex(X - 10, Y);
            public Hex NW => new Hex(X - 5, Y - 10);
            public Hex NE => new Hex(X + 5, Y - 10);

            public Hex GetNext(string direction)
            {
                switch (direction)
                {
                    case "e": return E;
                    case "se": return SE;
                    case "ne": return NE;
                    case "w": return W;
                    case "sw": return SW;
                    case "nw": return NW;
                }
                throw new NotImplementedException();
            }

            public override bool Equals(object? obj)
            {
                return Equals(obj as Hex);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public bool Equals(Hex other)
            {
                if (ReferenceEquals(other, this)) return true;
                if (ReferenceEquals(other, null)) return false;

                return X == other.X && Y == other.Y;
            }

            public void Mark()
            {
                IsBlack = !IsBlack;
            }

            public IEnumerable<Hex> GetAround()
            {
                yield return E;
                yield return SE;
                yield return NE;
                yield return W;
                yield return SW;
                yield return NW;
            }

            public Hex MarkNew()
            {
                return new Hex(X, Y, !IsBlack);
            }

            public override string ToString()
            {
                return (IsBlack ? "B " : "W ") + ((double)X)/10 + "-" + ((double)Y)/10;
            }
        }

        private List<List<string>> Directions = new List<List<string>>();
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var currentLine in splitContent)
            {
                var resplacePart1 = currentLine
                    .Replace("se", " se ")
                    .Replace("sw", " sw ")
                    .Replace("nw", " nw ")
                    .Replace("ne", " ne ");
                Directions.Add(resplacePart1
                    .Replace("ew", " e w ")
                    .Replace("we", " w e ")
                    .Replace("ee", " e e ")
                    .Replace("ww", " w w ")
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
        }

        public string SolveFirstProblem()
        {
            Hexes = new HashSet<Hex>();
            foreach (var directionList in Directions)
            {
                var currentHex = new Hex(0, 0);
                foreach (var direction in directionList)
                {
                    currentHex = currentHex.GetNext(direction);
                }

                if (Hexes.TryGetValue(currentHex, out var realCurrentHex))
                {
                    realCurrentHex.Mark();
                }
                else
                {
                    currentHex.Mark();
                    Hexes.Add(currentHex);
                }
            }

            return Hexes.Count(h => h.IsBlack).ToString();
        }

        HashSet<Hex> Hexes { get; set; }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            for (int i = 0; i < 100; i++)
                TurnOneRound();

            return Hexes.Count.ToString();
        }

        private void TurnOneRound()
        {
            HashSet<Hex> toTreat = new HashSet<Hex>();
            HashSet<Hex> finalBlack = new HashSet<Hex>();

            foreach (var hex in Hexes)
            {
                // Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                if (hex.IsBlack)
                {
                    int countBlackAround = 0;

                    foreach (var aroundHex in hex.GetAround())
                    {
                        if (!toTreat.TryGetValue(aroundHex, out var toTreatAroundHex))
                        {
                            if (!Hexes.TryGetValue(aroundHex, out toTreatAroundHex))
                                toTreatAroundHex = aroundHex;
                        }

                        if (toTreatAroundHex.IsBlack)
                            countBlackAround++; // count for rule 1
                        else
                            toTreat.Add(toTreatAroundHex); // keep around hexes for rule 2
                    }

                    if (countBlackAround == 1 || countBlackAround == 2)
                        finalBlack.Add(hex);
                }
            }

            //Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
            foreach (var hex in toTreat)
            {
                var blackAround = 0;
                foreach (var around in hex.GetAround())
                {
                    if (Hexes.TryGetValue(around, out var maybeBlackAround) && maybeBlackAround.IsBlack)
                        blackAround++;
                }

                if (blackAround == 2)
                {
                    finalBlack.Add(hex.MarkNew());
                }
            }

            Hexes = finalBlack;
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
