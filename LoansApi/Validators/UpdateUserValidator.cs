using FluentValidation;
using LoansApi.DTOs;

namespace LoansApi.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage("FirstName can't be null.")
                .MinimumLength(2)
                .WithMessage("Firstname should be at least 2 characters.")
                .MaximumLength(51)
                .WithMessage("Firstname should not be more than 51 characters.");

            RuleFor(x => x.LastName)
                    .NotNull()
                    .WithMessage("LastName can't be null.")
                    .MinimumLength(2)
                    .WithMessage("Lastname should be at least 2 characters.")
                    .MaximumLength(51)
                    .WithMessage("Lastname should not be more than 51 characters.");

            RuleFor(x => x.UserName)
                    .NotNull()
                    .WithMessage("UserName can't be null.")
                    .MinimumLength(2)
                    .WithMessage("UserName should be at least 2 characters.")
                    .MaximumLength(51)
                    .WithMessage("UserName should not be more than 51 characters.");

            RuleFor(x => x.Age)
                    .NotNull()
                    .WithMessage("Age can't be null.")
                    .GreaterThanOrEqualTo(18)
                    .WithMessage("User can't be younger than 18.")
                    .LessThanOrEqualTo(75)
                    .WithMessage("Users older than 75 can't get a loan.");

            RuleFor(x => x.Salary)
                    .NotNull()
                    .WithMessage("Salary can't be null.")
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Salary can't be less than 0.")
                    .LessThanOrEqualTo(1000000)
                    .WithMessage("Salary can't be more than 1 000 000.");


            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Password can't be null.")
                .MinimumLength(8)
                .WithMessage("Password should be at least 8 characters.")
                .MaximumLength(18)
                .WithMessage("Password should not be more than 18 characters.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\@\#\$\%\^\&\.]+").WithMessage("Your password must contain at least one (!?@#$%^ *.).");
        }
        
    }
}
