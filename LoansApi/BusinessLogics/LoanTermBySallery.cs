using System;

namespace LoansApi.BusinessLogics
{
    public class LoanTermBySallery
    {
        public int MinimalTerm(double salery, double amount)
        {
            return (int)Math.Ceiling(amount / (salery * 0.6));
        }

        public bool IsLoanTermAllowed(int minimalTerm, int loanTerm) 
        {
            if(minimalTerm > loanTerm)
                return false;

            return true;
        }
    }

    
}
