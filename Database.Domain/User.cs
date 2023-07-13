using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;


namespace Database.Domain
{
    public class User
    {
        [ForeignKey("Loan")]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public double Salary { get; set; }

        public bool IsBlocked { get; set; } = false;

        public string Password { get; set; }

        public string Password_salt { get; set; }

        public string UserRole { get; set; } = Role.Guest;

        public List<Loan> Loans { get; set; } = new();

    }

    public static class Role
    {
        public const string Admin = "Admin";
        public const string Assistant = "Assistant";
        public const string User = "User";
        public const string Guest = "Guest";

        public static List<string> List()
        {
            return typeof(Role)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
        }
    }
}
