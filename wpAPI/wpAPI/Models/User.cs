using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class User
{
    public long Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Type { get; set; }

    public string? Lastname { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? Nickname { get; set; }

    public string? EmailAddress { get; set; }

    public string? ContactNumber { get; set; }

    public string? HashCode { get; set; }

    public int Status { get; set; }

    public int IsDeactivated { get; set; }

    public string? DeactivateReason { get; set; }

    public int IsRegistered { get; set; }

    public int IsVerified { get; set; }

    public int? VerifiedBy { get; set; }

    public DateTime? VerifiedDate { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? Fullname { get; set; }

    public string? Fullname2 { get; set; }

    public string? Sbu { get; set; }

    public string? BranchCode { get; set; }

    public string? PositionCode { get; set; }

    public virtual ICollection<UserBranch> UserBranchCreatedByUsers { get; set; } = new List<UserBranch>();

    public virtual ICollection<UserBranch> UserBranchModifiedByUsers { get; set; } = new List<UserBranch>();

    public virtual ICollection<UserBranch> UserBranchUsers { get; set; } = new List<UserBranch>();

    public virtual ICollection<UserMenu> UserMenus { get; set; } = new List<UserMenu>();
}
