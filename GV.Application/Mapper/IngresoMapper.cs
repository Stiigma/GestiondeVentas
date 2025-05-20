using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Application.DTOs;
using GV.Domain.Models;
using GV.Domain.Repositorys;

namespace GV.Application.Mapper
{
    public static class IngresoMapper
    {
        public static async Task<IngresosDTO> ToDTO(Ingreso ingreso, IPersonaRepository repositoryP, IUsuarioRepository repositoryU)
        {
            var persona = await repositoryP.GetPersonaAsync(ingreso.IdPersona);
            var provedor = await repositoryU.GetUsuarioById(ingreso.IdUsuario);

            
            return new IngresosDTO
            {
                IdIngreso = ingreso.IdIngreso,
                NombreUsu = persona?.NombrePersona ?? "",
                IdPersona = persona?.IdPersona ?? 0,
                IdUsuario = provedor?.IdUsuario ?? 0,
                NombreProv = provedor?.NombreUsuario ?? "",
                TipoComprobante = ingreso.TipoComprobante,
                SerieComprobante = ingreso.SerieComprobante,
                NumeroComprabante = ingreso.NumeroComprabante,
                FechaHoraIngreso = ingreso.FechaHoraIngreso,
                Impuesto = ingreso.Impuesto,
                TotalIngreso = ingreso.TotalIngreso,
                Estado = ingreso.Estado
            };
        }
    }
}
