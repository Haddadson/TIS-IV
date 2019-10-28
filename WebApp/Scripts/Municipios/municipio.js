var municipioURL = "https://ibge.herokuapp.com/municipio/?val=MG";

$(document).ready(() => {
    // Driblou o CORS, don't know how.
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var municipios = Object.keys(JSON.parse(xhttp.responseText));
            $('#municipios').html = '';
            $('#municipios').append(municipios.map(m => {
                return "<option>" + m + "</option>";
            }));
        }
    };
    xhttp.open("GET", this.municipioURL, true);
    xhttp.send();
});