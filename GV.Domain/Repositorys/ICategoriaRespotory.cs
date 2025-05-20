using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;

namespace GV.Domain.Repositorys
{
    public interface ICategoriaRespository
    {
        Task<Categoria?> GetCategoriaById(int IdCategoria);

    }
}
