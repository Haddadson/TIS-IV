using MPMG.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AtualizarFamController : Controller
    {
        private PlanilhaFamService planilhaFamService;

        public AtualizarFamController()
        {
            planilhaFamService = new PlanilhaFamService();
        }

        public ActionResult Index()
        {
            return View("AtualizarFam");
        }

        public ActionResult AtualizarFam(AtualizacaoFam model)
        {
            byte[] arquivoBytesFam = Convert.FromBase64String(RemoverStringBase64(model.ArquivoFam));
            bool resultado; 

            try
            {
                resultado = planilhaFamService.AtualizarDadosTabelaFam(arquivoBytesFam, model.ExtensaoArquivoFam);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false });
            }

            return Json(new { sucesso = resultado });
        }

        private string RemoverStringBase64(string valor)
        {
            return valor.Substring(valor.IndexOf("base64,") + 7);
        }

    }
}