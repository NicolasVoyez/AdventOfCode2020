using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay12 : ISolver
    {
        enum Order
        {
            West = 0,
            North = 1,
            East = 2,
            South = 3,
            Forward,
            RotateLeft,
            RotateRight
        }

        struct NavigationAction
        {
            public Order Order { get; }
            public int Value { get; }

            public NavigationAction(string order):this(GetOrder(order[0]), int.Parse(order.Substring(1)))
            {

            }

            private static Order GetOrder(char c)
            {
                switch (c)
                {
                    case 'E': return Order.East;
                    case 'W': return Order.West;
                    case 'N': return Order.North;
                    case 'S': return Order.South;
                    case 'L': return Order.RotateLeft;
                    case 'R': return Order.RotateRight;
                    case 'F': return Order.Forward;
                }
                throw new NotImplementedException();
            }

            public NavigationAction(Order o, int value)
            {
                Order = o;
                Value = value;
            }
        }

        class Boat
        {
            public int East { get; set; }
            public int North { get; set; }

            public int WayPointEast { get; set; } = 10;
            public int WayPointNorth { get; set; } = 1;
            public Order FacingDirection { get; private set; } = Order.East;

            public Boat()
            {
            }

            public void MoveExercice1(NavigationAction action)
            {
                switch (action.Order)
                {
                    case Order.West:
                        East -= action.Value;
                        break;
                    case Order.North:
                        North += action.Value;
                        break;
                    case Order.East:
                        East += action.Value;
                        break;
                    case Order.South:
                        North -= action.Value;
                        break;
                    case Order.Forward:
                        MoveExercice1(new NavigationAction(FacingDirection, action.Value));
                        break;
                    case Order.RotateRight:
                        FacingDirection = (Order) (((int) FacingDirection + action.Value / 90) % 4);
                        break;
                    case Order.RotateLeft:
                        var val = ((int) FacingDirection - action.Value / 90);
                        if (val < 0)
                            val += 4;
                        FacingDirection = (Order)(val % 4);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            public void MoveExercice2(NavigationAction action)
            {
                switch (action.Order)
                {
                    case Order.West:
                        WayPointEast -= action.Value;
                        break;
                    case Order.North:
                        WayPointNorth += action.Value;
                        break;
                    case Order.East:
                        WayPointEast += action.Value;
                        break;
                    case Order.South:
                        WayPointNorth -= action.Value;
                        break;
                    case Order.Forward:
                        East += WayPointEast * action.Value;
                        North += WayPointNorth * action.Value;
                        break;
                    case Order.RotateRight:
                        var tempNorth = WayPointNorth;
                        var tempEast = WayPointEast;
                        if (action.Value == 90)
                        {
                            WayPointEast = tempNorth;
                            WayPointNorth = -tempEast;
                        }
                        else if (action.Value == 180)
                        {
                            WayPointEast = -tempEast;
                            WayPointNorth = -tempNorth;
                        }
                        else
                        {
                            WayPointEast = -tempNorth;
                            WayPointNorth = tempEast;
                        }
                        break;
                    case Order.RotateLeft:
                        var tn = WayPointNorth;
                        var te = WayPointEast;
                        if (action.Value == 90)
                        {
                            WayPointEast = -tn;
                            WayPointNorth = te;
                        }
                        else if (action.Value == 180)
                        {
                            WayPointEast = -te;
                            WayPointNorth = -tn;
                        }
                        else
                        {
                            WayPointEast = tn;
                            WayPointNorth = -te;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private List<NavigationAction> _navigationActions = new List<NavigationAction>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var currentLine in splitContent)
            {
                _navigationActions.Add(new NavigationAction(currentLine));
            }
        }

        public string SolveFirstProblem()
        {
            var boat = new Boat();
            foreach (var navigationAction in _navigationActions)
            {
                boat.MoveExercice1(navigationAction);
            }

            return (Math.Abs(boat.East) + Math.Abs(boat.North)).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            var boat = new Boat();
            foreach (var navigationAction in _navigationActions)
            {
                boat.MoveExercice2(navigationAction);
            }

            return (Math.Abs(boat.East) + Math.Abs(boat.North)).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
