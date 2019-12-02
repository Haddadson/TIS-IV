using MPMG.Interfaces.DTO;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class TabelasExportacaoModel
    {
        public TabelasExportacaoModel()
        {
            ListaTabelaAnpxNota = new List<AnpxNotaFiscalModel>();
            ListaCuponsFiscais = new List<CupomFiscalDto>();
            ListaOutrasInformacoes = new List<OutrasInformacoesModel>();
        }

        public DadosTabelaModel DadosTabela;
        public List<AnpxNotaFiscalModel> ListaTabelaAnpxNota;
        public List<CupomFiscalDto> ListaCuponsFiscais;
        public List<OutrasInformacoesModel> ListaOutrasInformacoes;
    }
}