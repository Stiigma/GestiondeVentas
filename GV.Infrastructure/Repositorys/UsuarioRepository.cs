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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ContextDB _context;
        public UsuarioRepository(ContextDB context) { 
            _context = context;
        }

        public async Task<Usuario?> GetUsuarioById(int IdUsuario)
        {
            var result = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == IdUsuario);

            
            return result;
        }

        public async Task<List<Usuario>?> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}
