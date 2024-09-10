$(document).ready(function () {

    // Remove Validação do form para o navegador não validar pelos atributos dos inputs
    //  $("form").attr("novalidate", "novalidate");

    definePicker();
    defineSelect2();

});

function definePicker() {
    //$('.input-group.date').datepicker("destroy");
    $('.input-group.date').datepicker({
        todayBtn: "linked",
        keyboardNavigation: true,
        forceParse: false,
        calendarWeeks: true,
        autoclose: true,
        language: 'pt-BR',
        format: 'dd/mm/yyyy',
    });

    $('.clockpicker').clockpicker();
}
function defineSelect2() {
    //setTimeout(function () {
    $(".select2_multiple").select2({
        theme: "bootstrap",
        language: "pt-BR"
    });
    $(".select2_simples").select2({
        placeholder: "Selecione...",
        allowClear: true,
        theme: "bootstrap",
        language: "pt-BR"
    });
    //}, 2);

}

function Calendarios() {
    $('body').on('click', '.input-group.date', function (event) {
        $(this).datepicker({
            todayBtn: "linked",
            keyboardNavigation: true,
            forceParse: false,
            calendarWeeks: true,
            autoclose: true,
            language: 'pt-BR',
            format: 'dd/mm/yyyy',
        }).focus();
    });
    //$(".calendario").mask("99/99/9999");
    $(".calendario").datepicker("destroy");

    $(".calendario:not([readonly='readonly']):not([readonly='true'])").datepicker({
        altFormat: "dd/mm/yy",
        dateFormat: "dd/mm/yy",
        dayNames: ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado"],
        dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sab"],
        monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        firstDay: 1,
        changeMonth: true,
        changeYear: true,
        showOn: 'focus',
        buttonText: "Alterar",
        onClose: function () {
            try {
                $(this).valid();
            }
            catch (e) {
            }
        }
    }).bind('click keyup focus', function () {
        if ($('#ui-datepicker-div :last-child').is('table'))
            $('#ui-datepicker-div').append('<p class="alerta-datepicker">Pressione ESC para fechar</p>');
    });
}

//function removeAll(querySelector) {
//    var obj = document.querySelector(querySelector);
//    qtd = obj.options.length;
//    for (var i = 0; i < qtd ; i++)
//        obj.remove(0); 
//}