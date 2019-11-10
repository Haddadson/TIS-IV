﻿using Dapper;
using MPMG.Repositories.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPMG.Repositories
{
    public class DadosAnpRepositorio : RepositorioBase<DadosAnp>
    {
        private const string SQL_OBTER_DADO_ANP = @"
            SELECT 
	            ano AS Ano,
                mes AS Mes,
                produto AS Produto,
                estado AS Estado,
                municipio AS Municipio,
                preco_medio_revenda AS PrecoMedioRevenda,
                preco_minimo_revenda AS PrecoMinimoRevenda,
                preco_maximo_revenda AS PrecoMaximoRevenda
            FROM TabelaANP
            WHERE mes = @Mes AND
                ano = @Ano AND
                municipio = @Municipio AND
                produto = @Produto";

        private const string SQL_LISTAR_MUNICIPIOS_ANP = @"
            SELECT DISTINCT 
                M.nome_municipio AS Municipio 
              FROM TabelaANP T 
              JOIN municipio M 
            ORDER BY M.nome_municipio";

        private const string SQL_LISTAR_MUNICIPIOS_ANP_POR_ANO = @"
            SELECT DISTINCT 
                M.nome_municipio AS Municipio 
              FROM TabelaANP T 
              JOIN municipio M 
            WHERE T.ano = @Ano
            ORDER BY M.nome_municipio";

        public DadosAnp ObterPorValoresNota(int mes, int ano, string estado, string municipio, string produto)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Mes", mes, DbType.Int32);
            parametros.Add("@Ano", ano, DbType.Int32);
            parametros.Add("@Estado", estado, DbType.AnsiString);
            parametros.Add("@Municipio", municipio, DbType.AnsiString);
            parametros.Add("@Produto", produto, DbType.AnsiString);

            return Obter(SQL_OBTER_DADO_ANP, parametros);
        }

        public List<string> ListarMunicipiosAnpPorAno(int anoReferente)
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("@Ano", anoReferente, DbType.Int32);

            return Listar(SQL_LISTAR_MUNICIPIOS_ANP_POR_ANO, parametros).Select(m => m.Municipio).ToList();
        }

    }
}
