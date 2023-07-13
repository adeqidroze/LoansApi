using AutoMapper;
using DataBaseContext.Data;
using System.Linq;
using LoansApi.DTOs;
using Database.Domain;
using System.Threading.Tasks;
using static Database.Domain.Loan;
using Microsoft.EntityFrameworkCore;

namespace LoansApi.Services
{
    public interface IUserInterface
    {
        Task<CreateUser> AddUserAsync(CreateUser model);
        Task<UpdateUser> UpdateUserAsync(UpdateUser model, int id);
        UserLogin Login(UserLogin model);
        GetUser[] GetAll();
        GetUser GetById(int id);
        Task<UserLoan> CreateLoan(UserLoan model, string loanType, int id);
        Task<UserLoan> UpdateLoan(UserLoan model, string loanType, string loanIdentityNumber);
        GetLoans[] GetUserLoans(int id);
    }

    public class UserService : IUserInterface
    {
        private readonly IMapper _mapper;
        private readonly LoanDatabaseContext _context;
        private readonly IHashService _hashService;
        public UserService(LoanDatabaseContext dataContext,  IMapper mapper, IHashService hashService)
        {
            _mapper = mapper;
            _context = dataContext;
            _hashService = hashService;
        }


        public async Task<CreateUser> AddUserAsync(CreateUser model)
        {

            var userMatch = _context.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();

            if (userMatch != null)
                return null;

            var user = new User();
            user.Password_salt = _hashService.GenerateSalt();
            model.Password = _hashService.GenerateHash(model.Password, user.Password_salt);
            _mapper.Map(model, user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<CreateUser>(_context.Users.Where(x => x.UserName == model.UserName).FirstOrDefault());

            return _mapper.Map<CreateUser>(_context.Users.Where(x=>x.UserName == model.UserName).FirstOrDefault());
        }
        public async Task<UpdateUser> UpdateUserAsync(UpdateUser model, int id)
        {
            var userMatch = _context.Users.Where(x => x.UserName == model.UserName);
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
    
            if (userMatch.Any() && user.Id != userMatch.FirstOrDefault().Id)
                return null;

            user.Password_salt = _hashService.GenerateSalt();
            model.Password = _hashService.GenerateHash(model.Password, user.Password_salt);
            _mapper.Map(model, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<UpdateUser>(_context.Users.Where(x => x.UserName == model.UserName).FirstOrDefault());

            return result;
        }


        public UserLogin Login(UserLogin loginModel)
        {
            var maxPasswordLength = 16;
            var hashedPassword = "";
            var hasher = new HashPasswordService();


            if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
            {
                return null;
            }

            var userCreds = _context.Users.SingleOrDefault(x => x.UserName == loginModel.Username);
            if(loginModel.Password.Length > maxPasswordLength)
                hashedPassword = loginModel.Password;
            else
                hashedPassword = hasher.GenerateHash(loginModel.Password, userCreds.Password_salt);

            if (userCreds == null)
            {
                return null;
            }
            if (userCreds.Password != hashedPassword)
            {
                return null;
            }
            return _mapper.Map<UserLogin>(userCreds);
        }


        public GetUser GetById(int id)
        {
            var myUser = _context.Users.Where(x => x.Id == id);
            if (!myUser.Any())
                return null;

            return _mapper.Map<GetUser>(myUser.FirstOrDefault());
        }
        public GetUser[] GetAll()
        {
            var myUsers = _context.Users.ToList();           
            return _mapper.Map<GetUser[]>(myUsers); 
        }


        public async Task<UserLoan> CreateLoan(UserLoan model, string loanType, int id)
        {
            var loan = new Loan();
            var loanIdNum = _context.Loans.OrderByDescending(u => u.Id);
           
            _mapper.Map(model, loan);
   
            loan.LoanIdentityNumber = !loanIdNum.Any() ? $"NKL{1001}" : $"NKL{1001 + loanIdNum.FirstOrDefault().Id}";
            loan.Status = LoanStatus.PreApplication;
            loan.LoanType = loanType;
            loan.UserId = id;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UserLoan>(_context.Loans.Where(x => x.LoanIdentityNumber == loan.LoanIdentityNumber).FirstOrDefault());

            return result;
        }
        public async Task<UserLoan> UpdateLoan(UserLoan model, string loanType, string loanIdentityNumber)
        {
            var loan = _context.Loans.Where(x=>x.LoanIdentityNumber == loanIdentityNumber).FirstOrDefault();
            if(loan.Status != LoanStatus.PreApplication)
                return null;
            model.LoanIdentityNumber = loanIdentityNumber;
            model.Status = loan.Status;
            _mapper.Map(model, loan);
            loan.LoanType = loanType;            
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UserLoan>(_context.Loans.Where(x => x.LoanIdentityNumber == loan.LoanIdentityNumber).FirstOrDefault());

            return result;
        }


        public GetLoans[] GetUserLoans(int id)
        {
            var myLoan = _context.Loans.Where(x => x.User.Id == id).ToList();

            return _mapper.Map<GetLoans[]>(myLoan);
        }


    }

}
