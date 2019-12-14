$(document).ready(function () {

    ocultarTabelas();

    let valorCampoSgdp = $("#sgdp_escolhido").val();
    $("#tabela-nf-anp").show();

    $("#selecao-tipo-tabela").on("change", function (event) {
        let valorCampo = $("#selecao-tipo-tabela").val();
        ocultarTabelas();

        if (valorCampo === "nf-anp")
            $(`#${valorCampoSgdp} #tabela-nf-anp`).show();

        else if (valorCampo === "cupom")
            $(`#${valorCampoSgdp} #tabela-cupom`).show();

        else if (valorCampo === "outros")
            $(`#${valorCampoSgdp} #tabela-outros`).show();

    });

    function ocultarTabelas() {
        $("#tabela-nf-anp").hide();
        $("#tabela-cupom").hide();
        $("#tabela-outros").hide();
    }

    $("#exportar-excel").on("click", function (e) {
        let listaTabelaAnpxNota = [];
        let listaCuponsFiscais = [];
        let listaOutrasInformacoes = [];

        let dadosTabela = {
            "Sgdp": $("#export-sgdp").html(),
            "Municipio": $("#export-municipio").html(),
            "MunicipioReferente": $("#export-municipio-ref").html(),
            "AnalistaResponsavel": $("#export-analista").html(),
            "DataGeracao": $("#export-data-geracao").html(),
            "AnosReferentes": $("#export-anos-referentes").html(),
            "Titulo1": $("#export-titulo-1").html(),
            "Titulo2": $("#export-titulo-2").html(),
            "Titulo3": $("#export-titulo-3").html(),
            "MesFam": $("#export-mes-fam").html(),
            "AnoFam": $("#export-ano-fam").html()
        };

        let dadosAnpxNota = $("#data-table-nf-anp").DataTable().rows().data();
        let dadosCuponsFiscais = $("#data-table-cupom").DataTable().rows().data();
        let dadosOutrasInformacoes = $("#data-table-outros").DataTable().rows().data();

        for (let i = 0; i < dadosAnpxNota.length; i++) {
            let dado = {
                "DataGeracao": dadosAnpxNota[i][0],
                "NumeroNotaFiscal": dadosAnpxNota[i][1],
                "Produto": dadosAnpxNota[i][2],
                "Quantidade": dadosAnpxNota[i][3],
                "ValorUnitario": dadosAnpxNota[i][4],
                "ValorTotalItem": dadosAnpxNota[i][5],
                "ValorTotalNota": dadosAnpxNota[i][6],
                "NumeroFolha": dadosAnpxNota[i][7],
                "ValorFam": dadosAnpxNota[i][8],
                "PrecoMedioAnp": dadosAnpxNota[i][9],
                "DiferencaMediaUnitaria": dadosAnpxNota[i][10],
                "DiferencaMediaTotal": dadosAnpxNota[i][11],
                "ValorMedioAtualizado": dadosAnpxNota[i][12],
                "PrecoMaximoAnp": dadosAnpxNota[i][13],
                "DiferencaMaximaUnitaria": dadosAnpxNota[i][14],
                "DiferencaMaximaTotal": dadosAnpxNota[i][15],
                "ValorMaximoAtualizado": dadosAnpxNota[i][16],
                "CuponsFiscaisVinculados": dadosAnpxNota[i][17],
                "MesAnp": dadosAnpxNota[i][18] && dadosAnpxNota[i][18].split('/')[0] ,
                "AnoAnp": dadosAnpxNota[i][18] && dadosAnpxNota[i][18].split('/')[1]
            };
            listaTabelaAnpxNota.push(dado);
        }

        for (let i = 0; i < dadosCuponsFiscais.length; i++) {
            let dado = {
                "NumeroNotaFiscal": dadosCuponsFiscais[i][0],
                "PostoReferente": dadosCuponsFiscais[i][1],
                "DataEmissao": dadosCuponsFiscais[i][2],
                "HoraEmissao": dadosCuponsFiscais[i][3],
                "Coo": dadosCuponsFiscais[i][4],
                "Produto": dadosCuponsFiscais[i][5],
                "Quantidade": dadosCuponsFiscais[i][6],
                "PrecoUnitario": dadosCuponsFiscais[i][7],
                "ValorTotal": dadosCuponsFiscais[i][8],
                "Cliente": dadosCuponsFiscais[i][9],
                "Veiculo": dadosCuponsFiscais[i][10],
                "PlacaVeiculo": dadosCuponsFiscais[i][11],
                "Hodometro": dadosCuponsFiscais[i][12]
            };
            listaCuponsFiscais.push(dado);
        }


        for (let i = 0; i < dadosOutrasInformacoes.length; i++) {
            let dado = {
                "NumeroNotaFiscal": dadosOutrasInformacoes[i][0],
                "CuponsFiscaisVinculados": dadosOutrasInformacoes[i][1],
                "NomeDepartamento": dadosOutrasInformacoes[i][2],
                "Veiculo": dadosOutrasInformacoes[i][3],
                "PlacaVeiculo": dadosOutrasInformacoes[i][4],
                "Produto": dadosOutrasInformacoes[i][5],
                "ValorTotalNota": dadosOutrasInformacoes[i][6],
                "DiferencaMediaTotal": dadosOutrasInformacoes[i][7],
                "ValorMedioAtualizado": dadosOutrasInformacoes[i][8],
                "DiferencaMaximaTotal": dadosOutrasInformacoes[i][9],
                "ValorMaximoAtualizado": dadosOutrasInformacoes[i][10]
            };
            listaOutrasInformacoes.push(dado);
        }

        parametros = {
            "DadosTabela": dadosTabela,
            "ListaTabelaAnpxNota": listaTabelaAnpxNota,
            "ListaCuponsFiscais": listaCuponsFiscais,
            "ListaOutrasInformacoes": listaOutrasInformacoes
        };

        ValidarNotas.chamadaAjax({
            url: exportarTabelasParaExcel,
            data: { ...parametros },
            sucesso: baixarExcelExportado,
            deveEsconderCarregando: true,
            deveEsconderCarregandoBloqueado: false
        });

    });

    function baixarExcelExportado(retorno) {
        if (retorno && retorno.Sucesso) {
            window.location = efetuarDownloadExcel + '?identificadorArquivo=' + retorno.IdentificadorArquivo;
        } else if (!retorno.Sucesso && !retorno.Mensagem) {
            alert(retorno.Mensagem);
        } else {
            alert("Ocorreu um erro inesperado");
        }
    }
});