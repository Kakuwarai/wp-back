using System;
using System.Collections.Generic;

namespace wpAPI.Models;

public partial class Token
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string? AuthToken { get; set; }

    public DateTime? IssuedOn { get; set; }

    public DateTime? ExpiresOn { get; set; }

    public short? IsExpire { get; set; }
}
