using System;
using System.Collections.Generic;

namespace LibraryApi.Models;

public partial class Bookcovers
{
    public long Id { get; set; }

    public int Coverfile { get; set; }

    public string BookKey { get; set; } = null!;

    public virtual Books BookKeyNavigation { get; set; } = null!;
}
