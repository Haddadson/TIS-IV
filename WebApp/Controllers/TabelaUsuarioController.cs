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
        private readonly TabelaUsuarioService TabelaUsuarioService;

        public TabelaUsuarioController()
        {
            TabelaUsuarioService = new TabelaUsuarioService();
        }

        public JsonResult CadastrarTabela(TabelaUsuario TabelaUsuario)
        {
            try
            {
                TabelaUsuarioService.CadastrarTabela(
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

        public JsonResult ListarTabelas()
        {
            var tabelas = TabelaUsuarioService.ListarTabelas();

            return Json(new
            {
                tabelas
            });
        }
        public ActionResult Index(string valorSgdp = null)
        {
            List<TabelaUsuarioDto> tabelas = new List<TabelaUsuarioDto>();
            TabelaUsuarioDto tabelaBuscada = new TabelaUsuarioDto();

            try
            {
                tabelaBuscada = TabelaUsuarioService.ObterTabela(valorSgdp);
                if (string.IsNullOrWhiteSpace(valorSgdp))
                    tabelas = TabelaUsuarioService.ListarTabelas();

            }
            catch (Exception ex) { }

            return View("ListarTabelas", new ListarTabelasModel { TabelasUsuario = tabelas, TabelaBuscada = tabelaBuscada });
        }
    }
}