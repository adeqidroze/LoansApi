using AutoMapper;
using Database.Domain;
using LoansApi.DTOs;

namespace LoansApi.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User,
                CreateUser>().ReverseMap();
            CreateMap<User,
                UpdateUser>().ReverseMap();
            CreateMap<User,
                UserLogin>().ReverseMap();
            CreateMap<User,
                GetUser>().ReverseMap();
            CreateMap<User,
                BlockOrRoleChangeUser>().ReverseMap();
            CreateMap<GetUser,
                User>().ReverseMap();

            CreateMap<Loan,
                UserLoan>().ReverseMap();
            CreateMap<Loan,
             GetLoans>().ReverseMap();
            CreateMap<Loan,
             AssistantCreateLoan>().ReverseMap();
            CreateMap<Loan,
             AssistantUpdateLoanFor>().ReverseMap();
        }
    }
}
