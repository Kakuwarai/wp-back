using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Branch
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? ShortName { get; set; }

    public string? Name { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? PostalCode { get; set; }

    public string? Area { get; set; }

    public string? Region { get; set; }

    public string? MapReference { get; set; }

    public string? Latitude { get; set; }

    public string? Longitude { get; set; }

    public int Status { get; set; }

    public int IsDelete { get; set; }

    public long? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public long? ModifiedByUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
