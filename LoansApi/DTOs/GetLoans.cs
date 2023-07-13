using Database.Domain;
using static Database.Domain.Loan;

namespace LoansApi.DTOs
{
    public class GetLoans
    {
        public string LoanIdentityNumber { get; set; } //easy way to get loan withowt using id

        public double Amount { get; set; }

        public string LoanType { get; set; }

        public string Currency { get; set; }

        public int LoanPeriod { get; set; }//loan length in months

        public string Status { get; set; } 
      
    }
}
