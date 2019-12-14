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

            int horas = int.Parse(cupom.Horario.Substring(0, 2));
            int minutos = int.Parse(cupom.Horario.Substring(3, 2));
            DateTime Data = new DateTime(cupom.Data.Year, cupom.Data.Month, cupom.Data.Day, horas, minutos, 0);
            string Combustivel = cupom.Combustivel;
            int Quantidade = cupom.Quantidade;
            double PrecoUnitario = cupom.PrecoUnitario;
            double ValorTotal = cupom.ValorTotal;
            string Cliente = cupom.Cliente;
            int Hodometro = cupom.Hodometro;
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
#pragma warning disable CS0168 // A variável "ex" está declarada, mas nunca é usada
            catch (Exception ex)
#pragma warning restore CS0168 // A variável "ex" está declarada, mas nunca é usada
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

            try
            {
                tabelas = tabelaUsuarioService.ListarTabelas();
            }
#pragma warning disable CS0168 // A variável "ex" está declarada, mas nunca é usada
            catch (Exception ex)
#pragma warning restore CS0168 // A variável "ex" está declarada, mas nunca é usada
            {

            }

            return View("CupomFiscal", new NotaFiscalModel { ValorSgdp = valorSgdp, TabelasUsuario = tabelas });
        }
    }
}