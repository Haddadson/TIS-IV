$(document).ready(function () {

    ocultarTabelas();
    $(".data-table-usuario").hide();

    let valorCampoSgdp = $("#selecao-tabela-sgdp").val();
    $(`#${valorCampoSgdp}`).show();
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

    $("#selecao-tabela-sgdp").on("change", function (event) {
        let valorCampo = $("#selecao-tabela-sgdp").val();
        $(".data-table-usuario").hide();
        $(`#${valorCampo}`).show();
    });

    function ocultarTabelas() {
        $("#tabela-nf-anp").hide();
        $("#tabela-cupom").hide();
        $("#tabela-outros").hide();
    }

});