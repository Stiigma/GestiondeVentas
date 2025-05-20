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
    }
}
