using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RaportAPI.Core.Domain.Entities;

namespace RaportAPI.Core.Application.Common.Interfaces
{
    public interface IRaportsDbContext
    {
        DbSet<ExportHistory> ExportHistory { get; set; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
