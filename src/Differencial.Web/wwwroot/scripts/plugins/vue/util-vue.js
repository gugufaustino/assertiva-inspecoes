
Vue.filter('dateToString', function (value) {
    var date = new Date(value);
    return date.toLocaleDateString() + " " + date.toLocaleTimeString();
})

//TODO Colocar essas funções em um arquivo de util do quadro de fotos
function fnSelecionarPorIndex(app, arrSelecao) {
    arrObjSelecionados = new Array();
    for (var i = 0; i < arrSelecao.length; i++) {
        arrObjSelecionados.push(app.fotos[arrSelecao[i]]);
    }
    return arrObjSelecionados;
}

function fnRemoverObjeto(app, objItem) {
    var i = app.fotos.indexOf(objItem);
    fnRemoverIndex(app, i);
}

function fnRemoverIndex(objApp, index) {
    objApp.fotos.splice(index, 1)
}

//function RemoverKey(objApp, index){
//    objApp.fotos.splice(index, 1)
//}

function InserirEm(objApp, index, novoItem) {
    objApp.fotos.splice(index, 0, novoItem);
}

function InserirFinal(objApp, novoItem) {
    objApp.fotos.push(novoItem);
}

function Substituir(appItems, index, novoItem) {
    Vue.set(appItems, index, novoItem)
}
 