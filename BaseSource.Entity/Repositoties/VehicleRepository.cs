using BaseSource.Domain.Catalog;
using BaseSource.Domain.Repositories;
using BaseSource.Entity.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Entity.Repositoties
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(BaseSourceDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account> GetAllVehicle()
        {
            return null;
        }
    }
}
