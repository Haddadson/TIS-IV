using MPMG.Interfaces.DTO;
using MPMG.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class EscolherTabelaController : Controller
    {
        private readonly TabelaUsuarioService tabelaUsuarioService;
        public EscolherTabelaController()
        {
            tabelaUsuarioService = new TabelaUsuarioService();
        }

        public ActionResult RedirecionarVisualizarTabela(string valorSgdp)
        {
            var urlBuilder = new UrlHelper(Request.RequestContext);
            var url = urlBuilder.Action("Index", "TabelaUsuario", new { valorSgdp });

            return Json(new { sucesso = true, urlRedirecionamento = url });
        }

        // GET: EscolherTabela
        public ActionResult Index()
        {

            List<TabelaUsuarioDto> tabelas = new List<TabelaUsuarioDto>();

            try
            {
                tabelas = tabelaUsuarioService.ListarTabelas();

            }
            catch (Exception ex)
            {
            }

            return View("EscolherTabela", new EscolherTabelaModel() { TabelasUsuario = tabelas });
        }
    }
}