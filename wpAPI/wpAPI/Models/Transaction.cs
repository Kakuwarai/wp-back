using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Transaction
{
    public long Id { get; set; }

    public string? ReferenceNumber { get; set; }

    public string ReferenceDate { get; set; } = null!;

    public string? CustomerCode { get; set; }

    public string? CustomerName { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? StorageType { get; set; }

    public string? ServiceAddress { get; set; }

    public string? Commodity { get; set; }

    public short IsRevised { get; set; }

    public int RevisedCount { get; set; }

    public string? RevisedReference { get; set; }

    public string? Remarks { get; set; }

    public string? BranchCode { get; set; }

    public string? Sbucode { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? NotedByUserId { get; set; }
}
