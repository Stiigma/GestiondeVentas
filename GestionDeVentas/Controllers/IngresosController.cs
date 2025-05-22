using Microsoft.AspNetCore.Mvc;
using GV.Application.DTOs;
using GV.Application.Servicios;
using GV.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeVentas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngresosController : Controller
    {
        private readonly ServicioIngresos _ingresoService;

        public IngresosController(ServicioIngresos ingresoService)
        {
            _ingresoService = ingresoService;
        }

        // Muestra la vista Razor al acceder a /Ingresos
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("createv2")]
        public IActionResult Createv2()
        {
            return View();
        }

        [HttpGet("editar/{id}")]

        public IActionResult Editar(int id)
        {
            ViewBag.ModoEdicion = true;
            ViewBag.IdEditar = id;
            return View("createv2");
        }

        // ✅ Este método devuelve datos JSON (como API interna opcional)
        [HttpGet("lista")]
        public async Task<ActionResult<IEnumerable<IngresosDTO>>> GetAll()
        {
            try
            {
                var ingresos = await _ingresoService.ListaIngresos();
                return Ok(ingresos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(IngresoViewDTO ingreso)
        {
            if (!ModelState.IsValid)
            {
                return View(ingreso);
            }

            try
            {
                await _ingresoService.RegistrarIngresoAsync(ingreso); // tu lógica de guardado
                return RedirectToAction("Index"); // o donde muestres la lista
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar ingreso: " + ex.Message);
                return View(ingreso);
            }
        }


        [HttpPost("editar")]
        public async Task<IActionResult> editar(IngresoViewDTO ingreso)
        {
            if (!ModelState.IsValid)
            {
                return View(ingreso);
            }

            try
            {
                await _ingresoService.editarIngreso(ingreso); 
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar ingreso: " + ex.Message);
                return View(ingreso);
            }
        }

        [HttpPost("CambiarEstado/{id}")]
        public async Task<IActionResult> CambiarEstado(int id)
        {

            try
            {
                await _ingresoService.CambiarEstado(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar ingreso: " + ex.Message);
                return View(id);
            }
        }


        [HttpGet("obtener-dI/{id}")]
        public async Task<ActionResult<List<DetalleIngreso>>> ObtenerPorId(int id)
        {
            var ingreso = await _ingresoService.ObtenerIngresoConDetalle(id);

            if (ingreso == null)
                return NotFound();

            return Ok(ingreso);
        }


        [HttpGet("lista-articulos")]
        public async Task<ActionResult<List<ArticuloDTO>>> listaArticulos()
        {
            try
            {
                var articulos = await _ingresoService.ListaArticulos();
                return Ok(articulos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet("lista-provedores")]
        public async Task<ActionResult<List<ProveedorDTO>>> listaProvedores()
        {
            try
            {
                List<ProveedorDTO> provedores = await _ingresoService.ListaProvedores();
                return Ok(provedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

        [HttpGet("lista-personas")]
        public async Task<ActionResult<List<PersonaDTO>>> listaPersonas()
        {
            try
            {
                List<PersonaDTO> personas = await _ingresoService.ListaPersonas();
                return Ok(personas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = ex.Message });
            }
        }

    }
}
