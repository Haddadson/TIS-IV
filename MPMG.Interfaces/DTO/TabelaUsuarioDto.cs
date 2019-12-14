using System;
using System.Collections.Generic;

namespace MPMG.Interfaces.DTO
{
    public class TabelaUsuarioDto
    {
        public TabelaUsuarioDto()
        {
            DadosAnpxNotaFiscal = new List<AnpxNotaFiscalDto>();
            AnosReferentes = new List<int>();
        }

        public string SGDP { get; set; }
        public DateTime DataGeracao { get; set; }
        public List<int> AnosReferentes { get; set; }
        public string Titulo1 { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string AnalistaResponsavel { get; set; }
        public MunicipioDto Municipio { get; set; }
        public MunicipioDto MunicipioReferente { get; set; }
        public List<AnpxNotaFiscalDto> DadosAnpxNotaFiscal { get; set; }
        public List<OutrasInformacoesDto> OutrasInformacoes { get; set; }
    }
}
