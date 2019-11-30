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

            if(string.IsNullOrWhiteSpace(model.ExtensaoArquivoFam) || 
                (model.ExtensaoArquivoFam != ".xls" && model.ExtensaoArquivoFam != ".xlsx"))
                return Json(new { sucesso = false, mensagem = "A extensão do arquivo informado é inválida. Selecione um arquivo .xlsx ou .xls" });

            try
            {
                resultado = planilhaFamService.AtualizarDadosTabelaFam(arquivoBytesFam, model.ExtensaoArquivoFam);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, mensagem = "Ocorreu um erro ao cadastrar uma nova tabela FAM" });
            }

            return Json(new { sucesso = resultado });
        }

        private string RemoverStringBase64(string valor)
        {
            return valor.Substring(valor.IndexOf("base64,") + 7);
        }

    }
}