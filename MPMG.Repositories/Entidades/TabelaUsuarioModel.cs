using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Repositories.Entidades
{
    class TabelaUsuarioModel
    {
        public int SGDP { get; set; }

        public Municipio Municipio { get; set; }
        public DateTime DataGeracao { get; set; }
        public double AnoReferente { get; set; }
        public double Titulo1 { get; set; }
        public double Titulo2 { get; set; }
        public double Titulo3 { get; set; }
        public string AnalistaResponsavel { get; set; }
    }
}
