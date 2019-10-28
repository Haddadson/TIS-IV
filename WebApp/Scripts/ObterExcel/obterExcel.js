$(document).ready(function () {
    $("#obter-excel").on('click', function (event) {
        ValidarNotas.chamadaAjax({
            url: urlObterExcel,
            sucesso: function () {
                console.log("sucesso");
            },
            deveEsconderCarregando: true
        });
    });
});