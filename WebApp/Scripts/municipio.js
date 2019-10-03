var municipioURL = "http://ibge.herokuapp.com/municipio/?val=MG";

window.onload = function () {

    fetch(municipioURL).then(res => res.json()).then((resp) => {
        var municipios = Object.keys(resp);

        $('#municipios').append(municipios.map(m => {
            return "<option>" + m + "</option>";
        }));
    })  .catch(error => {
            alert("Erro ao buscar municípios!");
        });
}