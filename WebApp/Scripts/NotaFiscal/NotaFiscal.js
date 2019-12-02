const initNotaFiscalFields = () => {
    VMasker(document.querySelector("#data_consulta_anp")).maskPattern("99/9999");

    //$("#sgdp").val(1);

    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#num_folha");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);

    validateCuponsFicais("#cupons_selecionados");

    validateDateFormField("#data_emissao");
    //setReadOnly("#sgdp");

};


$(document).ready(function () {
    initNotaFiscalFields();
    $("#municipios").on("change", function (event) {
        const urlObterMunicipioPorNomeAno = window.urlObterMunicipioPorNomeAno;
        const municipioSelecionado = $("#municipios").val();

        ValidarNotas.chamadaAjax({
            url: urlObterMunicipioPorNomeAno,
            sucesso: tratarMunicipios,
            deveEsconderCarregando: true
        });
    });

    const tratarMunicipios = response => {

        const listaMunicipios = response && response.listaMunicipios.filter((m) => { return m === response.municipioSelecionado; });
        const municipioReferente = response && response.municipioReferente;
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
    };

    $("#cadastrar-nota-fiscal").on("click", function (event) {
        if ($("#numero_nf").val() == false || $("#data_emissao").val() == false || $("#combustivel1").val() == false ||
            $("#quantidade1").val() == false || $("#preco_unitario1").val() == false || $("#valor_total1").val() == false || $("#valor_total_nf").val() == false) {
            alert("Preencha todos campos obrigatórios!");
        }
        else {
            const notaFiscalData = {
                "SGDP": $("#sgdp").val(),
                "NrNotaFiscal": $("#numero_nf").val(),
                "Posto": $("#posto_fornecedor").val(),
                "DataEmissao": $("#data_emissao").val(),
                "Combustivel": $("#combustivel").val(),
                "Quantidade": $("#quantidade").val(),
                "PrecoUnitario": parseFloat($("#preco_unitario").val().replace(",", ".")),
                "ValorTotal": parseFloat($("#valor_total").val().replace(",", ".")),
                "CuponsSelecionados": $("#cupons_selecionados").val().split(" ").filter(cupom => cupom.length > 0),
                "NumeroFolha": $("#num_folha").val(),
                "DataConsultaANP": '01/' + $("#data_consulta_anp").val(),
                "Departamento": $("#departamento").val(),
                "Veiculo": $("#veiculo").val(),
                "PlacaVeiculo": $("#placa_veiculo").val().replace('-', '')
            };
            const urlCadastrarNotaFiscal = window.urlCadastrarNotaFiscal;

            ValidarNotas.chamadaAjax({
                url: urlCadastrarNotaFiscal,
                data: notaFiscalData,
                sucesso: function () {
                    alert("Salvo com sucesso!");
                },
                deveEsconderCarregando: true
            });
        }
    });

    $("#departamento").on("change", function (event) {
        // Get Departamentos.
    });

    const autoSetValorTotal = evt => {
        const qtd = parseFloat($("#quantidade").val().replace(',', '.'));
        const vrUnit = parseFloat($("#preco_unitario").val().replace(',', '.'));

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit))
            $("#valor_total").val(((parseFloat(qtd) * parseFloat(vrUnit))).toFixed(3).replace('.', ','));
    };

    $("#quantidade").on("change", autoSetValorTotal);
    $("#preco_unitario").on("change", autoSetValorTotal);
    $("#chave_acesso").on("change", function (evt) {
        const value = $("#chave_acesso").val().split(" ").join("");

        if (value.length !== 44) {
            setInvalidField("#chave_acesso");
        } else {
            cleanInvalidField("#chave_acesso");
        }
    });

});

const validateCuponsFicais = fieldCssQuerySelector => {
    $(fieldCssQuerySelector).on("change", (evt) => {
        const value = $(fieldCssQuerySelector).val();
        if (/^[^A-Za-z]+$/.test(value)) {
            cleanInvalidField(fieldCssQuerySelector);
        } else {
            setInvalidField(fieldCssQuerySelector);
        }
    });
};
