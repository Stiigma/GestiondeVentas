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
    public class PersonaRepository : IPersonaRepository
    {
        private readonly ContextDB _context;

        public PersonaRepository(ContextDB context)
        {
            _context = context;
        }


        public async Task<Persona?> GetPersonaAsync(int IdPersona)
        {
            //  _context.Per
            return await _context.Personas.FirstOrDefaultAsync(p => p.IdPersona == IdPersona);
        }

        public async Task<List<Persona>> GetPersonas()
        {
            var result = await _context.Personas.ToListAsync();

            if (result.Count == 0)
                return new List<Persona>();

            return result;
        }
    }
}
