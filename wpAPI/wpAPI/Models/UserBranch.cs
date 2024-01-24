using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class UserBranch
{
    public int Id { get; set; }

    public long? UserId { get; set; }

    public string? BranchCode { get; set; }

    public int IsDefault { get; set; }

    public int Status { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual User? ModifiedByUser { get; set; }

    public virtual User? User { get; set; }
}
