using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay21 : ISolver
    {
        private List<string> _alergens = new List<string>();
        private HashSet<string> _food = new HashSet<string>();
        private Dictionary<string,HashSet<string>> _alergenMaybeIn = new Dictionary<string, HashSet<string>>();
        private Dictionary<string, string> _alergensAreIn = new Dictionary<string, string>();
        /// <summary>
        /// ingredients, alergens
        /// </summary>
        private List<(List<string>, List<string>)> _receipes = new List<(List<string>, List<string>)>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var currentLine in splitContent)
            {
                var s = currentLine.Split(new string[] { " (contains " }, StringSplitOptions.RemoveEmptyEntries);
                var food = s[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var alergens = s[1].TrimEnd(')').Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                _receipes.Add((new List<string>(food), new List<string>(alergens)));
                foreach (var ingredient in food)
                {
                    _food.Add(ingredient);
                }
                foreach (var alergen in alergens)
                {
                    _alergens.Add(alergen);
                }
            }
        }

        public string SolveFirstProblem()
        {
            CalculateAlergiesPossibilities();
            var safeIngredients = GetIngredientsThatWillNeverHaveAlergens();
            int count = 0;
            foreach (var ( ingredients, _) in _receipes)
            {
                foreach (var ingredient in ingredients)
                {
                    if (safeIngredients.Contains(ingredient))
                        count++;
                }
            }

            return count.ToString();
        }

        private List<string> GetIngredientsThatWillNeverHaveAlergens()
        {
            var alergizingIngredients = new HashSet<string>(_alergenMaybeIn.Values.SelectMany(i => i));
            return _food.Where(i => !alergizingIngredients.Contains(i)).ToList();
        }

        private void CalculateAlergiesPossibilities()
        {
            var hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                foreach (var (ingredients, alergens) in _receipes)
                {
                    foreach (var alergen in alergens)
                    {
                        if (!_alergenMaybeIn.TryGetValue(alergen, out var foodThatMayHaveAlergen))
                        {
                            _alergenMaybeIn[alergen] = new HashSet<string>(ingredients);
                            hasChanges = true;
                        }
                        else
                        {
                            foreach (var ingredient in _alergenMaybeIn[alergen].ToList())
                            {
                                if (!ingredients.Contains(ingredient))
                                {
                                    foodThatMayHaveAlergen.Remove(ingredient);
                                    hasChanges = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            while (_alergenMaybeIn.Count != _alergensAreIn.Count)
            {
                foreach (var (alergen, maybeIngredients) in _alergenMaybeIn)
                {
                    maybeIngredients.RemoveWhere(_alergensAreIn.ContainsValue);

                    if (maybeIngredients.Count == 1)
                        _alergensAreIn[alergen] = maybeIngredients.First();
                }
            }

            return string.Join(',', _alergensAreIn.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value));
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
