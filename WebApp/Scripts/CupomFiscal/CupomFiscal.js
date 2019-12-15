const initCupomFiscalFields = () => {
    VMasker(document.querySelector("#placa_veiculo")).maskPattern("AAA-9999");

    validateNumericRequiredFormField("#numero_nf", true, true);
    validateNumericRequiredFormField("#quantidade", true, true);
    validateNumericRequiredFormField("#hodometro");
    validateNumericRequiredFormField("#valor_total");
    validateNumericRequiredFormField("#preco_unitario", false, true);

    $("#data_emissao").datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        maxDate: moment([], '').format('L LT'),
        defaultDate: null
    });

    $("#data_emissao_value").val(null).trigger('change');
};


$(document).ready(function () {
    initCupomFiscalFields();
    const isInteger = value => (/^\d+$/.test(value));

    $("#cadastrar-cupom-fiscal").on("click", function (event) {
        let canSave = true;
        const coo = $("#coo").val();
        const quantidade = $("#quantidade").val();
        const nf = $("#numero_nf").val();
        const hodometro = $("#hodometro").val();

        if (!coo && !isInteger(coo) ) {
            canSave = false;
            alert('Favor preencher o campo COO adequadamente.');
        } else if ($("#data_emissao_value").val() == false) {
            canSave = false;
            alert('Favor selecionar uma data de emissão para o cupom selecionado.');
        } else if ($("#combustivel").val() == false) {
            canSave = false;
            alert('Favor selecionar um combustível.');
        } else if (quantidade == false || !isInteger(quantidade)) {
            canSave = false;
            alert('Favor informar a quantidade!');
        } else if ($("#preco_unitario").val() == false) {
            canSave = false;
            alert('Favor informar o preco unitário!');
        } else if ($("#valor_total").val() == false ) {
            canSave = false;
            alert('Favor informar o valor total do cupom!');
        } else if (nf != '' && !isInteger(nf)) {
            canSave = false;
            alert('O valor da nota fiscal está incorreto!');
        } else if (hodometro != '' && !isInteger(hodometro)) {
            canSave = false;
            alert('O valor do hodômetro está incorreto!');
        }

        if (canSave) {
            const cupomFiscalData = {
                "SGDP": $("#sgdp_escolhido").val(),
                "NrNotaFiscal": $("#numero_nf").val(),
                "COO": $("#coo").val(),
                "Data": $("#data_emissao_value").val(),
                "Combustivel": $("#combustivel").val(),
                "Quantidade": $("#quantidade").val(),
                "PrecoUnitario": parseFloat($("#preco_unitario").val().replace(",", ".")),
                "ValorTotal": parseFloat($("#valor_total").val().replace(",", ".")),
                "Hodometro": $("#hodometro").val(),
                "Veiculo": $("#veiculo").val(),
                "PlacaVeiculo": $("#placa_veiculo").val().replace('-', '')
            };

            const urlCadastrarCupomFiscal = window.urlCadastrarCupomFiscal;

            ValidarNotas.chamadaAjax({
                url: urlCadastrarCupomFiscal,
                data: cupomFiscalData,
                sucesso: function (response) {
                    if (response && response.Sucesso) {
                        alert(response.Mensagem ? response.Mensagem : "Sucesso ao cadastrar!");
                        limparCampos();
                    }
                    else {
                        alert(response.Mensagem ? response.Mensagem : "Ocorreu um erro ao cadastrar!");
                    }
                },
                deveEsconderCarregando: true
            });
        }
    });

    function limparCampos() {
        $("#numero_nf, #coo, #data_emissao_value, #combustivel, #quantidade, #preco_unitario, #valor_total, #hodometro, #veiculo, #placa_veiculo").val('');
    }

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
