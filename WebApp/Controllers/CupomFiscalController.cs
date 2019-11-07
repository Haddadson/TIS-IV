using MPMG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class CupomFiscalController : Controller
    {
        private readonly CupomFiscalService cupomFiscalService;
        public CupomFiscalController()
        {
            cupomFiscalService = new CupomFiscalService();
        }

        public JsonResult Cadastrar(CupomFiscal cupom)
        {
            int SGDP = cupom.SGDP ;
            int NrNotaFiscal = cupom.NrNotaFiscal;
            string COO = cupom.COO;
            string Posto = cupom.Posto;
            DateTime Data = cupom.Data;
            string Combustivel = cupom.Combustivel;
            int Quantidade = cupom.Quantidade;
            double PrecoUnitario = cupom.PrecoUnitario;
            double ValorTotal = cupom.ValorTotal;
            string Cliente = cupom.Cliente;
            int Hodometro = cupom.Hodometro;
            string Veiculo = cupom.Veiculo;
            string PlacaVeiculo = cupom.PlacaVeiculo;

            cupomFiscalService.Cadastrar(
                SGDP,
                NrNotaFiscal,
                COO,
                Posto,
                Data,
                Combustivel,
                Quantidade,
                PrecoUnitario,
                ValorTotal,
                Cliente,
                Hodometro,
                Veiculo,
                PlacaVeiculo);

            return Json(new
            {
                Mensagem = "Sucesso ao cadastrar cupom fiscal!",
                DataGeracao = DateTime.Now
            });
        }

        public ActionResult Index()
        {
            return View("CupomFiscal");
        }
    }
}