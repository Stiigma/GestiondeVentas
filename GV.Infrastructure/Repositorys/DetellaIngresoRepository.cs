using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;
using GV.Domain.Repositorys;
using GV.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GV.Infrastructure.Repositorys
{
    public class DetalleIngresoRepository : IDetalleIngreso
    {
        private readonly ContextDB _context;
        public DetalleIngresoRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> CrearDetalleIngreso(DetalleIngreso detalleIngreso)
        {
            _context.DetalleIngreso.Add(detalleIngreso);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DetalleIngreso>> ObtenerIngresoConDetalle(int idIngreso)
        {
            var detalles = await _context.DetalleIngreso
                       .Where(d => d.IdIngreso == idIngreso)
                       .ToListAsync();

            if (detalles.Count == 0)
                return new List<DetalleIngreso>();

            return detalles;
        }
    }
}
