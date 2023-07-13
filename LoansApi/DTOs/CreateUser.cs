using System.Text.Json.Serialization;
using static Database.Domain.User;

namespace LoansApi.DTOs
{
    public class CreateUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public double Salary { get; set; }

        public string Password { get; set; }

    }
}
