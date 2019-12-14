
// Load suggestions for COO field
const loadCOOsSuggestions = () => {
    const suggestionsData = {
        SGDP: $('#sgdp_escolhido').val()
    };

    ValidarNotas.chamadaAjax({
        url: urlObterSugestoesCOO,
        data: suggestionsData,
        sucesso: setSuggestionsForCOOField,
        deveEsconderCarregando: true
    });
};

const setSuggestionsForCOOField = response => {
    const { coos } = response;
    insertAutocompleteFeatureInElement(document.getElementById("coo_add"), coos);
};

const addCOOOnChangeHandler = evt => {
    const selectedCOO  = $("#coo_add").val();

    if (!selectedCOO) {
        alert('Por favor selecione um COO !');
    } else {
        const selectedCOOs = $("#cupons_selecionados").val();
        const allCOOs = selectedCOOs.split(/\s/);
        allCOOs.push(selectedCOO);
        $("#cupons_selecionados").val(allCOOs.join(' '));
    }
};

const initNotaFiscalFields = () => {
    VMasker(document.querySelector("#data_consulta_anp")).maskPattern("99/9999");
    VMasker(document.querySelector("#placa_veiculo")).maskPattern("AAA-9999");
    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#num_folha");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);
    validateCuponsFicais("#cupons_selecionados");
    validateDateFormField("#data_emissao");
    

    $("#adicionar_cupom").on("click", addCOOOnChangeHandler);

    $("#municipios").on("change", function (event) {
        const urlObterMunicipioPorNomeAno = window.urlObterMunicipioPorNomeAno;
        const municipioSelecionado = $("#municipios").val();

        ValidarNotas.chamadaAjax({
            url: urlObterMunicipioPorNomeAno,
            sucesso: tratarMunicipios,
            deveEsconderCarregando: true
        });
    });

    $("#cadastrar-nota-fiscal").on("click", function (event) {
        if ($("#numero_nf").val() == false || $("#data_emissao").val() == false || $("#combustivel1").val() == false ||
            $("#quantidade1").val() == false || $("#preco_unitario1").val() == false || $("#valor_total1").val() == false || $("#valor_total_nf").val() == false) {
            alert("Preencha todos campos obrigatórios!");
        }
        else {
            const notaFiscalData = {
                "SGDP": $("#sgdp_escolhido").val(),
                "NrNotaFiscal": $("#numero_nf").val(),
                "DataEmissao": $("#data_emissao").val(),
                "ValorTotal": parseFloat($("#valor_total_nf").val().replace(",", ".")),
                "CuponsSelecionados": $("#cupons_selecionados").val().split(" ").filter(cupom => cupom.length > 0),
                "NumeroFolha": $("#num_folha").val(),
                "DataConsultaANP": '01/' + $("#data_consulta_anp").val(),
                "Departamento": $("#departamento").val(),
                "Veiculo": $("#veiculo").val(),
                "PlacaVeiculo": $("#placa_veiculo").val().replace('-', '')
            };

            const itensNota = [
                {
                    "SGDP": $("#sgdp_escolhido").val(),
                    "Produto": $("#combustivel1").val(),
                    "Quantidade": $("#quantidade1").val(),
                    "ValorUnitario": parseFloat($("#preco_unitario1").val().replace(",", ".")),
                    "ValorTotal": parseFloat($("#valor_total1").val().replace(",", "."))
                },
                {
                    "SGDP": $("#sgdp_escolhido").val(),
                    "Produto": $("#combustivel2").val(),
                    "Quantidade": $("#quantidade2").val(),
                    "ValorUnitario": parseFloat($("#preco_unitario2").val().replace(",", ".")),
                    "ValorTotal": parseFloat($("#valor_total2").val().replace(",", "."))
                },
                {
                    "SGDP": $("#sgdp_escolhido").val(),
                    "Produto": $("#combustivel3").val(),
                    "Quantidade": $("#quantidade3").val(),
                    "ValorUnitario": parseFloat($("#preco_unitario3").val().replace(",", ".")),
                    "ValorTotal": parseFloat($("#valor_total3").val().replace(",", "."))
                },
                {
                    "SGDP": $("#sgdp_escolhido").val(),
                    "Produto": $("#combustivel4").val(),
                    "Quantidade": $("#quantidade4").val(),
                    "ValorUnitario": parseFloat($("#preco_unitario4").val().replace(",", ".")),
                    "ValorTotal": parseFloat($("#valor_total4").val().replace(",", "."))
                }
            ];

            const urlCadastrarNotaFiscal = window.urlCadastrarNotaFiscal;

            ValidarNotas.chamadaAjax({
                url: urlCadastrarNotaFiscal,
                data: { NotaFiscal: notaFiscalData, ItensNotaFiscal: itensNota },
                sucesso: function () {
                    alert("Salvo com sucesso!");
                    limparCampos();
                },
                deveEsconderCarregando: true
            });
        }
    });

    loadCOOsSuggestions();
};

$(document).ready(function () {
    initNotaFiscalFields();

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


    const autoSetValorTotal1 = evt => {
        const qtd = parseFloat($("#quantidade1").val().replace(',', '.'));
        const vrUnit = parseFloat($("#preco_unitario1").val().replace(',', '.'));

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit)) {
            $("#valor_total1").val(((parseFloat(qtd) * parseFloat(vrUnit))).toFixed(3).replace('.', ','));

            const vtotal1 = parseFloat($("#valor_total1").val().replace(',', '.'));
            const vtotal2 = parseFloat($("#valor_total2").val().replace(',', '.'));
            const vtotal3 = parseFloat($("#valor_total3").val().replace(',', '.'));
            const vtotal4 = parseFloat($("#valor_total4").val().replace(',', '.'));

            $("#valor_total_nf").val(((validaCampoValorNf(vtotal1) + validaCampoValorNf(vtotal2) + validaCampoValorNf(vtotal3) + validaCampoValorNf(vtotal4))).toFixed(3).replace('.', ','));
        }
    };

    const autoSetValorTotal2 = evt => {
        const qtd = parseFloat($("#quantidade2").val().replace(',', '.'));
        const vrUnit = parseFloat($("#preco_unitario2").val().replace(',', '.'));

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit)) {
            $("#valor_total2").val(((parseFloat(qtd) * parseFloat(vrUnit))).toFixed(3).replace('.', ','));


            const vtotal1 = parseFloat($("#valor_total1").val().replace(',', '.'));
            const vtotal2 = parseFloat($("#valor_total2").val().replace(',', '.'));
            const vtotal3 = parseFloat($("#valor_total3").val().replace(',', '.'));
            const vtotal4 = parseFloat($("#valor_total4").val().replace(',', '.'));

            $("#valor_total_nf").val(((validaCampoValorNf(vtotal1) + validaCampoValorNf(vtotal2) + validaCampoValorNf(vtotal3) + validaCampoValorNf(vtotal4))).toFixed(3).replace('.', ','));
        }
    };

    const autoSetValorTotal3 = evt => {
        const qtd = parseFloat($("#quantidade3").val().replace(',', '.'));
        const vrUnit = parseFloat($("#preco_unitario3").val().replace(',', '.'));

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit)) {
            $("#valor_total3").val(((parseFloat(qtd) * parseFloat(vrUnit))).toFixed(3).replace('.', ','));

        
            const vtotal1 = parseFloat($("#valor_total1").val().replace(',', '.'));
            const vtotal2 = parseFloat($("#valor_total2").val().replace(',', '.'));
            const vtotal3 = parseFloat($("#valor_total3").val().replace(',', '.'));
            const vtotal4 = parseFloat($("#valor_total4").val().replace(',', '.'));

            $("#valor_total_nf").val(((validaCampoValorNf(vtotal1) + validaCampoValorNf(vtotal2) + validaCampoValorNf(vtotal3) + validaCampoValorNf(vtotal4))).toFixed(3).replace('.', ','));
        }
    };

    const autoSetValorTotal4 = evt => {
        const qtd = parseFloat($("#quantidade4").val().replace(',', '.'));
        const vrUnit = parseFloat($("#preco_unitario4").val().replace(',', '.'));

        if (!Number.isNaN(qtd) && !Number.isNaN(vrUnit)) {
            $("#valor_total4").val(((parseFloat(qtd) * parseFloat(vrUnit))).toFixed(3).replace('.', ','));


            const vtotal1 = parseFloat($("#valor_total1").val().replace(',', '.'));
            const vtotal2 = parseFloat($("#valor_total2").val().replace(',', '.'));
            const vtotal3 = parseFloat($("#valor_total3").val().replace(',', '.'));
            const vtotal4 = parseFloat($("#valor_total4").val().replace(',', '.'));

            $("#valor_total_nf").val(((validaCampoValorNf(vtotal1) + validaCampoValorNf(vtotal2) + validaCampoValorNf(vtotal3) + validaCampoValorNf(vtotal4))).toFixed(3).replace('.', ','));
        }
    };

    const autoSetValorTotalNf = evt => {
        const vtotal1 = parseFloat($("#valor_total1").val().replace(',', '.'));
        const vtotal2 = parseFloat($("#valor_total2").val().replace(',', '.'));
        const vtotal3 = parseFloat($("#valor_total3").val().replace(',', '.'));
        const vtotal4 = parseFloat($("#valor_total4").val().replace(',', '.'));

        $("#valor_total_nf").val(((validaCampoValorNf(vtotal1) + validaCampoValorNf(vtotal2) + validaCampoValorNf(vtotal3) + validaCampoValorNf(vtotal4))).toFixed(3).replace('.', ','));
    };

    $("#quantidade1, #preco_unitario1").on("change", autoSetValorTotal1);
    $("#quantidade2, #preco_unitario2").on("change", autoSetValorTotal2);
    $("#quantidade3, #preco_unitario3").on("change", autoSetValorTotal3);
    $("#quantidade4, #preco_unitario4").on("change", autoSetValorTotal4);

    $("#valor_total1, #valor_total2, #valor_total3, #valor_total4").on("change", autoSetValorTotalNf);

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

function validaCampoValorNf(valTotal) {
    var parsedV = 0;
    if (Number.isNaN(valTotal))
        parsedV = 0;
    else
        parsedV = valTotal;
    return parseFloat(parsedV);
}

function limparCampos() {
    $("#numero_nf, #posto_fornecedor, #data_emissao, #valor_total_nf, #coo," + 
      "#num_folha, #data_consulta_anp, #departamento, #veiculo," +
      "#placa_veiculo, .itens-field, #coo_add, #cupons_selecionados").val('');
}

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
