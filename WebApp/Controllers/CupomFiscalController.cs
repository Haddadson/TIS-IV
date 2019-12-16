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

            string SGDP = cupom.SGDP;
            string NrNotaFiscal = cupom.NrNotaFiscal;
            string COO = cupom.COO;
            string Posto = cupom.Posto;
            DateTime Data = cupom.Data;
            string Combustivel = cupom.Combustivel;
            double Quantidade = cupom.Quantidade;
            double PrecoUnitario = cupom.PrecoUnitario;
            double ValorTotal = cupom.ValorTotal;
            string Cliente = cupom.Cliente;
            double Hodometro = cupom.Hodometro;
            string Veiculo = cupom.Veiculo;
            string PlacaVeiculo = cupom.PlacaVeiculo;

            try
            {
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
                    Sucesso= true,
                    DataGeracao = DateTime.Now
                });
            }
           catch (Exception ex)
            {
                return Json(new
                {
                    Mensagem = "Ocorreu um erro ao cadastrar.",
                    Sucesso = false
                });
            }


        }

        public ActionResult Index(string valorSgdp = null)
        {
            List<TabelaUsuarioDto> tabelas = new List<TabelaUsuarioDto>();
            CupomFiscalInfoDto cuponsInfo = new CupomFiscalInfoDto();

            try
            {
                tabelas = tabelaUsuarioService.ListarTabelas();
                cuponsInfo = cupomFiscalService.ObterInfoCuponsFiscais(valorSgdp);
            }
            catch (Exception ex)
            {

            }

            return View("CupomFiscal", new CupomFiscalModel { ValorSgdp = valorSgdp, CuponsInfo = cuponsInfo });
        }
    }
}