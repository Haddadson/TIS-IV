using System;

namespace MPMG.Repositories.Entidades
{
    public class TabelaUsuario
    {
        public int SGDP { get; set; }
        public int IdMunicipio { get; set; }
        public string NomeMunicipio { get; set; }
        public int IdMunicipioReferente { get; set; }
        public string NomeMunicipioReferente { get; set; }
        public DateTime DataGeracao { get; set; }
        public int AnoReferente { get; set; }
        public string Titulo1 { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string AnalistaResponsavel { get; set; }
    }
}
