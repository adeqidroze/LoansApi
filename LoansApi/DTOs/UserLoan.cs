using System.Text.Json.Serialization;
using static Database.Domain.Loan;

namespace LoansApi.DTOs
{
    public class UserLoan
    {
        [JsonIgnore]
        public string LoanIdentityNumber { get; set; } //easy way to get loan withowt using id
        public double Amount { get; set; }

        public string Currency { get; set; }

        public int LoanPeriod { get; set; }//loan length in months

        [JsonIgnore]
        public string Status { get; set; }

    }
}
