const initNotaFiscalFields = () => {
    VMasker(document.querySelector("#placa_veiculo")).maskPattern("AAA-9999");
    VMasker(document.querySelector("#data_consulta_anp")).maskPattern("99/9999");
    VMasker(document.querySelector("#chave_acesso")).maskPattern("9999 9999 9999 9999 9999 9999 9999 9999 9999 9999 9999");

    $("#sgdp").val(1);

    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#num_folha");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);

    validateCuponsFicais("#cupons_selecionados");

    $("#data_emissao")[0].value = moment().format('YYYY-MM-DD');

    validateDateFormField("#data_emissao");
    setReadOnly("#sgdp");

};


$(document).ready(function () {
    initNotaFiscalFields();
    $("#municipios").on("change", function (event) {
        const urlListarMunicipios = window.urlListarMunicipios;
        const municipioSelecionado = $("#municipios").val();

        ValidarNotas.chamadaAjax({
            url: urlListarMunicipios,
            sucesso: function (response) {
                const municipio = response.municipios.filter((m) => { m == municipioSelecionado });

                $('#municipios-anp').html = '';
                $('#municipios-anp').append(response.municipios.map(m => {
                    return "<option>" + m + "</option>";
                }));

                if (!(municipio.length === 1)) {
                    alert("O município selecionado não possui dados na tabela da ANP para o período selecionado. Favor escolher um município.");
                    $("#municipios-anp-div").removeClass("display-none");
                } else {
                    $('#municipios-anp').val(municipio[0]);
                }
            },
            deveEsconderCarregando: true
        });
    });

    $("#cadastrar-nota-fiscal").on("click", function (event) {
        const notaFiscalData = {
            "SGDP"                  : $("#sgdp").val(),
            "NrNotaFiscal"          : $("#numero_nf").val(),
            "ChaveAcesso"           : $("#chave_acesso").val().split(" ").join(""),
            "Posto"                 : $("#posto").val(),
            "DataEmissao"           : $("#data_emissao").val(),
            "Combustivel"           : $("#combustivel").val(),
            "Quantidade"            : $("#quantidade").val(),
            "PrecoUnitario"         : parseFloat($("#preco_unitario").val().replace(",", ".")),
            "ValorTotal":           parseFloat($("#valor_total").val().replace(",", ".")),
            "CuponsSelecionados": $("#cupons_selecionados").val().split(" ").filter(cupom => cupom.length > 0),
            "NumeroFolha"           : $("#num_folha").val(),
            "DataConsultaANP"       : '01/' + $("#data_consulta_anp").val(),
            "Departamento"          : $("#departamento").val(),
            "Veiculo": $("#veiculo").val(),
            "PlacaVeiculo": $("#placa_veiculo").val().replace('-','')
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