using BaseSource.Domain.Catalog;


namespace BaseSource.Domain.Repositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetUserByEmail(string email);
    }
}
