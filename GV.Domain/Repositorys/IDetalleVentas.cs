using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;

namespace GV.Domain.Repositorys
{
    public interface IDetalleVentas
    {
        Task<bool> CrearDetalleVenta(DetalleVenta detalleVenta);

        Task<List<DetalleVenta>> ObtenerVentaConDetalle(int IdVenta);

        Task<bool> EliminarPorId(int IdVenta);
    }
}
