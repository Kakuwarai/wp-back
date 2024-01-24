using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class UserMenu
{
    public int Id { get; set; }

    public int MenuId { get; set; }

    public long? UserId { get; set; }

    public int Add { get; set; }

    public int Edit { get; set; }

    public int Delete { get; set; }

    public string? BranchCode { get; set; }

    public int Status { get; set; }

    public int IsDelete { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual User? User { get; set; }
}
