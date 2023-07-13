using System.Text.Json.Serialization;

namespace LoansApi.DTOs
{
    public class AssistantUpdateLoanFor
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string LoanIdentityNumber { get; set; } //easy way to get loan withowt using id

        public double Amount { get; set; }

        public string LoanType { get; set; }

        public string Currency { get; set; }

        public int LoanPeriod { get; set; }//loan length in months

        public string Status { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
