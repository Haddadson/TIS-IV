using MPMG.Interfaces.DTO;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class ListarTabelasModel
    {
        public ListarTabelasModel()
        {
            TabelaCuponsFicais = new List<CupomFiscalDto>();
        }

        public string ValorSgdp { get; set; }
        public TabelaUsuarioDto TabelaAnpXNota { get; set; }
        public List<CupomFiscalDto> TabelaCuponsFicais { get; set; }
    }
}