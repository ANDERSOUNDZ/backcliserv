using APIClienteServicio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        [Route("ListaClienteServicios/{idCliente}")]
        public async Task<IActionResult> ListaClienteServicios(int idCliente)
        {
            var cliente = await _dbcliservContext.Clientes.FindAsync(idCliente);
            var servicios = await (from cs in _dbcliservContext.ClienteServicios
                                   join s in _dbcliservContext.Servicios on cs.IdServicio equals s.Id
                                   where cs.IdCliente == idCliente
                                   select new
                                   {
                                       NombreServicio = s.NombreServicio,
                                       Descripcion = s.Descripcion,
                                       Estado = s.Estado
                                   }).ToListAsync();
            var resultado = new
            {
                NombreCliente = cliente.NombreCliente,
                Servicios = servicios
            };
            return Ok(resultado);
        }

        [HttpPost]
        [Route("agregarServicioCliente")]
        public async Task<IActionResult> AgregarServicioCliente(int idCliente, [FromBody] IEnumerable<int?> listaIdsServicios)
        {
            try
            {
                var cliente = await _dbcliservContext.Clientes.FindAsync(idCliente);
                var idsServiciosExistentes = await _dbcliservContext.ClienteServicios
                    .Where(cs => cs.IdCliente == idCliente)
                    .Select(cs => cs.IdServicio)
                    .ToListAsync();

                var nuevosIdsServicios = listaIdsServicios.Except(idsServiciosExistentes);

                var nuevosServiciosCliente = nuevosIdsServicios.Select(idServicio => new ClienteServicio
                {
                    IdCliente = idCliente,
                    IdServicio = idServicio,
                    Estado = true
                });

                await _dbcliservContext.ClienteServicios.AddRangeAsync(nuevosServiciosCliente);

                await _dbcliservContext.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el servicio a un cliente: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditarClienteEstadoServicio/{idCliente}")]
        public async Task<IActionResult> PutClienteServicioEstado(int idCliente, int idServicio, bool nuevoEstado)
        {
            var servicio = await _dbcliservContext.Servicios.FindAsync(idServicio);
            var cliente = await _dbcliservContext.Clientes.FindAsync(idCliente);
            //var hasService = await _dbXcode.ClienteServicios.AnyAsync(cs => cs.IdCliente == idCliente && cs.IdServicio == idServicio);
            servicio.Estado = nuevoEstado;
            await _dbcliservContext.SaveChangesAsync();
            return Ok(cliente);
        }
    }
}
