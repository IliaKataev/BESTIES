using System;
using System.Collections.Generic;

namespace LibraryApi.Models;

public partial class Booksubjects
{
    public long Id { get; set; }

    public string Subject { get; set; } = null!;

    public string BookKey { get; set; } = null!;

    public virtual Books BookKeyNavigation { get; set; } = null!;
}
