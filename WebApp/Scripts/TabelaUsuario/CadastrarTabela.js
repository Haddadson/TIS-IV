$(document).ready(function () {
    $("#municipios-anp-div").hide();
    VMasker(document.querySelector("#data-geracao")).maskPattern("99/99/9999");

    validateNumericRequiredFormField("#sgdp");
    validateNumericRequiredFormField("#ano-referente");
    validateDateFormField("#data-geracao");
    setDefaultDate("#data-geracao");
    setReadOnly("#data-geracao");

    $("#check-disponibilidade-anp").on("click", validateANP);

    $("#cadastrar-tabela").on("click", function (event) {

        const tabelaUsuarioData = {
            "SGDP": $("#sgdp").val(),
            "NomeMunicipio": $("#municipios").val(),
            "NomeMunicipioReferente": $("#municipios-anp").val(),
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
            sucesso: function (retorno) {
                alert(retorno.Mensagem);
                limparCampos();
            },
            erro: TratarErro,
            deveEsconderCarregando: false
        });
    });

    function limparCampos() {
        $("#sgdp").val('');
        $("#municipios").val('');
        $("#municipios-anp").val('');
        $("#analista-resp").val('');
        $("#ano-referente").val('');
        $("#titulo1").val('');
        $("#titulo2").val('');
        $("#titulo3").val('');
    }

    function tratarMunicipios(response) {
        const listaMunicipios = response.listaMunicipios;

        if (listaMunicipios && listaMunicipios.length > 0) {
            const listaMunicipios = response.listaMunicipios.filter((m) => { return m === response.municipioSelecionado; });
            const municipioReferente = response && response.listaMunicipios && response.municipioReferente;
            if (municipioReferente) {
                $('#municipios-anp').html = '';
                $('#municipios-anp').append(`<option val='${municipioReferente.Codigo}'>${municipioReferente.Nome}</option>`);
                $('#municipios-anp').val(municipioReferente.Nome);
                $("#municipios-anp").prop('disabled', true);
                $("#municipios-anp-div").show();
            }
            else {
                $('#municipios-anp').html = '';
                $('#municipios-anp').append(response.listaMunicipios.map(m => {
                    return "<option>" + m + "</option>";
                }));
            }
            if (!municipioReferente && !(listaMunicipios.length === 1)) {
                alert("O município selecionado não possui dados na tabela da ANP para o período selecionado. Favor escolher um município.");
                $("#municipios-anp-div").show();
            }
            else if (!municipioReferente) {
                $("#municipios-anp-div").hide();
                $('#municipios-anp').val(listaMunicipios[0]);
            }
        }        
    }

    function TratarErro(retorno) {
        if (retorno && retorno.Mensagem)
            alert(retorno.Mensagem);
        else
            alert("Ocorreu um erro ao cadastrar");
    }

    function validateANP (event) {
        const municipioSelecionado = $("#municipios").val();
        const anosSelecionados = $("#anos-referentes").val();

        if (municipioSelecionado && (anosSelecionados.length > 0)) {
            const urlObterMunicipioPorNomeAno = window.urlObterMunicipioPorNomeAno;

            ValidarNotas.chamadaAjax({
                url: urlObterMunicipioPorNomeAno,
                data: { anosReferentes: anosSelecionados, nomeMunicipio: municipioSelecionado },
                sucesso: tratarMunicipios,
                deveEsconderCarregando: false
            });
        }
    }
});
