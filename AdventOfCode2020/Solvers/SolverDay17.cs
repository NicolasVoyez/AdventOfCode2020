using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay17 : ISolver
    {
        class Point
        {
            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int ZPrime { get; }

            public Point(int x, int y, int z) : this(x, y, z, 0)
            {
            }

            public Point(int x, int y, int z, int zPrime)
            {
                X = x;
                Y = y;
                Z = z;
                ZPrime = zPrime;
            }

            public bool IsNeighbor(Point p)
            {
                return Math.Abs(p.X - X) <= 1 && Math.Abs(p.Y - Y) <= 1 && Math.Abs(p.Z - Z) <= 1 && Math.Abs(p.ZPrime - ZPrime) <= 1; ;
            }
            public static bool operator !=(Point p1, Point p2)
            {
                return !(p1 == p2);
            }

            public static bool operator ==(Point p1, Point p2)
            {
                return p1.X == p2.X && p1.Y == p2.Y &&
                       p1.Z == p2.Z && p1.ZPrime == p2.ZPrime;

            }

        }

        private int _minX = 0;
        private int _maxX = 0;
        private int _minY = 0;
        private int _maxY = 0;
        private int _minZ = 0;
        private int _maxZ = 0;
        private int _minZPrime = 0;
        private int _maxZPrime = 0;

        private List<Point> _activePoints = new List<Point>();
        private List<Point> _initialActivePoints = new List<Point>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            for (int y = 0; y < splitContent.Length; y++)
            {
                var currentLine = splitContent[y];
                for (int x = 0; x < currentLine.Length; x++)
                {
                    if (currentLine[x] == '#')
                    {
                        _activePoints.Add(new Point(x, y, 0));
                        _initialActivePoints.Add(new Point(x, y, 0));
                    }
                }
            }

        }

        private void SizeMaxs(bool growZPrime = false)
        {
            _minX = 0;
            _maxX = 0;
            _minY = 0;
            _maxY = 0;
            _minZ = 0;
            _maxZ = 0;
            _minZPrime = 0;
            _maxZPrime = 0;

            foreach (var activePoint in _activePoints)
            {
                if (activePoint.X < _minX)
                    _minX = activePoint.X;
                if (activePoint.X > _maxX)
                    _maxX = activePoint.X;
                if (activePoint.Y < _minY)
                    _minY = activePoint.Y;
                if (activePoint.Y > _maxY)
                    _maxY = activePoint.Y;
                if (activePoint.Z < _minZ)
                    _minZ = activePoint.Z;
                if (activePoint.Z > _maxZ)
                    _maxZ = activePoint.Z;
                if (activePoint.ZPrime < _minZPrime)
                    _minZPrime = activePoint.ZPrime;
                if (activePoint.ZPrime > _maxZPrime)
                    _maxZPrime = activePoint.ZPrime;
            }

            _minX--;
            _maxX++;
            _minY--;
            _maxY++;
            _minZ--;
            _maxZ++;
            if (growZPrime)
            {
                _minZPrime --;
                _maxZPrime ++;
            }
        }

        public string SolveFirstProblem()
        {
            for (int cycle = 0; cycle < 6; cycle++)
            {
                SizeMaxs();
                Evolve();
            }

            return _activePoints.Count.ToString();
        }

        private void Evolve()
        {
            List<Point> newActives = new List<Point>();

            for (int z = _minZ; z <= _maxZ; z++)
            {
                for (int y = _minY; y <= _maxY; y++)
                {
                    for (int x = _minX; x <= _maxX; x++)
                    {
                        int count = 0;
                        var current = new Point(x, y, z);
                        var isActive = false;
                        foreach (var point in _activePoints)
                        {
                            if (point == current)
                            {
                                isActive = true;
                                continue;
                            }
                            if (point.IsNeighbor(current))
                                count++;

                        }
                        if (count == 3 || ((isActive && count == 2)))
                            newActives.Add(current);
                    }
                }
            }

            _activePoints = newActives;
        }
        private void Evolve4d()
        {
            List<Point> newActives = new List<Point>();

            for (int zprime = _minZPrime; zprime <= _maxZPrime; zprime++)
            {
                for (int z = _minZ; z <= _maxZ; z++)
                {
                    for (int y = _minY; y <= _maxY; y++)
                    {
                        for (int x = _minX; x <= _maxX; x++)
                        {
                            int count = 0;
                            var current = new Point(x, y, z, zprime);
                            var isActive = false;
                            foreach (var point in _activePoints)
                            {
                                if (point == current)
                                {
                                    isActive = true;
                                    continue;
                                }

                                if (point.IsNeighbor(current))
                                    count++;

                            }

                            if (count == 3 || ((isActive && count == 2)))
                                newActives.Add(current);
                        }
                    }
                }
            }

            _activePoints = newActives;
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            _activePoints = _initialActivePoints;
            for (int cycle = 0; cycle < 6; cycle++)
            {
                SizeMaxs(true);
                Evolve4d();
            }

            return _activePoints.Count.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
