using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RaportAPI.Core.Domain.Entities;
using RaportAPI.Core.Application.Common.Interfaces;

namespace RaportAPI.Infrastructure.Persistence;

public partial class RaportsDbContext : DbContext, IRaportsDbContext
{
    public RaportsDbContext(DbContextOptions<RaportsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ExportHistory> ExportHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
