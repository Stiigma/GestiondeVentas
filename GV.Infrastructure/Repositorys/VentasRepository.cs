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
    public class VentasRepository : IVentasRepository
    {
        private readonly ContextDB _context;

        public VentasRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> GetAllVentasAsync()
        {
            return await _context.Ventas.ToListAsync();
        }


        public async Task<int> CrearVenta(Venta venta)
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return venta.IdVenta;
        }

        public async Task<bool> EditarVenta(Venta venta)
        {
            var ventaExistente = await _context.Ventas.FindAsync(venta.IdVenta);

            if (ventaExistente == null)
            {
                return false; // No se encontró el ingreso con ese Id
            }

            // Opcional: actualizar solo propiedades específicas
            _context.Entry(ventaExistente).CurrentValues.SetValues(venta);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstado(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
                return false;

            venta.Estado = !venta.Estado;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
