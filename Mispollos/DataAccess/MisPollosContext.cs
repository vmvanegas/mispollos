using Microsoft.EntityFrameworkCore;
using Mispollos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mispollos.DataAccess
{
    public class MisPollosContext : DbContext
    {
        private const string _connectionString = @"Server=.;Database=mispollos;Trusted_Connection=True;";

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Rol> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region ModuleSeed

            modelBuilder.Entity<Rol>().HasData(
              new Rol
              {
                  Id = Guid.NewGuid(),
                  Nombre = "Admin"
              },
              new Rol
              {
                  Id = Guid.NewGuid(),
                  Nombre = "Usuario"
              }
            );

            #endregion ModuleSeed
        }
    }
}