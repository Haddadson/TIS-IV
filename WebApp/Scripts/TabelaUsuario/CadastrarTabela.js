$(document).ready(function () {
    $("#municipios-anp-div").hide();
    $("#anos-referentes").select2();
    VMasker(document.querySelector("#data-geracao")).maskPattern("99/99/9999");

    $("#anos-referentes").html(
        (() => {
            var anosOptions = '';
            var fiveYearsLater = moment().add('years', 5).format('YYYY');
            for (var i = 2013; i < fiveYearsLater; i++) {
                anosOptions += '<option value=' + i + '>' + i + '</option>';
            }
            return anosOptions;
        })()
    );

    validateNumericRequiredFormField("#sgdp", true, false, true);
    validateDateFormField("#data-geracao");
    setDefaultDate("#data-geracao");
    setReadOnly("#data-geracao");

    $("#check-disponibilidade-anp").on("click", validateANP);

    $("#cadastrar-tabela").on("click", function (event) {
        if (canSaveNewUserTable()) {
            const tabelaUsuarioData = {
                "SGDP": $("#sgdp").val(),
                "NomeMunicipio": $("#municipios").val(),
                "NomeMunicipioReferente": $("#municipios-anp").val(),
                "AnalistaResponsavel": $("#analista-resp").val(),
                "AnosReferentes": $("#anos-referentes").val(),
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
                    window.hasValidatedANP = false;
                    alert(retorno.Mensagem);
                    limparCampos();
                },
                erro: TratarErro,
                deveEsconderCarregando: false
            });
        }
    });

    const isInteger = value => {
        return /^\d+$/.test(value);
    };

    const canSaveNewUserTable = () => {
        let canSave = true;
        const anosReferentes = $("#anos-referentes").val();

        if (!isInteger($("#sgdp").val())) {
            alert('Favor preecher o campo SGDP somente com números!');
            canSave = false;
        } else if ( !$("#municipios").val() ) {
            alert('Favor preecher o campo de municípios!');
            canSave = false;
        } else if (!$("#titulo1").val()) {
            alert('Favor preecher o título da aba 1!');
            canSave = false;
        } else if (!$("#titulo2").val()) {
            alert('Favor preecher o título da aba 2!');
            canSave = false;
        } else if (!$("#titulo3").val()) {
            alert('Favor preecher o título da aba 3!');
            canSave = false;
        } else if (!anosReferentes || anosReferentes.length === 0) {
            alert('Favor selecionar o(s) ano(s) referente(s)!');
            canSave = false;
        } else if (!window.hasValidatedANP) {
            alert("É preciso checar a disponibilidade do município selecionado na ANP!");
            canSave = false;
        }

        return canSave;
    };


    function limparCampos() {
        $("#sgdp").val('');
        $("#municipios").val('');
        $("#municipios-anp").val('');
        $("#analista-resp").val('');
        $("#anos-referentes").val(null).trigger('change');
        $("#titulo1").val('');
        $("#titulo2").val('');
        $("#titulo3").val('');
        $("#municipios-anp-div").hide();
        setDefaultDate("#data-geracao");
    }

    function tratarMunicipios(response) {
        const municipioReferente = response.municipioReferente;
        const listaMunicipios = response.listaMunicipios;

        if (municipioReferente) {
            alert('O municipio existe na ANP!');
            $("#municipios-anp-div").hide();
            $('#municipios-anp').val(listaMunicipios[0]);
        } else {
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
        window.hasValidatedANP = true;
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
