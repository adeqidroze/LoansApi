using AutoMapper;
using Database.Domain;
using DataBaseContext.Data;
using LoansApi.DTOs;

namespace LoansApi.BusinessLogics
{
    public interface ILoanTypeLogicInterface
    {
        string GetLoanType(double loanAmount);
        bool IsLoanPossible(double salary, UserLoan model, out string ErrorMessage);

    }
    public class LoanTypeLogic : ILoanTypeLogicInterface
    {

        public string GetLoanType(double loanAmount)
        {
            if (loanAmount <= 500)
                return MyLoanType.CreditCard;
            if (loanAmount > 500 && loanAmount <= 5000)
                return MyLoanType.FastLoan;
            if (loanAmount > 5000 && loanAmount <= 50000)
                return MyLoanType.MiniLoan;
            if (loanAmount > 50000 && loanAmount <= 200000)
                return MyLoanType.MicroLoan;
            return MyLoanType.BusinessLoan;
        }

        public bool IsLoanPossible(double salary, UserLoan model, out string ErrorMessage)
        {
            var loanTermLogics = new LoanTermBySallery();
            var loanType = GetLoanType(model.Amount);

            if (loanTermLogics.IsLoanTermAllowed(loanTermLogics.MinimalTerm(salary, model.Amount), model.LoanPeriod) == false)
            {
                ErrorMessage = $"Salary is not enough for this term.You need at least {loanTermLogics.MinimalTerm(salary, model.Amount)} months";
                return false;

            }
            if (loanType == MyLoanType.CreditCard && salary < LoanSalleryIntervals.CreditCardSalary)
            {
                ErrorMessage = $"Salary should be more than {LoanSalleryIntervals.CreditCardSalary} to qualify for {MyLoanType.CreditCard}.";
                return false;

            }
            if (loanType == MyLoanType.FastLoan && salary < LoanSalleryIntervals.FastLoanSalary)
            {
                ErrorMessage = $"Salary should be more than {LoanSalleryIntervals.FastLoanSalary} to qualify for {MyLoanType.FastLoan}.";
                return false;

            }
            if (loanType == MyLoanType.MiniLoan && salary < LoanSalleryIntervals.MiniLoanSalary)
            {
                ErrorMessage = $"Salary should be more than {LoanSalleryIntervals.MiniLoanSalary} to qualify for {MyLoanType.MiniLoan}.";
                return false;

            }
            if (loanType == MyLoanType.MicroLoan && salary < LoanSalleryIntervals.MicroLoanSalary)
            {
                ErrorMessage = $"Salary should be more than {LoanSalleryIntervals.MicroLoanSalary} to qualify for {MyLoanType.MicroLoan}.";
                return false;

            }
            if (loanType == MyLoanType.BusinessLoan && salary < LoanSalleryIntervals.BusinessLoanSalary)
            {
                ErrorMessage = $"Salary should be more than {LoanSalleryIntervals.BusinessLoanSalary} to qualify for {MyLoanType.BusinessLoan}.";
                return false;

            }
            ErrorMessage = null;
            return true;

        }
    }
       
    public class LoanSalleryIntervals
    {
        public const int CreditCardSalary = 700;
        public const int FastLoanSalary = 1000;
        public const int MiniLoanSalary = 3000;
        public const int MicroLoanSalary = 10000;
        public const int BusinessLoanSalary = 80000;
    }
}
