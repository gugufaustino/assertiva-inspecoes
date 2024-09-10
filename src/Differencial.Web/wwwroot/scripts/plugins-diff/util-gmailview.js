

// Your Client ID can be retrieved from your project in the Google
// Developer Console, https://console.developers.google.com
var CLIENT_ID = document.getElementById("gmailview-client_id").value;
var SCOPES = ['https://mail.google.com/', 'https://www.googleapis.com/auth/gmail.modify', 'https://www.googleapis.com/auth/gmail.readonly'];

var USERID = 'me';// 'baitacarros@gmail.com';
var FROM_filtro =  document.getElementById("gmailview-from").value; //'well.faustino@hotmail.com';
var LOGIN_HINT = document.getElementById("gmail-hint").value; //'baitacarros@gmail.com'

var INTEGRACAO_HABILITADA = document.getElementById("gmailview-integracaohabilitada").value === "true";
/** 
 * Check if current user has authorized this application.
 */
function checkAuth() { 
    if (INTEGRACAO_HABILITADA == true) {
        gapi.auth.authorize(
          {
              'client_id': CLIENT_ID,
              'scope': SCOPES.join(' '),
              'immediate': true,
              'login_hint': LOGIN_HINT
          }, handleAuthResult);
    }

}
/**
 * Handle response from authorization server.
 *
 * @param {Object} authResult Authorization result.
 */
function handleAuthResult(authResult) {
    var authorizeDiv = document.getElementById('authorize-div');
    if (authResult && !authResult.error) {
        // Hide auth UI, then load client library.

        loadGmailApi();
    } else {
        // Show auth UI, allowing the user to initiate authorization by
        // clicking authorize button.
        montarCabecalhoHtml(null);
    }
}

/**
 * Initiate auth flow in response to user clicking authorize button.
 *
 * @param {Event} event Button click event.
 */
function handleAuthClick(event) {
    gapi.auth.authorize(
      {
          client_id: CLIENT_ID,
          scope: SCOPES, immediate: false,
          login_hint: LOGIN_HINT,

      },
      handleAuthResult);
    return false;
}




/**
 * Load Gmail API client library. List labels once client library
 * is loaded.
 */
function loadGmailApi() {
    gapi.client.load('gmail', 'v1', listMessage);

}
var lstEmails = Array();

function listMessage() {

    var from = FROM_filtro != "" ? FROM_filtro : "NONE";

    var requestProfile = gapi.client.gmail.users.getProfile({ userId: USERID });
    requestProfile.execute(function (obj) {
        $(".formEmail h5 small").html(obj.emailAddress)
        $("#gmail-hint").val(obj.emailAddress)
    });


    var request = gapi.client.gmail.users.messages.list({
        userId: USERID,
        includeSpamTrash: 'false',
        labelIds: ['INBOX'],
        q: "in:inbox is:unread -category:(promotions OR social OR forums) from:" + from //OR from:no-reply@accounts.google.com",        
        //q: "in:inbox is:unread",
    });

    request.execute(function (resp) {
        var labels = resp.messages;
        lstEmails = [];
        if (labels && labels.length > 0) {

            for (i = 0; i < labels.length; i++) {
                var label = labels[i];
                pushLstEmail(label.id)
            }

            setTimeout(function () {
                montarCabecalhoHtml(lstEmails.length);
                $('.feed-activity-list *').remove();
                lstEmails.sort(comparaDataSort);

                for (var i = 0; i < lstEmails.length; i++) {
                    montarListaHtml(lstEmails[i]);
                }
            }, 1500);


        } else {
            montarCabecalhoHtml(0);
        }
    });
}



function pushLstEmail(idMens) {
    var request = gapi.client.gmail.users.messages.get({
        userId: USERID,
        id: idMens
        //format : 'raw'
    });
    request.execute(function (result) {
        lstEmails.push(new Email(result));
    });
}

function getMessage(idMens) {
    var request = gapi.client.gmail.users.messages.get({
        userId: USERID,
        id: idMens
    });
    request.execute(function (result) {
        return new Email(result);
    });
}

function getMessageNovaSolicitacao(idMens, form) {
    mostrarCarregando();
    var request = gapi.client.gmail.users.messages.get({
        userId: USERID,
        id: idMens
    });
    request.execute(function (result) {
        _email = new Email(result);

        try {

            //form = document.querySelector("form")

            var elId = document.createElement("input");
            elId.type = "hidden";
            elId.name = "id"; elId.value = _email.id;

            var elAssunto = document.createElement("input");
            elAssunto.type = "hidden";
            elAssunto.name = "assunto"; elAssunto.value = _email.assunto;

            var elcorpoTexto = document.createElement("input");
            elcorpoTexto.type = "hidden";
            elcorpoTexto.name = "corpoTexto"; elcorpoTexto.value = _email.corpoTexto;

            var elcorpoHtml = document.createElement("input");
            elcorpoHtml.type = "hidden";
            elcorpoHtml.name = "corpoHtml"; elcorpoHtml.value = _email.corpoHtml;

            var elRemetente = document.createElement("input");
            elRemetente.type = "hidden";
            elRemetente.name = "remetente"; elRemetente.value = _email.remetente;


            form.appendChild(elId);
            form.appendChild(elAssunto);
            form.appendChild(elcorpoTexto);
            form.appendChild(elcorpoHtml);
            form.appendChild(elRemetente);
            form.submit();


            //var objForm = $("form[method=post]");
            //var formData = new FormData(document.querySelector("form"));

            //var formData = new FormData();
            //formData.append("id", _email.id);
            //formData.append("assunto", _email.assunto);
            // formData.append("corpoByte", _email.corpoByte);
            //formData.append("corpoTexto", _email.corpoTexto);
            //formData.append("corpoHtml", _email.corpoHtml);



            //$.ajax({
            //    type: 'POST',
            //    url: "/Solicitacao/Inserir",
            //    data: formData,
            //    processData: false,
            //    contentType: false,
            //    success: function (data) { 
            //        window.document.innerHTML = data;
            //    },
            //    error: function (msg) {
            //        console.log(msg);
            //        alert("error: " + msg.statusText + ". Press F12 for details");
            //    }
            //}).done(function (data) {
            //    //alert("aham");
            //});

        } catch (e) {
            alert("Exceção não tratada: " + e.message);
        }

    });

}

function Email(result) {
    try {
        this.id = result.id;

        var objAssunto = findName(result.payload.headers, "Subject");
        this.assunto = objAssunto != undefined ? objAssunto.value : "";
        var objRemetente = findName(result.payload.headers, "From");
        this.remetente = objRemetente != undefined ? objRemetente.value : "";
        this.fragmento = result.snippet;
        this.data = new Date(parseFloat(result.internalDate));
        this.dataDiaSemana = formatarDiaSemana(this.data.getDay());
        this.dataDiasDecorridos = formatarDiasDecorriddos(new Date(parseFloat(result.internalDate)));
        this.dataLocal = this.data.toLocaleString();

        var rowBase64Text;
        var rowBase64Html;
        var texto
        var html
        try {
            if (result.payload.parts["0"].parts != null) {
                rowBase64Text = result.payload.parts["0"].parts[0].body.data;
                rowBase64Html = result.payload.parts["0"].parts[1].body.data;
            }
            else {
                rowBase64Text = result.payload.parts[0].body.data;
                rowBase64Html = result.payload.parts[1].body.data;

                if (rowBase64Text == undefined) {
                    rowBase64Text = findMimeType(result.payload.parts, "text/plain");
                    rowBase64Html = findMimeType(result.payload.parts, "text/html");
                }
                texto = atob(rowBase64Text.replace(/-/g, '+').replace(/_/g, '/'));
                html = b64DecodeUnicode(rowBase64Html.replace(/-/g, '+').replace(/_/g, '/'));

            }
        } catch (e) {

            texto = "Não foi possível ler o email";
            html = "Não foi possível ler o email"
        }


        this.corpoHtml = html;
        this.corpoTexto = texto;
        this.corpoByte = result.payload.parts[0].body.data;

        return this;

    } catch (e) {
        alert("Contate suporte técnico: Erro na leitura do email: " + e.message);
        console.log(e);
        console.log(result);
    }

}

function montarCabecalhoHtml(qtdmens) {
    if (qtdmens == null) {
        $('#btn-autenticar-gmail').show();

    } else {
        $('#btn-autenticar-gmail').hide();

        if (qtdmens > 1)
            $('.ibox-heading small').text("Você tem " + qtdmens + " novas mensagens.");
        else if (qtdmens == 1)
            $('.ibox-heading small').text("Você tem " + qtdmens + " nova mensagen.");
        else if (qtdmens == 0)
            $('.ibox-heading small').text("Você não tem novas mensagens.");
    }
}

function montarListaHtml(email) {


    var strLi = "<div class=\"feed-element\"><div>" +
                    "<small class=\"pull-right text-navy\">" + email.dataDiasDecorridos + "</small>" +
                    "<strong>" + email.remetente + "</strong>" +
                    "<div>" + email.assunto + "</div>" +
                    "<small class=\"text-muted\">" + email.dataDiaSemana + " " + email.dataLocal + "</small>" +
                    "<button class=\"pull-right btn btn-secondary btn-sm btn-novasolicitacaoemail\" type=\"button\" data-email=\"" + email.id + "\" >Nova</button>"
    "</div></div>";
    $('.feed-activity-list').append(strLi);
}


function findName(list, nome) {
    for (var i = 0; i < list.length; i++) {
        value = list[i];
        if (value.name == nome) {
            return value;
        }
    }
    return undefined;
}
function findMimeType(list, nome) {
    for (var i = 0; i < list.length; i++) {
        value = list[i];
        if (value.mimeType == nome) {
            return value;
        }
    }
    return undefined;
}

///https://developer.mozilla.org/en-US/docs/Web/API/WindowBase64/Base64_encoding_and_decoding
function b64EncodeUnicode(str) {
    return btoa(encodeURIComponent(str).replace(/%([0-9A-F]{2})/g, function (match, p1) {
        return String.fromCharCode('0x' + p1);
    }));
}

function b64DecodeUnicode(str) {
    return decodeURIComponent(Array.prototype.map.call(atob(str), function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
}

function formatarDiaSemana(dia) {
    switch (dia) {
        case 0: return "Domingo";
        case 1: return "Segunda";
        case 2: return "Terça";
        case 3: return "Quarta";
        case 4: return "Quinta";
        case 5: return "Sexta";
        case 6: return "Sábado";
    }
}

function formatarDiasDecorriddos(data) {
    var hj = new Date();
    hj.setHours(0, 0, 0, 0);

    data.setHours(0, 0, 0, 0);
    var qtdDias = diffDateDays(hj, data);

    if (qtdDias == 0)
        return "Hoje";
    else if (qtdDias == 1)
        return "Há 1 dia atrás";
    else
        return "Há " + qtdDias + " dias atrás";
}

function diffDateDays(date1, date2) {

    var timeDiff = Math.abs(date2.getTime() - date1.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
    return diffDays;
}


function comparaDataSort(a, b) {

    if (a.data < b.data) {
        return 1;
    }
    if (a.data > b.data) {
        return -1;
    }
    // a must be equal to b
    return 0;
}