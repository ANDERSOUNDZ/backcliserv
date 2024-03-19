using System;
using System.Collections.Generic;

namespace APIClienteServicio.Models;

public partial class ClienteServicio
{
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public int IdServicio { get; set; }

    public bool Estado { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public class AgregarServicioClienteRequest
    {
        public int IdCliente { get; set; }
        public int IdServicio { get; set; }
        public bool Estado { get; set; }
    }
    public class ClienteServicioCompleto
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }
        public int IdServicio { get; set; }
        public string NombreServicio { get; set; }
        public bool Estado { get; set; }
    }
}
