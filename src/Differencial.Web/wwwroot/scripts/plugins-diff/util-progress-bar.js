

function Progress(options) {
    var Progress = {};
    Progress.version = '1.0';


    const maxProgress = 100;
    Progress.progresstotalQueue = 0;
    Progress.sending = 0;
    Progress.complete = 0;
    Progress.progress = 0;

    Progress.DOM = null;

    Progress.settings = {
        idinc: "progress-overlay-",
        title: "",
        htmlTemplate: '<div id="{idinc}" class="overlay overlay-progress text-center"> ' +
               '    <div class="box-overlay"> ' +
               '       <h3 class="m-t-none">{title}</h3> ' +
               '       <div class="progress m-b-none">' +
               '           <div class="progress-bar progress-bar-striped active progress-bar-danger text-nowrap" role="Progress" aria-valuenow="{progress}" aria-valuemin="0" aria-valuemax="100" style="width:{progress}%">' +
               ' ' +
               '         </div>' +
               '       </div>' +
               '    </div>' +
               '</div>'


    }

    var _settings = Progress.settings;

    Init = function (options) {
        var key, value;
        for (key in options) {
            value = options[key];
            if (value !== undefined && options.hasOwnProperty(key)) _settings[key] = value;
        }

        //random Id idinc;

        //create
        _settings.htmlTemplate = _settings.htmlTemplate.replaceAll("{idinc}", _settings.idinc);
        _settings.htmlTemplate = _settings.htmlTemplate.replaceAll("{title}", _settings.title);
        _settings.htmlTemplate = _settings.htmlTemplate.replaceAll("{progress}", Progress.progress);

        Progress.DOM = $(_settings.htmlTemplate).get(0);

        //apend body
        $("body").append(Progress.DOM);

        return this;
    };

    Progress.destroy = function () {
        var id = Progress.settings.idinc;
        $("#" + id).hide(0, Progress.DOM.remove);

        Progress = null;
    }

    Progress.incSend = function (qtdQueue) {
        Progress.progresstotalQueue = qtdQueue;
        Progress.sending += 1;
        refresh();
    }

    Progress.incComplete = function () {
        if (Progress.complete < Progress.sending)
            Progress.complete += 1
        refresh();
    }
    Progress.getTotal = function () {
        return Progress.progresstotalQueue + Progress.sending;
    }
    refresh = function () {
        Progress.progress = maxProgress / Progress.getTotal() * Progress.complete;

        // "Enviando/paralelo" + (Progress.sending - Progress.complete)

        $(Progress.DOM).find("h3").text("ENVIANDO");
        $(Progress.DOM).find(".progress-bar").css("width", Progress.progress + "%");
        if (Progress.complete > 0)
            $(Progress.DOM).find(".progress-bar").text( Progress.complete + " de "  + Progress.getTotal());


        if (Progress.getTotal() - Progress.complete === 0) {
            setTimeout(Progress.Done, 1000);
        } else {
            show();
        }

    };

    Progress.Done = function () {
        var id = Progress.settings.idinc;
        $("#" + id).hide();

        Progress.progresstotalQueue = 0;
        Progress.sending = 0;
        Progress.complete = 0;

    }
    show = function () {
        $(Progress.DOM).show();
    }

    Init(options);

    return Progress;
}