using APIClienteServicio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static APIClienteServicio.Models.ClienteServicio;
using static APIClienteServicio.Models.Servicio;

namespace APIClienteServicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteServicioController : ControllerBase
    {
        
        private readonly DbcliservContext _dbcliservContext;

        public ClienteServicioController(DbcliservContext database)
        {
            _dbcliservContext = database;
        }

        [HttpGet("listacompletaserviciosclientes")]
        public async Task<IActionResult> ListaCompleta()
        {
            try
            {
                var lista = await _dbcliservContext.ClienteServicios
                    .Select(cs => new ClienteServicioCompleto
                    {
                        IdCliente = cs.IdCliente,
                        NombreCliente = cs.IdClienteNavigation.NombreCliente,
                        IdServicio = cs.IdServicio,
                        NombreServicio = cs.IdServicioNavigation.NombreServicio,
                        Estado = cs.Estado
                    })
                    .ToListAsync();

                if (lista.Count == 0)
                {
                    return NotFound("No se encontraron registros.");
                }

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista completa: {ex.Message}");
            }
        }

        [HttpGet("obtenerservicioscliente/{idCliente}")]
        public async Task<IActionResult> ObtenerServiciosCliente(int idCliente)
        {
            try
            {
                var cliente = await _dbcliservContext.Clientes.FindAsync(idCliente);

                if (cliente == null)
                {
                    return NotFound("No se encontró el cliente con el ID especificado.");
                }

                var tieneServicios = await _dbcliservContext.ClienteServicios.AnyAsync(x => x.IdCliente == idCliente);

                if (!tieneServicios)
                {
                    return NotFound("El cliente no tiene servicios asociados.");
                }

                var servicios = await _dbcliservContext.ClienteServicios
                .Where(cs => cs.IdCliente == idCliente)
                .Select(cs => new Servicio
                {
                    Id = cs.IdServicio,
                    NombreServicio = cs.IdServicioNavigation.NombreServicio,
                    Descripcion = cs.IdServicioNavigation.Descripcion,
                    Estado = cs.IdServicioNavigation.Estado
                })
                .ToListAsync();

                var response = new ClienteServicioResponse
                {
                    Servicios = servicios
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los servicios del cliente: {ex.Message}");
            }
        }
     
        [HttpPost("agregarserviciocliente")]
        public async Task<IActionResult> AgregarServicioCliente(AgregarServicioClienteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var cliente = await _dbcliservContext.Clientes.FindAsync(request.IdCliente);
                if (cliente == null)
                {
                    return NotFound("Cliente no encontrado.");
                }

                var servicio = await _dbcliservContext.Servicios.FindAsync(request.IdServicio);
                if (servicio == null)
                {
                    return NotFound("Servicio no encontrado.");
                }

                var existeRelacion = await _dbcliservContext.ClienteServicios
                    .AnyAsync(x => x.IdCliente == request.IdCliente && x.IdServicio == request.IdServicio);
                if (existeRelacion)
                {
                    return BadRequest("El cliente ya tiene asociado el servicio.");
                }

                var clienteServicio = new ClienteServicio
                {
                    IdCliente = request.IdCliente,
                    IdServicio = request.IdServicio,
                    Estado = request.Estado
                };

                _dbcliservContext.ClienteServicios.Add(clienteServicio);
                await _dbcliservContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el servicio al cliente: {ex.Message}");
            }
        }

        [HttpPut("actualizarestadoservicio/{idCliente}/{idServicio}")]
        public async Task<IActionResult> ActualizarEstadoServicio(int idCliente, int idServicio, bool estado)
        {
            try
            {
                if (idCliente <= 0)
                {
                    return BadRequest("El ID del cliente debe ser un número entero positivo.");
                }

                if (idServicio <= 0)
                {
                    return BadRequest("El ID del servicio debe ser un número entero positivo.");
                }

                var clienteServicio = await _dbcliservContext.ClienteServicios
                    .Where(cs => cs.IdCliente == idCliente && cs.IdServicio == idServicio)
                    .FirstOrDefaultAsync();

                if (clienteServicio == null)
                {
                    return NotFound("No se encontró la relación entre el cliente y el servicio.");
                }

                clienteServicio.Estado = estado;

                await _dbcliservContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado del servicio: {ex.Message}");
            }
        }

        [HttpDelete("EliminarServicio/{idCliente}/{idServicio}")]
        public async Task<IActionResult> EliminarServicio(int idCliente, int idServicio)
        {
            try
            {
                if (idCliente <= 0)
                {
                    return BadRequest("El ID del cliente debe ser un número entero positivo.");
                }

                if (idServicio <= 0)
                {
                    return BadRequest("El ID del servicio debe ser un número entero positivo.");
                }

                var clienteServicio = await _dbcliservContext.ClienteServicios
                    .Where(cs => cs.IdCliente == idCliente && cs.IdServicio == idServicio)
                    .FirstOrDefaultAsync();

                if (clienteServicio == null)
                {
                    return NotFound("No se encontró la relación entre el cliente y el servicio.");
                }

                _dbcliservContext.ClienteServicios.Remove(clienteServicio);

                await _dbcliservContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el servicio del cliente: {ex.Message}");
            }
        }
    }
}
