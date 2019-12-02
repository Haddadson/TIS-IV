const initNotaFiscalFields = () => {
    VMasker(document.querySelector("#placa_veiculo")).maskPattern("AAA-9999");
    VMasker(document.querySelector("#data_consulta_anp")).maskPattern("99/9999");

    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#num_folha");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);

    validateCuponsFicais("#cupons_selecionados");

    $("#data_emissao")[0].value = moment().format('YYYY-MM-DD');

    validateDateFormField("#data_emissao");

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
        const notaFiscalData = {
            "SGDP"                  : $("#sgdp").val(),
            "NrNotaFiscal"          : $("#numero_nf").val(),
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

    const autoSetValorTotal = evt => {
        const qtd = parseFloat($("#quantidade").val().replace(',', '.'));
        const vrUnit = parseFloat($("#preco_unitario").val().replace(',', '.'));

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit))
            $("#valor_total").val(((parseFloat(qtd) * parseFloat(vrUnit))).toFixed(3).replace('.', ','));
    };

    $("#quantidade").on("change", autoSetValorTotal);
    $("#preco_unitario").on("change", autoSetValorTotal);
    
    const urlObterDepartamentos = window.urlObterDepartamentos;

    ValidarNotas.chamadaAjax({
        url: urlObterDepartamentos,
        sucesso: (response) => {
            response.departamentos.forEach((dpto) => {
                $("#departamento").append('<option value=' + dpto.Codigo + '>' + dpto.Nome + '</option>');
            });
        },
        deveEsconderCarregando: true
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