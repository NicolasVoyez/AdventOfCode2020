using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay22 : ISolver
    {
        class Player
        {
            public LinkedList<int> Cards { get; }

            public Player(LinkedList<int> cards)
            {
                Cards = cards;
            }

            public bool HasLost => Cards.Count == 0;

            public BigInteger GetDeckValue()
            {
                BigInteger score = 0;
                int pow = 1;
                for (int i = Cards.Count - 1; i >= 0; i--)
                {
                    score += Cards.ElementAt(i) * pow;
                    pow++;
                }

                return score;
            }

            public override string ToString()
            {
                return string.Join(',', Cards);
            }
        }

        class Game
        {
            private readonly bool _mainRound;
            public HashSet<string> P1History = new HashSet<string>();
            public HashSet<string> P2History = new HashSet<string>();
            public Player Player1 { get; }
            public Player Player2 { get; }

            public Game(Player player1, Player player2, bool mainRound = false)
            {
                _mainRound = mainRound;
                Player2 = player2;
                Player1 = player1;
            }

            public void PlayARound()
            {
                _roundsDone++;
                //if (_mainRound && _roundsDone % 20 == 0))
                //    Console.WriteLine("Starting main round #" + _roundsDone);

                var p1Card = Player1.Cards.First.Value;
                Player1.Cards.RemoveFirst();
                var p2card = Player2.Cards.First.Value;
                Player2.Cards.RemoveFirst();

                if (p1Card > p2card)
                {
                    Player1.Cards.AddLast(p1Card);
                    Player1.Cards.AddLast(p2card);
                }
                else if (p1Card == p2card)
                    throw new NotImplementedException();
                else
                {
                    Player2.Cards.AddLast(p2card);
                    Player2.Cards.AddLast(p1Card);
                }
            }

            ///<summary> Returns true if P1 won </summary>
            public bool PlayAGame()
            {
                while (!Player1.HasLost && !Player2.HasLost)
                {
                    PlayARound();
                }

                return Player2.HasLost;
            }

            private int _roundsDone = 0;
            /// <summary>
            /// If True, end game with P1 victory
            /// </summary>
            /// <returns></returns>
            public bool PlayARecursiveRound()
            {
                _roundsDone++;
                //if (_mainRound && _roundsDone % 20 == 0)
                //    Console.WriteLine("Starting main round #" + _roundsDone);

                var p1HandHash = Player1.ToString();
                var p2HandHash = Player2.ToString();
                // early end
                if (!P1History.Add(p1HandHash))
                    return true;
                if (!P2History.Add(p2HandHash))
                    return true;

                /*
                if (_mainRound == false && Player1.Cards.Max() > Player2.Cards.Max())
                {
                    AlreadyTried.Add(roundHash, true);
                    return true;
                }*/

                bool p1Won;
                var p1Card = Player1.Cards.First.Value;
                Player1.Cards.RemoveFirst();
                var p2Card = Player2.Cards.First.Value;
                Player2.Cards.RemoveFirst();

                // recursivity for fun
                if (Player1.Cards.Count >= p1Card && Player2.Cards.Count >= p2Card)
                {
                    var subGame = new Game(new Player(new LinkedList<int>(Player1.Cards.Take(p1Card))),
                                           new Player(new LinkedList<int>(Player2.Cards.Take(p2Card))));
                    p1Won = subGame.PlayARecursiveGame();

                }
                else
                {
                    // normal game
                    p1Won = p1Card > p2Card;
                }

                if (p1Won)
                {
                    Player1.Cards.AddLast(p1Card);
                    Player1.Cards.AddLast(p2Card);
                }
                else
                {
                    Player2.Cards.AddLast(p2Card);
                    Player2.Cards.AddLast(p1Card);
                }

                return false;
            }

            public bool PlayARecursiveGame()
            {
                bool quickP1Win = false;
                while (!Player1.HasLost && !Player2.HasLost && !quickP1Win)
                {
                    quickP1Win = PlayARecursiveRound();
                }

                return Player2.HasLost || quickP1Win;
            }
        }

        private Player _me;
        private Player _crab;

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            LinkedList<int> myCards = new LinkedList<int>();
            LinkedList<int> enemyCards = new LinkedList<int>();
            bool getMyCards = true;
            foreach (var currentLine in splitContent)
            {
                if (currentLine.StartsWith("Player")) continue;
                if (string.IsNullOrEmpty(currentLine)) getMyCards = !getMyCards;
                else if (getMyCards)
                    myCards.AddLast(int.Parse(currentLine));
                else
                    enemyCards.AddLast(int.Parse(currentLine));
            }

            _me = new Player(myCards);
            _crab = new Player(enemyCards);
        }

        public string SolveFirstProblem()
        {
            var game = new Game(new Player(new LinkedList<int>(_me.Cards)), new Player(new LinkedList<int>(_crab.Cards)), true);
            if (!game.PlayAGame())
                return game.Player2.GetDeckValue().ToString();
            else
                return game.Player1.GetDeckValue().ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            var game = new Game(new Player(new LinkedList<int>(_me.Cards)), new Player(new LinkedList<int>(_crab.Cards)), true);
            if (!game.PlayARecursiveGame())
                return game.Player2.GetDeckValue().ToString();
            else
                return game.Player1.GetDeckValue().ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
