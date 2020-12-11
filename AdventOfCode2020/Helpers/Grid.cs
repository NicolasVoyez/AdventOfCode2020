using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode2020.Solvers;

namespace AdventOfCode2020
{
    public class Grid<T>
    {
        public struct Cell<T>
        {
            public int Y { get; }
            public int X { get; }
            public T Value { get; }

            public Cell(int y, int x, T value)
            {
                Y = y;
                X = x;
                Value = value;
            }
        }

        // Y,X
        private readonly T[,] _innerGrid;
        public int YMax { get; }
        public int XMax { get; }


        public Grid(string grid, Func<char, T> transform)
        {
            var splitContent = grid.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            YMax = splitContent.Length;
            XMax = splitContent[0].Length;

            _innerGrid = new T[YMax, XMax];

            for (int y = 0; y < YMax; y++)
            {
                for (int x = 0; x < XMax; x++)
                {
                    _innerGrid[y, x] = transform(splitContent[y][x]);
                }
            }
        }

        public Grid(T[,] grid, int yMax, int xMax)
        {
            this.YMax = yMax;
            this.XMax = xMax;
            this._innerGrid = grid;
        }

        public IEnumerable<Cell<T>> All()
        {
            for (int y = 0; y < YMax; y++)
            {
                for (int x = 0; x < XMax; x++)
                {
                    yield return this[y, x];
                }
            }
        }


        public IEnumerable<Cell<T>> Around(Cell<T> cell)
        {
            return Around(cell.Y, cell.X);
        }
        public IEnumerable<Cell<T>> Around(int y, int x)
        {
            if (y > 0)
            {
                if (x > 0)
                    yield return this[y - 1, x - 1];
                yield return this[y - 1, x];
                if (x < XMax - 1)
                    yield return this[y - 1, x + 1];
            }

            if (x > 0)
                yield return this[y, x - 1];

            if (x < XMax - 1)
                yield return this[y, x + 1];

            if (y < YMax - 1)
            {
                if (x > 0)
                    yield return this[y + 1, x - 1];
                yield return this[y + 1, x];
                if (x < XMax - 1)
                    yield return this[y + 1, x + 1];
            }
        }

        public IEnumerable<Cell<T>> GetFirstInEachDirection(int y, int x, Predicate<T> condition)
        {
            for (int dy = -1; dy <= 1; dy++)
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0)
                    continue;
                var cell = FindInDirection(y, x, dy, dx, condition);
                if (cell.HasValue)
                    yield return cell.Value;
            }
        }

        private Cell<T>? FindInDirection(int y, int x, int dy, int dx, Predicate<T> condition)
        {
            while(true)
            {
                y += dy;
                x += dx;

                if (y < 0 || x < 0 || y > YMax - 1 || x > XMax - 1)
                    return null;

                if (condition(_innerGrid[y,x]))
                    return this[y, x];
            }
        }

        public Cell<T> this[int y, int x] => new Cell<T>(y, x, _innerGrid[y, x]);

        public void Set(int y, int x, T value) => _innerGrid[y, x] = value;

        public void Print(Action<T> printElement)
        {
            for (int y = 0; y < YMax; y++)
            {
                for (int x = 0; x < XMax; x++)
                {
                    printElement(_innerGrid[y, x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
