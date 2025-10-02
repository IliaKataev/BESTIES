using System;
using System.Collections.Generic;

namespace LibraryApi.Models;

public partial class Issues
{
    public long Issueid { get; set; }

    public long Customerid { get; set; }

    public string Bookkey { get; set; } = null!;

    public DateOnly Dateofissue { get; set; }

    public DateOnly Returnuntil { get; set; }

    public DateOnly? Returndate { get; set; }

    public virtual Books BookkeyNavigation { get; set; } = null!;

    public virtual Customers Customer { get; set; } = null!;
}
