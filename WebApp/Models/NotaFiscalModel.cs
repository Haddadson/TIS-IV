using MPMG.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class NotaFiscalModel
    {
        public NotaFiscalModel()
        {
            TabelasUsuario = new List<TabelaUsuarioDto>();
        }

        public string ValorSgdp { get; set; }
        public List<TabelaUsuarioDto> TabelasUsuario { get; set; }
    }
}