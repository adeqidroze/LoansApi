using AutoMapper;
using DataBaseContext.Data;
using LoansApi.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace LoansApi.Services
{
    public interface IAssistantInterface
    {
        Task<GetUser> BlockOrRoleChange(BlockOrRoleChangeUser model, int id);
        Task<AssistantUpdateLoanFor> UpdateUserLoans(AssistantUpdateLoanFor model);
    }
    public class AssistantService : IAssistantInterface
    {
        private readonly IMapper _mapper;
        private readonly LoanDatabaseContext _context;
        public AssistantService(LoanDatabaseContext dataContext, IMapper mapper)
        {
            _mapper = mapper;
            _context = dataContext;
        }

        public async Task<GetUser> BlockOrRoleChange(BlockOrRoleChangeUser model, int id)
        {
            var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
           
            _mapper.Map(model, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<GetUser>(user);

            return result;
        }

        public async Task<AssistantUpdateLoanFor>  UpdateUserLoans(AssistantUpdateLoanFor model)
        {
            var loan = _context.Loans.Where(x=>x.LoanIdentityNumber == model.LoanIdentityNumber);
            if(!loan.Any())
                return null;

            _mapper.Map(model, loan);
            _context.Loans.Update(loan.FirstOrDefault());
            await _context.SaveChangesAsync();
            var result = _mapper.Map<AssistantUpdateLoanFor>(loan);

            return result;
        }
    }
}
