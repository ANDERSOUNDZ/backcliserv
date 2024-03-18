using System;
using System.Collections.Generic;

namespace APIClienteServicio.Models;

public partial class ClienteServicio
{
    public int Id { get; set; }

    public int? IdCliente { get; set; }

    public int? IdServicio { get; set; }

    public bool? Estado { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }
}
