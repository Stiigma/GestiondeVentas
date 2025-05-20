using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GV.Domain.Models;

namespace GV.Infrastructure.Data
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options)
           : base(options)
        {
        }


        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<DetalleIngreso> DetalleIngreso { get; set; }

        // 🔧 Agrega esta línea
        public DbSet<Persona> Personas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Articulo> Articulos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

    }
}
