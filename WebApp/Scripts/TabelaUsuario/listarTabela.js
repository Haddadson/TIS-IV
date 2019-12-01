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

});