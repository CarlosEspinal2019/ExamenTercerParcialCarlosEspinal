using System;
using System.Collections.Generic;
using System.Text;

namespace ExamenTercerParcialCarlosEspinal.Models
{
    public class Nota
    {
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        public string Photo_Record { get; set; }
        public string Audio_Record { get; set; }

        public string Key { get; set; }
    }
}
