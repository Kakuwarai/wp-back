using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Customer
{
    public long Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Position { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress1 { get; set; }

    public string? CompanyAddress2 { get; set; }

    public bool? Status { get; set; }

    public bool IsDelete { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedByUserId { get; set; }

    public string? Salutation { get; set; }
}
