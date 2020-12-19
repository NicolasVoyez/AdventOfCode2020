using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class BrokenSolverBruteforceDay19 : ISolver
    {
        internal interface IRule
        {
            int RuleNumber { get; }
            IEnumerable<string> Flatify();
        }

        class CharacterRule : IRule
        {
            public int RuleNumber { get; }
            private readonly string _letter;

            public CharacterRule(string letter, int ruleNumber)
            {
                RuleNumber = ruleNumber;
                _letter = letter;
            }

            public IEnumerable<string> Flatify()
            {
                yield return _letter;
            }
        }

        class ThenRule : IRule
        {
            public int RuleNumber { get; }
            public List<int> Rules { get; }

            public ThenRule(List<int> rules, int ruleNumber)
            {
                Rules = rules;
                RuleNumber = ruleNumber;
            }

            public IEnumerable<string> Flatify()
            {
                List<string> flatWords = _rules[Rules[0]].Flatify().ToList();
                for (int i = 1; i < Rules.Count; i++)
                {
                    List<string> newWords = new List<string>();
                    foreach (var word in flatWords)
                    {
                        foreach (var word2 in _rules[Rules[i]].Flatify())
                        {
                            newWords.Add(word + word2);
                        }
                    }

                    flatWords = newWords;
                }

                return flatWords;
            }
        }

        class OrRule : IRule
        {
            public int RuleNumber { get; }
            public ThenRule RuleOne { get; }
            public ThenRule RuleOr { get; }

            public OrRule(ThenRule rule1, ThenRule rule2, int ruleNumber)
            {
                RuleOne = rule1;
                RuleOr = rule2;
                RuleNumber = ruleNumber;
            }

            public IEnumerable<string> Flatify()
            {
                foreach (var one in RuleOne.Flatify())
                {
                    yield return one;
                }
                foreach (var orAnother in RuleOr.Flatify())
                {
                    yield return orAnother;
                }
            }
        }

        class EightRule : IRule
        {
            public int RuleNumber { get; } = 8;

            public IEnumerable<string> Flatify()
            {
                List<string> initflatWords = _rules[42].Flatify().ToList();
                List<string> flatWords = new List<string>();
                foreach (var word in initflatWords)
                {
                    yield return word;
                    flatWords.Add(word);
                }
                for (int i = 1; i < 4; i++)
                {
                    List<string> newWords = new List<string>();
                    foreach (var word in flatWords)
                    {
                        foreach (var word2 in initflatWords)
                        {
                            var newWord = word + word2;
                            newWords.Add(newWord);
                            yield return newWord;
                        }
                    }

                    flatWords = newWords;
                }
            }
        }

        class ElevenRule : IRule
        {
            public int RuleNumber { get; } = 11;

            public IEnumerable<string> Flatify()
            {
                HashSet<string> flatWords42 = new HashSet<string>(_rules[42].Flatify());
                HashSet<string> flatWords31 = new HashSet<string>(_rules[31].Flatify());
                HashSet<string> flatWords = new HashSet<string>();
                foreach (var word in flatWords42)
                {
                    foreach (var word2 in flatWords31)
                    {
                        var newWord = word + word2;
                        yield return newWord;
                        flatWords.Add(newWord);
                    }
                }
                for (int i = 1; i < 4; i++)
                {
                    HashSet<string> newWords = new HashSet<string>();
                    foreach (var word11 in flatWords)
                    {
                        foreach (var word42 in flatWords42)
                        {
                            foreach (var word31 in flatWords31)
                            {
                                var newWord = word42 + word11 + word31;
                                newWords.Add(newWord);
                                yield return newWord;
                            }
                        }
                    }

                    flatWords = newWords;
                }
            }
        }

        private static readonly Dictionary<int, IRule> _rules = new Dictionary<int, IRule>();
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
                    var ruleNum = int.Parse(s[0]);

                    if (s[1].Contains('\"'))
                    {
                        _rules[ruleNum] = new CharacterRule(s[1].Trim('\"'), ruleNum);
                        continue;
                    }

                    if (s[1].Contains(('|')))
                    {
                        var split = s[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        _rules[ruleNum] = new OrRule(CreateThenRule(split[0], -1), CreateThenRule(split[1], -1), ruleNum);
                    }
                    else
                        _rules[ruleNum] = CreateThenRule(s[1], ruleNum);

                }
            }
        }

        private ThenRule CreateThenRule(string str, int ruleNumber)
        {
            return new ThenRule(str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList(), ruleNumber);
        }

        public string SolveFirstProblem()
        {
            HashSet<string> flatChoices = new HashSet<string>();
            foreach (var choice in _rules[0].Flatify())
            {
                flatChoices.Add(choice);
            }

            Console.WriteLine($"BTW, {flatChoices.Count} choices generated");

            return _proposals.Count(flatChoices.Contains).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            _rules[8] = new EightRule();
            _rules[11] = new ElevenRule();
            HashSet<string> flatChoices = new HashSet<string>();
            foreach (var choice in _rules[0].Flatify())
            {
                flatChoices.Add(choice);
            }
            Console.WriteLine($"BTW, {flatChoices.Count} choices generated");

            return _proposals.Count(flatChoices.Contains).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
