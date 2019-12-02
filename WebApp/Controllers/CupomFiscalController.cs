using MPMG.Interfaces.DTO;
using MPMG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class CupomFiscalController : Controller
    {
        private readonly CupomFiscalService cupomFiscalService;
        private readonly TabelaUsuarioService tabelaUsuarioService;

        public CupomFiscalController()
        {
            cupomFiscalService = new CupomFiscalService();
            tabelaUsuarioService = new TabelaUsuarioService();
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
            List<TabelaUsuarioDto> tabelas = new List<TabelaUsuarioDto>();

            try
            {
                tabelas = tabelaUsuarioService.ListarTabelas();

            }
            catch (Exception ex)
            {
            }

            return View("CupomFiscal", new NotaFiscalModel { TabelasUsuario = tabelas } );
        }

        public ActionResult Index(string valorSgdp = null)
        {

            return View("ListarTabelas");
        }

    }
}