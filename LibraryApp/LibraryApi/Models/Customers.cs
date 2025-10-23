using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LibraryApi.Models;

public partial class Customers
{
    public long Customerid { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Zip { get; set; }

    public string? City { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
    [JsonIgnore]

    public virtual ICollection<Issues> Issues { get; set; } = new List<Issues>();
}
