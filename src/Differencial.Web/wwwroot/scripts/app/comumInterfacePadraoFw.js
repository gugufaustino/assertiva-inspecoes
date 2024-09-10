
// nenhuma requisição ajax necessita de tratamento com a função .error pois todos erros são capturados dessa forma:
$(document).ajaxError(function (event, xhr, settings, thrownError) {
    
    if (xhr.status === 500 && xhr.responseJSON !== null && xhr.responseJSON?.success === false) {
        alertaResponseResult(xhr.responseJSON);
    } else {
        erroMensagem(mensagemAmigavel(xhr.statusText) + "<br>" +  mensagemTecnica(settings, xhr, thrownError));

    }
    esconderCarregando();
});
function mensagemTecnica(settings, xhr, thrownError) {
    return "Requisição: " + settings.url + " \n" + xhr.status + " - " + thrownError;
}

function mensagemAmigavel(statusText) {

    let mensagem = "";
    switch (statusText) {
        case "Unauthorized":
            mensagem = "Não Autorizado - Você não tem acesso ou sua sessão expirou.<br>Faça login no sistema e tente novamente.";
            break;
        default:
    }

    return  "<p class='text-center'>" + mensagem +"<p>"

}

$(document).ajaxSend(function (event, xhr, settings) {
    //console.log(settings)
    if (settings.bMostrarCarregando !== false) {
        mostrarCarregando();
    }
});

$(document).ajaxComplete(function (event, xhr, settings) {
    AplicaJqueryValidation();
    esconderCarregando();
});

function mostrarCarregando() {

    $("#overlay").show();
}

function esconderCarregando() {
    setTimeout(function () { $("#overlay").fadeOut(); }, 100)

}

function alertaResponseResult(responseResultDTO) {
    try {
        if (responseResultDTO.showMessage)
            ezBSAlert({ type: "alert", alertType: responseResultDTO.TipoResponseResult, headerText: responseResultDTO.title, messageText: responseResultDTO.message });

    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - alertaResponseResult:\n" + e.message);
        console.log(e);
    }

}

function alertaMensagem(mensagem) {
    try {
        ezBSAlert({ type: "alert", alertType: "warning", messageText: mensagem, modalSize: "modal-sm" });
    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - alertaMensagem:\n" + e.message);
        console.log(e);
    }
}

function erroMensagem(mensagem) {
    try {
        ezBSAlert({ type: "alert", alertType: "danger", messageText: mensagem, modalSize: "modal-sm" });
    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - erroMensagem:\n" + e.message);
        console.log(e);
    }
}

function validacaoMensagem(mensagem) {
    try {
        ezBSAlert({ type: "alert", alertType: "warning", headerText: "Validação", messageText: mensagem, modalSize: "modal-sm" });
    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - erroMensagem:\n" + e.message);
        console.log(e);
    }
}

function confirmaPost(mensagem, fnPost) {
    try {
        ezBSAlert({ type: "confirm", headerText: "Confirmar", messageText: mensagem })
            .done(function (e) {
                if (e === true) {
                    fnPost();
                }
            });

    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - confirmaPost:\n" + e.message);
        console.log(e);
    }
}


function loadModal(url, modalSize) {
    try {
        var obj = ezBSAlert({ type: "loadUrl", url: url, modalSize: modalSize });
    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - loadModal:\n" + e.message);
        console.log(e);
    }
}

function promptDone(mensagem, titulo, inputType, fnDone, modalSize, labelTexto, valorPadraoNoCampo) {
    try {
        ezBSAlert({
            type: "prompt", headerText: titulo, messageText: mensagem, labelText: labelTexto, inputFieldType: inputType,
            buttonOkYesTheme: "btn-success", okButtonText: "Concluir", cancelButtonText: "Fechar", modalSize: modalSize, inputFieldValue: valorPadraoNoCampo
        })
                .done(function (sucesso, texto) {
                    fnDone(sucesso, texto);
                });

    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - promptDone:\n" + e.message);
        console.log(e);
    }
}



/// todas requisições Ajax devem ser centralizadas nesse metodo
function ajaxJsonResponseResult(options) {
    var setting = {
        url: "",
        data: null,
        fnSucesso: function () { },
        bMostrarCarregando: true 
    }
    $.extend(setting, options);

    try {
        $.ajax({
            bMostrarCarregando: setting.bMostrarCarregando,
            url: setting.url, type: 'POST',
            dataType: 'json',
        data: setting.data,
        success: function (result) {
            // Se voltar Json da contoller base.ResponseResult
            if (result === null || result.success === null) {
                erroMensagem("Retorno não foi serializado pelo metodo base.ResponseResult");
            } else if (result.success === false) {
                alertaResponseResult(result);
            } else {
                setting.fnSucesso(result);
                // se voltar showMessage exibe 
                if (result.showMessage === true)
                    alertaResponseResult(result);
            }
            //TODO tratar outros retornos como urlredirect
        }
    });

} catch (e) {
    alert("Exceção não tratada, contate o suporte técnico - ajaxJsonResponseResult:\n" + e.message);
    console.log(e);
}
}

function getJSONResponseResult(options) {
    var setting = {
        url: "",
        data: null,
        fnSucesso: function () { },
        bMostrarCarregando: true, 
    }
    $.extend(setting, options);

    try {
        $.ajax({
            bMostrarCarregando: setting.bMostrarCarregando,
            url: setting.url, type: 'GET',
            dataType: 'json',
            data: setting.data,
            success: function (result) {
                // Se voltar Json da contoller base.ResponseResult
                if (result === null || result.success === null) {
                    erroMensagem("Retorno não foi serializado pelo metodo base.ResponseResult");
                } else if (result.success === false) {
                    alertaResponseResult(result);
                } else {
                    setting.fnSucesso(result);
                    // se voltar showMessage exibe 
                    if (result.showMessage === true)
                        alertaResponseResult(result);
                }
                //TODO tratar outros retornos como urlredirect
            }
        });

    } catch (e) {
        alert("Exceção não tratada, contate o suporte técnico - ajaxJsonResponseResult:\n" + e.message);
        console.log(e);
    }
}
