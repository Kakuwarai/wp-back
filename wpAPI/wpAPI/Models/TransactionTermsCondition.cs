using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class TransactionTermsCondition
{
    public int Id { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? TermsAndConditionCode { get; set; }

    public string? TermsAndConditionDescription { get; set; }

    public string? Parameter1 { get; set; }

    public string? Parameter2 { get; set; }

    public short Status { get; set; }

    public short IsDelete { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
