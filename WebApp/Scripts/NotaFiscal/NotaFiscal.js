const initNotaFiscalFields = () => {
    /*    VMasker(document.querySelector("#preco_unitario")).maskMoney({
        precision: 2,
        separator: ',',
        delimiter: '.',
        unit: 'R$'
    });

    VMasker(document.querySelector("#valor_total")).maskMoney({
        precision: 2,
        separator: ',',
        delimiter: '.',
        unit: 'R$'
    }); */

    VMasker(document.querySelector("#placa_veiculo")).maskPattern("AAA-9999");
    VMasker(document.querySelector("#data_consulta_anp")).maskPattern("99/9999");

    $("#sgdp").val(1);

    validateNumericRequiredFormField("#numero_nf");
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
            "SGDP"                  : $("sgdp").val(),
            "NrNotaFiscal"          : $("numero_nf").val(),
            "ChaveAcesso"           : $("chave_acesso").val(),
            "Posto"                 : $("posto").val(),
            "DataEmissao"           : $("data_emissao").val(),
            "Combustível"           : $("combustivel").val(),
            "Quantidade"            : $("quantidade").val(),
            "PrecoUnitario"         : $("preco_unitario").val(),
            "ValorTotal"            : $("valor_total").val(),
            "CuponsSelecionados"    : $("cupons_selecionados").val(),
            "NumeroFolha"           : $("num_folha").val(),
            "DataConsultaANP"       : $("data_consulta_anp").val(),
            "Cliente"               : $("cliente").val(),
            "Departamento"          : $("departamento").val(),
            "Veiculo"               : $("veiculo").val(),
            "PlacaVeiculo"          : $("placa_veiculo").val(),
            "Hodometro"             : $("hodometro").val()
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

    const autoSetValorTotal = evt => {
        const qtd = parseFloat($("#quantidade").val());
        const vrUnit = parseFloat($("#preco_unitario").val());

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit))
            $("#valor_total").val(parseFloat(qtd * vrUnit).toFixed(3));
    };

    $("#quantidade").on("change", autoSetValorTotal);
    $("#preco_unitario").on("change", autoSetValorTotal);
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