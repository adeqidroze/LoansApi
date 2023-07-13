using FluentValidation;
using LoansApi.DTOs;

namespace LoansApi.Validators
{
    public class LoansValidator : AbstractValidator<UserLoan>
    {
        public LoansValidator()
        {
         

            RuleFor(x => x.Amount)
                 .NotNull()
                 .WithMessage("Amount can't be null.")
                 .GreaterThanOrEqualTo(100)
                 .WithMessage("Minimal Loan Amount is 100.")
                 .LessThanOrEqualTo(10000000)
                 .WithMessage("Maximum Loan Amount is 10 000 000.");

            RuleFor(x => x.LoanPeriod)
                 .NotNull()
                 .WithMessage("Loan Period in Months can't be null.")
                 .GreaterThanOrEqualTo(3)
                 .WithMessage("Loan should be at least 3 months long.")
                 .LessThanOrEqualTo(240)
                 .WithMessage("Loan can't be more that 240 months(20 years) long.");


            RuleFor(x => x.Currency)
              .NotNull()
              .WithMessage("Currency can't be null.")
              .MinimumLength(2)
              .WithMessage("Currency should be at least 2 characters.")
              .MaximumLength(51)
              .WithMessage("Currency should not be more than 51 characters.");
        }

    }
}
