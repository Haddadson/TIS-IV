using MPMG.Interfaces.DTO;
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
        private readonly NotaFiscalService notaFiscalService;
        private readonly TabelaUsuarioService tabelaUsuarioService;
        private readonly DepartamentoService departamentoService;

        public NotaFiscalController()
        {
            notaFiscalService = new NotaFiscalService();
            tabelaUsuarioService = new TabelaUsuarioService();
            departamentoService = new DepartamentoService();
        }

        public JsonResult Cadastrar(NotaFiscal NotaFiscal)
        {
            int NrNotaFiscal = NotaFiscal.NrNotaFiscal;
            int SGDP = NotaFiscal.SGDP;
            double ValorTotal = NotaFiscal.ValorTotal;
            string ChaveAcesso = NotaFiscal.ChaveAcesso;
            DateTime DataEmissao = new DateTime(
                int.Parse(NotaFiscal.DataEmissao.Substring(0, 4)),
                int.Parse(NotaFiscal.DataEmissao.Substring(5, 2)),
                int.Parse(NotaFiscal.DataEmissao.Substring(8, 2)));
            double PrecoMaximo = NotaFiscal.PrecoMaximo;
            double PrecoMedio = NotaFiscal.PrecoMedio;
            DateTime DataConsultaANP = new DateTime(
                int.Parse(NotaFiscal.DataConsultaANP.Substring(6, 4)),
                int.Parse(NotaFiscal.DataConsultaANP.Substring(3, 2)),
                int.Parse(NotaFiscal.DataConsultaANP.Substring(0, 2)));
            string Veiculo = NotaFiscal.Veiculo;
            string PlacaVeiculo = NotaFiscal.PlacaVeiculo;
            string Combustivel = NotaFiscal.Combustivel;
            int Quantidade = NotaFiscal.Quantidade;
            double PrecoUnitario = NotaFiscal.PrecoUnitario;
            int NumeroFolha = NotaFiscal.NumeroFolha;
            int Departamento = NotaFiscal.Departamento;
            List<string> CuponsSelecionados =  NotaFiscal.CuponsSelecionados;

            CuponsSelecionados = CuponsSelecionados ?? new List<string>();

            notaFiscalService.cadastrarNotaFiscal(
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

        public JsonResult ListarDepartamentos(NotaFiscal NotaFiscal)
        {
            try
            {
                return Json(new
                {
                    departamentos = departamentoService.ListarDepartamentos()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Error = ex.Message
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
            catch (Exception ex)
            {
            }

            return View("NotaFiscal", new NotaFiscalModel { TabelasUsuario = tabelas, ValorSgdp = valorSgdp });
        }
    }
}