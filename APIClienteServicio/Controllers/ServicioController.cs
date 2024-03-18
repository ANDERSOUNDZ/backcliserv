using APIClienteServicio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace APIClienteServicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
 
        private readonly DbcliservContext _dbcliservContext;

        public ServicioController(DbcliservContext database)
        {
            _dbcliservContext = database;
        }

        [HttpGet]
        [Route("listaServicio")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var listaServicio = await _dbcliservContext.Servicios.ToListAsync();

                if (listaServicio == null || !listaServicio.Any())
                {
                    return NotFound("No hay lista de servicios que mostrar");
                }
                return Ok(listaServicio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de servicios: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("obtenerServicio/{id:int}")]
        public async Task<IActionResult> ObtenerServicioPorId(int id)
        {
            try
            {
                var servicio = await _dbcliservContext.Servicios.FindAsync(id);

                if (servicio == null)
                {
                    return NotFound("No se encontró el servicio con el ID especificado");
                }

                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el servicio: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("agregarServicio")]
        public async Task<IActionResult> AgregarServicio([FromBody] Servicio servicio)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _dbcliservContext.Servicios.AddAsync(servicio);
                await _dbcliservContext.SaveChangesAsync();
                return Ok(servicio);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el servicio: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("actualizarServicio/{id}")]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] Servicio servicio)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var servicioActual = await _dbcliservContext.Servicios.FindAsync(id);
                if (servicioActual == null)
                {
                    return NotFound("No se encontró el servicio");
                }

                if (servicio.NombreServicio != null)
                {
                    servicioActual.NombreServicio = servicio.NombreServicio;
                }

                if (servicio.Descripcion != null)
                {
                    servicioActual.Descripcion = servicio.Descripcion;
                }

                if (servicio.Estado != null)
                {
                    servicioActual.Estado = servicio.Estado;
                }

                await _dbcliservContext.SaveChangesAsync();
                return Ok(servicioActual);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el servicio: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var servicioEliminar = await _dbcliservContext.Servicios.FindAsync(id);
            if (servicioEliminar == null)
            {
                return BadRequest("No existe el servicio.");
            }

            _dbcliservContext.Servicios.Remove(servicioEliminar);
            await _dbcliservContext.SaveChangesAsync();
            return Ok();
        }

    }

}
