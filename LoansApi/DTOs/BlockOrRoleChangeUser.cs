using static Database.Domain.User;

namespace LoansApi.DTOs
{
    public class BlockOrRoleChangeUser
    {
        public string UserRole { get; set; }

        public bool IsBlocked { get; set; }
    }
}
