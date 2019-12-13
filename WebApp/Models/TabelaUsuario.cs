using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class TabelaUsuario
    {
        public string SGDP { get; set; }
        public string NomeMunicipio { get; set; }
        public string NomeMunicipioReferente { get; set; }
        public DateTime DataGeracao { get; set; }
        public List<int> AnosReferentes { get; set; }
        public string Titulo1 { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string AnalistaResponsavel { get; set; }
    }
}