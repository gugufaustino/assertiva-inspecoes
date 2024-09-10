function abaSolicitante(params) {
    var _btnAbrir = params.btnAbrir;
    var _btnNovo = params.btnNovo;
    var _btnExcluir = params.btnExcluir;
    var _Id = params.Id;
      
    $(_btnAbrir).click(abrir);

    $(_btnNovo).click(novo);

    $(_btnExcluir).click(excluir);

    var _gride = $(params.gride).Gride({
        bSelecionavel: true,
        bfiltroColuna: true,
        ordering: true,
        autoWidth: false,
        ajax: {
            url: "/Seguradora/GridSeguradoraSolicitante/" + _Id,
            type: "GET"
        },
        order: [[1, 'desc']],
        columns: [
              { data: "DT_RowId" },
              { data: "NomeOperador", "className": "" },
              { data: "TipoSolicitante", "className": "text-center" },
              { data: "Telefone", "className": "" },
              { data: "Email", "className": "" },
              { data: "DataCadastro", "className": "text-center", type: 'date-euro' },
              { data: "DataModificacao", "className": "text-center", type: 'date-euro' }
        ]
    });
    this._gride = _gride;

    function abrir() {
        _gride.abrirRegistroSelecionadoModal("/Solicitante/Editar/", "");
    }
    function novo () {
        loadModal("/Solicitante/Inserir/?idSeguradora=" + _Id);
    }
    function excluir() {
        _gride.excluirRegistrosSelecionados("/Solicitante/Excluir/");
    }
} 
 
function abaFilial(params) {
    var _btnAbrir = params.btnAbrir;
    var _btnNovo = params.btnNovo;
    var _btnExcluir = params.btnExcluir;
    var _Id = params.Id;

    $(_btnAbrir).click(abrir);

    $(_btnNovo).click(novo);

    $(_btnExcluir).click(excluir);

    var _gride = $(params.gride).Gride({
        bSelecionavel: true,
        bfiltroColuna: true,
        ordering: true,
        autoWidth: false,
        ajax: {
            url: "/Seguradora/GridSeguradoraFilial/" + _Id,
            type: "GET"
        },
        order: [[1, 'desc']],
        columns: [
              { data: "DT_RowId" },
              { data: "NomeFlial", "className": "" }, 
              { data: "DataCadastro", "className": "text-center", type: 'date-euro' },
              { data: "DataModificacao", "className": "text-center", type: 'date-euro' }
        ]
    });
    this._gride = _gride;

    function abrir() {
        _gride.abrirRegistroSelecionadoModal("/Seguradora/EditarFilial/", "");
    }
    function novo() {
        loadModal("/Seguradora/InserirFilial/?idSeguradora=" + _Id);
    }
    function excluir() {
        _gride.excluirRegistrosSelecionados("/Seguradora/ExcluirFilial/");
    }
}
