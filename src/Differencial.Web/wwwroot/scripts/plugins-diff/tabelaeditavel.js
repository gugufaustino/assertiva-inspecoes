

$("form").on("click", "#btnAddRow.dropdown-menu a, #btnAddRow.t1", function () {

    var containerTabelaIntervaloEditavel = $(this).parentsUntil(".TabelaIntervaloEditavel").parent();
    containerTabelaIntervaloEditavel = containerTabelaIntervaloEditavel.first();

    var classTemplate = ""; // Equivalente ao TiposLancamentoValor/TipoQuantitativoVariacao: {DeAte, AcimaDe}
    if ($(this).hasClass("t1")) {
        classTemplate = ".t1";      // DeAte
    } else if ($(this).hasClass("t2")) {
        classTemplate = ".t2";      // AcimaDe
    } else {
        return;
    }

    var trTemplate = containerTabelaIntervaloEditavel.find('template' + classTemplate).html();

    var iLast = 0;
    var domTrLast = containerTabelaIntervaloEditavel.find(".row[tbeditindex]:last");

    if (domTrLast !== null && !isNaN(domTrLast.attr("tbeditindex"))) {

        iLast = parseInt(domTrLast.attr("tbeditindex"));
        iLast = iLast + 1;
    }
    
    trTemplate = trTemplate.replace('tbeditindex="0"', 'tbeditindex="' + iLast + '"');
    trTemplate = trTemplate.replaceAll('[#].', '[' + iLast + '].');

    if (iLast > 0) { 
         //Regra de Interface - Somente ultimo registro pode ser do tipo "Acima De", por tanto se já houver assume-se que não pode mais incluir nova linha
         if (domTrLast.find("input[name$='.TipoQuantitativoVariacao']").val() === "AcimaDe" ) {
             validacaoMensagem("Não é permitido adicionar novo 'intervalo de valores' pois já há um do tipo 'Acima De'.")
             return;
        }

        $(trTemplate).insertAfter(domTrLast)

    } else {
      //  console.log(containerTabelaIntervaloEditavel.get(0))
        containerTabelaIntervaloEditavel.prepend(trTemplate)
    }
});


$("form").on("click", "#btnExcluirRowIntervaloEditavel", function () {

    var containerTabelaIntervaloEditavel = $(this).parentsUntil(".TabelaIntervaloEditavel").parent();

    $(this).parentsUntil(".row").parent().remove();

    //Re-sincroniza indices do html
    $(containerTabelaIntervaloEditavel).find(".row[tbeditindex]").each(function (iNovo, e) {
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


$("form").on("change", "input[name$='.IndPreAcordo']", function () {

    var containerTabelaIntervaloEditavel = $(this).parentsUntil(".row").parent();
 
    var lblValorLancamento = $(containerTabelaIntervaloEditavel).find("label[id$='.lblValorLancamento']");
    var inputValorLanvamento = $(containerTabelaIntervaloEditavel).find("input[name$='.ValorLancamento']");
    var labelErrorValorLanvamento = $(containerTabelaIntervaloEditavel).find("span[id$='_ValorLancamento-error']");

    if ($(this).is(':checked')) {

        inputValorLanvamento.val("");
        inputValorLanvamento.attr("disabled", "disabled")


        lblValorLancamento.removeClass("required");
        inputValorLanvamento.removeAttr("required");
        inputValorLanvamento.removeClass("error");
        inputValorLanvamento.removeClass("field-validation-error");
        labelErrorValorLanvamento.hide();

    } else {
        inputValorLanvamento.removeAttr("disabled");

      
        lblValorLancamento.addClass("required");
        inputValorLanvamento.attr("required", "required");
    }  
    

});