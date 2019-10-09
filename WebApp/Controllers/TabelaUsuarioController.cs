using System.Web.Mvc;
using MPMG.Services;

namespace WebApp.Controllers
{
    public class TabelaUsuarioController : Controller
    {
        private readonly TabelaUsuarioService tabelaUsuarioService;
        public TabelaUsuarioController()
        {
            tabelaUsuarioService = new TabelaUsuarioService ();
        }

        public JsonResult CadastrarTabela()
        {


            return Json(tabelaUsuarioService.ToString());
        }        

        // GET: TabelaUsuario
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TabelaUsuarioModel tabela)
        {
            int personId  = tabela.AnoReferente;
            string name   = tabela.SGDP;

            tabelaUsuarioService.cadastrarTabela(tabela);

            return View();
        }
    }
}