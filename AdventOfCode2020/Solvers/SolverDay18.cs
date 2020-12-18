using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay18 : ISolver
    {
        class Expression
        {
            internal string _line;

            public Expression(string line)
            {
                _line = line;
            }

            public void ForcePlusPrecedence()
            {
                int currentPlusIndex = 0;
                while ((currentPlusIndex = _line.IndexOf('+', currentPlusIndex)) != -1)
                {

                    if (_line[currentPlusIndex + 1] != '(')
                        _line = _line.Insert(currentPlusIndex + 2, ")");
                    else
                    {
                        var opIdx = FindClosingParenthesis(currentPlusIndex + 1);

                        _line = _line.Insert(opIdx, ")");
                    }

                    if (_line[currentPlusIndex - 1] != ')')
                    {
                        _line = _line.Insert(currentPlusIndex - 1, "(");
                    }
                    else
                    {
                        var opIdx = FindOpeningParenthesis(currentPlusIndex - 1);

                        _line = _line.Insert(opIdx, "(");
                    }

                    currentPlusIndex += 2;
                }
            }

            public BigInteger Calculate()
            {
                BigInteger currentValue = 0;
                bool previousOrderIsAdd = true;
                List<Expression> subExpressions = new List<Expression>();

                for (int i = 0; i < _line.Length; i++)
                {
                    if (int.TryParse(_line[i].ToString(), out var intValue))
                    {
                        if (previousOrderIsAdd)
                            currentValue += intValue;
                        else
                            currentValue *= intValue;
                    }
                    else if (_line[i] == '+')
                        previousOrderIsAdd = true;
                    else if (_line[i] == '*')
                        previousOrderIsAdd = false;
                    else if (_line[i] == '(')
                    {
                        (BigInteger exprVal, int endIndex) = TreatSubExpression(i);
                        if (previousOrderIsAdd)
                            currentValue += exprVal;
                        else
                            currentValue *= exprVal;
                        i = endIndex;
                    }
                    else
                        throw new Exception("Duh !");
                }

                return currentValue;
            }

            private int FindOpeningParenthesis(in int startIndex)
            {
                var depth = 0;
                for (int i = startIndex - 1; i >= 0; i--)
                {
                    if (_line[i] == ')')
                        depth++;
                    else if (_line[i] == '(')
                    {
                        if (depth > 0)
                            depth--;
                        else
                            return i;
                    }
                }
                throw new Exception("Dah !");
            }
            private int FindClosingParenthesis(in int startIndex)
            {
                var depth = 0;
                for (int i = startIndex + 1; i < _line.Length; i++)
                {
                    if (_line[i] == '(')
                        depth++;
                    else if (_line[i] == ')')
                    {
                        if (depth > 0)
                            depth--;
                        else
                            return i;
                    }
                }
                throw new Exception("Dah !");
            }

            private (BigInteger, int) TreatSubExpression(in int startIndex)
            {
                var cp = FindClosingParenthesis(startIndex);
                return (new Expression(_line.Substring(startIndex + 1, cp - startIndex - 1)).Calculate(),
                    cp);
            }
        }

        private List<Expression> _expressions = new List<Expression>();
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var currentLine in splitContent)
            {
                _expressions.Add(new Expression(currentLine.Replace(" ", "")));
            }
        }

        public string SolveFirstProblem()
        {
            BigInteger result = 0;
            foreach (var expression in _expressions)
            {
                result += expression.Calculate();
            }
            return result.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            BigInteger result = 0;
            foreach (var expression in _expressions)
            {
                //var oldExpr = expression._line;
                expression.ForcePlusPrecedence();


                var res = expression.Calculate();
                //Console.WriteLine($"{oldExpr} became {expression._line} = {res}");
                result += res;

            }
            return result.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
