using AutoMapper;
using Database.Domain;
using DataBaseContext.Data;
using LoansApi.DTOs;
using LoansApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        private Mock<IUserInterface> _userservice  = new Mock<IUserInterface>();
        private Mock<LoanDatabaseContext> _context = new Mock<LoanDatabaseContext>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IHashService>  _hashService = new Mock<IHashService>();
        
        [TestMethod]
        public async Task AddUserAsync_Success()
        {
             
           
            _hashService.Setup(x => x.GenerateSalt()).Returns("asbhabuybab1A");
            _hashService.Setup(x => x.GenerateHash(It.IsAny<string>(),It.IsAny<string>())).Returns("AZgabkkn^_njgdjdhajhvabvv%$#FFAEAFQDBSNAbvdyaiyv6b_");
            var dbset = new Mock<DbSet<User>>();
            var userList = new List<User>() { new User { Id = 1 } };

            var queryable = userList.AsQueryable();
            dbset.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbset.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbset.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbset.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            _context.Setup(x => x.Set<User>()).Returns(dbset.Object);

            var service = new UserService(_context.Object,_mapper.Object,_hashService.Object);
        

            var user = new CreateUser
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                UserName = "UnitTest",
                Age = 10,
                Salary = 20,              
            };
            var result = await service.AddUserAsync(user);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddUserAsync_Fail()
        {
            var service = new UserService(_context.Object, _mapper.Object, _hashService.Object);

            var user = new CreateUser
            {
                FirstName = "UnitTest",
                LastName = "UnitTest",
                UserName = "UnitTest",
                Age = 10,
                Salary = 20,
            };
            Task<CreateUser> task = service.AddUserAsync(user);
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