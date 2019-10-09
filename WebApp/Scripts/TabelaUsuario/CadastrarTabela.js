$(document).ready(function () {    
    VMasker(document.querySelector("#data-geracao")).maskPattern("99/99/9999");

    validateNumericRequiredFormField("#sgdp");
    validateNumericRequiredFormField("#ano-referente");
    validateDateFormField("#data-geracao");
    setDefaultDate("#data-geracao");
    setReadOnly("#data-geracao");

    // Facilitador. Se não convier, apenas remover
    // Seta o Ano Referente, automaticamente como o ano atual.
    $("#ano-referente").val(moment().format("YYYY"));

    $("#cadastrar-tabela").on("click", function (event) {
        const tabelaUsuarioData = {
            "SGDP": $("#sgdp").val(),
            "AnoReferente": $("#ano-referente").val(),
            "dtGeracao": $("#data-geracao").val(),
            "titulo1": "",
            "titulo2": "",
            "titulo3": ""
        };

        ValidarNotas.chamadaAjax({
            url: urlTabelaUsuario,
            data: tabelaUsuarioData,
            sucesso: function () {
                console.log("sucesso");
            },
            deveEsconderCarregando: true
        });
    });
});