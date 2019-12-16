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
        private readonly CupomFiscalService cupomFiscalService;
        private readonly TabelaUsuarioService tabelaUsuarioService;
        private readonly DepartamentoService departamentoService;
        private readonly AnoANPService anoANPService;

        public NotaFiscalController()
        {
            cupomFiscalService = new CupomFiscalService();
            notaFiscalService = new NotaFiscalService();
            tabelaUsuarioService = new TabelaUsuarioService();
            departamentoService = new DepartamentoService();
            anoANPService = new AnoANPService();
        }

        public JsonResult ListarCupons(string SGDP)
        {
            try
            {
                return Json(new
                {
                    coos = cupomFiscalService.ListarCuponsDisponiveisPorSgdp(SGDP)
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

        public JsonResult Cadastrar(NotaFiscal NotaFiscal, List<ItemNotaFiscalDto> ItensNotaFiscal)
        {
            try
            {
                string SGDP = NotaFiscal.SGDP;
                string NrNotaFiscal = NotaFiscal.NrNotaFiscal;
                double ValorTotal = NotaFiscal.ValorTotal;
                string ChaveAcesso = NotaFiscal.ChaveAcesso;
                DateTime DataEmissao = new DateTime(
                    int.Parse(NotaFiscal.DataEmissao.Substring(6, 4)),
                    int.Parse(NotaFiscal.DataEmissao.Substring(3, 2)),
                    int.Parse(NotaFiscal.DataEmissao.Substring(0, 2)));
                double PrecoMaximo = NotaFiscal.PrecoMaximo;
                double PrecoMedio = NotaFiscal.PrecoMedio;

                DateTime DataConsultaANP;
                if (NotaFiscal.DataConsultaANP != null)
                {
                    DataConsultaANP = new DateTime(
                        int.Parse(NotaFiscal.DataConsultaANP.Substring(6, 4)),
                        int.Parse(NotaFiscal.DataConsultaANP.Substring(3, 2)),
                        int.Parse(NotaFiscal.DataConsultaANP.Substring(0, 2)));
                } else
                {
                    DataConsultaANP = new DateTime(
                        int.Parse(NotaFiscal.DataEmissao.Substring(6, 4)),
                        int.Parse(NotaFiscal.DataEmissao.Substring(3, 2)),
                        int.Parse(NotaFiscal.DataEmissao.Substring(0, 2)));
                }

                string Veiculo = NotaFiscal.Veiculo;
                string PlacaVeiculo = NotaFiscal.PlacaVeiculo;
                string Combustivel = NotaFiscal.Combustivel;
                double Quantidade = NotaFiscal.Quantidade;
                double PrecoUnitario = NotaFiscal.PrecoUnitario;
                int NumeroFolha = NotaFiscal.NumeroFolha;
                int Departamento = NotaFiscal.Departamento;
                List<string> CuponsSelecionados = NotaFiscal.CuponsSelecionados;
                List<ItemNotaFiscalDto> itens = ItensNotaFiscal.Where(item => item.Quantidade > 0 &&
                    item.ValorTotal > 0 && item.ValorUnitario > 0 && !string.IsNullOrWhiteSpace(item.Sgdp) &&
                    !string.IsNullOrWhiteSpace(item.Produto)).ToList();

                CuponsSelecionados = CuponsSelecionados ?? new List<string>();

                notaFiscalService.CadastrarNotaFiscal(
                    NrNotaFiscal,
                    SGDP,
                    ValorTotal,
                    ChaveAcesso,
                    DataEmissao,
                    DataConsultaANP,
                    Veiculo,
                    PlacaVeiculo,
                    NumeroFolha,
                    Departamento,
                    CuponsSelecionados,
                    itens
                );

                return Json(new
                {
                    Sucesso = true,
                    Mensagem = "Sucesso ao cadastrar nota fiscal!",
                    DataGeracao = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Error = ex.Message,
                    Mensagem = "Ocorreu um erro ao cadastrar a nota fiscal!"
                });
            }
            
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
            List<AnoANPDto> anosANP = new List<AnoANPDto>();
            List<string> mesesANP = new List<string>();
            List<double> precos = new List<double>();
            try
            {
                anosANP = anoANPService.ListarMesesPorSGDP(valorSgdp);
                foreach (var anoANP in anosANP)
                {
                    foreach(var mes in anoANP.meses)
                    {
                        mesesANP.Add(mes.ToString("D2") + '/' + anoANP.ano.ToString());
                    }
                }

                precos = notaFiscalService.buscarPrecos(valorSgdp);
            }
            catch (Exception ex)
            {
            }

            return View("NotaFiscal", new NotaFiscalModel { ValorSgdp = valorSgdp, MesesANP = mesesANP, Precos = precos});
        }
    }
}