$(document).ready(function () {

    $("#btnConcluirCroqui").click(ConcluirCroquiVistoriador);
    $("body").on('click', '#btnConcluirVistoriadorSugerido', DefinirVistoriador);
    $("body").on('click', '#btnDefinirVistoriadorManual', DefinirVistoriadorManual);
    $("body").on('click', '#btnDefinirAnalistaManual', DefinirAnalistaManual);
    $("body").on('change', '#IdSeguradora', SelectSeguradora);
    $("body").on('change', '#IdSolicitante', SelectSolicitante);
    $("body").on('click', '#btnBaixarFotosInspecaoZip', BaixarFotosInspecaoZip);
    $("body").on('click', '#btnBaixarFotosQuadroZip', BaixarFotosQuadroZip);
    $("body").on('click', '#btnAbrirConfigBaixarFotosQuadroDoc', AbrirConfigBaixarFotosQuadroDoc);
    $("body").on('click', '#btnBaixarFotosQuadroDoc', BaixarFotosQuadroDoc);
    $("body").on('click', '#btnGerarValorizacao', GerarValorizacao);
    $("body").on('click', '#btnConcluiCheckList', ConcluiCheckList);

    $("body").on('click', '#btnConcluirLaudoAnalise', ConcluirLaudoAnalise);
    $("body").on('click', '#btnBaixarLaudoAnalise', BaixarLaudoAnalise);
    $("#btnConcluirCroquiAnalista").click(ConcluirCroquiAnalista);

});


function ConcluirCroquiVistoriador(e) {

    confirmaPost("Deseja salvar o croqui e concluír essa atividades?", function () {
        mostrarCarregando();
        var myFormData = new FormData();
        myFormData.append('arquivocroquie', $("input[name=arquivocroquie]")[0].files[0]);

        $.ajax({
            url: "/Solicitacao/SalvarAtividadeCroquiVistoriador/" + idSolicitacaoInspecao,
            type: 'POST',
            processData: false, // important
            contentType: false, // important
            dataType: 'json',
            data: myFormData,
            success: function (data) {
                window.location.reload(true);
            },
            error: function () {
                esconderCarregando();
            }
        });

    });

}

function ConcluirCroquiAnalista(e) {

    confirmaPost("Deseja salvar concluír essa atividades?", function () {
        mostrarCarregando();
        var myFormData = new FormData();
        myFormData.append('arquivocroquieanalista', $("input[name=arquivocroquieanalista]")[0].files[0]);

        $.ajax({
            url: "/Solicitacao/SalvarAtividadeCroquiAnalista/" + idSolicitacaoInspecao,
            type: 'POST',
            processData: false, // important
            contentType: false, // important
            dataType: 'json',
            data: myFormData,
            success: function (data) {
                window.location.reload(true);
            },
            error: function () {
                esconderCarregando();
            }
        });

    });

}

function DefinirVistoriador() {

    var id = $("#tbVistoriadorSugerido input[type=radio]:checked").val();

    if (!id > 0) {
        alertaMensagem("Selecione um item da lista");

    } else {

        if ($("#tbVistoriadorSugerido input[type=radio]:first")[0] != $("#tbVistoriadorSugerido input[type=radio]:checked")[0]) {
            promptDone("Por favor justifique o motivo da definição deste vistoriador?", "Definir Vistoriador", "textarea",
                function (sucesso, textoPrompt) {
                    if (sucesso) {
                        $("#txtJustificativaVistoriadorDefinido").val(textoPrompt);
                        $("#formSalvarVistoriador").submit();
                    }
                }, null, "Justificativa pela definição");

        } else if ($("#tbVistoriadorSugerido input[type=radio]:first:checked").val() > 0) {

            $("#formSalvarVistoriador").submit();
        }
    }
}

function DefinirVistoriadorManual() {
    var arrVisto = grideVistoriadorPesquisar.rows({ selected: true }).ids().toArray();

    if (arrVisto.length == 0 || arrVisto.length > 1) {
        alertaMensagem("Selecione um item da lista");
    }
    else {
        $("#IdVistoriador").val(arrVisto[0]);
        promptDone("Por favor justifique o motivo da definição deste vistoriador?", "Definir Vistoriador", "textarea",
                 function (sucesso, textoPrompt) {
                     if (sucesso) {
                         $("#txtJustificativaVistoriadorDefinido").val(textoPrompt);
                         $("#formSalvarVistoriador").submit();
                     }
                 }, null, "Justificativa pela definição");
    }
}

function DefinirAnalistaManual() {
    var arrSelec = grideAnalistaPesquisar.rows({ selected: true }).ids().toArray();

    if (arrSelec.length == 0 || arrSelec.length > 1) {
        alertaMensagem("Selecione um item da lista");
    }
    else {
        $("#IdAnalista").val(arrSelec[0]);
        $("#formSalvarAnalista").submit();
    }

}

function fnAcaoExterna(Acao) {
    if (Acao != null) {
        if (Acao == "agendar" && $("#btnAgendar").length > 0) {
            $("#btnAgendar").trigger("click");
        }
        else if (Acao == "apropriar") {
            ezBSAlert({ type: 'alert', alertType: '', headerText: 'Confirmação', messageText: 'Confirmamos o pedido de vistoria.', modalSize: 'modal-sm' })
        }
    }
}

function SelectSeguradora(e) {
    var id = $(this).val();
    if (id != "") {
        Reset("#IdProduto");
        Reset("#IdSolicitante");
        Reset("#IdFilial");
        $.getJSON('/Solicitacao/SelecionarProdutos?idSeguradora=' + id, function (data) {
            if (!data.success) {
                alertaResponseResult(data);
            } else {
                var option = '<option></option>';
                $.each(data.content, function (key, obj) {
                    option += '<option value="' + obj.Id + '">' + obj.NomeProduto + '</option>';
                    $('#IdProduto').html(option);
                });
            }
        });

        $.getJSON('/Solicitacao/SelecionarSolicitantes?idSeguradora=' + id, function (data) {
            if (!data.success) {
                alertaResponseResult(data);
            } else {
                var option = '<option></option>';
                $.each(data.content, function (key, obj) {
                    option += '<option value="' + obj.Id + '">' + obj.NomeOperador + '</option>';
                    $('#IdSolicitante').html(option);
                });
            }
        });

        getJSONResponseResult({
            url: '/Solicitacao/SelecionarFilial?idSeguradora=' + id,
            fnSucesso: function (data) {
                if (!data.success) {
                    alertaResponseResult(data);
                } else {
                    var option = '<option></option>';
                    $.each(data.content, function (key, obj) {
                        option += '<option value="' + obj.Id + '">' + obj.NomeFilial + '</option>';
                        $('#IdFilial').html(option);
                    });
                }
            }
        });
    }
    else {
        Reset("#IdProduto");
        Reset("#IdSolicitante");
        Reset("#IdFilial")
    }
}

function SelectSolicitante(e) {
    var id = $(this).val();
    if (id != "") {
        $.getJSON('/Solicitacao/SelecionarSolicitante?idSolicitante=' + id, function (data) {
            if (!data.success) {
                alertaResponseResult(data);
            } else {
                $("#SolicitanteTelefone").val(data.content.Telefone);
                $("#SolicitanteEmail").val(data.content.Email);
            }
        });
    }
    else {
        $("#SolicitanteTelefone").val("");
        $("#SolicitanteEmail").val("");
    }
}

function Reset(strSeletor) {
    document.querySelector(strSeletor).options.removeAll();
}

function BaixarFotosInspecaoZip() {
    var url = "/Foto/BaixarFotosZip/" + idSolicitacaoInspecao
    window.location.href = url;
}

function BaixarFotosQuadroZip() {
    var url = "/Foto/BaixarFotosQuadroZip/" + idSolicitacaoInspecao
    window.location.href = url;
}

function AbrirConfigBaixarFotosQuadroDoc() {
    $("#modalConfigurarQuadroFotos").modal({
        backdrop: "static",
        keyboard: true
    });
}

function BaixarFotosQuadroDoc() {
    var url = "/Foto/BaixarFotosQuadroDoc/" + idSolicitacaoInspecao + "?chkQuadroNumSeqLegenda=" + ($("#chkQuadroNumSeqLegenda:checked").val() ? true : false).toString();
    window.location.href = url;

    $("#modalConfigurarQuadroFotos").modal('hide');
}

function GerarValorizacao(e) {

    confirmaPost("Deseja realizar o lançamento dos valores financeiros?", function () {
        mostrarCarregando();
        $.ajax({
            url: "/Solicitacao/GerarValorizar/" + idSolicitacaoInspecao,
            type: 'POST',
            processData: false, // important
            contentType: false, // important
            dataType: 'json',
            //data: myFormData,
            success: function (data) {
                alertaResponseResult(data);
                if (data.success) {
                    window.location.reload(true);
                }
                esconderCarregando();
            },
            error: function () {
                esconderCarregando();
            }
        });

    });

}

function ConcluiCheckList(e) {

    confirmaPost("Deseja salvar e concluír essa atividade?", function () {
        mostrarCarregando();
        $.ajax({
            url: "/Solicitacao/SalvarCheckList/" + idSolicitacaoInspecao,
            type: 'POST',
            processData: false, // important
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ AreaConstruida: $("#AreaConstruida").val(), BlocoConstruido: $("#BlocoConstruido").val() }),
            success: function (data) {
                alertaResponseResult(data);
                if (data.success) {
                    window.location.reload(true);
                }
                esconderCarregando();
            },
            error: function () {
                esconderCarregando();
            }
        });

    });

}

function ConcluirLaudoAnalise(e) {


    confirmaPost("Deseja salvar e concluír essa atividade?", function () {
        mostrarCarregando();

        var myFormData = new FormData();
        myFormData.append('arquivolaudoanalista', $("input[name=arquivolaudoanalista]")[0].files[0]);

        $("#idtabLaudo").find("input:not([type=file],[type=checkbox])").each(function (i, elem) {
            if ($(elem).attr('name') != undefined) {
                myFormData.append($(elem).attr('name'), $(elem).val())
            }
        });

        myFormData.set("IndRelatorioExigenciaMelhoria", $("#IndRelatorioExigenciaMelhoria:checked").val())

        $.ajax({
            url: "/Solicitacao/SalvarAtividadeLaudoAnalista/" + idSolicitacaoInspecao,
            type: 'POST',
            processData: false, // important
            contentType: false,
            dataType: 'json',
            data: myFormData,
            success: function (data) {
                alertaResponseResult(data);
                if (data.success) {
                    window.location.reload(true);
                }
                esconderCarregando();
            },
            error: function () {
                esconderCarregando();
            }
        });

    });



}

function BaixarLaudoAnalise() {
    var url = "/ArquivoAnexo/BaixarLaudoAnalise/" + idSolicitacaoInspecao
    window.location.href = url;
}

