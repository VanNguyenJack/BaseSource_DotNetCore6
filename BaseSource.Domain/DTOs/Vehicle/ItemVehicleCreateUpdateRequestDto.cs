using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSource.Domain.DTOs.Vehicle
{
    public class ItemVehicleCreateUpdateRequestDto : BaseDto<ItemVehicleCreateUpdateRequestDto, Domain.Catalog.Vehicle>
    {
        public int Model { get; set; }

        public int NumberOfSeat { get; set; }

        public int? YearOfManufature { get; set; }

        public string VehiclePlateNumber { get; set; } = null!;

        public string? Insurance { get; set; }

        public int Provider { get; set; }

        public double TotalKmtravelled { get; set; }

        public double LimitKmtravelled { get; set; }

        public int DriverId { get; set; }

        public int LimitNumberOfSeat { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public int CreateBy { get; set; }

        public int UpdateBy { get; set; }

        public int RouteId { get; set; }

        public bool IsThaiLand { get; set; }

        public int? CarType { get; set; }

        public bool IsCanRunInsideLsp { get; set; }

        public bool IsCarWithMuffler { get; set; }

        public bool IsCarHavePickup { get; set; }
    }
}
