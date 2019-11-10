$(document).ready(function () {
    $("#municipios-anp-div").hide();
    VMasker(document.querySelector("#data-geracao")).maskPattern("99/99/9999");

    validateNumericRequiredFormField("#sgdp");
    validateNumericRequiredFormField("#ano-referente");
    validateDateFormField("#data-geracao");
    setDefaultDate("#data-geracao");
    setReadOnly("#data-geracao");

    // Facilitador. Se não convier, apenas remover
    // Seta o Ano Referente, automaticamente como o ano atual.
    $("#ano-referente").val(moment().format("YYYY"));

    $(".municipios-anp-busca").on("change", function (event) {
        const municipioSelecionado = $("#municipios").val();
        const anoSelecionado = $("#ano-referente").val();

        if (municipioSelecionado && anoSelecionado) {

            const urlListarMunicipios = window.urlListarMunicipios;

            ValidarNotas.chamadaAjax({
                url: urlListarMunicipios,
                data: { anoReferente: anoSelecionado },
                sucesso: function (response) {
                    const municipio = response.municipios.filter((m) => { return m == municipioSelecionado; });

                    $('#municipios-anp').html = '';
                    $('#municipios-anp').append(response.municipios.map(m => {
                        return "<option>" + m + "</option>";
                    }));

                    if (!(municipio.length === 1)) {
                        alert("O município selecionado não possui dados na tabela da ANP para o período selecionado. Favor escolher um município.");
                        $("#municipios-anp-div").show();
                    } else {
                        $("#municipios-anp-div").hide();
                        $('#municipios-anp').val(municipio[0]);
                    }
                },
                deveEsconderCarregando: false
            });
        }

    });

    $("#cadastrar-tabela").on("click", function (event) {
        const tabelaUsuarioData = {
            "SGDP": $("#sgdp").val(),
            "IdMunicipio": $("#municipios").val(),
            "IdMunicipioReferente": 1, // $("#municipios-anp").val(),
            "AnalistaResponsavel": $("#analista-resp").val(),
            "AnoReferente": $("#ano-referente").val(),
            "DataGeracao": $("#data-geracao").val(),
            "Titulo1": $("#titulo1").val(),
            "Titulo2": $("#titulo2").val(),
            "Titulo3": $("#titulo3").val()
        };
        const urlTabelaUsuario = window.urlCadastrarTabelaUsuario;

        ValidarNotas.chamadaAjax({
            url: urlTabelaUsuario,
            data: tabelaUsuarioData,
            sucesso: function () {
                console.log("sucesso");
                alert("Cadastrado com sucesso!");
                limparCampos();
            },
            deveEsconderCarregando: false
        });
    });

    function limparCampos() {
        $("#sgdp").val('');
        $("#municipios").val('');
        $("#municipios-anp").val('');
        $("#analista-resp").val('');
        $("#ano-referente").val('');
        $("#data-geracao").val('');
        $("#titulo1").val('');
        $("#titulo2").val('');
        $("#titulo3").val('');
    }

});