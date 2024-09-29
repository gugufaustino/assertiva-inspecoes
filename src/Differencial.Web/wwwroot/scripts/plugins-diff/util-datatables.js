$.fn.dataTable.enum(['Pendente', 'Laudo Atrasado', 'Cancelado', 'Re-Agendado', 'Agendado']);

$.fn.dataTableCli = function () {
    var table = this.DataTable({
        bLengthChange: false,
        pageLength: 25,
        language: datatablesLangPtBr,
        dom: 'rt'
                + '<"#tbPag.pull-left" p><"#tbInfo.pull-left" i>'
                + '<"#tbFilt" <"btn-gride form-group" B> >'
                + 'Tlg<"clear">',
        buttons: [
            { extend: 'copy', text: 'Copiar', "className": "btn-sm" },
            { extend: 'excel', title: '', text: 'Baixar Excel', "className": "btn-sm", orientation: 'landscape' },
            { extend: 'pdf', title: '', text: 'Baixar PDF', "className": "btn-sm", orientation: 'landscape' },
        ],
        select: { style: 'os', selector: 'td' },
        order: [[1, 'desc']],

        columnDefs: [{
            className: 'select-checkbox',
            orderable: false,
            searchable: false,
            targets: 0
        },
        //{ 
        //    targets: 1,
        //    width: "200px",
        //},
        //{ 
        //    targets: 2,
        //    width: "200px",
        //}
        //
        ],
        //scrollX: 1024,
        //bScrollCollapse: true,
        //scrollCollapse: true,
        //autoWidth: false,



    });

    // Setup - add a text input to each footer cell
    $(this).find('tfoot th:not(:first)').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Filtrar ' + title + '"  title="Filtrar ' + title + '" />');
    });

    // Apply the search
    table.columns().every(function () {
        var that = this;
        $(this.footer()).on('keyup change', 'input', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });

    // corrigir css botões classe
    setTimeout(function () {
        $(".btn-gride a.btn-primary").removeClass("btn-default").css("border-color", "#d4b1b5");
    }, 400);

    return table;
};



$.fn.dataTableFw = function (bSelecionavel, bfiltroRodape, bOrdenar, bExportacao, oColumns, oOrder) {
    var _datatablesLangPtBr = datatablesLangPtBr;
    if (bSelecionavel == null || bSelecionavel == undefined || bSelecionavel == false) {
        bSelecionavel = false;
        _datatablesLangPtBr.select.rows["0"] = "";
    }

    if (bfiltroRodape == null || bfiltroRodape == undefined)
        bfiltroRodape = false;

    if (bOrdenar == null || bOrdenar == undefined)
        bOrdenar = false;

    if (bExportacao == null || bExportacao == undefined)
        bExportacao = false;

    if (oColumns == null || oColumns == undefined)
        oColumns = null;

    if (oOrder == null || oOrder == undefined)
        oOrder = [];

    var defaultConfig = {
        bLengthChange: false,
        pageLength: 25,
        language: _datatablesLangPtBr,
        ordering: bOrdenar,
        order: bOrdenar ? oOrder : [],
        dom: 'rt'
                + '<"#tbPag.pull-left" p>'
                + (bExportacao ? '<"#tbInfo.pull-left" i>' : '<"#tbInfo.pull-right" i>')
                + '<"#tbFilt" <"btn-gride form-group" B> >'
                + 'Tlg<"clear">',
        buttons: bExportacao ? [
                { extend: 'copy', text: 'Copiar', "className": "btn-sm" },
                { extend: 'excel', title: 'consulta', text: 'Baixar Excel', "className": "btn-sm", orientation: 'landscape' },
                { extend: 'pdf', title: 'consulta', text: 'Baixar PDF', "className": "btn-sm", orientation: 'landscape' }, ]
                : [],
        columnDefs: bSelecionavel ? [{ className: 'select-checkbox', orderable: false, searchable: false, targets: 0 }] : [],
        select: bSelecionavel ? { style: 'os', selector: 'td' } : false,
        columns: oColumns

    }

    var table = this.DataTable(defaultConfig);

    // Setup - add a text input to each footer cell
    if (bfiltroRodape)
        $(this).find('tfoot th:not(:first)').each(function () {
            var title = $(this).text();
            $(this).html('<input type="text" placeholder="Filtrar ' + title + '" title="Filtrar ' + title + '" />');
        });

    // Apply the search
    table.columns().every(function () {
        var that = this;
        $(this.footer()).on('keyup change', 'input', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });

    // corrigir css botões classe
    setTimeout(function () {
        $(".btn-gride a.btn-primary").removeClass("btn-default").css("border-color", "#d4b1b5");
    }, 400);

    table.mapJsonParaObjeto = function () { };

    table.atualizarDados = function (arrObj, fnMapObject) {
        table.clear();
        for (var i = 0; i < arrObj.length; i++) {
            var oItem = arrObj[i]
            table.row.add(fnMapObject.call(this, oItem));
        }
        table.draw();
    };

    table.abrirRegistroSelecionado = function (urlLocation) {
        var arrSel = table.rows({ selected: true }).ids().toArray();

        if (arrSel.length == 1) {
            mostrarCarregando();

            window.location = urlLocation + arrSel[0];
        } else if (arrSel.length == 0) {
            alertaMensagem("Selecione um item da lista");
        } else {
            alertaMensagem("Selecione apenas um item da lista");
        }
    };

    table.excluirRegistrosSelecionados = function (urlPost) {
        try {
            var arrSel = table.rows({ selected: true }).ids().toArray();

            if (arrSel.length == 0) {
                alertaMensagem("Selecione um item da lista");
            } else {

                var msgConfirm = (arrSel.length == 1) ? "Confirma a exclusão do registro selecionado ?" : "Confirma a exclusão dos registros selecionados ?";

                confirmaPost(msgConfirm, function () {
                    mostrarCarregando();
                    $.post(urlPost, { Id: arrSel }, function (data) {
                        if (!data.success) {
                            alertaResponseResult(data);
                        } else {
                            alertaResponseResult(data);

                            table.atualizarDados(data.content, table.mapJsonParaObjeto)
                        }
                        esconderCarregando();
                    });

                });

            }

        } catch (e) {
            alert("Exceção não tratada, contate o suporte técnico - excluirRegistrosSelecionados:\n" + e.message);
        }



    };

    return table;
};

$.fn.Gride = function (options) {
    var _datatablesLangPtBr = datatablesLangPtBr;
    if (options.bSelecionavel == null || options.bSelecionavel == undefined || options.bSelecionavel == false) {
        options.bSelecionavel = false;
        _datatablesLangPtBr.select.rows["0"] = "";
    }

    var setting = {
        bLengthChange: false,
        pageLength: 25,
        language: _datatablesLangPtBr,
        ordering: false,
        order: [],
        dom: 'rt'
                + '<"#tbPag.pull-left" p>'
                + (options.bExportacao != undefined && options.bExportacao ? '<"#tbInfo.pull-left" i>' : '<"#tbInfo.pull-right" i>')
                + '<"#tbFilt" <"btn-gride form-group" B> >'
                + 'Tlg<"clear">',
        buttons: options.bExportacao != undefined && options.bExportacao ? [
                { extend: 'copy', text: 'Copiar', "className": "btn-sm" },
                { extend: 'excel', title: 'tabela de dados', text: 'Baixar Excel', "className": "btn-sm", orientation: 'landscape' },
                { extend: 'pdf', title: 'tabela de dados', text: 'Baixar PDF', "className": "btn-sm", orientation: 'landscape' }, ]
                : [],
        columnDefs: options.bSelecionavel != undefined && options.bSelecionavel ? [{ className: 'select-checkbox', orderable: false, searchable: false, targets: 0 }] : [],
        select: options.bSelecionavel != undefined && options.bSelecionavel ? { style: 'os', selector: 'td' } : false,
        // columns: oColumns

        mapJsonParaObjeto: function (obj) { return {} }
    }
    $.extend(setting, options);

    setting.createdRow = function (row, data, dataIndex) {
        if (data.IndUrgente != undefined && (data.IndUrgente == true || data.IndUrgente === "True")) {
            $(row).addClass('indUrgente');
        }
    }

    var table = this.DataTable(setting);

    //filtro colunas 
    if (setting.bfiltroColuna) {

        var header = $(table.tables().header()[0]);
        header.append("<tr role='row' class='filtroColuna'></tr>")
        var trFiltro = header.find("tr:eq(1)")

        table.columns().every(function (i) {
            var that = this;

            if (setting.bSelecionavel && i == 0) {
                $(trFiltro).append('<th class="select-checkbox"></th>');
            } else if (that.visible()) {

                var th = $('<th></th>')
                var input = $('<input type="text" placeholder="Filtrar" title="Filtrar" />').on('keyup change', function () {
                    if (that.search() !== this.value) {
                        that.search(this.value)
                            .draw();
                    }
                });

                th.append(input);
                trFiltro.append(th)
            }
        });



    }

    //filtro rodape 
    // Setup - add a text input to each footer cell
    if (setting.bfiltroRodape)
        $(this).find('tfoot th:not(:first)').each(function () {
            var title = $(this).text();
            $(this).html('<input type="text" placeholder="Filtrar ' + title + '" title="Filtrar ' + title + '" />');
        });

    // Apply the search
    table.columns().every(function () {
        var that = this;

        $(this.footer()).on('keyup change', 'input', function () {
            if (that.search() !== this.value) {
                that.search(this.value)
                    .draw();
            }
        });

    });




    // corrigir css botões classe
    setTimeout(function () {
        $(".btn-gride a.btn-primary").removeClass("btn-default").css("border-color", "#d4b1b5");
    }, 400);

    table.mapJsonParaObjeto = setting.mapJsonParaObjeto;

    table.atualizarDados = function (arrObj) {
        table.clear();
        for (var i = 0; i < arrObj.length; i++) {
            var oItem = arrObj[i]

            table.row.add(table.mapJsonParaObjeto.call(this, oItem));
        }
        table.draw();
    };


    table.atualizarLinha = function (indexRow, dataRow) {

        grideTodasSolic
            .row(0).data(roww)
            .draw();

    };

    table.abrirRegistroSelecionado = function (urlLocation) {
        var arrSel = table.rows({ selected: true }).ids().toArray();

        if (arrSel.length == 1) {
            mostrarCarregando(); 
            window.location = urlLocation + arrSel[0];
        } else if (arrSel.length == 0) {
            alertaMensagem("Selecione um item da lista");
        } else {
            alertaMensagem("Selecione apenas um item da lista");
        }
    };

    table.abrirRegistroSelecionadoModal = function (urlLocation, modalSize) {
        var arrSel = table.rows({ selected: true }).ids().toArray();

        if (arrSel.length == 1) {
            loadModal(urlLocation + arrSel[0], modalSize == null ? "" : modalSize);
        } else if (arrSel.length == 0) {
            alertaMensagem("Selecione um item da lista");
        } else {
            alertaMensagem("Selecione apenas um item da lista");
        }
    };

    table.excluirRegistrosSelecionados = function (urlPost) {
        try {
            var arrSel = table.rows({ selected: true }).ids().toArray();

            if (arrSel.length == 0) {
                alertaMensagem("Selecione um item da lista");
            } else {

                var msgConfirm = (arrSel.length == 1) ? "Confirma a exclusão do registro selecionado ?" : "Confirma a exclusão dos registros selecionados ?";
                console.log(arrSel)
                confirmaPost(msgConfirm, function () {
                    mostrarCarregando();
                    $.post(urlPost, { Id: arrSel }, function (data) {
                        if (!data.success) {
                            alertaResponseResult(data);
                        } else {
                            alertaResponseResult(data);
                            if (data.content != null)
                                table.atualizarDados(data.content)
                            else if (setting.ajax != undefined)
                                table.ajax.reload();
                        }
                        esconderCarregando();
                    });

                });

            }

        } catch (e) {
            alert("Exceção não tratada, contate o suporte técnico - excluirRegistrosSelecionados:\n" + e.message);
        }
    };
    table.getSetting = function () { return setting };
    return table;
}