using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay25 : ISolver
    {
        private BigInteger publicKey1;
        private BigInteger publicKey2;
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.None);

            publicKey1 = BigInteger.Parse(splitContent[0]);
            publicKey2 = BigInteger.Parse(splitContent[1]);
        }

        public BigInteger LoopOver(BigInteger sn, BigInteger value)
        {
            return (value * sn) % 20201227;
        }

        public int FindLoopValueFor(BigInteger finalValue, BigInteger sn)
        {
            BigInteger value = 1;
            for (int i = 1; i > -1; i++)
            {
                value = LoopOver(sn, value);
                if (value == finalValue)
                    return i;
            }

            return -1;
        }
        public BigInteger CalculateLoop(BigInteger loopValue, BigInteger sn)
        {
            BigInteger current = 1;
            for (int i = 0; i < loopValue; i++)
            {
                current = LoopOver(sn, current);
            }

            return current;
        }

        public string SolveFirstProblem()
        {
            var loopSize1 = FindLoopValueFor(publicKey1, 7);
            var loopSize2 = FindLoopValueFor(publicKey2, 7);
            var serial = CalculateLoop(loopSize1, publicKey2);
            var serial2 = CalculateLoop(loopSize2, publicKey1);

            if (serial != serial2)
                throw new NotImplementedException();

            return serial.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            throw new NotImplementedException();
        }

        public bool Question2CodeIsDone { get; } = false;
    }
}
