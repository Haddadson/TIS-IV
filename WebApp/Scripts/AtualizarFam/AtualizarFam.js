$(document).ready(function () {

    $("#atualizar-tabela-anp").on("click", function (e) {
        ValidarNotas.chamadaAjax({
            url: urlAtualizarTabelaAnp,
            sucesso: tratarSucesso,
            deveEsconderCarregando: true,
            deveEsconderCarregandoBloqueado: false
        });
    });

    $("#cadastrar-tabela-fam").on("click", function (e) {

        if ($("#fam-file").prop('files')) {

            var fileReader = new FileReader();
            fileReader.onload = function () {
                var data = fileReader.result;
                if (!data) {
                    alert("Por favor, selecione um arquivo Excel (.xlsx ou .xls)");
                }

                else {

                    let arquivoFam = data;
                    let nomeArquivoFam = $("#fam-file").prop('files')[0].name;
                    let tipoArquivoFam = $("#fam-file").prop('files')[0].type;
                    let extensaoArquivoFam = "";

                    if (tipoArquivoFam === "application/vnd.ms-excel") {
                        extensaoArquivoFam = ".xls";
                    }
                    else if (tipoArquivoFam === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        extensaoArquivoFam = ".xlsx";
                    }

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
                        deveEsconderCarregando: true,
                        deveEsconderCarregandoBloqueado: false
                    });
                }
            };
            fileReader.readAsDataURL($('#fam-file').prop('files')[0]);
        }
        else {
            alert("Por favor, selecione um arquivo Excel (.xlsx ou .xls)");
            $("#fam-file").val('');
        }

    });

    function tratarSucesso(response) {
        if (response && response.sucesso && response.mensagem) {
            alert(response.mensagem);
            $("#fam-file").val('');
        }
        else if (response && !response.sucesso) {
            if (response.mensagem) {
                alert(response.mensagem);
            }
            if (response.erro) {
                console.log(response.erro);
            }
        }
    }

});