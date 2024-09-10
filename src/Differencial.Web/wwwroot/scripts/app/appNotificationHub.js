
appNotificacao = new Vue({
    el: '#ddlNotificacao',
    data: { notificacoes: [] },
    methods: {
        load: function () {
            getJSONResponseResult({
                url: "/Operador/ListarMinhasNotificacoes",
                fnSucesso: function (responseJson) {
                    for (var i = 0; i < responseJson.content.length; i++) {
                        appNotificacao.notificacoes.splice(0, 0, responseJson.content[i]);
                    }
                },
                bMostrarCarregando: false
            })
        },

        add: function (notificacaoDTO) {
            notificacaoDTO = $.parseJSON(notificacaoDTO)
            appNotificacao.notificacoes.splice(0, 0, notificacaoDTO);
        },

        deleteAll: function () {
            ajaxJsonResponseResult({
                url: "/Operador/ExcluirTodasMinhasNotificacoes",
                fnSucesso: function (data) { appNotificacao.notificacoes.splice(0) },
                bMostrarCarregando: false
            });
        }
    }
});

$(function () {

    //appNotificacao.load();
    //var connection = new signalR.HubConnectionBuilder()
    //    .withUrl("/hubs")
    //    /*.withAutomaticReconnect()*/
    //    .build();

    //connection.start().then(function () {
    //   // console.log("signalr connection start")
    //}).catch(function (err) {
    //    console.log(err)
    //    return console.error(err.toString());
    //});

    //connection.on("NovaNotificacao", function (message) {
    //    appNotificacao.add(message);
    //});

    
});