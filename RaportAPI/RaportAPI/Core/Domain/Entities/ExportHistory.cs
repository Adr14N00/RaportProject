using System;
using System.Collections.Generic;

namespace RaportAPI.Core.Domain.Entities;

public partial class ExportHistory
{
    public int Id { get; set; }

    public string Exportname { get; set; } = null!;

    public DateTime Exportdatetime { get; set; }

    public string Username { get; set; } = null!;

    public string LocationName { get; set; } = null!;
}
