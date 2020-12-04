namespace AdventOfCode2020.Solvers
{
    internal interface ISolver
    {
        public void InitInput(string content);

        public string SolveFirstProblem();


        public string SolveSecondProblem(string firstProblemSolution);
        bool Question2CodeIsDone { get; }
    }
}