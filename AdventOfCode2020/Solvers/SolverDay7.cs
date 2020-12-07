using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    class SolverDay7 : ISolver
    {
        internal class Bag
        {
            public string Name { get; }

            public Dictionary<Bag, int> CanContain { get; } = new Dictionary<Bag, int>();
            public HashSet<Bag> ContainedIn { get;  } = new HashSet<Bag>();

            public Bag(string name)
            {
                Name = name;
            }

            public override bool Equals(object? obj)
            {
                if (!(obj is Bag othBag))
                    return false;
                return Equals(othBag);
            }

            protected bool Equals(Bag other)
            {
                return Name == other.Name;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Name);
            }

            public override string ToString()
            {
                return Name;
            }

            public int CountsInnerBags()
            {
                int count = 0;
                foreach (var kvp in CanContain)
                {
                    count += kvp.Value * (1 + kvp.Key.CountsInnerBags());
                }

                return count;
            }
        }

        private static Dictionary<string, Bag> Bags = new Dictionary<string, Bag>();

        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var currentLine in splitContent)
            {
                var containsSplit = currentLine.Split(new[] {" bags contain "}, StringSplitOptions.RemoveEmptyEntries);
                var container = GetOrCreateBag(containsSplit[0]);

                if (containsSplit[1] == "no other bags.")
                    continue;

                foreach (var innerBag in containsSplit[1].TrimEnd('.').Split(new []{", "}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var count = int.Parse(innerBag[0].ToString());
                    var name = innerBag.Substring(2, innerBag.Length - 6 - (count == 1 ? 0 : 1));
                    var contained = GetOrCreateBag(name);
                    container.CanContain[contained] = count;
                    contained.ContainedIn.Add(container);
                }
            }
        }

        private Bag GetOrCreateBag(string name)
        {
            if (!Bags.TryGetValue(name, out var container))
            {
                container = new Bag(name);
                Bags.Add(name, container);
            }

            return container;
        }

        public string SolveFirstProblem()
        {
            var shinyGold = Bags["shiny gold"];
            HashSet<Bag> canContainShinyGold = new HashSet<Bag>();
            Stack<Bag> toTreat = new Stack<Bag>();

            foreach (var bag in shinyGold.ContainedIn)
            {
                toTreat.Push(bag);
            }

            while (toTreat.Count != 0)
            {
                var bag = toTreat.Pop();
                if (!canContainShinyGold.Add(bag))
                    continue;

                foreach (var innerBag in bag.ContainedIn)
                {
                    toTreat.Push(innerBag);
                }
            }

            return canContainShinyGold.Count.ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            var shinyGold = Bags["shiny gold"];

            int result = shinyGold.CountsInnerBags();
            return result.ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
