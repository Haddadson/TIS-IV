using System;

namespace MPMG.Interfaces.DTO
{
    public class DadosTabelaDto
    {
        public string Sgdp { get; set; }
        public string Municipio { get; set; }
        public string MunicipioReferente { get; set; }
        public string AnalistaResponsavel { get; set; }
        public DateTime DataGeracao { get; set; }
        public string AnosReferentes { get; set; }
    }
}