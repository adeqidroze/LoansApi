using FluentValidation;
using LoansApi.DTOs;
using static Database.Domain.User;

namespace LoansApi.Validators
{
    public class RoleOrBlockValidator : AbstractValidator<BlockOrRoleChangeUser>
    {

        public RoleOrBlockValidator()
        {
            RuleFor(x => x.UserRole)
                .NotNull()
                .WithMessage("Role can't be null.")
                .IsInEnum()
                .WithMessage("Role doesn't exist.");


            RuleFor(x => x.IsBlocked)
                  .NotNull()
                  .WithMessage("IsBlocked can't be null.")
                  .NotEqual(true || false)
                  .WithMessage("IsBlocked should be true or false.");
                  
        }
    }
}
