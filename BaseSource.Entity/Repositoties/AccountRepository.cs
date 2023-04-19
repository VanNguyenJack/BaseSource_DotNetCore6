using BaseSource.Domain.Catalog;
using BaseSource.Domain.DTOs.Identity;
using BaseSource.Domain.Repositories;
using BaseSource.Entity.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BaseSource.Entity.Repositoties
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(BaseSourceDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account> GetUserByEmail(string email)
        {
            return await DbContext.Accounts.AsNoTracking().Where(x => x.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();

        }
    }
}
