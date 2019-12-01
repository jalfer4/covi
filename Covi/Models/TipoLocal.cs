using System;
using System.Collections.Generic;

namespace Covi.Models
{
    public partial class TipoLocal
    {
        public TipoLocal()
        {
            Local = new HashSet<Local>();
        }

        public int TipoLocald { get; set; }
        public string Nombre { get; set; }
        public bool EstaHabilitado { get; set; }
        public bool EstaPublicado { get; set; }

        public virtual ICollection<Local> Local { get; set; }
    }
}
