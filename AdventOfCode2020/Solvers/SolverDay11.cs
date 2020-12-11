using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay11 : ISolver
    {
        enum CellStatus
        {
            EmptySeat,
            OccupiedSeat,
            Floor,
        }

        private Grid<CellStatus> _currentGrid;
        private Grid<CellStatus> _initialGrid;
        public void InitInput(string content)
        {
            _initialGrid = new Grid<CellStatus>(content, c =>
            {
                switch (c)
                {
                    case '.':
                        return CellStatus.Floor;
                    case 'L':
                        return CellStatus.EmptySeat;
                    case '#':
                        return CellStatus.OccupiedSeat;
                }

                return CellStatus.Floor; // default
            });
            _currentGrid = _initialGrid;
        }

        public string SolveFirstProblem()
        {
            //_currentGrid.Print(PrintCell);
            bool hasChanged = true;
            while (hasChanged)
            {
                var newGrid = new CellStatus[_currentGrid.YMax, _currentGrid.XMax];
                hasChanged = false;
                foreach (var cell in _currentGrid.All().ToList())
                {
                    if (cell.Value == CellStatus.EmptySeat && AreNoOccupiedSeatAround(_currentGrid.Around(cell)))
                    {
                        newGrid[cell.Y, cell.X] = CellStatus.OccupiedSeat;
                        hasChanged = true;
                    }
                    else if (cell.Value == CellStatus.OccupiedSeat && AreMoreTHan4NeighborOccupied(_currentGrid.Around(cell)))
                    {
                        newGrid[cell.Y, cell.X] = CellStatus.EmptySeat;
                        hasChanged = true;
                    }
                    else
                        newGrid[cell.Y, cell.X] = cell.Value;
                }

                _currentGrid = new Grid<CellStatus>(newGrid, _currentGrid.YMax, _currentGrid.XMax);
                //_currentGrid.Print(PrintCell);
            }

            return _currentGrid.All().Count(s => s.Value == CellStatus.OccupiedSeat).ToString();
        }

        private void PrintCell(CellStatus cell)
        {
            switch (cell)
            {
                case CellStatus.EmptySeat:
                    Console.Write('L');
                    break;
                case CellStatus.OccupiedSeat:
                    Console.Write('#');
                    break;
                case CellStatus.Floor:
                    Console.Write('.');
                    break;
                default:
                    Console.Write('?');
                    break;
            }
        }

        private bool AreMoreTHan4NeighborOccupied(IEnumerable<Grid<CellStatus>.Cell<CellStatus>> around)
        {
            int count = 0;
            foreach (var cell in around)
            {
                if (cell.Value == CellStatus.OccupiedSeat)
                    count++;
                if (count >= 4)
                    return true;
            }

            return false;
        }

        private bool AreNoOccupiedSeatAround(IEnumerable<Grid<CellStatus>.Cell<CellStatus>> around)
        {
            foreach (var cell in around)
            {
                if (cell.Value == CellStatus.OccupiedSeat)
                    return false;
            }

            return true;
        }

        private bool AreMoreTHan5VisibleOccupied(IEnumerable<Grid<CellStatus>.Cell<CellStatus>> visible)
        {
            int count = 0;
            foreach (var cell in visible)
            {
                if (cell.Value == CellStatus.OccupiedSeat)
                    count++;
                if (count >= 5)
                    return true;
            }

            return false;
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            _currentGrid = _initialGrid;
            bool hasChanged = true;
            while (hasChanged)
            {
                var newGrid = new CellStatus[_currentGrid.YMax, _currentGrid.XMax];
                hasChanged = false;
                foreach (var cell in _currentGrid.All().ToList())
                {
                    if (cell.Value == CellStatus.Floor)
                        newGrid[cell.Y, cell.X] = CellStatus.Floor;
                    else
                    {
                        var inDirection = _currentGrid.GetFirstInEachDirection(cell.Y, cell.X, c => c != CellStatus.Floor).ToList();
                        if (cell.Value == CellStatus.EmptySeat && AreNoOccupiedSeatAround(inDirection))
                        {
                            newGrid[cell.Y, cell.X] = CellStatus.OccupiedSeat;
                            hasChanged = true;
                        }
                        else if (cell.Value == CellStatus.OccupiedSeat && AreMoreTHan5VisibleOccupied(inDirection))
                        {
                            newGrid[cell.Y, cell.X] = CellStatus.EmptySeat;
                            hasChanged = true;
                        }
                        else
                            newGrid[cell.Y, cell.X] = cell.Value;
                    }
                }

                _currentGrid = new Grid<CellStatus>(newGrid, _currentGrid.YMax, _currentGrid.XMax);
               // _currentGrid.Print(PrintCell);
            }

            return _currentGrid.All().Count(s => s.Value == CellStatus.OccupiedSeat).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
