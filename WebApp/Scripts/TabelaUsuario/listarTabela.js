$(document).ready(function () {

    ocultarTabelas();
    $("#tabela-nf-anp").show();

    $("#selecao-tipo-tabela").on("change", function (event) {
        let valorCampo = $("#selecao-tipo-tabela").val();
        ocultarTabelas();

        if (valorCampo === "nf-anp") 
            $("#tabela-nf-anp").show();

        else if (valorCampo === "cupom") 
            $("#tabela-cupom").show();

        else if (valorCampo === "outros")
            $("#tabela-outros").show();
        
    });

    function ocultarTabelas() {
        $("#tabela-nf-anp").hide();
        $("#tabela-cupom").hide();
        $("#tabela-outros").hide();
    }

});