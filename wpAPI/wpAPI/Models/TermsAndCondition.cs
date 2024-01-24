using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class TermsAndCondition
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public short Status { get; set; }

    public short IsDelete { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
