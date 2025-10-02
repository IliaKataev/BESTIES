using System;
using System.Collections.Generic;

namespace LibraryApi.Models;

public partial class Authors
{
    public string Key { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Bio { get; set; }

    public string? Birthdate { get; set; }

    public string? Deathdate { get; set; }

    public string? Wikipedia { get; set; }

    public virtual ICollection<Books> Bookkey { get; set; } = new List<Books>();
}
