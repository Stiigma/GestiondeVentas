﻿using System;
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
    public class IngresoRepository : IIngresoRepository
    {
        private readonly ContextDB _context;

        public IngresoRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingreso>> GetAllIngresosAsync()
        {
            return await _context.Ingresos.ToListAsync();
        }


        public async Task<int> CrearIngreso(Ingreso ingreso)
        {
            _context.Ingresos.Add(ingreso);
            await _context.SaveChangesAsync();

            return ingreso.IdIngreso;
        }

        public async Task<bool> EditarIngreso(Ingreso ingreso)
        {
            var ingresoExistente = await _context.Ingresos.FindAsync(ingreso.IdIngreso);

            if (ingresoExistente == null)
            {
                return false; // No se encontró el ingreso con ese Id
            }

            // Opcional: actualizar solo propiedades específicas
            _context.Entry(ingresoExistente).CurrentValues.SetValues(ingreso);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CambiarEstado(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
                return false;

            ingreso.Estado = !ingreso.Estado;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
