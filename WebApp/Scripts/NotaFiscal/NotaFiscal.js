const initNotaFiscalFields = () => {
    VMasker(document.querySelector("#data_consulta_anp")).maskPattern("99/9999");
    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#num_folha");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);
    validateCuponsFicais("#cupons_selecionados");
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
        if ($("#numero_nf").val() == false || $("#data_emissao").val() == false || $("#combustivel1").val() == false ||
            $("#quantidade1").val() == false || $("#preco_unitario1").val() == false || $("#valor_total1").val() == false || $("#valor_total_nf").val() == false) {
            alert("Preencha todos campos obrigatórios!");
        }
        else {
            const notaFiscalData = {
                "SGDP": $("#sgdp_escolhido").val(),
                "NrNotaFiscal": $("#numero_nf").val(),
                "Posto": $("#posto_fornecedor").val(),
                "DataEmissao": $("#data_emissao").val(),
                "ValorTotal": parseFloat($("#valor_total_nf").val().replace(",", ".")),

                //"CuponsSelecionados": $("#cupons_selecionados").val().split(" ").filter(cupom => cupom.length > 0),
                "CuponsSelecionados": $("#coo").val(),
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

function limparCampos() {
    $("#numero_nf").val('');
    $("#posto_fornecedor").val('');
    $("#data_emissao").val('');
    $("#valor_total_nf").val('');
    $("#coo").val('');
    $("#num_folha").val('');
    $("#data_consulta_anp").val('');
    $("#departamento").val('');
    $("#veiculo").val('');
    $("#placa_veiculo").val('');
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
