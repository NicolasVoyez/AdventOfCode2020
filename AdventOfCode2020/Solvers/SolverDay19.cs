using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Solvers
{
    class SolverDay19 : ISolver
    {
        private static readonly Dictionary<string, string> _rules = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _flatenedRulesPart1 = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _flatenedRulesPart2 = new Dictionary<string, string>();
        private List<string> _proposals = new List<string>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            bool doProposals = false;

            foreach (var currentLine in splitContent)
            {
                if (string.IsNullOrEmpty(currentLine))
                    doProposals = true;
                else if (doProposals)
                    _proposals.Add(currentLine);
                else
                {
                    var s = currentLine.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                    var ruleNum = s[0];

                    if (s[1].Contains('\"'))
                    {
                        _rules[ruleNum] = s[1].Trim('\"');
                        _flatenedRulesPart1[ruleNum] = _rules[ruleNum];
                        _flatenedRulesPart2[ruleNum] = _rules[ruleNum];
                    }
                    else if (s[1].Contains(('|')))
                    {
                        _rules[ruleNum] = "(" + s[1] + ")";
                    }
                    else
                        _rules[ruleNum] = s[1];
                }
            }
        }

        public string SolveFirstProblem()
        {
            FlatenProblem(_flatenedRulesPart1);

            var zeroString = "^" + _flatenedRulesPart1["0"].Replace(" ", "") + "$";
            var regex = new Regex(zeroString);

            return _proposals.Count(p => Regex.IsMatch(p,zeroString) ).ToString();
        }

        private static void FlatenProblem(Dictionary<string,string> flatenedDic)
        {
            // simple flaten
            while (_rules.Count != flatenedDic.Count)
            {
                Dictionary<string,string> newFlaten = new Dictionary<string, string>();
                foreach (var (ruleKey, ruleValue) in _rules)
                {
                    if (flatenedDic.ContainsKey(ruleKey))
                        continue;
                    var newRule = ruleValue;

                    foreach (var searchedKey in newRule.Split(new[] { ' ', '|', '(', ')' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (flatenedDic.TryGetValue(searchedKey, out var flatRuleValue))
                        {
                            int curr = 0;
                            var idx = newRule.IndexOf(searchedKey, curr);
                            while (idx != -1)
                            {
                                if (!((idx != 0 && char.IsDigit(newRule[idx - 1])) ||
                                    (idx != newRule.Length - searchedKey.Length && char.IsDigit(newRule[idx + searchedKey.Length]))))
                                {
                                    newRule = newRule.Substring(0, idx) + flatRuleValue +
                                              newRule.Substring(idx + searchedKey.Length);
                                }

                                curr = idx + 1;
                                idx = newRule.IndexOf(searchedKey, curr);
                            }
                        }
                    }

                    if (!newRule.Any(char.IsDigit))
                    {
                        flatenedDic[ruleKey] = newRule;
                    }
                }
            }
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            _rules["8"] = "( 42 )+";
            // theoretically, it could have gone to 16 times, but well, it works well with only 6 max:-)
            _rules["11"] = "( 42 31 | 42 42 31 31 | 42 42 42 31 31 31 | 42 42 42 42 31 31 31 31 | 42 42 42 42 42 31 31 31 31 31 | 42 42 42 42 42 31 31 31 31 31 31 )";
            FlatenProblem(_flatenedRulesPart2);

            var zeroString = "^" + _flatenedRulesPart2["0"].Replace(" ", "") + "$";
            var regex = new Regex(zeroString);

            return _proposals.Count(p => Regex.IsMatch(p, zeroString)).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
