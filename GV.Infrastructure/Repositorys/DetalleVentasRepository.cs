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
    public class DetalleVentasRepository : IDetalleVentas
    {
        private readonly ContextDB _context;
        public DetalleVentasRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<bool> CrearDetalleVenta(DetalleVenta detalleVenta)
        {
            _context.DetalleVentas.Add(detalleVenta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DetalleVenta>> ObtenerVentaConDetalle(int IdVenta)
        {
            var detalles = await _context.DetalleVentas
                       .Where(d => d.IdVenta == IdVenta)
                       .ToListAsync();

            if (detalles.Count == 0)
                return new List<DetalleVenta>();

            return detalles;
        }


        public async Task<bool> EliminarPorId(int IdVenta)
        {

            try
            {
                // Supongamos que tu contexto es _context y la entidad es Ingresos
                var ventas = _context.DetalleVentas.Where(x => x.IdVenta == IdVenta);

                if (!ventas.Any())
                    return false;

                _context.DetalleVentas.RemoveRange(ventas);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error si lo necesitas
                return false;
            }

        }
    }
}
