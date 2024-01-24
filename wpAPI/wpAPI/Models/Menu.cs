using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ParentMenuId { get; set; }

    public int Sort { get; set; }

    public int IsMobile { get; set; }

    public int MobileDefault { get; set; }

    public int IsBrowser { get; set; }

    public int BrowserDefault { get; set; }

    public int IsTransaction { get; set; }

    public short Status { get; set; }

    public int IsDelete { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
