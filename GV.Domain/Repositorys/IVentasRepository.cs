using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;

namespace GV.Domain.Repositorys
{
    public interface IVentasRepository
    {

        Task<IEnumerable<Venta>> GetAllVentasAsync();

        Task<int> CrearVenta(Venta venta);

        Task<bool> EditarVenta(Venta venta);
        Task<bool> CambiarEstado(int id);
    }
}
