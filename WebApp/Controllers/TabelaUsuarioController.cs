using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MPMG.Interfaces.DTO;
using MPMG.Services;
using Newtonsoft.Json;
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
            int SGDP = TabelaUsuario.SGDP;
            int IdMunicipioReferente = TabelaUsuario.IdMunicipioReferente;
            string IdMunicipio = TabelaUsuario.IdMunicipio;
            int AnoReferente = TabelaUsuario.AnoReferente;
            DateTime DataGeracao = TabelaUsuario.DataGeracao;
            string Titulo1 = TabelaUsuario.Titulo1,
                    Titulo2 = TabelaUsuario.Titulo2,
                    Titulo3 = TabelaUsuario.Titulo3,
                    AnalistaResponsavel = TabelaUsuario.AnalistaResponsavel;

            try
            {
                TabelaUsuarioService.CadastrarTabela(
                SGDP,
                AnoReferente,
                IdMunicipioReferente,
                IdMunicipio,
                DataGeracao,
                Titulo1,
                Titulo2,
                Titulo3,
                AnalistaResponsavel);


                return Json(new {
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
        public ActionResult Index()
        {
            List<TabelaUsuarioDto> tabelas = new List<TabelaUsuarioDto>();

            try
            {
               tabelas = TabelaUsuarioService.ListarTabelas();

            }catch(Exception ex)
            {
            }

            return View("ListarTabelas", new ListarTabelasModel { TabelasUsuario = tabelas } );
        }
    }
}