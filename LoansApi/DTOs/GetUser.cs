using System.Text.Json.Serialization;

namespace LoansApi.DTOs
{
    public class GetUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public double Salary { get; set; }

        [JsonIgnore]
        public string UserRole { get; set; }
        [JsonIgnore]
        public bool IsBlocked { get; set; }
    }
}
