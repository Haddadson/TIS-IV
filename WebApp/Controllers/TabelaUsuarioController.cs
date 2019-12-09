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
        private readonly ExportacaoExcelService exportacaoExcelService;

        public TabelaUsuarioController()
        {
            tabelaUsuarioService = new TabelaUsuarioService();
            cupomFiscalService = new CupomFiscalService();
            exportacaoExcelService = new ExportacaoExcelService();
        }

        public JsonResult CadastrarTabela(TabelaUsuario TabelaUsuario)
        {
            try
            {
                tabelaUsuarioService.CadastrarTabela(
                TabelaUsuario.SGDP,
                TabelaUsuario.AnoReferente,
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

        public ActionResult ExportarTabelasParaExcel(DadosTabelaDto DadosTabela,
                                                     List<AnpxNotaFiscalModelDto> ListaTabelaAnpxNota,
                                                     List<CupomFiscalDto> ListaCuponsFiscais,
                                                     List<OutrasInformacoesModelDto> ListaOutrasInformacoes)

        {

            try
            {
                var documento = exportacaoExcelService.ExportarDadosParaExcel(DadosTabela, ListaTabelaAnpxNota, ListaCuponsFiscais, ListaOutrasInformacoes);

                if(documento == null || documento.Length == 0)
                    return Json(new
                    {
                        Sucesso = false,
                        Mensagem = "Ocorreu um erro ao exportar o documento"
                    });

                string identificadorArquivo = Guid.NewGuid().ToString();
                TempData[identificadorArquivo] = documento;
                TempData.Keep(identificadorArquivo);

                return RetornarIdentificadorArquivoParaDownload(identificadorArquivo);
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro inesperado"
                });
            }
            
        }

        public ActionResult FazerDownloadDeExcel(string identificadorArquivo)
        {
            try
            {
                if (TempData[identificadorArquivo] != null)
                {
                    byte[] arquivoGerado = (byte[])TempData[identificadorArquivo];
                    return File(arquivoGerado, "application/vnd.ms-excel", identificadorArquivo + ".xlsx");
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro ao efetuar download"
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

        private JsonResult RetornarIdentificadorArquivoParaDownload(string identificadorArquivo)
        {
            return Json(new
            {
                Sucesso = true,
                Mensagem = string.Empty,
                IdentificadorArquivo = identificadorArquivo
            });
        }
    }
}