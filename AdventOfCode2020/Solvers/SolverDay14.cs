using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay14 : ISolver
    {
        class Operation
        {
            public int MemoryPosition { get; }
            public int Value { get; }
            private readonly string _mask;

            public Operation(string mask, int memoryPosition, int value)
            {
                MemoryPosition = memoryPosition;
                Value = value;
                _mask = mask;
            }

            public BigInteger MaskedValue
            {
                get
                {
                    string binary = Convert.ToString(Value, 2);
                    var diff = _mask.Length - binary.Length;
                    char[] res = new char[_mask.Length];
                    for (int i = _mask.Length - 1; i >= 0; i--)
                    {
                        if (_mask[i] == 'X')
                        {
                            if (i >= diff)
                            {
                                res[i] = binary[i -  diff];
                            }
                            else res[i] = '0';
                        }
                        else
                        {
                            res[i] = _mask[i];
                        }
                    }

                    BigInteger result = 0;
                    int exponent = 0;

                    for (int i = res.Length - 1; i >= 0; i--, exponent++)
                    {
                        if (res[i] == '1')
                            result += BigInteger.Pow(2, exponent);
                    }

                    return result;
                }
            }
            public IEnumerable<string> MaskedMemoryPositions
            {
                get
                {
                    string binary = Convert.ToString(MemoryPosition, 2);
                    var diff = _mask.Length - binary.Length;
                    char[] res = new char[_mask.Length];
                    for (int i = _mask.Length - 1; i >= 0; i--)
                    {
                        if (_mask[i] == '0')
                        {
                            if (i >= diff)
                            {
                                res[i] = binary[i - diff];
                            }
                            else res[i] = '0';
                        }
                        else
                        {
                            res[i] = _mask[i];
                        }
                    }

                    return GetMaskedMemoryPositions(new string(res));
                }
            }

            private IEnumerable<string> GetMaskedMemoryPositions(string s)
            {
                if (s.Contains('X'))
                {
                    var iof = s.IndexOf('X');
                    var sb = new StringBuilder(s);
                    sb[iof] = '0';
                    foreach (var subStr in GetMaskedMemoryPositions(sb.ToString()))
                    {
                        yield return subStr;
                    }

                    sb[iof] = '1';
                    foreach (var subStr in GetMaskedMemoryPositions(sb.ToString()))
                    {
                        yield return subStr;
                    }
                }
                else
                    yield return s;
            }
        }

        private List<Operation> _operations = new List<Operation>();
        private Dictionary<int, BigInteger> _memoryValues = new Dictionary<int, BigInteger>();
        private Dictionary<string, BigInteger> _ex2MemoryValues = new Dictionary<string, BigInteger>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            string currentMask = "";
            foreach (var currentLine in splitContent)
            {
                if (currentLine.StartsWith("mask = "))
                {
                    currentMask = currentLine.Replace("mask = ", "");
                }
                else
                {
                    var split = currentLine.Split(new string[] { "] = " }, StringSplitOptions.RemoveEmptyEntries);
                    _operations.Add(new Operation(currentMask, int.Parse(split[0].Replace("mem[", "")), int.Parse(split[1])));
                }

            }
        }

        public string SolveFirstProblem()
        {
            foreach (var operation in _operations)
            {
                _memoryValues[operation.MemoryPosition] = operation.MaskedValue;
            }

            BigInteger result = 0;
            foreach (var memoryValue in _memoryValues)
            {
                result += memoryValue.Value;
            }

            return result.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            foreach (var operation in _operations)
            {
                foreach (var operationMaskedMemoryPosition in operation.MaskedMemoryPositions)
                {
                    _ex2MemoryValues[operationMaskedMemoryPosition] = operation.Value;
                }
            }

            BigInteger result = 0;
            foreach (var memoryValue in _ex2MemoryValues)
            {
                result += memoryValue.Value;
            }

            return result.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
