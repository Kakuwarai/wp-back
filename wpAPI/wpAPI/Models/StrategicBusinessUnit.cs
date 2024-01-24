using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class StrategicBusinessUnit
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Alias { get; set; }

    public string? Name { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public short Status { get; set; }

    public short IsDelete { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
