using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GV.Application.DTOs;
using GV.Application.Mapper;
using GV.Domain.Models;
using GV.Domain.Repositorys;

namespace GV.Application.Servicios
{
    public class ServicioIngresos
    {

        private readonly IIngresoRepository _ingresoRepository;
        private readonly IPersonaRepository _personaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly ICategoriaRespository _categoriaRespository;
        private readonly IDetalleIngreso _detalleIngreso;

        public ServicioIngresos(IIngresoRepository ingresoRepository, IPersonaRepository personaRepository, IUsuarioRepository usuarioRepository, IArticuloRepository articuloRepository, ICategoriaRespository categoriaRespository, IDetalleIngreso detalleIngreso)
        {
            _ingresoRepository = ingresoRepository;
            _personaRepository = personaRepository;
            _usuarioRepository = usuarioRepository;
            _articuloRepository = articuloRepository;
            _categoriaRespository = categoriaRespository;
            _detalleIngreso = detalleIngreso;
        }


        public async Task<List<IngresosDTO>>  ListaIngresos()
        {
            var lista = await _ingresoRepository.GetAllIngresosAsync();

            if (lista == null)
                throw new Exception("No hay ingresos");

            var dtoList = new List<IngresosDTO>();

            foreach (var ingreso in lista)
            {
                var dto = await IngresoMapper.ToDTO(ingreso, _personaRepository, _usuarioRepository);
                dtoList.Add(dto);
            }

            return dtoList;

        }

        public async Task<List<DetalleIngreso>> ObtenerIngresoConDetalle(int IdIngreso)
        {
            return await _detalleIngreso.ObtenerIngresoConDetalle(IdIngreso);



        }


        //Deberia de ir en un servicio Usuario propio (por claridad)
        public async Task<List<ProveedorDTO>> ListaProvedores()
        {
            var provedores = await _usuarioRepository.GetUsuarios();

            if (provedores == null)
                throw new Exception("No hay proovedores");

            var dtoList = new List<ProveedorDTO>();

            foreach (var proovedor in provedores)
            {
                if (proovedor.IdRol != 2)
                    continue;
                var dto = new ProveedorDTO
                {
                    IdUsuario = proovedor.IdUsuario,
                    NombreUsuario = proovedor.NombreUsuario,

                };
                dtoList.Add(dto);
            }

            return dtoList;

        }


        public async Task<List<PersonaDTO>> ListaPersonas()
        {

            var personaslst = await _personaRepository.GetPersonas();

            var newList = new List<PersonaDTO>();

            foreach (var persona in personaslst)
            {
                if (!persona.Estado)
                    continue;
                var dto = new PersonaDTO
                {
                    IdPersona = persona.IdPersona,
                    NombrePersona = persona.NombrePersona,
                };

                newList.Add(dto);
            }

            return newList;

        }

        public async Task<bool> CambiarEstado(int id)
        {
            var result = await _ingresoRepository.CambiarEstado(id);
            if(!result)
                return false;

            return true;
        }

        public async Task<bool> editarIngreso(IngresoViewDTO ingresoView)
        {


            
            var newIngreso = new Ingreso
            {
                IdIngreso = ingresoView.IdIngreso,
                IdPersona = ingresoView.IdPersona,
                IdUsuario = ingresoView.IdUsuario,
                SerieComprobante = ingresoView.SerieComprobante,
                Estado = true,
                FechaHoraIngreso = DateTime.Now,
                Impuesto = ingresoView.Impuesto,
                NumeroComprabante = ingresoView.NumeroComprabante,
                TipoComprobante = ingresoView.TipoComprobante,
                TotalIngreso = ingresoView.TotalIngreso,
            };

            if(ingresoView.Articulos.Count == 0)
                newIngreso.TotalIngreso = 0;

            var editbool = await _ingresoRepository.EditarIngreso(newIngreso);
            var boolR = await _detalleIngreso.EliminarPorId(ingresoView.IdIngreso);

            foreach (var articulo in ingresoView.Articulos)
            {
                for (int i = 0; i < articulo.Cantidad; i++)
                {
                    var newDetalle = new DetalleIngreso
                    {
                        IdArticulo = articulo.IdArticulo,
                        IdIngreso = newIngreso.IdIngreso,
                        Cantidad = 1,
                        PrecioVenta = articulo.PrecioVenta,
                    };

                    var result = await _detalleIngreso.CrearDetalleIngreso(newDetalle);
                    if (!result)
                        await _detalleIngreso.CrearDetalleIngreso(newDetalle);



                }
            }
            return editbool;


        }


        //Deberia de ir en un servicio Usuarios propio (por claridad)
        public async Task<List<UsuarioDTO>> ListaUsuarios()
        {
            var usuarios = await _usuarioRepository.GetUsuarios();

            if (usuarios == null)
                throw new Exception("No hay proovedores");

            var dtoList = new List<UsuarioDTO>();

            foreach (var usuario in usuarios)
            {
                if (!usuario.Estado)
                    continue;
                var dto = new UsuarioDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreUsuario = usuario.NombreUsuario,

                };
                dtoList.Add(dto);
            }

            return dtoList;

        }

        //Deberia de ir en un servicio Articulos propio (por claridad)

        public async Task<List<ArticuloDTO>> ListaArticulos()
        {
            var articulos = await _articuloRepository.GetArticulos();

            var dtoList = new List<ArticuloDTO>();

            foreach (var articulo in articulos)
            {
                if (!articulo.Estado)
                    continue;
                var categoria = await _categoriaRespository.GetCategoriaById(articulo.IdCategoria);
                var dto = new ArticuloDTO
                {
                    IdArticulo = articulo.IdArticulo,
                    CodigoArticulo = articulo.CodigoArticulo,
                    NombreCategoria = categoria.NombreCategoria,
                    DescripcionArticulo = articulo.DescripcionArticulo,
                    NombreArticulo = articulo.NombreArticulo,
                    Estado = articulo.Estado,
                    PrecioVenta = articulo.PrecioVenta,
                    Stock = articulo.Stock,

                };
                dtoList.Add(dto);
            }

            return dtoList;
        }


        public async Task RegistrarIngresoAsync(IngresoViewDTO dto)
        {
            var newIngreso = new Ingreso
            {
                IdPersona = dto.IdPersona,
                IdUsuario = dto.IdUsuario,
                SerieComprobante = dto.SerieComprobante,
                Estado = true,
                FechaHoraIngreso = DateTime.Now,
                Impuesto = dto.Impuesto,
                NumeroComprabante = dto.NumeroComprabante,
                TipoComprobante = dto.TipoComprobante,
                TotalIngreso = dto.TotalIngreso,
            };

            var idIngreso = await _ingresoRepository.CrearIngreso(newIngreso);

            foreach(var articulo in dto.Articulos)
            {
                for(int i = 0; i < articulo.Cantidad; i++)
                {
                    var newDetalle = new DetalleIngreso
                    {
                        IdArticulo = articulo.IdArticulo,
                        IdIngreso = idIngreso,
                        Cantidad = 1,
                        PrecioVenta = articulo.PrecioVenta,
                    };

                    var result = await _detalleIngreso.CrearDetalleIngreso(newDetalle);
                    if(!result)
                        await _detalleIngreso.CrearDetalleIngreso(newDetalle);



                }
            }


        }
    }
}
