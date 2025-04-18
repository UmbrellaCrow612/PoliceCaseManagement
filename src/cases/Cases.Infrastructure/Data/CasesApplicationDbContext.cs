﻿using System.Reflection;
using Cases.Core.Models;
using Cases.Core.Models.Joins;
using Microsoft.EntityFrameworkCore;

namespace Cases.Infrastructure.Data
{
    public class CasesApplicationDbContext(DbContextOptions<CasesApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Case> Cases { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<CaseIncidentType> CaseIncidentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
