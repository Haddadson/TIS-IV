using MPMG.Interfaces.DTO;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class EscolherTabelaModel
    {
        public EscolherTabelaModel()
        {
            TabelasUsuario = new List<TabelaUsuarioDto>();
        }

        public List<TabelaUsuarioDto> TabelasUsuario { get; set; }
    }
}