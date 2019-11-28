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
        public ActionResult Index()
        {
            return View("AtualizarFam");
        }

        public ActionResult AtualizarFam(AtualizacaoFam model)
        {
            byte[] arquivoBytesFam = Convert.FromBase64String(RemoverStringBase64(model.ArquivoFam));
            //TODO
            //MemoryStream stream = new MemoryStream(arquivoBytesFam);

            //if (model.ExtensaoArquivoFam.ToLower().EndsWith("xls"))
            //    //1. Reading from a binary Excel file ('97-2003 format; *.xls) 
            //    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //else
            //    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            //    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            return Json(new { sucesso = true });
        }

        private string RemoverStringBase64(string valor)
        {
            return valor.Substring(valor.IndexOf("base64,") + 7);
        }

    }
}