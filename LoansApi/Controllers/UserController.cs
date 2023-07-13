using AutoMapper;
using Database.Domain;
using DataBaseContext.Data;
using LoansApi.BusinessLogics;
using LoansApi.DTOs;
using LoansApi.Helpers;
using LoansApi.Services;
using LoansApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Database.Domain.User;

namespace LoansApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly IUserInterface _userservice;
        private readonly ITokenInterface _tokenService;
        private readonly IHashService _hashService;
        private readonly LoanDatabaseContext _context;
        private readonly AppSettings  _settings;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly ILoanTypeLogicInterface _loanLogic;

        public UserController(IUserInterface userservice,
            ITokenInterface tokenservice,
            IHashService hashService,
            IOptions<AppSettings> appSettings,
            LoanDatabaseContext context, 
            IMapper mapper,
            ILogger<UserController> logger,
            ILoanTypeLogicInterface loanLogic)
        {
            _userservice = userservice;
            _tokenService = tokenservice;
            _hashService = hashService;
            _context = context;
            _settings = appSettings.Value;
            _mapper = mapper;
            _logger = logger;
            _loanLogic = loanLogic;
        }



        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUser newUser)
        {      
            var userValidator = new UserValidator();
            var validationResult = userValidator.Validate(newUser);      
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            var createdUser = await _userservice.AddUserAsync(newUser);
            if (createdUser == null)
                return BadRequest(new { message = $"Username {newUser.UserName} already exists. Try somthing like {newUser.UserName}123 " });

            var userCredentials = new UserLogin{Username = createdUser.UserName,Password = createdUser.Password};
            var userLogin = _userservice.Login(userCredentials);                                   
             
            return Ok(new
            {
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Age = createdUser.Age,
                Salary = createdUser.Salary,                       
                Tocken = _tokenService.GenerateToken(userLogin)
            });
            

        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUser updateUser)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var userValidator = new UpdateUserValidator();
            var usernameMatch = _context.Users.Where(x => x.UserName == updateUser.UserName).Any();
            var matchedUserId = _context.Users.Where(x => x.UserName == updateUser.UserName).FirstOrDefault().Id;

            if(_context.Users.Where(x=>x.Id == currentUserId).FirstOrDefault().IsBlocked == true)
                return BadRequest(new { message = "User Blocked!" });

            var validationResult = userValidator.Validate(updateUser);
            if (!validationResult.IsValid)
                return BadRequest(validationResult);
           
            var changedUser = await _userservice.UpdateUserAsync(updateUser, currentUserId);
            if (changedUser == null)
                return BadRequest(new { message = $"Username {updateUser.UserName} already exists. Try somthing like {updateUser.UserName}123 " });

            var userCredentials = new UserLogin { Username = changedUser.UserName, Password = changedUser.Password };
            var userLogin = _userservice.Login(userCredentials);

            if (userLogin == null)
                return BadRequest(new { message = "Access denied. Wrong Username or Password." });

            return Ok(new
            {
                FirstName = changedUser.FirstName,
                LastName = changedUser.LastName,
                Age = changedUser.Age,
                Salary = changedUser.Salary,
                Tocken = _tokenService.GenerateToken(userLogin)
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            var userLogin = _userservice.Login(user);
            if (userLogin == null)          
                return BadRequest(new { message = "Access denied. Wrong Username or Password." });

            if (_context.Users.Where(x => x.UserName == user.Username).FirstOrDefault().IsBlocked == true)
                return BadRequest(new { message = "User Blocked!" });

            var myUser = _context.Users.Where(x => x.UserName == userLogin.Username).FirstOrDefault();

            return Ok(new
            {
                FirstName = myUser.FirstName,
                LastName = myUser.LastName,
                Age = myUser.Age,
                Salary = myUser.Salary,
                Tocken = _tokenService.GenerateToken(userLogin)
            });
        }


        [HttpGet("GetUsers")]
        public ActionResult<GetUser> GetAll() 
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currUser = _context.Users.Where(x=> x.Id == currentUserId).FirstOrDefault();
            if (currUser.UserRole != Role.Admin || currUser.UserRole != Role.Assistant)
                return Forbid("User doesn't have permissions.");

            if (currUser.IsBlocked == true)
                return BadRequest(new { message = "User Blocked!" });

            return Ok(_userservice.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();
            if (currentUserId != id && (currUser.UserRole != Role.Admin || currUser.UserRole != Role.Assistant))
                return Forbid("User doesn't have permissions.");

            if (currUser.IsBlocked == true)
                return BadRequest(new { message = "User Blocked!" });

            var myUser = _userservice.GetById(id);
            if (myUser ==null)
                return NotFound(new { message = $"User with id {id} doesn't exist." });

            return Ok(myUser);

        }


        [HttpPost("loan/create")]
        public async Task<ActionResult> CreateLoan([FromBody] UserLoan newLoan)
        {
            var errorMessage = "";
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x=>x.Id == currentUserId).FirstOrDefault();
            if (currentUser.IsBlocked == true)
                return BadRequest(new {message = "User Blocked!"});

            var loanValidator = new LoansValidator();
            var validationResult = loanValidator.Validate(newLoan);
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            if (_loanLogic.IsLoanPossible(currentUser.Salary, newLoan,out errorMessage) == false)
                return BadRequest(errorMessage);
                      
            var createdLoan = await _userservice.CreateLoan(newLoan, _loanLogic.GetLoanType(newLoan.Amount), currentUserId);            
            return Ok(new
            {
                LoanIdentityNumber = createdLoan.LoanIdentityNumber,
                LoanType = _loanLogic.GetLoanType(newLoan.Amount),
                Amount = createdLoan.Amount,
                Currency = createdLoan.Currency,
                LoanPeriodInMonths = createdLoan.LoanPeriod,
                Status = createdLoan.Status
            });            
        }

        [HttpPut("loan/update")]
        public async Task<ActionResult> UpdateLoan([FromBody] UserLoan updateLoan, string loanIdentityNumber)
        {
            var errorMessage = "";
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();
            if (currentUser.IsBlocked == true)
                return BadRequest(new {message = "User Blocked!" });

            var loanValidator = new LoansValidator();
            var validationResult = loanValidator.Validate(updateLoan);
            if (!validationResult.IsValid)
                return BadRequest(validationResult);

            if (_loanLogic.IsLoanPossible(currentUser.Salary, updateLoan, out errorMessage) == false)
                return BadRequest(errorMessage);
           
            var createdLoan = await _userservice.UpdateLoan(updateLoan, _loanLogic.GetLoanType(updateLoan.Amount), loanIdentityNumber);              
            if (createdLoan == null)
                return BadRequest(new {message = $"Loan with number {loanIdentityNumber} can't be edited. Status is not PreApplication." });

            return Ok(new
            {
                LoanIdentityNumber = createdLoan.LoanIdentityNumber,
                Amount = createdLoan.Amount,
                Currency = createdLoan.Currency,
                LoanPeriodInMonths = createdLoan.LoanPeriod,
                Status = createdLoan.Status
             });
            

        }

        [HttpGet("GetLoans")]
        public IActionResult GetLoansByUserId()
        {
            var currentUserId = int.Parse(User.Identity.Name);
            var currentUser = _context.Users.Where(x => x.Id == currentUserId).FirstOrDefault();
            if (currentUser.IsBlocked == true)
                return BadRequest(new {message = "User Blocked!" });

            var myUser = _userservice.GetUserLoans(currentUserId);
            return Ok(myUser);
        }

    }
}
