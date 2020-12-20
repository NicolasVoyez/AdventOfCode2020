using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay20 : ISolver
    {
        internal class Possibility
        {
            public int Tile { get; }
            // top left bot right
            public int[] Directions { get; }
            public int Rotations { get; }
            public bool SwapHorizontally { get; }
            public bool SwapVertically { get; }

            public Possibility(int tile, int[] directions, int rotations, bool swapHorizontally, bool swapVertically)
            {
                Tile = tile;
                Directions = directions;
                Rotations = rotations;
                SwapHorizontally = swapHorizontally;
                SwapVertically = swapVertically;
            }

            public override bool Equals(object? obj)
            {
                if (!(obj is Possibility pos))
                    return false;
                return Equals(pos);
            }

            private bool Equals(Possibility other)
            {
                if (ReferenceEquals(null, other))
                    return false;
                return Tile == other.Tile &&
                       Directions[0] == other.Directions[0] &&
                       Directions[1] == other.Directions[1] &&
                       Directions[2] == other.Directions[2] &&
                       Directions[3] == other.Directions[3];
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Tile, Directions);
            }

            public override string ToString()
            {
                return Tile.ToString();
            }
        }

        internal class Tile
        {
            private int Number { get; }
            public string[] Grid { get; private set; }
            public int[] Borders { get; private set; }
            public int[] InvertedBorders { get; private set; }

            public HashSet<int> PossibleBordersValues { get; } = new HashSet<int>();

            public Tile(string[] grid, int number)
            {
                Grid = grid;
                Number = number;
                CalculateBorders();
            }

            private Tile(string[] grid)
            {
                Grid = grid;
            }

            private void CalculateBorders()
            {
                string top = "";
                string invTop = "";
                string bot = "";
                string invBot = "";
                for (int i = 0; i < Grid[0].Length; i++)
                {
                    top += Grid[0][i] == '#' ? "1" : "0";
                    bot += Grid[Grid.Length - 1][i] == '#' ? "1" : "0";
                    invTop = (Grid[0][i] == '#' ? "1" : "0") + invTop;
                    invBot = (Grid[Grid.Length - 1][i] == '#' ? "1" : "0") + invBot;
                }

                string left = "", invLeft = "", right = "", invRight = "";

                for (int i = 0; i < Grid.Length; i++)
                {
                    left += Grid[i][0] == '#' ? "1" : "0";
                    right += Grid[i][Grid[i].Length - 1] == '#' ? "1" : "0";
                    invLeft = (Grid[i][0] == '#' ? "1" : "0") + invLeft;
                    invRight = (Grid[i][Grid[i].Length - 1] == '#' ? "1" : "0") + invRight;
                }

                Borders = new[] { top, right, bot, left }.Select(s => Convert.ToInt32(s, 2)).ToArray();
                InvertedBorders = new[] { invTop, invRight, invBot, invLeft }.Select(s => Convert.ToInt32(s, 2)).ToArray();

                foreach (var border in Borders)
                    PossibleBordersValues.Add(border);
                foreach (var border in InvertedBorders)
                    PossibleBordersValues.Add(border);
            }

            public IEnumerable<Possibility> GetAllPossibilities()
            {
                yield return new Possibility(Number, Borders, 0, false, false);
                yield return new Possibility(Number, new[] { Borders[1], Borders[2], Borders[3], Borders[1] }, 3, false, false);
                yield return new Possibility(Number, new[] { Borders[3], Borders[2], Borders[1], Borders[0] }, 1, true, false);
                yield return new Possibility(Number, new[] { InvertedBorders[2], InvertedBorders[3], InvertedBorders[0], InvertedBorders[1] }, 2, false, false);
                yield return new Possibility(Number, new[] { InvertedBorders[1], InvertedBorders[0], InvertedBorders[3], InvertedBorders[2] }, 1, false, true);
                yield return new Possibility(Number, new[] { InvertedBorders[3], Borders[0], InvertedBorders[1], Borders[2] }, 1, false, false);
                yield return new Possibility(Number, new[] { InvertedBorders[0], Borders[3], InvertedBorders[2], Borders[1] }, 0, false, true);
                yield return new Possibility(Number, new[] { Borders[2], InvertedBorders[1], Borders[0], InvertedBorders[3] }, 2, false, true);
                yield return new Possibility(Number, new[] { Borders[1], InvertedBorders[2], Borders[3], InvertedBorders[0] }, 1, true, true);
            }

            public IEnumerable<Possibility> GetAllPossibilities(int right)
            {
                if (!PossibleBordersValues.Contains(right))
                    yield break;
                foreach (var orientation in GetAllPossibilities())
                {
                    if (orientation.Directions[3] == right)
                        yield return orientation;
                }

            }

            public IEnumerable<Possibility> GetAllPossibilitiesTopOnly(int top)
            {
                if (!PossibleBordersValues.Contains(top))
                    yield break;
                foreach (var orientation in GetAllPossibilities())
                {
                    if (orientation.Directions[0] == top)
                        yield return orientation;
                }

            }

            public IEnumerable<Possibility> GetAllPossibilities(int right, int top)
            {
                if (!PossibleBordersValues.Contains(right) || !PossibleBordersValues.Contains(top))
                    yield break;
                foreach (var orientation in GetAllPossibilities())
                {
                    if (orientation.Directions[3] == right && orientation.Directions[0] == top)
                        yield return orientation;
                }

            }

            public override string ToString()
            {
                return Number.ToString();
            }

            public void Rotate()
            {
                char[][] newGrid = new char[Grid.Length][];


                for (int y = 0; y < Grid.Length; y++)
                {
                    newGrid[y] = new char[Grid.Length];
                }

                for (int y = 0; y < Grid.Length; y++)
                {
                    for (int x = 0; x < Grid.Length; x++)
                    {
                        newGrid[x][Grid.Length - y - 1] = Grid[y][x];
                    }
                }

                Grid = newGrid.Select(cs => new string(cs)).ToArray();
            }

            public void SwapHorizontally()
            {
                string[] newGrid = new string[Grid.Length];
                int min = 0, max = Grid.Length - 1;
                while (min < max)
                {
                    newGrid[min] = Grid[max];
                    newGrid[max] = Grid[min];
                    min++;
                    max--;
                }

                Grid = newGrid;
            }

            public void SwapVertically()
            {
                char[][] newGrid = new char[Grid.Length][];


                for (int y = 0; y < Grid.Length; y++)
                {
                    newGrid[y] = new char[Grid.Length];
                }

                int min = 0, max = Grid.Length - 1;
                while (min < max)
                {
                    for (int y = 0; y < Grid.Length; y++)
                    {
                        newGrid[y][min] = Grid[y][max];
                        newGrid[y][max] = Grid[y][min];

                    }
                    min++;
                    max--;
                }

                Grid = newGrid.Select(cs => new string(cs)).ToArray();
            }

            public static Tile FromSolvedGrid()
            {
                var megaGrid = new List<string>();

                foreach (var possibility in _solvedGrid)
                {
                    var tile = _tiles[possibility.Tile];
                    if (possibility.SwapHorizontally)
                        tile.SwapHorizontally();
                    if (possibility.SwapVertically)
                        tile.SwapVertically();
                    for (int i = 0; i < possibility.Rotations; i++)
                        tile.Rotate();
                }

                //Console.WriteLine();
        var len = _tiles.First().Value.Grid.Length;
                for (int yTile = 0; yTile < MaxSize; yTile++)
                {
                    for (int yInTile = 1; yInTile < len - 1; yInTile++)
                    {
                        var currentLine = "";
                        for (int xTile = 0; xTile < MaxSize; xTile++)
                        {
                            var l = _tiles[_solvedGrid[yTile, xTile].Tile].Grid[yInTile];
                            currentLine += l.Substring(1, l.Length - 2);
                        }
                        megaGrid.Add(currentLine);
                        //Console.WriteLine(currentLine);
                    }
                }

                //Console.WriteLine();
                return new Tile(megaGrid.ToArray());
            }

            public int GetSeaMonsterCount()
            {
                var count = 0;
                for (int y = 0; y <= Grid.Length - 3; y++)
                {
                    for (int x = 0; x <= Grid[0].Length - 20; x++)
                    {
                        if (IsSeaMonster(x, y))
                            count++;
                    }
                }

                return count;
            }

            private bool IsSeaMonster(int x, int y)
            {
                //                  #
                //#    ##    ##    ###
                //#  #  #  #  #  #
                if (x > Grid[0].Length - 20)
                    return false;
                if (y > Grid.Length - 3)
                    return false;
                return Grid[y][x + 18] == '#' &&
                       Grid[y + 1][x] == '#' && Grid[y + 1][x + 5] == '#' && Grid[y + 1][x + 6] == '#' &&
                       Grid[y + 1][x + 11] == '#' && Grid[y + 1][x + 12] == '#' && Grid[y + 1][x + 17] == '#' &&
                       Grid[y + 1][x + 18] == '#' && Grid[y + 1][x + 19] == '#' &&
                       Grid[y + 2][x +1] == '#' && Grid[y + 2][x + 4] == '#' && Grid[y + 2][x + 7] == '#' &&
                       Grid[y + 2][x + 10] == '#' && Grid[y + 2][x + 13] == '#' && Grid[y + 2][x + 16] == '#';
            }
        }

        internal static Dictionary<int, Tile> _tiles = new Dictionary<int, Tile>();
        internal static int MaxSize;

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            int tileNumber = -1;
            List<string> grid = new List<string>();
            foreach (var currentLine in splitContent)
            {
                if (currentLine.Contains("Tile "))
                    tileNumber = int.Parse(currentLine.Replace("Tile ", "").TrimEnd(':'));
                else if (string.IsNullOrEmpty(currentLine))
                {
                    _tiles.Add(tileNumber, new Tile(grid.ToArray(), tileNumber));
                    grid = new List<string>();
                }
                else
                    grid.Add(currentLine);
            }

            MaxSize = (int)Math.Sqrt(_tiles.Count);
            _solvedGrid = new Possibility[MaxSize, MaxSize];
        }

        //private int[,] force = new int[3, 3]
        //{
        //    {1951, 2311, 3079},
        //    {2729, 1427, 2473},
        //    {2971, 1489, 1171}
        //};

        internal static Possibility[,] _solvedGrid;
        private Dictionary<int, Tile> _remainingTiles = new Dictionary<int, Tile>();
        public string SolveFirstProblem()
        {
            foreach (var (key, value) in _tiles)
            {
                _remainingTiles.Add(key, value);
            }

            //Console.WriteLine();
            //Console.WriteLine("1951 : " + _tiles[1951].PossibleBordersValues.Aggregate("", (s, i) => s + "," + i));
            //Console.WriteLine("2311 : " + _tiles[2311].PossibleBordersValues.Aggregate("", (s, i) => s + "," + i));
            //Console.WriteLine("2729 : " + _tiles[2729].PossibleBordersValues.Aggregate("", (s, i) => s + "," + i));
            //Console.WriteLine();

            if (!SolveCell(0, 0))
                return "error, DUH !";
            else
            {
                var res = new BigInteger(_solvedGrid[0, 0].Tile);
                res *= _solvedGrid[0, MaxSize - 1].Tile;
                res *= _solvedGrid[MaxSize - 1, MaxSize - 1].Tile;
                res *= _solvedGrid[MaxSize - 1, 0].Tile;
                return res.ToString();
            }
        }

        public bool SolveCell(int x, int y)
        {
            foreach (var (tileId, tile) in _remainingTiles.ToList())
            {
                //  var tileId = force[y, x];
                //var tile = _tiles[tileId];
                _remainingTiles.Remove(tileId);
                IEnumerable<Possibility> possibilities;
                if (x == 0 && y == 0)
                    possibilities = tile.GetAllPossibilities();
                else if (y == 0)
                    possibilities = tile.GetAllPossibilities(_solvedGrid[y, x - 1].Directions[1]);
                else if (x == 0)
                    possibilities = tile.GetAllPossibilitiesTopOnly(_solvedGrid[y - 1, x].Directions[2]);
                else
                    possibilities = tile.GetAllPossibilities(_solvedGrid[y, x - 1].Directions[1], _solvedGrid[y - 1, x].Directions[2]);
                foreach (var possibility in possibilities)
                {
                    _solvedGrid[y, x] = possibility;
                    if (y == MaxSize - 1 && x == MaxSize - 1)
                        return true;

                    var nextX = x + 1;
                    var nextY = y;
                    if (nextX == MaxSize)
                    {
                        nextX = 0;
                        nextY = y + 1;
                    }

                    if (SolveCell(nextX, nextY))
                        return true;
                    _solvedGrid[y, x] = null;

                }
                _remainingTiles.Add(tileId, tile);
            }

            return false;
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            var megaGrid = Tile.FromSolvedGrid();
            int step = 0; // 0 : normal, 1: flipHorizontally, 2: flip both, 3: flip vertically
            while (step < 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    var count = megaGrid.GetSeaMonsterCount();
                    if (count != 0)
                    {
                        int seadepth = 0;
                        foreach (var line in megaGrid.Grid)
                            seadepth += line.Count(c => c == '#');
                        return (seadepth - count * 15).ToString();
                    }

                    megaGrid.Rotate();
                }
                if (step == 0)
                    megaGrid.SwapHorizontally();
                else if (step == 1)
                    megaGrid.SwapVertically();
                else if (step == 2)
                    megaGrid.SwapHorizontally();


                step++;
            }

            return "Not found at all :-(";
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
