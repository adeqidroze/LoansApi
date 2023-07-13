using AutoMapper;
using Database.Domain;
using DataBaseContext.Data;
using LoansApi.BusinessLogics;
using LoansApi.DTOs;
using LoansApi.Services;
using LoansApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LoansApi.Controllers
{

    [Authorize]
    [Route("api/system/[controller]")]
    public class AssistantController : Controller
    {
        private readonly IMapper _mapper;
        private readonly LoanDatabaseContext _context;
        private readonly IAssistantInterface  _assistant;
        private readonly ILoanTypeLogicInterface _loanLogic;
        public AssistantController(LoanDatabaseContext dataContext, IMapper mapper, IAssistantInterface assistant, ILoanTypeLogicInterface logic)
        {
            _mapper = mapper;
            _context = dataContext;
            _assistant = assistant;
            _loanLogic = logic;
        }
        [HttpPut("update/user/{id}")]
        public async Task<ActionResult> UpdateUser([FromBody] BlockOrRoleChangeUser updateUser, int userId)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();

            if (currentUser.UserRole != Role.Assistant || currentUser.UserRole != Role.Admin)
                return Forbid();

            if (currentUser.Id == userId)
                return BadRequest(new { message = "User can't update itself!" });

            var userValidator = new UserBlockedAndRolesValidator();
            var validationResult = userValidator.Validate(updateUser);
            var usernameId = _context.Users.Where(x => x.Id == userId).Any();
            var matchedUser = _context.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (_context.Users.Where(x => x.Id == currentUserId).FirstOrDefault().IsBlocked == true)
                return BadRequest("User Blocked!");

            if (currentUser.UserRole != Role.Admin && matchedUser.UserRole == Role.Admin)
                return Forbid("Only Admin User can edit Admins.");
         
            if (!usernameId)
                return NotFound($"User with Id {userId} doesn't exist in Database.");

            if (validationResult.IsValid)
            {
                var changedUser = await _assistant.BlockOrRoleChange(updateUser, userId);      

                return Ok(new
                {
                    Id = userId,
                    FirstName = changedUser.FirstName,
                    LastName = changedUser.LastName,
                    Age = changedUser.Age,
                    Salary = changedUser.Salary,
                    UserRole  = changedUser.UserRole,
                    IsBlocked = changedUser.IsBlocked,
                });
            }

            return BadRequest(validationResult);
        }

        [HttpPut("update/loan")]
        public async Task<ActionResult> UpdateLoan([FromBody] AssistantUpdateLoanFor updateLoan)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();

            if (currentUser.UserRole != Role.Admin || currentUser.UserRole != Role.Assistant)
                return Forbid();

            if (currentUser.IsBlocked == true)
                return BadRequest("User Blocked!");

            var loanValidator = new AssistantLoansValidator();
            var validationResult = loanValidator.Validate(updateLoan);

            if (validationResult.IsValid)
            {
                var updatedLoan = await _assistant.UpdateUserLoans(updateLoan);

                if(updatedLoan == null) 
                    return NotFound($"Loan with idetityNumber {updateLoan.LoanIdentityNumber} doesn't exist in database.");

                return Ok(updatedLoan);
               
            }

            return BadRequest(validationResult);
        }

        [HttpDelete("Delete/user/{id}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();

            if (currentUser.UserRole != Role.Admin || currentUser.UserRole != Role.Assistant)
                return Forbid();

            if (currentUser.IsBlocked == true)
                return BadRequest("User Blocked!");

            if (currentUser.Id == userId)
                return BadRequest(new { message = "User can't delete itself!" });
       
            var deletedUser = await _assistant.DeleteUser(userId);
            if (deletedUser == null)
                return NotFound($"User with id {userId} not found in database.");

            return Ok(deletedUser);
        }


        [HttpDelete("Delete/Loan")]
        public async Task<ActionResult> DeleteLoan(string loanIdentificationNumber)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();

            if (currentUser.UserRole != Role.Admin || currentUser.UserRole != Role.Assistant)
                return Forbid();

            if (currentUser.IsBlocked == true)
                return BadRequest("User Blocked!");

            var deletedLoan = await _assistant.DeleteLoan(loanIdentificationNumber);
            if (deletedLoan == null)
                return NotFound($"Loan with identification Number {loanIdentificationNumber} not found in database.");

            return Ok(deletedLoan);
        }

    }
}
