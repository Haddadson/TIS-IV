using MPMG.Services;
using MPMG.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class NotaFiscalController : Controller
    {
        private readonly NotaFiscalService NotaFiscalService;

        public NotaFiscalController()
        {
            NotaFiscalService = new NotaFiscalService();
        }

        public JsonResult Cadastrar(NotaFiscal NotaFiscal)
        {
            int NrNotaFiscal = NotaFiscal.NrNotaFiscal;
            int SGDP = NotaFiscal.SGDP;
            double ValorTotal = NotaFiscal.ValorTotal;
            string ChaveAcesso = NotaFiscal.ChaveAcesso;
            DateTime DataEmissao = new DateTime(
                Int32.Parse(NotaFiscal.DataEmissao.Substring(0, 4)),
                Int32.Parse(NotaFiscal.DataEmissao.Substring(5, 2)),
                Int32.Parse(NotaFiscal.DataEmissao.Substring(8, 2)));
            double PrecoMaximo = NotaFiscal.PrecoMaximo;
            double PrecoMedio = NotaFiscal.PrecoMedio;
            DateTime DataConsultaANP = new DateTime(
                Int32.Parse(NotaFiscal.DataConsultaANP.Substring(6, 4)),
                Int32.Parse(NotaFiscal.DataConsultaANP.Substring(3, 2)),
                Int32.Parse(NotaFiscal.DataConsultaANP.Substring(0, 2)));
            string Veiculo = NotaFiscal.Veiculo;
            string PlacaVeiculo = NotaFiscal.PlacaVeiculo;
            string Combustivel = NotaFiscal.Combustivel;
            int Quantidade = NotaFiscal.Quantidade;
            double PrecoUnitario = NotaFiscal.PrecoUnitario;
            int NumeroFolha = NotaFiscal.NumeroFolha;
            int Departamento = NotaFiscal.Departamento;
            List<string> CuponsSelecionados =  NotaFiscal.CuponsSelecionados;

            NotaFiscalService.addNotaFiscal(
                NrNotaFiscal,
                SGDP,
                ValorTotal,
                ChaveAcesso,
                DataEmissao,
                PrecoMaximo,
                PrecoMedio,
                DataConsultaANP,
                Veiculo,
                PlacaVeiculo,
                Combustivel,
                Quantidade,
                PrecoUnitario,
                NumeroFolha,
                Departamento,
                CuponsSelecionados
            );

            return Json(new
            {
                Mensagem = "Sucesso ao cadastrar nota fiscal!",
                DataGeracao = DateTime.Now
            });
        }

        public ActionResult Index()
        {
            return View("NotaFiscal");
        }

    }
}