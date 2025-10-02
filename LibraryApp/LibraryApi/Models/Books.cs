using System;
using System.Collections.Generic;

namespace LibraryApi.Models;

public partial class Books
{
    public string Key { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Subtitle { get; set; }

    public string? Firstpublishdate { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Bookcovers> Bookcovers { get; set; } = new List<Bookcovers>();

    public virtual ICollection<Booksubjects> Booksubjects { get; set; } = new List<Booksubjects>();

    public virtual ICollection<Issues> Issues { get; set; } = new List<Issues>();

    public virtual ICollection<Authors> Authorkey { get; set; } = new List<Authors>();
}
