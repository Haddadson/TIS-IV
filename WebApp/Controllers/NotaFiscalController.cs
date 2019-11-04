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
        private NotaFiscalService NotaFiscalService;
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
            DateTime DataEmissao = NotaFiscal.DataEmissao;
            double PrecoMaximo = NotaFiscal.PrecoMaximo;
            double PrecoMinimo = NotaFiscal.PrecoMinimo;
            double PrecoMedio = NotaFiscal.PrecoMedio;
            DateTime DataConsultaANP = NotaFiscal.DataConsultaANP;
            string Veiculo = NotaFiscal.Veiculo;
            string PlacaVeiculo = NotaFiscal.PlacaVeiculo;
            string Posto = NotaFiscal.Posto;
            int Combustivel = NotaFiscal.Combustivel;
            int Quantidade = NotaFiscal.Quantidade;
            double PrecoUnitario = NotaFiscal.PrecoUnitario;
            int NumeroFolha = NotaFiscal.NumeroFolha;
            string Cliente = NotaFiscal.Cliente;
            int Departamento = NotaFiscal.Departamento;
            string Hodometro = NotaFiscal.Hodometro;

            NotaFiscalService.addNotaFiscal(
                NrNotaFiscal,
                SGDP,
                ValorTotal,
                ChaveAcesso,
                DataEmissao,
                PrecoMaximo,
                PrecoMinimo,
                PrecoMedio,
                DataConsultaANP,
                Veiculo,
                PlacaVeiculo,
                Posto,
                Combustivel,
                Quantidade,
                PrecoUnitario,
                NumeroFolha,
                Cliente,
                Departamento,
                Hodometro
                );

            return Json(new
            {
                Mensagem = "Sucesso ao cadastrar tabela",
                DataGeracao = DateTime.Now
            });
        }

        public ActionResult Index()
        {
            return View("NotaFiscal");
        }

    }
}