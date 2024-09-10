/*
 *
 *   INSPINIA - Responsive Admin Theme
 *   version 2.4
 *
 */


$(document).ready(function () {

    // Add body-small class if window less than 768px
    if ($(this).width() < 769) {
        $('body').addClass('body-small')
    } else {
        $('body').removeClass('body-small')
    }

    SkinConfig();

    // MetsiMenu
    $('#side-menu').metisMenu();

    // Collapse ibox function
    $('.collapse-link').click(function () {
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        var content = ibox.find('div.ibox-content');
        content.slideToggle(200);
        button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
        ibox.toggleClass('').toggleClass('border-bottom');
        setTimeout(function () {
            ibox.resize();
            ibox.find('[id^=map-]').resize();
        }, 50);
    });

    // Close ibox function
    $('.close-link').click(function () {
        var content = $(this).closest('div.ibox');
        content.remove();
    });

    // Fullscreen ibox function
    $('.fullscreen-link').click(function () {
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        $('body').toggleClass('fullscreen-ibox-mode');

        //***********
        var urlHref = document.location.href
        var urlSearch = (document.location.search.length === 0) ? "?" : "";
        var urlParamFullScreen = "";
        if (button.hasClass('fa-expand')) {
            if (urlHref.indexOf("fullscreen") === -1) {
                urlParamFullScreen = "&fullscreen=true";
            } if (urlHref.indexOf("fullscreen=false") > 0) {
                urlHref = urlHref.replace("fullscreen=false", "fullscreen=true")
            }
            window.history.pushState("", "", urlHref + urlSearch + urlParamFullScreen);
        } else if (button.hasClass('fa-compress')) {
            if (urlHref.indexOf("fullscreen") === -1) {
                urlParamFullScreen = "&fullscreen=false";
            } else if (urlHref.indexOf("fullscreen=true") > 0) {
                urlHref = urlHref.replace("fullscreen=true", "fullscreen=false")
            }
            $('body').removeClass('fullscreen-ibox-mode');
            window.history.pushState("", "", urlHref + urlSearch + urlParamFullScreen);
        }

        //***********

        button.toggleClass('fa-expand').toggleClass('fa-compress');
        ibox.toggleClass('fullscreen');
        setTimeout(function () {
            $(window).trigger('resize');
        }, 100);
         

    });

    // Close menu in canvas mode
    $('.close-canvas-menu').click(function () {
        $("body").toggleClass("mini-navbar");
        SmoothlyMenu();

        $.post("/Configuracao/Aplicar/", { chave: "collapse_menu", valor: $("body").hasClass("mini-navbar") ? 'on' : 'off' })
    });

    // Run menu of canvas
    $('body.canvas-menu .sidebar-collapse').slimScroll({
        height: '100%',
        railOpacity: 0.9
    });

    // Open close right sidebar
    $('.right-sidebar-toggle').click(function () {
        $('#right-sidebar').toggleClass('sidebar-open');
    });

    // Initialize slimscroll for right sidebar
    $('.sidebar-container').slimScroll({
        height: '100%',
        railOpacity: 0.4,
        wheelStep: 10
    });

    // Open close small chat
    $('.open-small-chat').click(function () {
        $(this).children().toggleClass('fa-comments').toggleClass('fa-remove');
        $('.small-chat-box').toggleClass('active');
    });

    // Initialize slimscroll for small chat
    $('.small-chat-box .content').slimScroll({
        height: '234px',
        railOpacity: 0.4
    });

    // Small todo handler
    $('.check-link').click(function () {
        var button = $(this).find('i');
        var label = $(this).next('span');
        button.toggleClass('fa-check-square').toggleClass('fa-square-o');
        label.toggleClass('todo-completed');
        return false;
    });

    // Minimalize menu
    $('.navbar-minimalize').click(function () {
        $("body").toggleClass("mini-navbar");
        SmoothlyMenu();
        //if (localStorageSupport)
        //    localStorage.setItem("collapse_menu", $("body").hasClass("mini-navbar") ? 'on' : 'off');
         
        ajaxJsonResponseResult({
            url: "/Configuracao/Aplicar/",
            data: { chave: "collapse_menu", valor: $("body").hasClass("mini-navbar") ? 'on' : 'off' },
            bMostrarCarregando: false
        });
        
    });

    // Tooltips demo
    $('body').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    });

    // Move modal to body
    // Fix Bootstrap backdrop issu with animation.css
    $('.modal').appendTo("body");

    fix_height();

    // Fixed Sidebar
    $(window).bind("load", function () {
        if ($("body").hasClass('fixed-sidebar')) {
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }
    });

    // Move right sidebar top after scroll
    $(window).scroll(function () {
        if ($(window).scrollTop() > 0 && !$('body').hasClass('fixed-nav')) {
            $('#right-sidebar').addClass('sidebar-top');
        } else {
            $('#right-sidebar').removeClass('sidebar-top');
        }
    });

    $(window).bind("load resize scroll", function () {
        if (!$("body").hasClass('body-small')) {
            fix_height();
        }
    });

    $("[data-toggle=popover]")
        .popover();

    // Add slimscroll to element
    $('.full-height-scroll').slimscroll({
        height: '100%'
    })


    // IMPLEMENTAÇÕES DIFFERENCIAL


    $("#btnSalvarDrop a").click(function () {
        var action = $(this).attr("acaoretorno");
        $("#retornosalvar").val(action);

        $("form:eq(0)").submit();
        return false;
    });


    if (($(".validation-summary-errors").length > 0 && $(".validation-summary-errors li:eq(0)").css("display") !== "none") ||
       (sTrim($("#myModalMessage .modal-body").text()).length > 0)) {

        setTimeout(function () {
            $("#myModalMessage").modal("show");

        }, 100);


       
    }
    $('#myModalMessage').on('hidden.bs.modal', function () { 
        $("#myModalMessage .modal-body").html("")
         
    });

    var url = document.location.toString();
    if (url.match('#')) {
        //$('.nav-tabs a[href="#' + url.split('#')[1] + '"]').tab('show');
        for (var iTab = 0; iTab < url.split('#').length ; iTab++) {
            var sTab = url.split('#')[iTab]
             $('.nav-tabs a[href="#' + sTab + '"]').trigger('click');
        } 
    }

    // Change hash for page-reload
    $('.nav-tabs a').on('shown.bs.tab', function (e) {
        //window.location.hash = e.target.hash;
    })
});



// Minimalize menu when screen is less than 768px
$(window).bind("resize", function () {
    if ($(this).width() < 769) {
        $('body').addClass('body-small')
    } else {
        $('body').removeClass('body-small')
    }
});


function SkinConfig() {
    if (localStorageSupport) {

        var collapse = localStorage.getItem("collapse_menu");
        var fixedsidebar = localStorage.getItem("fixedsidebar");
        var fixednavbar = localStorage.getItem("fixednavbar");
        var boxedlayout = localStorage.getItem("boxedlayout");
        var fixedfooter = localStorage.getItem("fixedfooter");

        var body = $('body');

        //if (collapse == 'on') {
        //    if (body.hasClass('fixed-sidebar')) {
        //        if (!body.hasClass('body-small')) {
        //            body.addClass('mini-navbar');
        //        }
        //    } else {
        //        if (!body.hasClass('body-small')) {
        //            body.addClass('mini-navbar');
        //        }

        //    }
        //}

        if (fixedsidebar === 'on') {
            body.addClass('fixed-sidebar');
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }

        if (fixednavbar === 'on') {
            $(".navbar-static-top").removeClass('navbar-static-top').addClass('navbar-fixed-top');
            body.addClass('fixed-nav');
        }

        if (boxedlayout === 'on') {
            body.addClass('boxed-layout');
        }

        if (fixedfooter === 'on') {
            $(".footer").addClass('fixed');
        }
    }
}

// check if browser support HTML5 local storage
function localStorageSupport() {
    return (('localStorage' in window) && window['localStorage'] !== null)
}

// For demo purpose - animation css script
function animationHover(element, animation) {
    element = $(element);
    element.hover(
        function () {
            element.addClass('animated ' + animation);
        },
        function () {
            //wait for animation to finish before removing classes
            window.setTimeout(function () {
                element.removeClass('animated ' + animation);
            }, 2000);
        });
}

function SmoothlyMenu() {
    if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
        // Hide menu in order to smoothly turn on when maximize menu
        $('#side-menu').hide();
        // For smoothly turn on menu
        setTimeout(
            function () {
                $('#side-menu').fadeIn(400);
            }, 200);
    } else if ($('body').hasClass('fixed-sidebar')) {
        $('#side-menu').hide();
        setTimeout(
            function () {
                $('#side-menu').fadeIn(400);
            }, 100);
    } else {
        // Remove all inline style from jquery fadeIn function to reset menu state
        $('#side-menu').removeAttr('style');
    }
}

// Dragable panels
function WinMove() {
    var element = "[class*=col]";
    var handle = ".ibox-title";
    var connect = "[class*=col]";
    $(element).sortable(
        {
            handle: handle,
            connectWith: connect,
            tolerance: 'pointer',
            forcePlaceholderSize: true,
            opacity: 0.8
        })
        .disableSelection();
}

// Limpa espaços em branco
function sTrim(str) {
    return str.replace(/^\s+/, '').replace(/\s+$/, '');
}


// Full height of sidebar
function fix_height() {
    var heightWithoutNavbar = $("body > #wrapper").height() - 61;
    $(".sidebard-panel").css("min-height", heightWithoutNavbar + "px");

    var navbarHeigh = $('nav.navbar-default').height();
    var wrapperHeigh = $('#page-wrapper').height();

    if (navbarHeigh > wrapperHeigh) {
        $('#page-wrapper').css("min-height", navbarHeigh + "px");
    }

    if (navbarHeigh < wrapperHeigh) {
        $('#page-wrapper').css("min-height", $(window).height() + "px");
    }

    if ($('body').hasClass('fixed-nav')) {
        $('#page-wrapper').css("min-height", $(window).height() - 60 + "px");
    }

}



//window.onbeforeunload = function () {
//    localStorage.setItem("IndBackNav", true);
//}

//function IndBackNavegacao() {

//    if (localStorageSupport() && localStorage.getItem("IndBackNav")) {         
//        localStorage.removeItem("IndBackNav");
//        alert("true");
//        return true;
//    }
//    alert("false");
//    return false;
//}
