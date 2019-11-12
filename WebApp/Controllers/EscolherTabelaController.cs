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

        // GET: EscolherTabela/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EscolherTabela/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EscolherTabela/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EscolherTabela/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EscolherTabela/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EscolherTabela/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EscolherTabela/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
