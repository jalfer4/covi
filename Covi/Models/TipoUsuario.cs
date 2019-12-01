using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class TipoUsuario
    {
        public TipoUsuario()
        {
            Usuario = new HashSet<Usuario>();
        }

        public int TipoUsuarioId { get; set; }
        public string Nombre { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
