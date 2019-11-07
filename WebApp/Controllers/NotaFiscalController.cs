using MPMG.Services;
using MPMG.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class NotaFiscalController : Controller
    {

        public NotaFiscalController()
        {
        }

        public ActionResult Index()
        {
            return View("NotaFiscal");
        }

    }
}