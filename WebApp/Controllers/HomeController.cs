using MPMG.Services;
using MPMG.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private ObterExcelCombustivelService ObterExcelPetroleoService;
        private ObterPrecoMedioExcelService LerDadosExcelService;

        public HomeController()
        {
            ObterExcelPetroleoService = new ObterExcelCombustivelService();
            LerDadosExcelService = new ObterPrecoMedioExcelService();
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

        public JsonResult ObterExcel()
        {
            ObterExcelPetroleoService.ObterExcelPrecosCombustivelESalvar();
            LerDadosExcelService.PreencherDadosNotaFiscalSuperFaturamento("ADENDO_ID_2927539.xlsx", Constantes.NOME_ARQUIVO_ANP_PRECOS);

            return new JsonResult();
        }
    }
}