using MPMG.Services;
using MPMG.Util;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ObterExcelCombustivelService ObterExcelPetroleoService;
        private readonly ObterPrecoMedioExcelService LerDadosExcelService;
        private readonly ListarMunicipiosAnpService ListarMunicipiosService;

        public HomeController()
        {
            ObterExcelPetroleoService = new ObterExcelCombustivelService();
            LerDadosExcelService = new ObterPrecoMedioExcelService();
            ListarMunicipiosService = new ListarMunicipiosAnpService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult ListarMunicipiosAnp(int anoReferente)
        {
            var listaMunicipios = ListarMunicipiosService.ListarMunicipiosAnpPorAno(anoReferente);

            return Json(new
            {
                municipios = listaMunicipios
            });
        }

        public JsonResult ObterExcel()
        {
            ObterExcelPetroleoService.ObterExcelPrecosCombustivelESalvar();
            LerDadosExcelService.PreencherDadosNotaFiscalSuperFaturamento("ADENDO_ID_2927539.xlsx", Constantes.NOME_ARQUIVO_ANP_PRECOS);

            return new JsonResult();
        }

    }
}