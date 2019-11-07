using MPMG.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ListarTabelasModel
    {
        public ListarTabelasModel()
        {
            TabelasUsuario = new List<TabelaUsuarioDto>();
        }

        public List<TabelaUsuarioDto> TabelasUsuario { get; set; }
    }
}