using System;
using System.Collections.Generic;

namespace APIClienteServicio.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public string? NombreCliente { get; set; }

    public string? Correo { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<ClienteServicio> ClienteServicios { get; set; } = new List<ClienteServicio>();
}
