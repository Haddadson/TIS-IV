using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class EscolherTabelaController : Controller
    {
        // GET: EscolherTabela
        public ActionResult Index()
        {
            return View("EscolherTabela");
        }
    }
}