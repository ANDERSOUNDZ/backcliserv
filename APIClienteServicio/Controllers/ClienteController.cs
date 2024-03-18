using APIClienteServicio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIClienteServicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly DbcliservContext _dbcliservContext;

        public ClienteController(DbcliservContext database)
        {
            _dbcliservContext = database;
        }

        [HttpGet]
        [Route("listaCliente")]
        public async Task<IActionResult> Lista()
        {
            try
            {
                var listaCliente = await _dbcliservContext.Clientes.ToListAsync();

                if (listaCliente == null || !listaCliente.Any())
                {
                    return NotFound("No hay lista de cliente que mostrar");
                }
                return Ok(listaCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la lista de clientes: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("obtenerCliente/{id:int}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            try
            {
                var cliente = await _dbcliservContext.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return NotFound("No se encontró el cliente con el ID especificado");
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el cliente: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("agregarCliente")]
        public async Task<IActionResult> AgregarCliente([FromBody] Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _dbcliservContext.Clientes.AddAsync(cliente);
                await _dbcliservContext.SaveChangesAsync();
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el cliente: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("actualizarCliente/{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var clienteActual = await _dbcliservContext.Clientes.FindAsync(id);
                if (clienteActual == null)
                {
                    return NotFound("No se encontró el cliente");
                }

                if (cliente.NombreCliente != null)
                {
                    clienteActual.NombreCliente = cliente.NombreCliente;
                }

                if (cliente.Correo != null)
                {
                    clienteActual.Correo = cliente.Correo;
                }

                if (cliente.Estado != null)
                {
                    clienteActual.Estado = cliente.Estado;
                }

                await _dbcliservContext.SaveChangesAsync();
                return Ok(clienteActual);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el correo: {ex.Message}");
            }
        }


        [HttpDelete]
        [Route("eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var clienteEliminar = await _dbcliservContext.Clientes.FindAsync(id);
            if (clienteEliminar == null)
            {
                return BadRequest("No existe el cliente");
            }

            _dbcliservContext.Clientes.Remove(clienteEliminar);
            await _dbcliservContext.SaveChangesAsync();
            return Ok();
        }
    }
}
