
$(document).ready(function () {
 
    // Fomulários com POST type=submit
    $("body").on("submit", "form", mostrarCarregando);

    $("body").on("submit", "form[fw-form=fw-form]", function (e) {
        if (e.preventDefault)
            e.preventDefault(); 
      
        var form = this;
        var urlPost = $(form).attr("action");
        var data = $(form).serialize();

        var fnSucesso = function () { };
        if ($(form).attr("fw-fn-sucesso").length > 0)
        eval("fnSucesso = function(result){ " +  $(form).attr("fw-fn-sucesso") +" }");
       
        ajaxJsonResponseResult({ url: urlPost, data: data, fnSucesso: fnSucesso });
        
    });

    $("#btnExcluir").click(function () {
        $("form:eq(0)").submit();
    });
   
    $("#btnSalvarDrop a").click(function (e) {
        e.preventDefault();
        var action = $(this).attr("acaoretorno");
        $("#retornosalvar").val(action);
        $("form:eq(0)").submit();
        return false;
    });
    //

    

    $("#btnAbrir").click(function (e) {
        e.preventDefault();

        var link = $(this).attr("href");
        var id = $("input[name=Id]:checked:first").val();
        if (id != undefined)
            document.location = link + "/" + id;
    }); 

    if ($(".TabelaEditavel tbody tr:last").length == 0) {
        $(".TabelaEditavel #add_row").trigger("click");
    }

    $("body").on("click", 'a[fw-click="modal"], button[fw-click="modal"]', function (e) {
         e.preventDefault(); 
         var urlPost = $(this).attr("data-href"); 
         
         if (urlPost == undefined || urlPost == null) {
             erroMensagem("Falha ao carregar endereço da modal. Contate o suporte técnico.")
             return;
         }

         loadModal(urlPost, $(this).attr("fw-modal"));
    });

    $("body").on("click", 'button[fw-click="prompt"],a[fw-click="prompt"]', function (e) {
        e.preventDefault();
        var urlPost = $(this).attr("data-href");
 
        if (urlPost == undefined || urlPost == null) {
            erroMensagem("Falha ao carregar endereço da modal. Contate o suporte técnico.")
            return;
        }

        var objformgroup = $(this).parentsUntil("div.form-group").parent().get(0);
        var label = $(objformgroup).find("label");
        var input = $(objformgroup).find("input:eq(0)");
          inputType = $(input).attr("type");


        var fnDonePrompt = function (sucesso, textoPrompt) {
            if (sucesso) {
                mostrarCarregando();
                textoPrompt = (inputType == "number") ? textoPrompt.replace(".", ",") : textoPrompt;
                  myFormData = new FormData();
                myFormData.append($(input).attr("name"),  String(textoPrompt));

                $.ajax({
                    url: urlPost,  type: 'POST',
                    processData: false, contentType: false, // important
                    dataType: 'json',
                    data: myFormData,
                    success: function (result) {
                        esconderCarregando()
                        if (!result.success) {
                            alertaResponseResult(result);
                        } else {
                            window.location.reload(true);
                        } 
                    }


                });
            }
        }

        promptDone("", "Editar", inputType, fnDonePrompt, "modal-sm", $(label).text());

    });

    $("body").on("click", 'button[fw-click="confirmaPost"]', function (e) {
        e.preventDefault();
        var urlPost = $(this).attr("data-href");

        if (urlPost == undefined || urlPost == null) {
            erroMensagem("Falha ao carregar url data-href. Contate o suporte técnico.")
            return;
        }
        
        var fnPost = function () {
            ajaxJsonResponseResult({ url: urlPost });
        };
        
        confirmaPost($(this).attr("data-val-mensagem"), fnPost);
    });

});

$(document).on('change', ':file', function () {
    var input = $(this);
    var numFiles = input.get(0).files ? input.get(0).files.length : 1;
    var label = input.val().replace(/\\/g, '/').replace(/.*\//, '');

    input.trigger('fileselect', [numFiles, label]);
}).on('fileselect', ':file', function (event, numFiles, label) {
    var input = $(this).parents('.input-group').find(':text');
    var log = numFiles > 1 ? numFiles + ' files selected' : label;

    if (input.length) {
        input.val(log);
    } else {
        //if (log)
        //    alert(log);
    }
});
 

$("form").on("click", "#add_row", function () {

    var trTemplate = $('.TabelaEditavel template').html()

    var iLast = 0;
    var domTrLast = $(".TabelaEditavel tbody tr:last");

    if (domTrLast != null && !isNaN(domTrLast.attr("tbeditindex"))) {
        iLast = parseInt(domTrLast.attr("tbeditindex"));
        iLast = iLast + 1;
    }


    trTemplate = trTemplate.replace('tbeditindex="0"', 'tbeditindex="' + iLast + '"')
    trTemplate = trTemplate.replace('[0].', '[' + iLast + '].');
    trTemplate = trTemplate.replace('[0].', '[' + iLast + '].')

    $('.TabelaEditavel tbody').append(trTemplate);

});

$("form").on("click", ".TabelaEditavel .btnExcluirRow", function () {
    $(this).parentsUntil("tr").parent().remove();

    //Re-sincroniza indices do html
    $(".TabelaEditavel tbody tr").each(function (iNovo, e) {
        var iAtual = $(e).attr("tbeditindex");
        arrInput = $(e).find("input");

        for (var x = 0; x < arrInput.length; x++) {
            var nameEl = $(arrInput[x]).attr("name");
            nameEl = nameEl.replace('[' + iAtual + '].', '[' + iNovo + '].');

            $(arrInput[x]).attr("name", nameEl);
        }

        $(e).attr("tbeditindex", iNovo);


    });

});


function formPostModal(form, fnSucessoCallBack) {
        
        var urlPost = $(form).attr("action");
        var data = $(form).serialize();

        var fnSucesso = function () { };
        if (fnSucessoCallBack != undefined && fnSucessoCallBack != null)
            fnSucesso = fnSucessoCallBack;

        ajaxJsonResponseResult({ url: urlPost, data: data, fnSucesso: fnSucesso });

    }