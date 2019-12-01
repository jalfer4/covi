using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Local = new HashSet<Local>();
            Reserva = new HashSet<Reserva>();
        }

        public int UsuarioId { get; set; }
        public int TipoUsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Apellido { get; set; }
        public string ApyNomb { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual TipoUsuario TipoUsuarioNavigation { get; set; }
        public virtual ICollection<Local> Local { get; set; }
        public virtual ICollection<Reserva> Reserva { get; set; }
    }
}
