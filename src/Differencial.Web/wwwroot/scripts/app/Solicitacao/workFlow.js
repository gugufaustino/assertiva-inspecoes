$(document).ready(function () {

    $('#btnSalvar').click(function () {
        $("form").submit()
        //function (event) {
        //    //return true;
        //    //event.preventDefault();
        //}); 
    });

    $('#btnApropriar').click(function () {
        var id = $(this).attr("data-id");
        var url = $(this).attr("data-href");

        var varMens = "Deseja se apropriar dessa solicitação ?";

        confirmaPost(varMens, function () {
            mostrarCarregando();
            $.post(url, { Id: id },
                function (data) {
                    alertaResponseResult(data);
                    if (data.success) {
                        window.location.reload(true);
                    }
                    esconderCarregando();
                });
        });

    });

    $('#btnConcluir').click(function () {
        var id = $(this).attr("data-id");
        var url = $(this).attr("data-href");

        var varMens = "Deseja concluír a solicitação ?";

        confirmaPost(varMens, function () {
            mostrarCarregando();
            $.post(url, { Id: id },
                function (data) {
                    alertaResponseResult(data);
                    if (data.success) {
                        window.location.reload(true);
                    }
                    esconderCarregando();
                });
        });

    });

    $('#btnDevolver, #btnEnviar, #btnCancelar').click(function (e) {
        var id = $(this).attr("data-id");
        var url = $(this).attr("data-href");

        loadModal(url)

    });


});
