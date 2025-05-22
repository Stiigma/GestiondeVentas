using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Domain.Models;

namespace GV.Domain.Repositorys
{
    public interface IDetalleIngreso
    {
        Task<bool> CrearDetalleIngreso(DetalleIngreso detalleIngreso);

        Task<List<DetalleIngreso>> ObtenerIngresoConDetalle(int idIngreso);

        Task<bool> EliminarPorId(int IdIngreso);




    }
}
