using MPMG.Services;
using MPMG.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ObterExcelCombustivelService ObterExcelPetroleoService;
        private readonly PlanilhaAnpService LerDadosExcelService;
        private readonly ObterMunicipioAnpService listarMunicipiosService;

        public HomeController()
        {
            ObterExcelPetroleoService = new ObterExcelCombustivelService();
            LerDadosExcelService = new PlanilhaAnpService();
            listarMunicipiosService = new ObterMunicipioAnpService();
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

        public JsonResult ObterMunicipioAnpPorNomeAno(int anoReferente, string nomeMunicipio)
        {
            try
            {
                var municipio = listarMunicipiosService.ObterMunicipioAnpPorNomeAno(anoReferente, nomeMunicipio);
                List<string> listaMunicipios = new List<string>();

                if (municipio == null)
                    listaMunicipios = listarMunicipiosService.ListarMunicipiosAnpPorAno(anoReferente);

                return Json(new
                {
                    municipioSelecionado = nomeMunicipio,
                    anoSelecionado = anoReferente,
                    municipioReferente = municipio,
                    listaMunicipios
                });

            }
            catch (Exception ex)
            {
                return Json(new { });
            }
        }

        [HttpGet]
        public ActionResult ObterExcel()
        {
            ObterExcelPetroleoService.ObterExcelPrecosCombustivelESalvar();
            LerDadosExcelService.PopularBancoComDadosAnp(Constantes.NOME_ARQUIVO_ANP_PRECOS);

            return Json("Sucesso", JsonRequestBehavior.AllowGet);
        }

    }
}