using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Solvers
{
    public class Passport
    {
        public void Append(string str)
        {
            foreach (var kvp in str.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var split = kvp.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                Append(split[0], split[1]);
            }
        }

        public void Append(string key, string value)
        {
            switch (key)
            {
                case "byr":
                    BirthYear = value;
                    break;
                case "iyr":
                    IssueYear = value;
                    break;
                case "eyr":
                    ExpirationYear = value;
                    break;
                case "hgt":
                    Height = value;
                    break;
                case "hcl":
                    HairColor = value;
                    break;
                case "ecl":
                    EyeColor = value;
                    break;
                case "pid":
                    PassportId = value;
                    break;
                case "cid":
                    CountryId = value;
                    break;
            }
        }

        public string CountryId { get; set; }

        public string PassportId { get; set; }

        private static readonly string[] ValidEyeColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        public string EyeColor { get; set; }

        public string HairColor { get; set; }

        public string Height { get; set; }

        public string ExpirationYear { get; set; }

        public string IssueYear { get; set; }

        public string BirthYear { get; set; }

        public bool IsValidForExercise1 =>
            !string.IsNullOrEmpty(PassportId) &&
            !string.IsNullOrEmpty(EyeColor) &&
            !string.IsNullOrEmpty(HairColor) &&
            !string.IsNullOrEmpty(Height) &&
            !string.IsNullOrEmpty(ExpirationYear) &&
            !string.IsNullOrEmpty(IssueYear) &&
            !string.IsNullOrEmpty(BirthYear);

        private static readonly HashSet<char> HexaChars = new HashSet<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public bool IsValidForExercise2
        {
            get
            {
                if (!IsValidForExercise1) return false;

                if (!ValidateInt(BirthYear, 4, 1920, 2002)) return false;
                if (!ValidateInt(IssueYear, 4, 2010, 2020)) return false;
                if (!ValidateInt(ExpirationYear, 4, 2020, 2030)) return false;

                if (Height.EndsWith("in"))
                {
                    if (!int.TryParse(Height.Substring(0, Height.Length - 2), out var h))
                        return false;
                    if (h < 59 || h > 76) return false;
                }
                else if (Height.EndsWith("cm"))
                {
                    if (!int.TryParse(Height.Substring(0, Height.Length - 2), out var h))
                        return false;
                    if (h < 150 || h > 193) return false;
                }
                else return false;

                if (HairColor.Length != 7) return false;
                if (HairColor[0] != '#') return false;
                for (var i = 1; i < 7; i++)
                    if (!HexaChars.Contains(HairColor[i]))
                        return false;

                if (!ValidEyeColors.Contains(EyeColor))
                    return false;

                if (PassportId.Length != 9) return false;
                for (var i = 0; i < 9; i++)
                    if (!char.IsDigit(PassportId[i]))
                        return false;

                return true;
            }
        }

        private bool ValidateInt(string toValidate, int digits, int min, int max)
        {
            if (toValidate.Length != digits) return false;
            if (!int.TryParse(toValidate, out var by))
                return false;
            if (@by < min || @by > max) return false;
            return true;
        }
    }
    public class SolverDay4 : ISolver
    {
        private readonly List<Passport> _passports = new List<Passport>();
        public void InitInput(string content)
        {
            var splitContent = content.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            var currentPassport = new Passport();
            foreach (var currentLine in splitContent)
            {
                if (string.IsNullOrEmpty(currentLine))
                {
                    _passports.Add(currentPassport);
                    currentPassport = new Passport();
                }
                else
                {
                    currentPassport.Append(currentLine);
                }
            }
        }

        public string SolveFirstProblem()
        {
            return _passports.Count(p => p.IsValidForExercise1).ToString();
        }

        public string SolveSecondProblem(string firstProblemSolution)
        {
            return _passports.Count(p => p.IsValidForExercise2).ToString();
        }

        public bool Question2CodeIsDone { get; } = true;
    }
}
