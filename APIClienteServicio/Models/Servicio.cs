using System;
using System.Collections.Generic;

namespace APIClienteServicio.Models;

public partial class Servicio
{
    public int Id { get; set; }

    public string? NombreServicio { get; set; }

    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<ClienteServicio> ClienteServicios { get; set; } = new List<ClienteServicio>();
    public class ClienteServicioResponse
    {
        public IEnumerable<Servicio> Servicios { get; set; }
    }
}


