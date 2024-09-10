function editarFilial(params) {
    var _Id = params.Id; 
    var _Form = params.Form;
    var _fnSalvarCallback = params.fnSalvarCallback;
    this._btnSalvar = params.btnSalvar;

    $(this._btnSalvar).click(salvar);
 

    function salvar() { 
        if ($(_Form).valid()) {
            formPostModal(_Form, function (result) {

                if (result.success) {
                    _Id = result.content;
                    $(_Form).find("#Id").val(_Id);
                    _fnSalvarCallback();
                }
            });
        }
    }
    function salvarCallback() {
        _fnSalvarCallback();
    } 
}


 