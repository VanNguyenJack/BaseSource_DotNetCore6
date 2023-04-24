using BaseSource.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.Repositories
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<Account> GetAllVehicle();
    }
}
