using System;
using System.Collections.Generic;

namespace BaseSource.Domain.Catalog;

public partial class Account
{
    public int Uid { get; set; }

    public string Email { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string? Phone { get; set; }

    public int? DriverId { get; set; }

    public int? ProviderId { get; set; }

    public string? CostCenter { get; set; }

    public int? OrganizationId { get; set; }

    public int? ApproveLevelId { get; set; }

    public bool IsActive { get; set; }
}
