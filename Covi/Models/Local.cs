using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class Local
    {
        public int LocalId { get; set; }
        public int UsuarioId { get; set; }
        public int TipoLocalId { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual TipoLocal TipoLocal { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Evento> Evento { get; set; }
    }
}
