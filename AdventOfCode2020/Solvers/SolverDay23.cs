using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2020.Solvers
{
    class SolverDay23 : ISolver
    {
        internal class Node
        {
            public int Value { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }

            public Node(int v)
            {
                Value = v;
            }

            public override string ToString()
            {
                return (Prev?.Value.ToString() ??"") + ("<=" + Value + "=>") + (Next?.Value.ToString() ?? "");
            }
        }

        private Node _firstCupsEx1;
        private Node _firstCupsEx2;
        private Dictionary<int, Node> _nodesDictionary = new Dictionary<int, Node>();
        private Dictionary<int, Node> _ex1NodesDictionary = new Dictionary<int, Node>();
        private static int Ex1Count = 0;
        private const int Ex2Count = 10_000_000;
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            Node prev1 = null;
            Node prev2 = null;
            int max = 0;
            foreach (var value in splitContent[0].Select(x => int.Parse(x.ToString())))
            {
                Ex1Count++;
                if (value > max)
                    max = value;
                if (_firstCupsEx1 == null)
                {
                    _firstCupsEx1 = new Node(value);
                    prev1 = _firstCupsEx1;
                    _firstCupsEx2 = new Node(value);
                    prev2 = _firstCupsEx2;
                }
                else
                {
                    var current1 = new Node(value);
                    var current2 = new Node(value);
                    prev1.Next = current1;
                    current1.Prev = prev1;
                    prev2.Next = current2;
                    current2.Prev = prev2;

                    prev1 = current1;
                    prev2 = current2;
                }
            }

            _firstCupsEx1.Prev = prev1;
            prev1.Next = _firstCupsEx1;

            for (int i = max + 1; i <= 1000000; i++)
            {
                var current2 = new Node(i);
                prev2.Next = current2;
                current2.Prev = prev2;
                prev2 = current2;
            }
            _firstCupsEx2.Prev = prev2;
            prev2.Next = _firstCupsEx2;

            for (var node = _firstCupsEx1; node.Next != _firstCupsEx1; node = node.Next)
            {
                _ex1NodesDictionary[node.Value] = node;
            }
            _ex1NodesDictionary[_firstCupsEx1.Prev.Value] = _firstCupsEx1.Prev;

            for (var node = _firstCupsEx2; node.Next != _firstCupsEx2; node = node.Next)
            {
                _nodesDictionary[node.Value] = node;
            }
            _nodesDictionary[_firstCupsEx2.Prev.Value] = _firstCupsEx2.Prev;
        }

        public string SolveFirstProblem()
        {
            var current = _firstCupsEx1;
            for (int i = 0; i < 100; i++)
            {
                SwapCups(current, _ex1NodesDictionary);
                current = current.Next;
            }

            string result = "";
            var startNode = _ex1NodesDictionary[1];
            for (int i = 1; i < Ex1Count; i++)
            {
                startNode = startNode.Next;
                result += startNode.Value;
            }

            return result;
        }

        private void SwapCups(Node node, IDictionary<int, Node> dict)
        {
            var current = node.Value;

            #region get the 3 elements to move

            List<int> toMove = new List<int>();
            var startToMove = node.Next;
            var midToMove = node.Next.Next;
            var endToMove = node.Next.Next.Next;

            #endregion

            var next = current;
            do
            {
                next--;
                if (next == 0) next = dict.Count;
            } while (startToMove.Value == next || midToMove.Value == next || endToMove.Value == next);

            var nextNode = dict[next];

            // 1 2 3 (4 5  6 ) 7 8 |9|
            // 1 2 3  7 8 |9| (4 5  6)
            var tempOldNext = nextNode.Next;
            var tempstartToMovePrev = startToMove.Prev;
            var tempOldEndMove = endToMove.Next;
            var tempnextnext = nextNode.Next;

            nextNode.Next = startToMove;
            startToMove.Prev = nextNode;
            endToMove.Next = tempnextnext;

            tempstartToMovePrev.Next = tempOldEndMove;
            tempOldEndMove.Prev = tempstartToMovePrev;
            tempOldNext.Prev = endToMove;
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            var current = _firstCupsEx2;
            for (int i = 0; i < 10000000; i++)
            {
                //if (i % 100000 == 0)
                //    Console.WriteLine("Round" + i);
                SwapCups(current, _nodesDictionary);
                current = current.Next;
            }

            BigInteger result = 1;
            var startNode = _nodesDictionary[1];
            for (int i = 0; i < 2; i++)
            {
                startNode = startNode.Next;
                result *= startNode.Value;
            }

            return result.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}