using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;
using GV.Domain.Repositorys;
using GV.Infrastructure.Data;

namespace GV.Infrastructure.Repositorys
{
    public class CategoriaRepository : ICategoriaRespository
    {
        private readonly ContextDB _context;
        public CategoriaRepository(ContextDB context) { 
            _context = context;
        }

        public async Task<Categoria?> GetCategoriaById(int IdCategoria)
        {
            return _context.Categorias.FirstOrDefault(u => u.IdCategoria == IdCategoria);
        }
    }
}
