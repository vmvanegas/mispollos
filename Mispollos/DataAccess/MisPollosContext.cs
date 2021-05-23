﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mispollos.Configuration;
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
        private AppSettings _appSettings;

        public MisPollosContext()
        {
        }

        public MisPollosContext(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoProducto> PedidoProducto { get; set; }

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

            modelBuilder.Entity<PedidoProducto>().HasKey(x => new { x.IdPedido, x.IdProducto });
        }
    }
}