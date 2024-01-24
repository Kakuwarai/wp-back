using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class TransactionDetail
{
    public int Id { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? ChargeCode { get; set; }

    public string? Currency { get; set; }

    public string? Rates { get; set; }

    public string? Uom { get; set; }

    public string? Remarks { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
