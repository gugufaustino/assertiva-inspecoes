
$(document).ready(function () {
    $.extend($.validator.messages, jqueryValidatePtBr);
    AplicaJqueryValidation()

});

// fix: http://www.miguelborges.com/jquery-validation-plugin-twiiter-bootstrap-3/
function AplicaJqueryValidation() {
    $("form").each(function (i, obj) {
        if ($.data($(obj).get(0)).validator == null) {
            $(obj).validate({
                errorElement: 'span',
                errorClass: 'error field-validation-error',
                ignore: "input[type=hidden]",

                showErrors: function (errorMap, errorList) { /* função pesquisa se o item está oculto em aba e força o foco clicando na aba */

                    if (errorList.length > 0) {
                        var el = errorList[0].element;
                        var jqEl = $(el);

                        if (jqEl.is(":hidden")) {
                            console.log(jqEl.parentsUntil(".tab-pane").parent())
                            var tabPane = jqEl.parentsUntil(".tab-pane").parent().first();
                            var idtabPane = tabPane.attr("id");

                            $(".nav-tabs li a[href$=" + idtabPane + "]").trigger("click");
                        }
                    }
                    this.defaultShowErrors();
                },
                errorPlacement: function (error, element) {
                    if (element.parent('.input-group').length) {
                        error.insertAfter(element.parent());
                    } else {
                        error.insertAfter(element);
                    }
                }
            });
        }
    }); 

}
