$(document).ready(function () {

    $('input[name=rdgAnalise]').change(function () {
        var value = $(this).val()
        if (value == "numero") {
            $("span.ansNumero").show();
            $("span.ansData").hide();
            $("span.ansExprecao").hide();

        } else if (value == "data") {
            $("span.ansNumero").hide();
            $("span.ansData").show();
            $("span.ansExprecao").hide();
        } else //expressoes
        {
            $("span.ansNumero").hide();
            $("span.ansData").hide();
            $("span.ansExprecao").show();
        }
    });

    $('#btnAnalisar').click(function () {
        mostrarCarregando();
        setTimeout(function () {
            var value = $("input[name=rdgAnalise]:checked").val();
            //console.log(value);
            if (value == "numero") {
                appBoxTemp.fotos.sort(sortNumeroAns)
            } else if (value == "data") {
                appBoxTemp.fotos.sort(sortDataAns)
            } else //expressoes
            {

            }
            setTimeout(esconderCarregando, 200);
        }, 400);
        
    });

    $('.btnInserirQuadroFotosFim').click(function () {
        mostrarCarregando();
        setTimeout(fnInserirQuadroFoto, 10, true); 
    });

    $('.btnInserirQuadroFotosEm').click(function () {

        var fnInserirEm = function (sucesso, textoPrompt) {
            if (sucesso) {
                mostrarCarregando();
                setTimeout(fnInserirQuadroFoto, 10, false, textoPrompt); 
            }
        }
        promptDone("", "Inserir", "number", fnInserirEm, "modal-sm", "Inserir na Posição:");

    });
     
    $('.btnExcluirTemp').click(function () {
        var arrSelecao = [];
        var elSelect = $('#box-fotos1').finderSelect('selected');
        for (var i = 0; i < elSelect.length; i++) {
            arrSelecao.push($(elSelect[i]).attr("finderSelectIndex"));
        }

        var arrObjSelTemp = fnSelecionarPorIndex(appBoxTemp, arrSelecao)

        for (var i = 0; i < arrObjSelTemp.length; i++) {
            fnRemoverObjeto(appBoxTemp, arrObjSelTemp[i]);
        }
        $('#box-fotos1').finderSelect('unHighlightAll');
    });

    $('.btnRenomear').click(function () {
        var arrSelecao = [];
        var elSelect = $('#box-fotos1').finderSelect('selected');
        for (var i = 0; i < elSelect.length; i++) {
            arrSelecao.push($(elSelect[i]).attr("finderSelectIndex"));
        }

        var tmpArquivoNomeSelecionado = ""
        if (arrSelecao.length == 1) {
            tmpArquivoNomeSelecionado = fnSelecionarPorIndex(appBoxTemp, arrSelecao)[0].ArquivoNome;  
        }

        var fnRenomearDone = function (sucesso, textoPrompt) {
            if (sucesso) {
                var strNovoNome = textoPrompt;
                var arrObjSelTemp = fnSelecionarPorIndex(appBoxTemp, arrSelecao);
                var arrFotoModel = [];
                for (var i = 0; i < arrObjSelTemp.length; i++) {
                    arrObjSelTemp[i].ArquivoNome = strNovoNome + (arrObjSelTemp.length > 1 ? " (" + (i + 1) + ")" : "");
                    arrFotoModel.push(MontarArquivoFoto(arrObjSelTemp[i]));
                } 
                SalvarQuadroFotosNome(arrFotoModel, idSolicitacaoInspecao, arrObjSelTemp);
            }
        }
        if (arrSelecao.length > 0)  
        promptDone("", "Renomear", "text", fnRenomearDone, "modal-sm", "Novo nome:", tmpArquivoNomeSelecionado);
    });

    $('.btnReposicionar').click(function () {
        var arrSelecao = [];
        var elSelect = $('#box-fotos2').finderSelect('selected');

        for (var i = 0; i < elSelect.length; i++) {
            arrSelecao.push($(elSelect[i]).attr("finderSelectIndex"));
        }
        //var arrObjSelTemp = fnSelecionarPorIndex(appBoxQuadro, arrSelecao);

        fnMoverDone = function (sucesso, textoPrompt) {
            if (sucesso) {
                var iPos = parseInt(textoPrompt);
                iPos = iPos < 1 ? 1 : iPos;
                iPos = iPos + 1 > appBoxQuadro.fotos.length ? appBoxQuadro.fotos.length : iPos;
                iPos = iPos - 1  
                appBoxQuadro.fotos.move(arrSelecao, iPos); 

                SalvarQuadroFotosPosicao();
                $('#box-fotos2').finderSelect('unHighlightAll');
            }
        }

        promptDone("", "Reposicionar", "number", fnMoverDone, "modal-sm", "Nova Posição:")
    });

    $('.btnRemoverQuadro').click(function () {
        var arrSelecao = [];
        var elSelect = $('#box-fotos2').finderSelect('selected');
        for (var i = 0; i < elSelect.length; i++) {
            arrSelecao.push($(elSelect[i]).attr("finderSelectIndex"));
        }

        var arrObjSelTemp = fnSelecionarPorIndex(appBoxQuadro, arrSelecao)

        for (var i = 0; i < arrObjSelTemp.length; i++) {
            var foto = arrObjSelTemp[i]
            fnRemoverObjeto(appBoxQuadro, foto);
            InserirFinal(appBoxTemp, foto);

            SalvarQuadroFotosRemover(foto);

        }
        SalvarQuadroFotosPosicao();
        $('#box-fotos1').finderSelect('unHighlightAll');
    });

});


function MontarArquivoFoto(foto, indexFotosPosicao) {
    var objFoto = {};
    $.extend(objFoto, foto);
    objFoto.ImgData = null;

    return objFoto;
}

function fnInserirQuadroFoto(bFinal, indexPosicaoInserir) {
    
    var arrSelecao = [];
    var elSelect = $('#box-fotos1').finderSelect('selected');
    for (var i = 0; i < elSelect.length; i++) {
        arrSelecao.push($(elSelect[i]).attr("finderSelectIndex"));
    }

    var arrObjSelTemp = fnSelecionarPorIndex(appBoxTemp, arrSelecao)

    for (var i = 0; i < arrObjSelTemp.length; i++) {

        if (bFinal) {
            InserirFinal(appBoxQuadro, arrObjSelTemp[i]);
        }
        else {
            if (isNaN(indexPosicaoInserir))
                throw "Erro: fnInserirQuadroFoto"
            var iPos = parseInt(indexPosicaoInserir);
            iPos = iPos < 1 ? 1 : iPos;
            iPos = iPos > appBoxQuadro.fotos.length ? appBoxQuadro.fotos.length + 1 : iPos;
            iPos = iPos - 1;
            InserirEm(appBoxQuadro, iPos, arrObjSelTemp[i]);
        }
        fnRemoverObjeto(appBoxTemp, arrObjSelTemp[i]);
    }

    SalvarQuadroFotosPosicao();

    $('#box-fotos1').finderSelect('unHighlightAll');

    esconderCarregando()
}

function sortNumeroAns(a, b) {
    var dataA = parseInt(a.fotoAnsNumero);
    var dataB = parseInt(b.fotoAnsNumero);
    if (dataA > dataB) {
        return 1;
    }
    if (dataA < dataB) {
        return -1;
    }
    // a must be equal to b
    return 0;
};

function sortDataAns(a, b) {
    var dataA = new Date(a.ArquivoDataModificacao);
    var dataB = new Date(b.ArquivoDataModificacao);
    if (dataA > dataB) {
        return 1;
    }
    if (dataA < dataB) {
        return -1;
    }
    // a must be equal to b
    return 0;
};

function SalvarQuadroFotosPosicao() { 
    var quadroFotoModel = [];
    for (var i = 0; i < appBoxQuadro.fotos.length; i++) {
        var foto = appBoxQuadro.fotos[i];
        foto.QuadroFotosPosicao = i;

        foto.ArquivoNome = null;
        foto.GuidFoto = null;
        foto.ArquivoDataModificacao = null;
        foto.ArquivoExtencao = null;
        foto.ArquivoTamanhoBytes = null;

        quadroFotoModel.push({ Id: foto.Id, QuadroFotosPosicao: foto.QuadroFotosPosicao })
    }
    
    $.ajax({
        url: "/Foto/SalvarQuadroFotosPosicao/" + idSolicitacaoInspecao,
        data: JSON.stringify({  quadroFotoModel: quadroFotoModel  }),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            alertaResponseResult(result); 
        },
        error: function (xhr, ajaxOptions, thrownError) {
            esconderCarregando();
            erroMensagem(xhr.status + " : " + thrownError);
             
        }
        
    });
}

function SalvarQuadroFotosRemover(foto) { 
     
    $.ajax({
        url: "/Foto/SalvarQuadroFotosRemover/",
        data: JSON.stringify({ Id: foto.Id, IdSolicitacao: idSolicitacaoInspecao }),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            alertaResponseResult(result); 
        },
        error: function (xhr, ajaxOptions, thrownError) {
            esconderCarregando();
            erroMensagem(xhr.status + " : " + thrownError);

        }
    });
}

function SalvarQuadroFotosNome(arrFotoModel, idSolicitacao, arrObjSelTemp) {

    $.ajax({
        url: "/Foto/SalvarQuadroFotosNome/" + idSolicitacao,
        data: JSON.stringify({ quadroFotoModel: arrFotoModel }),
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (result) {
            alertaResponseResult(result);

            for (var i = 0; i < result.content.length; i++) {
                var fotoResult = result.content[i];

                var fotoQuadro = arrObjSelTemp.find(function (x) { return x.Id == fotoResult.Id; });
                fotoQuadro.fotoAnsNumero = fotoResult.fotoAnsNumero
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            esconderCarregando();
            erroMensagem(xhr.status + " : " + thrownError);

        }
    });

}