var ValidarNotas = {};
ValidarNotas.chamadaAjax = function (parametros) {
    parametros.data = JSON.stringify(parametros.data);
    var parametrosDefault = {
        type: "POST",
        cache: false,
        data: {},
        dataType: 'json',
        traditional: true,
        contentType: 'application/json',
        deveEsconderCarregando: false,
        sucesso: function () { },
        erro: function () { },
        exibirCarregando: function () { $('.carregando').show(); },
        esconderCarregando: function () { $('.carregando').hide(); },
        beforeSend: function () {
            if (!this.deveEsconderCarregando) {
                this.exibirCarregando();
            }
        },
        complete: function () {
            if (!this.deveEsconderCarregando) {
                this.esconderCarregando();
            }
        },
        success: function (args) {
            if (args && !args.expirou) {
                this.sucesso(args);
            }
        },
        error: function (reqObj, tipoErro, mensagemErro) {
            tratarErroChamadaAjax(reqObj, tipoErro, mensagemErro, this.erro);
        }
    };
    parametros.data = parametros.data || JSON.stringify(parametros.data);
    $.extend(parametrosDefault, parametros);
    $.ajax(parametrosDefault);
};

function chamadaAjax(url, parametros, callbackSucesso, callbackErro, async, naoExibirCarregando) {

    $.ajax({
        type: "POST",
        url: url,
        cache: false,
        data: JSON.stringify(parametros),
        dataType: 'json',
        traditional: true,
        contentType: 'application/json',
        async: async,
        beforeSend: validarSeDeveExibirCarregando(naoExibirCarregando),
        complete: validarSeDeveEsconderCarregando(naoExibirCarregando),
        success: function (args) {
            if (args && !args.expirou) {
                callbackSucesso(args);
            }
        },
        error: function (reqObj, tipoErro, mensagemErro) {
            tratarErroChamadaAjax(reqObj, tipoErro, mensagemErro, callbackErro);
        }
    });
}

function tratarErroChamadaAjax(reqObj, tipoErro, mensagemErro, callbackErro) {

    if (!nuloOuVazio(reqObj) && !nuloOuVazio(reqObj.status) && reqObj.status === 401) {
        var urlDirecionarParaAcessoNaoAutorizado = (urlBaseSistema + "/Home/NaoAutorizado");
        window.location.href = urlDirecionarParaAcessoNaoAutorizado;
    } else {

        if (!nuloOuVazio(reqObj) && !nuloOuVazio(reqObj.responseText)) {
            var jsonValue = jQuery.parseJSON(reqObj.responseText);
            alertaErro(jsonValue);
        }

        if (callbackErro) {
            callbackErro(tipoErro, mensagemErro);
        }
    }
}

function obterRetornoChamadaAjaxSincrona(url, parametros, callbackErro, naoExibirCarregando) {
    var resposta = $.ajax({
        type: "POST",
        url: url,
        cache: false,
        data: JSON.stringify(parametros),
        dataType: 'json',
        traditional: true,
        contentType: 'application/json',
        async: false,
        beforeSend: validarSeDeveExibirCarregando(naoExibirCarregando),
        complete: validarSeDeveEsconderCarregando(naoExibirCarregando),
        success: function (args) {
            if (args && args.expirou) {
                window.location = args.urlSessaoExpirada;
            }
        },
        error: function (reqObj, tipoErro, mensagemErro) {

            tratarErroChamadaAjax(reqObj, tipoErro, mensagemErro, callbackErro);
        }
    }).responseText;

    return nuloOuVazio(resposta) ? undefined : jQuery.parseJSON(resposta);
}


//----------------------------------------------------------------------------------------


function validarSeDeveExibirCarregando(naoExibirCarregando) {
    if (!naoExibirCarregando)
        exibirCarregando();
}

function validarSeDeveEsconderCarregando(naoExibirCarregando) {
    if (!naoExibirCarregando)
        esconderCarregando();
}

function nuloOuVazio(valor) {
    return valor === undefined || valor === null || valor === "";
}

function nuloOuVazioVetor(vetor) {
    return vetor === undefined || vetor === null || vetor.length === 0;
}
