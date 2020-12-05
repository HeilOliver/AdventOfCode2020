using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AdventSolver.Util;

namespace AdventSolver.Solver.DayFour
{
    public class Passport
    {
        private readonly Dictionary<string, string> entries = new Dictionary<string, string>();

        public Passport(string serializedPassport)
        {
            var kvValues = serializedPassport.Split(" ");
            kvValues
                .Where(val => !string.IsNullOrEmpty(val))
                .Select(e => e.Split(":"))
                .ToList()
                .ForEach(e => entries.TryAdd(e[0], e[1]));
        }

        public bool IsConsistent()
        {
            return entries.ContainsKeys(new[]
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid"
            });
        }

        public bool IsValid()
        {
            if (!IsConsistent())
                return false;

            var validValues = new bool[]
            {
                CheckBirth(),
                CheckIssueYear(),
                CheckExpiration(),
                CheckHeight(),
                CheckHairColor(),
                CheckEyeColor(),
                CheckPassportId()
            };
            return validValues.All(e => e);
        }

        private bool CheckBirth()
        {
            string s = entries["byr"];
            int.TryParse(s, out int val);
            return val >= 1920 && val <= 2002;
        }

        private bool CheckIssueYear()
        {
            string s = entries["iyr"];
            int.TryParse(s, out int val);
            return val >= 2010 && val <= 2020;
        }

        private bool CheckExpiration()
        {
            string s = entries["eyr"];
            int.TryParse(s, out int val);
            return val >= 2020 && val <= 2030;
        }

        private bool CheckHeight()
        {
            string s = entries["hgt"];
            if (s.Contains("cm"))
            {
                int.TryParse(s.Replace("cm", ""), out int val);
                return val >= 150 && val <= 193;
            }

            int.TryParse(s.Replace("in", ""), out int valin);
            return valin >= 59 && valin <= 76;
        }

        private bool CheckHairColor()
        {
            string s = entries["hcl"];
            var regex = new Regex("#[a-f0-9]{6}");
            return regex.IsMatch(s);
        }

        private bool CheckEyeColor()
        {
            string s = entries["ecl"];
            return s switch
            {
                "amb" => true,
                "blu" => true,
                "brn" => true,
                "gry" => true,
                "grn" => true,
                "hzl" => true,
                "oth" => true,
                _ => false
            };
        }

        private bool CheckPassportId()
        {
            string s = entries["pid"];
            return s.Length == 9 && int.TryParse(s, out int _);
        }
    }
}