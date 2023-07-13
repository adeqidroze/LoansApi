using DataBaseContext.Data;
using LoansApi.DTOs;
using LoansApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansApi.Test.ServiceTests
{
    [TestClass]
    public  class UnitTestUserService
    {
        private IUserInterface _userservice;
        private LoanDatabaseContext _context;


        [TestInitialize]
        public void setup(IUserInterface userservice, LoanDatabaseContext context)
        {
            _userservice = userservice;
            _context = context;
        }



        [TestMethod]
        public async Task AddUserAsync_Success()
        {     
            var usernameMatch = _context.Users.OrderByDescending(x => x.UserName.Contains("UnitTest"));
            var newUsername = usernameMatch.FirstOrDefault().Id.ToString();

            if (!usernameMatch.Any())
                newUsername = "";

            var user = new CreateUser
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                UserName = "UnitTest" + newUsername,
                Age = 10,
                Salary = 20,              
            };
            var result = await _userservice.AddUserAsync(user);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddUserAsync_Fail()
        {
            var user = new CreateUser
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                UserName = "UnitTest",
                Age = 10,
                Salary = 20,
            };
            Task<CreateUser> task = _userservice.AddUserAsync(user);
            CreateUser result = task.Result;

            Assert.IsNotNull(result);

        }
    }
}
/*Task<CreateUser> AddUserAsync(CreateUser model);
        Task<UpdateUser> UpdateUserAsync(UpdateUser model, int id);
        UserLogin Login(UserLogin model);
        GetUser[] GetAll();
        GetUser GetById(int id);
        Task<UserLoan> CreateLoan(UserLoan model, string loanType, int id);
        Task<UserLoan> UpdateLoan(UserLoan model, string loanType, string loanIdentityNumber);
        GetLoans[] GetUserLoans(int id);*/