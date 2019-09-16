using System;

namespace MPMG.Interfaces.DTO
{
    public class DadosPlanilhaAnpDto
    {
        public DateTime MesAnoInformacao { get; set; }
        public string Combustivel { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public double PrecoMedioRevenda { get; set; }
    }
}
