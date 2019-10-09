var municipioURL = "https://ibge.herokuapp.com/municipio/?val=MG";

window.onload = function () {

    ValidarNotas.chamadaAjax({
        url: this.municipioURL,
        sucesso: function (retorno) {
            console.log(retorno);
            var municipios = retorno.municipios;

            $('#municipios').append(municipios.map(m => {
                return "<option>" + m + "</option>";
            }));
        },
        error: function () {
            alert("Erro ao buscar municípios!");
        },
        deveEsconderCarregando: true
    });
};