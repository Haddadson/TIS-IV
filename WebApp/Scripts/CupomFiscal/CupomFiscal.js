const initNotaFiscalFields = () => {
    VMasker(document.querySelector("#placa_veiculo")).maskPattern("AAA-9999");
    
    //$("#sgdp").val(1);
    //setReadOnly("#sgdp");

    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#hodometro");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);

    $("#data_cupom")[0].value = moment().format('YYYY-DD-MMTHH:M');
};


$(document).ready(function () {
    initNotaFiscalFields();

    $("#cadastrar-cupom-fiscal").on("click", function (event) {
        const cupomFiscalData = {
            "SGDP": $("#sgdp").val(),
            "NrNotaFiscal": $("#numero_nf").val(),
            "COO": $("#coo").val(),
            "Posto": $("#posto_fornecedor").val(),
            "Data": $("#data_cupom").val(),
            "Combustivel": $("#combustivel").val(),
            "Quantidade": $("#quantidade").val(),
            "PrecoUnitario": parseFloat($("#preco_unitario").val().replace(",", ".")),
            "ValorTotal": parseFloat($("#valor_total").val().replace(",", ".")),
            "Cliente": $("#cliente").val(),
            "Hodometro": $("#hodometro").val(),
            "Veiculo": $("#veiculo").val(),
            "PlacaVeiculo": $("#placa_veiculo").val().replace('-', '')
        };

        const urlCadastrarCupomFiscal = window.urlCadastrarCupomFiscal;

        ValidarNotas.chamadaAjax({
            url: urlCadastrarCupomFiscal,
            data: cupomFiscalData,
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