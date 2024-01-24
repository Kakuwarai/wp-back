using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Trans3
{
    public int Id { get; set; }

    public string YearMonth { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string Sbu { get; set; } = null!;

    public string? ReferenceInitial { get; set; }
}
