using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.DTOs.Vehicle
{
    public class VehicleDto
    {
        public string VehiclePlateNumber { get; set; } = "";
        public int DriverId { get; set; } = -1;
    }
}
