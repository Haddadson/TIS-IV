using MPMG.Interfaces.DTO;
using System.Collections.Generic;

namespace WebApp.Models
{
    public class EscolherTabelaModel
    {
        public EscolherTabelaModel()
        {
            SgdpsTabelasUsuario = new List<int>();
        }

        public List<int> SgdpsTabelasUsuario { get; set; }
    }
}