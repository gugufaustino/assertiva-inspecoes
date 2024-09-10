 

gride = $('#gridSeguradora').dataTableFw(true, true, true, true, null, [[2, 'asc']]);
$("#btnAbrir").click(function () {
    gride.abrirRegistroSelecionado("/Seguradora/Editar/");
});

$("#btnExcluir").click(function () {
    gride.excluirRegistrosSelecionados("/Seguradora/Excluir/");
});

 