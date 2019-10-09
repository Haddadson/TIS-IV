using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Repositories.Entidades
{
    class TabelaUsuarioModel
    {
        public int SGDP { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public DateTime DataGeracao { get; set; }
        public double AnoReferente { get; set; }
        public double Titulo1 { get; set; }
        public double Titulo2 { get; set; }
        public double Titulo3 { get; set; }
        public string AnalistaResponsavel { get; set; }
    }
}
