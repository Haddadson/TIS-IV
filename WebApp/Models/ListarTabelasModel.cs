using MPMG.Interfaces.DTO;

namespace WebApp.Models
{
    public class ListarTabelasModel
    {
        public ListarTabelasModel()
        {
        }

        public string ValorSgdp { get; set; }
        public TabelaUsuarioDto TabelaAnpXNota { get; set; }
    }
}