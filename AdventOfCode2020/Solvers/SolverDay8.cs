using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay8 : ISolver
    {
        public enum Order
        {
            Accumulate,
            Jump,
            NoAction
        }

        public class Instruction
        {
            public Order Action { get; private set; }
            public int Value { get;  }
            public bool ExecutedOnce { get; set; }

            public Instruction( int value, Order action)
            {
                Value = value;
                Action = action;
                ExecutedOnce = false;
            }

            public Instruction Copy()
            {
                return new Instruction(Value, Action);
            }
            public bool Switch()
            {
                if (Action == Order.Accumulate)
                    return false;

                Action = Action == Order.Jump ? Order.NoAction : Order.Jump;
                return true;
            }
        }

        class VideoGameAsm
        {
            public List<Instruction> Instructions { get; } = new List<Instruction>();
            public int Accumulator { get; private set; }
            internal int CurrentPosition { get; private set; } = 0;

            public void Init(string[] instructions)
            {
                foreach (var instruction in instructions)
                {
                    var split = instruction.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    Order order = Order.NoAction;
                    switch (split[0])
                    {
                        case "acc":
                            order = Order.Accumulate;
                            break;
                        case "jmp":
                            order = Order.Jump;
                            break;
                    }

                    Instructions.Add(new Instruction(int.Parse(split[1]), order));
                }
            }

            private void Execute(Instruction i)
            {
                i.ExecutedOnce = true;
                switch (i.Action)
                {
                    case Order.Accumulate:
                        Accumulator += i.Value;
                        CurrentPosition += 1;
                        break;
                    case Order.Jump:
                        CurrentPosition += i.Value;
                        break;
                    case Order.NoAction:
                        CurrentPosition += 1;
                        break;
                }
            }

            public void Execute(Predicate<Instruction> stopCondition)
            {
                var currentInstruction = Instructions[CurrentPosition];
                while (!stopCondition(currentInstruction))
                {
                    Execute(currentInstruction);
                    currentInstruction = Instructions[CurrentPosition];
                }
            }

            public bool Execute(Predicate<Instruction> instructionStopCondition, Predicate<int> positionStopCcondition)
            {
                var currentInstruction = Instructions[CurrentPosition];
                while (!instructionStopCondition(currentInstruction))
                {
                    Execute(currentInstruction);
                    if (positionStopCcondition(CurrentPosition))
                        return true;
                    currentInstruction = Instructions[CurrentPosition];
                }

                return false;
            }

            public VideoGameAsm Copy()
            {
                var asm = new VideoGameAsm();
                foreach (var instruction in Instructions)
                {
                    asm.Instructions.Add(instruction.Copy());
                }

                return asm;
            }
        }

        private VideoGameAsm _game = new VideoGameAsm();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            _game.Init(splitContent);
        }

        public string SolveFirstProblem()
        {
            _game.Execute(i => i.ExecutedOnce);
            return _game.Accumulator.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            for (int i = 0; i < _game.Instructions.Count; i++)
            {
                if (_game.Instructions[i].Action == Order.Accumulate)
                    continue;

                var gameCopy = _game.Copy();
                gameCopy.Instructions[i].Switch();
                if (gameCopy.Execute(i => i.ExecutedOnce, p => p == _game.Instructions.Count))
                    return gameCopy.Accumulator.ToString();
            }

            return "Not Found";

        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
