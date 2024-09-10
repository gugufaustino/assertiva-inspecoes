

function ezBSAlert(options) {
    var deferredObject = $.Deferred();
    var defaults = {
        type: "alert", //alert, prompt, confirm 
        modalSize: '', //modal-sm, modal-lg
        okButtonText: 'Ok',
        cancelButtonText: 'Cancelar',
        yesButtonText: 'Sim',
        noButtonText: 'Não',
        headerText: '',
        messageText: '',
        labelText : '',
        alertType: 'primary', //default, primary, success, info, warning, danger
        inputFieldType: 'text', //could ask for number,email,etc ou textarea
        inputFieldValue: "" , //Valor padrão já apresentar no campo
        buttonOkYesTheme: "btn-primary",
        url: ""
    }
    $.extend(defaults, options);

    var _show = function () {
        var headClass = "";// "navbar-default";
        var sClassIcon = "";
        var sClassText = "";
        var headerTextDefault = "";

        switch (defaults.alertType) {
            case "primary":
                headClass = "alert-primary";
                break;
            case "success":
            case 0:
                headClass = "alert-success";
                sClassIcon = "fa-check-circle";
                sClassText = "text-success";
                headerTextDefault = "Sucesso";
                break;
            case "info":
            case 2:
                headClass = "alert-info";
                sClassIcon = "fa-info-circle";
                sClassText = "text-info";
                headerTextDefault = "Informação";
                break;
            case "warning":
            case 3:
                headClass = "alert-warning";
                sClassIcon = "fa-exclamation-triangle";
                sClassText = "text-warning";
                headerTextDefault = "Atenção";
                break;
            case "danger":
            case 1:
                headClass = "alert-danger";
                sClassIcon = "fa-times-circle";
                sClassText = "text-danger";
                headerTextDefault = "Erro";
                break;
        }
        if (defaults.headerText == null || defaults.headerText.length == 0)
            defaults.headerText = headerTextDefault;

        var id = "ezAlerts_" + Math.floor(Math.random() * 100000) + 1
        var qtdmodals =  $(".modal").length ;
        var zIndezModals = 2050 + (qtdmodals * 10);

        $('BODY').append(
			'<div id="' + id + '" class="modal fade" style="z-index:'+ zIndezModals + '" >' +
			'<div class="modal-dialog ' + defaults.modalSize + '" >' +
			'<div class="modal-content">' +
			    '<div id="ezAlerts-header" class="modal-header ' + headClass + '">' +
			        '<button id="close-button" type="button" class="close" data-dismiss="modal"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>' +
			        '<h4 id="ezAlerts-title" class="modal-title ' + sClassText + ' ">' +
                    '<i class="fa ' + sClassIcon + '" aria-hidden="true"></i>&nbsp;' + defaults.headerText +
                    '</h4>' +
			    '</div>' +
			    '<div id="ezAlerts-body" class="modal-body" style="white-space:pre-wrap">' +
			        '<div id="ezAlerts-message" ></div>' +
			    '</div>' +
			    '<div id="ezAlerts-footer" class="modal-footer">' +
			    '</div>' +
			'</div>' +
			'</div>' +
			'</div>'
		);

        var objModal = $('#' + id);

        objModal.find('#ezAlerts-header.modal-header').css({
            'padding': '15px 15px',
            '-webkit-border-top-left-radius': '5px',
            '-webkit-border-top-right-radius': '5px',
            '-moz-border-radius-topleft': '5px',
            '-moz-border-radius-topright': '5px',
            'border-top-left-radius': '5px',
            'border-top-right-radius': '5px'
        });



        objModal.find('#ezAlerts-message').html(defaults.messageText);

        var keyb = true;
        var backd = "static";
        var calbackParam = "";
        var calbackParam2 = null;

        switch (defaults.type) {
            case "alert":
                keyb = "true";
                backd = "true";
                objModal.find('#ezAlerts-footer').html('<button class="btn btn-primary" autofocus>' + defaults.okButtonText + '</button>')
                    .on('click', ".btn", function () {
                        calbackParam = true;
                        objModal.modal('hide');
                    });
                break;
            case "confirm":
                var btnhtml = "";
                if (defaults.noButtonText && defaults.noButtonText.length > 0) {
                    btnhtml += '<button id="ezclose-btn" class="btn btn-default">' + defaults.noButtonText + '</button>';
                }
                btnhtml += '<button id="ezok-btn" class="btn btn-primary" autofocus>' + defaults.yesButtonText + '</button>';

                objModal.find('#ezAlerts-footer').html(btnhtml).on('click', 'button', function (e) {
                    if (e.target.id === 'ezok-btn') {
                        calbackParam = true;
                    } else if (e.target.id === 'ezclose-btn') {
                        calbackParam = false;
                    }
                    objModal.modal('hide');
                });
                break;
            case "prompt":
                objModal.find('#ezAlerts-message').addClass("row")

                var messageHtml = defaults.messageText.length > 0 ? '<div class="col-md-12 form-group m-t-n-lg">' + defaults.messageText + '</div>' : '';
                var labelHtml = defaults.labelText.length > 0 ? '<label class="text-nowrap required" for="prompt">' + defaults.labelText + '</label> ' : '';
                var inputClass = defaults.inputFieldType == "number" ? "form-control text-right" : "form-control";

                var inputHtml = '<input type="' + defaults.inputFieldType + '" class="' + inputClass + '" id="prompt" required="required" value="' + defaults.inputFieldValue + '" /> '
                if (defaults.inputFieldType == "textarea")
                    inputHtml = '<textarea class="form-control" maxlength="1000" id="prompt" rows="4" required="required"></textarea> '
               
                objModal.find('#ezAlerts-message').html('<form action="#" onsubmit="return false;" id="form-prompt">' + messageHtml + '<div class="form-group col-md-12 m-b-n-sm">' + labelHtml + inputHtml + '</div></form>');

                var btnhtml = "";
                if (defaults.cancelButtonText && defaults.cancelButtonText.length > 0) {
                    btnhtml += '<button id="ezclose-btn" class="btn btn-default">' + defaults.cancelButtonText + '</button>';
                }
                btnhtml += '<button id="ezok-btn" class="btn ' + defaults.buttonOkYesTheme + '" autofocus>' + defaults.okButtonText + '</button>';

                objModal.find('#ezAlerts-footer').html(btnhtml).on('click', "button", function (e) {
                        if (e.target.id === 'ezok-btn' && $("#prompt").valid()) {
                            calbackParam = true;
                            calbackParam2 = $('#prompt').val()
                            objModal.modal('hide');
                        } else if (e.target.id === 'ezclose-btn') {
                            calbackParam = false;
                            calbackParam2 = null;
                            objModal.modal('hide');
                        }
                        
                });
                break;

            case 'loadUrl':
                objModal.find(".modal-content").html("");
                console.log(options.url);
                $.ajax({
                    bMostrarCarregando: true,
                    url: options.url, type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    //dataType: 'json',
                    data: null,
                    success: function (result) {
                        // Se voltar Json da contoller base.ResponseResult
                        if (result == null || result.success ===  null) {
                            erroMensagem("Retorno não foi serializado pelo metodo base.ResponseResult");
                        } else if (result.success === false) {
                            alertaResponseResult(result);
                        } else { // Se voltar conteudo html - exibe na modal 
                            objModal.find(".modal-content").html(result);

                            objModal.find(".modal-body")
                                .css("overflow-y", "auto")
                                .css('max-height', $(window).height() * 0.73)
                            objModal.modal('show');

                            // se voltar showMessage exibe 
                            //if (result.success === true)
                            //    alertaResponseResult(result);
                        }
                    }
                });  
                break;

        }


        objModal.modal({
            show: false,
            backdrop: backd,
            keyboard: keyb
        }).on('hidden.bs.modal', function (e) {
            objModal.remove();
            deferredObject.resolve(calbackParam, calbackParam2);

            $("body").css("padding-right", "") // Fix : multiplas modais estava gerando padding-right desnecessário

        }).on('shown.bs.modal', function (e) {
             
            $(this).next().css("z-index", (parseInt(zIndezModals) - 1))

            if (objModal.find('#prompt').length > 0) {
                objModal.find('#prompt').focus();
            } else {
                document.activeElement.blur();
                objModal.find("#ezAlerts-footer .btn").focus();
            }

        })
        if (defaults.type != "loadUrl") {
            objModal.modal('show');
        }

    }

    _show();
    return deferredObject.promise();
}

 
$("body").on('keydown', 'input#prompt', function (event) {
    if (event.keyCode == 13) {
        event.preventDefault();
        return false;
    }
});

 
$("body").on('keyup', 'input#prompt',
    function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            $("#ezok-btn").trigger("click");
            return false;
        }
});
 
