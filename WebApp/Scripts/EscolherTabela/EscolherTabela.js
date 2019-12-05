$(document).ready(function () {     

    var switchRoutes = (id) => {
        if (id === 'visualizar_tabela') {
            return window.urlRedirecionarVisualizarTabela;
        } else if (id === 'cadastrar_nota_fiscal') {
            return window.urlRedirecionarCadastrarCupom;
        } else if (id === 'cadastrar_cupom_fiscal') {
            return window.urlCadastrarNotaFiscal;
        }
    };

    $("#visualizar_tabela, #cadastrar_nota_fiscal, #cadastrar_cupom_fiscal").on("click", function (event) {
        let valorSgdp = $("#sgdp").val();

        if (valorSgdp) {
            const urlRedirecionarVisualizarTabela = switchRoutes(event.currentTarget.id);

            ValidarNotas.chamadaAjax({
                url: urlRedirecionarVisualizarTabela,
                data: { valorSgdp: valorSgdp },
                sucesso: function (response) {
                    window.location.href = response.urlRedirecionamento;
                },
                deveEsconderCarregando: true
            });
        } else {
            alert("Selecione um SGDP");
        }
    });

});