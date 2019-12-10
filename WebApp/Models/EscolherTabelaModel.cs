using MPMG.Interfaces.DTO;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class EscolherTabelaModel
    {
        public EscolherTabelaModel()
        {
            SgdpsTabelasUsuario = new List<string>();
        }

        public List<string> SgdpsTabelasUsuario { get; set; }
    }
}