$(document).ready(function () {

    $("#cadastrar-tabela-fam").on("click", function (e) {

        if ($("#'fam-file").prop('src').length == 0) {
            alert("Por favor, selecione um arquivo Excel (.xlsx ou .xls)");
        }
        else {

            let arquivoFam = $("#'fam-file").prop('src');
            let nomeArquivoFam = $("#fam-file").val();
            let tipoArquivoFam = $("#fam-file")[0].files[0].type;
            let extensaoArquivoFam = tipoArquivoFam.split("/")[1];

            parametros = {
                arquivoFam,
                nomeArquivoFam,
                tipoArquivoFam,
                extensaoArquivoFam
            };

            ValidarNotas.chamadaAjax({
                url: urlAtualizarTabelaFam,
                data: parametros,
                sucesso: tratarSucesso,
                deveEsconderCarregando: false
            });
        }

        function tratarSucesso(response) {
            if (response && response.sucesso) {
                alert("Cadastrado com sucesso");
            }
        }

    });

});