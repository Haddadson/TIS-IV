using MPMG.Interfaces.DTO;
using MPMG.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TabelaUsuarioController : Controller
    {
        private readonly TabelaUsuarioService tabelaUsuarioService;
        private readonly CupomFiscalService cupomFiscalService;

        public TabelaUsuarioController()
        {
            tabelaUsuarioService = new TabelaUsuarioService();
            cupomFiscalService = new CupomFiscalService();
        }

        public JsonResult CadastrarTabela(TabelaUsuario TabelaUsuario)
        {
            try
            {
                tabelaUsuarioService.CadastrarTabela(
                TabelaUsuario.SGDP,
                TabelaUsuario.AnosReferentes,
                TabelaUsuario.NomeMunicipioReferente,
                TabelaUsuario.NomeMunicipio,
                TabelaUsuario.DataGeracao,
                TabelaUsuario.Titulo1,
                TabelaUsuario.Titulo2,
                TabelaUsuario.Titulo3,
                TabelaUsuario.AnalistaResponsavel);

                return Json(new
                {
                    Mensagem = "Sucesso ao cadastrar tabela",
                    DataGeracao = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Mensagem = "Ocorreu um erro ao cadastrar tabela",
                    MensagemExcecao = ex.Message,
                    StackTraceExcecao = ex.StackTrace
                });
            }
        }

        public ActionResult Index(string valorSgdp = null)
        {
            TabelaUsuarioDto tabelaAnpXNota = new TabelaUsuarioDto();
            TabelaUsuarioDto tabelaOutrasInfos = new TabelaUsuarioDto();
            List<CupomFiscalDto> tabelaCuponsFiscais = new List<CupomFiscalDto>();

            try
            {
                tabelaAnpXNota = tabelaUsuarioService.ObterTabelaComDadosAnpxNotaFiscal(valorSgdp);
                tabelaCuponsFiscais = cupomFiscalService.ListarCuponsFiscaisPorSgdp(valorSgdp);
                tabelaOutrasInfos = tabelaUsuarioService.ObterTabelaOutrasInformacoes(valorSgdp);
            }
            catch (Exception ex) { }
                return View("ListarTabelas", new ListarTabelasModel
            {
                ValorSgdp = valorSgdp,
                TabelaAnpXNota = tabelaAnpXNota,
                TabelaOutrasInformacoes = tabelaOutrasInfos,
                TabelaCuponsFicais = tabelaCuponsFiscais
            });
        }
    }
}