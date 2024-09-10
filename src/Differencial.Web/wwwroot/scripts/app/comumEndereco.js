
$(document).ready(function () {

    $("#Endereco_Cep, #Vistoriador_EnderecoBase_Cep").change(function () {
        pesquisaCEP(this);
    });

    $("#Endereco_Logradouro, #Endereco_Numero, #Vistoriador_EnderecoBase_Logradouro, #Vistoriador_EnderecoBase_Numero").change(function () {
        pesquisaGeoLatLong(this);
    });

    var objDivEnderecoMap = $(".divEnderecoMap:eq(0)").get(0);
    carregarMap($("#Endereco_Latitude").val(), $("#Endereco_Longitude").val(), objDivEnderecoMap);

    $("#btnEditarLatitudeLongitude").click(editarLatitudeLongitude);

});


function pesquisaCEP(objDom) {

    if ($(objDom).val().length == 0)
        return false;

    var objContainerEndereco = $(objDom).parentsUntil('.classContainerEndereco').last();

    $.post("/Endereco/PesquisaCEP/", { cep: $(objDom).val() }, function (data) {
        if (data.Erro) {
            alert(data.Result);
        } else {
            if (data.Result != null) {
                $(objContainerEndereco).find("input[name$=SiglaUf]").val(data.Result.SiglaUf);
                $(objContainerEndereco).find("input[name$=NomeMunicipio]").val(data.Result.NomeMunicipio);
                $(objContainerEndereco).find("input[name$=Bairro]").val(data.Result.Bairro);
                $(objContainerEndereco).find("input[name$=Logradouro]").val(data.Result.Logradouro);
            } 
        }
    });
}

function pesquisaGeoLatLong(objDom) {

    var objContainerEndereco = $(objDom).parentsUntil('.classContainerEndereco').last();

    if ($(objContainerEndereco).find("input[name$=NomeMunicipio]").val().length == 0 ||
        $(objContainerEndereco).find("input[name$=SiglaUf]").val().length == 0 ||
        $(objContainerEndereco).find("input[name$=Logradouro]").val().length == 0)
        return false;

    $.post("/Endereco/PesquisaGeoLatLong/", $(objContainerEndereco).find("input").serialize(), function (data) {
        console.log(data)
        
        if (!data.success) {
            alertaResponseResult(data);
        } else {
            aplicaCoordenadasCarregarMap(data.content.Latitude, data.content.Longitude, objContainerEndereco);

            //$(objContainerEndereco).find("input[name$='.Latitude']").val(data.content.Latitude);
            //$(objContainerEndereco).find("input[name$='.Longitude']").val(data.content.Longitude);

            //var lat_long = data.content.Latitude + ", " + data.content.Longitude;
            //$(objContainerEndereco).find("input[name*=txtLatitudeLongitude]").val(lat_long);

            ////nem todas interfaces tem mapa
            //var objDivEnderecoMap = $(objContainerEndereco).parent().find(".divEnderecoMap").get(0);
            //if (objDivEnderecoMap != undefined && objDivEnderecoMap != null) {
            //    carregarMap($(objContainerEndereco).find("input[name$='.Latitude']").val(),
            //                $(objContainerEndereco).find("input[name$='.Longitude']").val(),
            //                objDivEnderecoMap);
            //}
        }
    });
}
///Formato esperado:
/// sLat_ptBr : -30,0048071
/// sLong_ptBr: -51,1890003
function carregarMap(sLat_ptBr, sLong_ptBr, objDivEnderecoMap) {
    setTimeout(function () {

        sLat_ptBr = sLat_ptBr?.replace(",", ".");
        sLong_ptBr = sLong_ptBr?.replace(",", ".");
        InitLocation(sLat_ptBr + ", " + sLong_ptBr, objDivEnderecoMap);

    }, 50)

}

function editarLatitudeLongitude() {
    promptDone("Preencha formato corretamente com separador decimal vírgula ',' e algarismos por vírgula e espaço ', '</br>Ex.: '-29,5760641, -51,3657249'", "Editar", "text",
            function (sucesso, textoPrompt) {
                if (sucesso) {
                    try {
                        var lat = textoPrompt.split(", ")[0];
                        var long = textoPrompt.split(", ")[1];
                        if ((lat.length == 0 || long.length == 0)
                            || lat.indexOf(".") > -1 || long.indexOf(".") > -1) {
                            validacaoMensagem("Formato inválido!");
                            return true;
                        }
                        var objContainerEndereco = $("#btnEditarLatitudeLongitude").parentsUntil('.classContainerEndereco').last();
                        aplicaCoordenadasCarregarMap(lat.trim(), long.trim(), objContainerEndereco);

                    } catch (e) {
                        validacaoMensagem("Formato inválido!\n\r" + e)
                    }
                    return true;
                }

            }, "modal-sm", "Latitude x Longitude");

}

function aplicaCoordenadasCarregarMap(lat, long, objContainerEndereco) {
    var Latitude = lat;
    var Longitude = long;
    $(objContainerEndereco).find("input[name$='.Latitude']").val(Latitude);
    $(objContainerEndereco).find("input[name$='.Longitude']").val(Longitude);

    var lat_long = Latitude + ", " + Longitude;
    $(objContainerEndereco).find("input[name*=txtLatitudeLongitude]").val(lat_long);

    //nem todas interfaces tem mapa
    var objDivEnderecoMap = $(objContainerEndereco).parent().find(".divEnderecoMap").get(0);
    if (objDivEnderecoMap != undefined && objDivEnderecoMap != null) {
        carregarMap($(objContainerEndereco).find("input[name$='.Latitude']").val(),
                    $(objContainerEndereco).find("input[name$='.Longitude']").val(),
                    objDivEnderecoMap);
    }

}