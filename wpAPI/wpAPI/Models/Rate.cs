using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Rate
{
    public int Id { get; set; }

    public string? ChargeCode { get; set; }

    public string? ChargeName { get; set; }

    public string? ChargeCategory { get; set; }

    public decimal Amount { get; set; }

    public string? Currency { get; set; }

    public string? Uom { get; set; }

    public string? DefaultRemarks { get; set; }

    public short Status { get; set; }

    public short IsDelete { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
