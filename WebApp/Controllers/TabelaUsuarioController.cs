using System.Web.Mvc;
using MPMG.Services;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TabelaUsuarioController : Controller
    {
        private readonly TabelaUsuarioService tabelaUsuarioService;
        public TabelaUsuarioController()
        {
            tabelaUsuarioService = new TabelaUsuarioService ();
        }

        public JsonResult CadastrarTabela(TabelaUsuario TabelaUsuario)
        {
            int teste = TabelaUsuario.IdMunicipio;

            return Json(tabelaUsuarioService.ToString());
        }
    }
}