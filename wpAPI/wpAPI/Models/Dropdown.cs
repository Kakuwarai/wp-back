using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Dropdown
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Value { get; set; }

    public string? Display { get; set; }

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public string? LookUp { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
