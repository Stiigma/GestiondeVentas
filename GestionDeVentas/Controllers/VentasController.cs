using GV.Application.DTOs;
using GV.Application.Servicios;
using GV.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GestionDeVentas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VentasController : Controller
    {
        private readonly ServicioIngresos _ingresoService;
        private readonly ServicioVentas _servicioVentas;

        public VentasController(ServicioIngresos ingresoService, ServicioVentas servicioVentas)
        {
            _ingresoService = ingresoService;
            _servicioVentas = servicioVentas;
        }

        // Muestra la vista Razor al acceder a /Ventas
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("editar/{id}")]
        public IActionResult Editar(int id)

        {
            ViewBag.ModoEdicion = true;
            ViewBag.IdEditar = id;
            return View("create");
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(VentaViewDTO venta)
        {
            if (!ModelState.IsValid)
            {
                return View(venta);
            }

            try
            {
                await _servicioVentas.RegistrarIngresoAsync(venta); // tu lógica de guardado
                return RedirectToAction("Index"); // o donde muestres la lista
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar ingreso: " + ex.Message);
                return View(venta);
            }
        }

        [HttpPost("editar")]
        public async Task<IActionResult> editar(VentaViewDTO venta)
        {
            if (!ModelState.IsValid)
            {
                return View(venta);
            }

            try
            {
                await _servicioVentas.editarVenta(venta);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar ingreso: " + ex.Message);
                return View(venta);
            }
        }


        [HttpPost("CambiarEstado/{id}")]
        public async Task<IActionResult> CambiarEstado(int id)
        {

            try
            {
                await _servicioVentas.CambiarEstado(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar ingreso: " + ex.Message);
                return View(id);
            }
        }

        [HttpGet("obtener-dI/{id}")]
        public async Task<ActionResult<List<DetalleVenta>>> ObtenerPorId(int id)
        {
            var ventas = await _servicioVentas.ObtenerVentaConDetalle(id);

            if (ventas == null)
                return NotFound();

            return Ok(ventas);
        }

        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> GetAll()
        {
            try
            {
                var ventas = await _servicioVentas.ListaVentas();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet("lista-clientes")]
        public async Task<ActionResult<List<ClienteDTO>>> listaProvedores()
        {
            try
            {
                List<ClienteDTO> clientes = await _servicioVentas.ListaClientes();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }


    }
}
