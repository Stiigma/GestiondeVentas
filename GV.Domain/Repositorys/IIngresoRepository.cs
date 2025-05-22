using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;

namespace GV.Domain.Repositorys
{
    public interface IIngresoRepository
    {

        Task<IEnumerable<Ingreso>> GetAllIngresosAsync();
        Task<int> CrearIngreso(Ingreso ingreso);

        Task<bool> EditarIngreso(Ingreso ingreso);
        Task<bool> CambiarEstado(int id);
    }
}
