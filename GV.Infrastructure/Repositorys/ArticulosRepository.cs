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
    public class ArticulosRepository : IArticuloRepository
    {
        private readonly ContextDB _context;
        public ArticulosRepository(ContextDB context) { 
            _context = context;
        }

        public async Task<List<Articulo>> GetArticulos()
        {
            return await _context.Articulos.ToListAsync();
        }

    }
}
