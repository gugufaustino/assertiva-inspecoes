$(function () {

    grade = $('#grade').Gride({
        bSelecionavel: true,
        bfiltroColuna: true,
        bExportacao: true,
        ordering: true,
        order: [[1, 'desc']],
        columns: [
            { data: "0" },
            { data: "Id", "className": "text-right" },
            { data: "NomeAssunto", "className": "" },
            { data: "DataCadastro", "className": "text-center", type: 'date-euro' },
            { data: "DataModificacao", "className": "text-center", type: 'date-euro' },
        ],
        mapJsonParaObjeto: function (obj) {
            return {
                "0": "",
                Id: obj.Id,
                NomeAssunto: obj.NomeAssunto,
                DataCadastro: obj.DataCadastro,
                DataModificacao: obj.DataModificacao,
                DT_RowId: obj.Id
            }
        },


    });


    document.getElementById("btnAbrir").addEventListener("click", function () {
        grade.abrirRegistroSelecionado("/TipoAssunto/Editar/");
    });

    document.getElementById("btnExcluir").addEventListener("click", function () {
        grade.excluirRegistrosSelecionados("/TipoAssunto/Excluir/");
    });
}); 