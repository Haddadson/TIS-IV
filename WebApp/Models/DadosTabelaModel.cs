using System;

namespace WebApp.Models
{
    public class DadosTabelaModel
    {
        public string Sgdp { get; set; }
        public string Municipio { get; set; }
        public string MunicipioReferente { get; set; }
        public string AnalistaResponsavel { get; set; }
        public DateTime DataGeracao { get; set; }
        public string AnosReferentes { get; set; }
    }
}