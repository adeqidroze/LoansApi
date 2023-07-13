using Database.Domain;
using FluentValidation;
using LoansApi.DTOs;

namespace LoansApi.Validators
{
    public class UserBlockedAndRolesValidator : AbstractValidator<BlockOrRoleChangeUser>
    {
        public UserBlockedAndRolesValidator()
        {
            RuleFor(x => x.IsBlocked)
                .Must(x => x == false || x == true)
                .WithMessage("IsBlocked should be either true or false.");


            RuleFor(x => x.UserRole)
                  .NotNull()
                  .WithMessage("LastName can't be null.")
                  .Must(x => x == Role.Guest || x == Role.User || x == Role.Assistant || x == Role.Admin)
                  .WithMessage("Role shoould be in values:Guest,User,Assistant,Admin");
         
        }
    }
}
