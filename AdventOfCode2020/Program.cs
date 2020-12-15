using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventOfCode2020.Solvers;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var exerciceDone = false;
            var exerciceSolved = 0;

            while (!exerciceDone)
            {
                Console.WriteLine("What problem do you wanna solve ? (default: last)");
                string result = Console.ReadLine();
                Console.WriteLine();
                if (int.TryParse(result, out exerciceSolved))
                {
                    exerciceDone = TrySolveExercice(exerciceSolved);
                }

                if (!exerciceDone)
                {
                    foreach (var exerciseNumber in Directory.GetFiles("./Inputs/").Select(GetNum)
                        .OrderByDescending(x => x))
                    {
                        exerciceDone = TrySolveExercice(exerciseNumber);
                        if (exerciceDone)
                        {
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static int GetNum(string path)
        {
            var txtNum = path.Split(new[] { "/Inputs/Input" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(".txt", "");
            return int.Parse(txtNum);
        }

        private static bool TrySolveExercice(int num)
        {
            var path = $"./Inputs/Input{num}.txt";
            if (File.Exists(path))
            {
                var input = File.ReadAllText(path);
                ISolver solver = (ISolver)Assembly.GetAssembly(typeof(ISolver))?.GetType($"AdventOfCode2020.Solvers.SolverDay{num}")
                    ?.GetConstructor(new Type[0])?.Invoke(new object[0]);
                if (solver != null)
                {
                    var sw = Stopwatch.StartNew();
                    solver.InitInput(input);
                    var ex1 = solver.SolveFirstProblem();
                    Console.WriteLine($"Solving exercice of day {num}");
                    Console.WriteLine("Answer to exercice 1 is : " + ex1 + " ... (answer found in " + sw.ElapsedMilliseconds + " ms).");
                    sw = Stopwatch.StartNew();
                    if (solver.Question2CodeIsDone)
                        Console.WriteLine("Answer to exercice 2 is : " + solver.SolveSecondProblem(ex1) + " ... (answer found in " + sw.ElapsedMilliseconds + " ms).");
                    Console.WriteLine();
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
