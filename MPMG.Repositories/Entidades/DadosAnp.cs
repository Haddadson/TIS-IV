using System;
using System.Collections.Generic;
using System.Text;

namespace MPMG.Repositories.Entidades
{
    public class DadosAnp
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Combustivel { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public double PrecoMedioRevenda { get; set; }
        public double PrecoMinimoRevenda { get; set; }
        public double PrecoMaximoRevenda { get; set; }
    }
}
