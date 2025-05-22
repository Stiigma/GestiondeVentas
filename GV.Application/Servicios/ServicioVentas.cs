using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV.Application.DTOs;
using GV.Application.Mapper;
using GV.Domain.Models;
using GV.Domain.Repositorys;

namespace GV.Application.Servicios
{
    public class ServicioVentas
    {
        private readonly IVentasRepository _ventasRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IDetalleVentas _detalleVentas;


        public ServicioVentas(IVentasRepository ventasRepository, IUsuarioRepository usuarioRepository, IPersonaRepository personaRepository, IDetalleVentas detalleVentas)
        {
            _ventasRepository = ventasRepository;
            _usuarioRepository = usuarioRepository;
            _personaRepository = personaRepository;
            _detalleVentas = detalleVentas;
        }

        public async Task<List<VentaDTO>> ListaVentas()
        {
            var lista = await _ventasRepository.GetAllVentasAsync();

            if (lista == null)
                throw new Exception("No hay ingresos");

            var dtoList = new List<VentaDTO>();

            foreach (var venta in lista)
            {

               var usuario = await _usuarioRepository.GetUsuarioById(venta.IdUsuario);
               var persona = await _personaRepository.GetPersonaAsync(venta.IdPersona);

               var dto = new VentaDTO
               {
                   IdVenta = venta.IdVenta,
                   IdPersona = venta.IdPersona,
                   IdUsuario = venta.IdUsuario,
                   ImpuestoVenta = venta.ImpuestoVenta,
                   FechaHora = venta.FechaHora,
                   NombreUsu = usuario.NombreUsuario,
                   NombreProv = persona.NombrePersona,
                   NumeroComprobante = venta.NumeroComprobante,
                   SerieComprobante = venta.SerieComprobante,
                   TipoComprobante = venta.TipoComprobante,
                   TotalVenta = venta.TotalVenta,
                   Estado = venta.Estado,
               };

                dtoList.Add(dto);
            }

            return dtoList;

        }

        public async Task RegistrarIngresoAsync(VentaViewDTO dto)
        {
            var newVenta = new Venta
            {
                IdPersona = dto.IdPersona,
                IdUsuario = dto.IdUsuario,
                SerieComprobante = dto.SerieComprobante,
                Estado = true,
                FechaHora = DateTime.Now,
                ImpuestoVenta = dto.ImpuestoVenta,
                NumeroComprobante = dto.NumeroComprabante,
                TipoComprobante = dto.TipoComprobante,
                TotalVenta = dto.TotalVenta,
            };

            var idVenta = await _ventasRepository.CrearVenta(newVenta);

            foreach (var articulo in dto.Articulos)
            {
                for (int i = 0; i < articulo.Cantidad; i++)
                {
                    var newDetalle = new DetalleVenta
                    {
                        IdArticulo = articulo.IdArticulo,
                        IdVenta = idVenta,
                        Cantidad = 1,
                        PrecioVenta = articulo.PrecioVenta,
                        Descuento = articulo.Descuento,
                    };

                    var result = await _detalleVentas.CrearDetalleVenta(newDetalle);
                    if (!result)
                        await _detalleVentas.CrearDetalleVenta(newDetalle);



                }
            }


        }

        public async Task<bool> CambiarEstado(int id)
        {
            var result = await _ventasRepository.CambiarEstado(id);
            if (!result)
                return false;

            return true;
        }

        public async Task<bool> editarVenta(VentaViewDTO ventaView)
        {



            var newVenta = new Venta
            {
                IdVenta = ventaView.IdVenta,
                IdPersona = ventaView.IdPersona,
                IdUsuario = ventaView.IdUsuario,
                SerieComprobante = ventaView.SerieComprobante,
                Estado = true,
                FechaHora = DateTime.Now,
                ImpuestoVenta = ventaView.ImpuestoVenta,
                NumeroComprobante = ventaView.NumeroComprabante,
                TipoComprobante = ventaView.TipoComprobante,
                TotalVenta = ventaView.TotalVenta,
            };
            var editbool = await _ventasRepository.EditarVenta(newVenta);
            var boolR = await _detalleVentas.EliminarPorId(newVenta.IdVenta);

            foreach (var articulo in ventaView.Articulos)
            {
                for (int i = 0; i < articulo.Cantidad; i++)
                {
                    var newDetalle = new DetalleVenta
                    {
                        IdArticulo = articulo.IdArticulo,
                        IdVenta = newVenta.IdVenta,
                        Descuento = articulo.Descuento,
                        Cantidad = 1,
                        PrecioVenta = articulo.PrecioVenta,
                    };

                    var result = await _detalleVentas.CrearDetalleVenta(newDetalle);
                    if (!result)
                        await _detalleVentas.CrearDetalleVenta(newDetalle);



                }
            }
            return editbool;


        }

        public async Task<List<DetalleVenta>> ObtenerVentaConDetalle(int idVenta)
        {
            return await _detalleVentas.ObtenerVentaConDetalle(idVenta);
        }

        public async Task<List<ClienteDTO>> ListaClientes()
        {
            var provedores = await _usuarioRepository.GetUsuarios();

            if (provedores == null)
                throw new Exception("No hay proovedores");

            var dtoList = new List<ClienteDTO>();

            foreach (var proovedor in provedores)
            {
                if (proovedor.IdRol != 1)
                    continue;
                var dto = new ClienteDTO
                {
                    IdUsuario = proovedor.IdUsuario,
                    NombreUsuario = proovedor.NombreUsuario,

                };
                dtoList.Add(dto);
            }

            return dtoList;

        }
    }
}
