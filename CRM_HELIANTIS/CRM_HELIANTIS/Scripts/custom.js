$(function () {
    $.preloadCssImages();
   // $("#wysiwyg").wysiwyg({ css: "css/wysiwyg.css" });
    $(".nav li a").each(function () {
        $(this).click(function () {
            $(".nav li a").removeClass("selected");
            $(".popup").css("display", "none");
            $(this).addClass("selected");
            var a = $(this).parent().find(".popup");
            if (a.length > 0) {
                var d = $(this).parent().offset();
                var b = $(this).parent().width();
                var c = d.left - 80 + (b / 2) + 1;
                a.css({ left: c + "px", top: d.top + 60 + "px" });
                a.show(); return false
            }
        })
    });


    $("#identifiantUtilisateur").click(function () {
        $(".popup").css("display", "none");
        
            $(this).addClass("selected");
            var a = $(this).parent().find(".popup");
            if (a.length > 0) {
                var d = $(this).parent().offset();
                var b = $(this).parent().width();
                var c = d.left - 80 + (b / 2) + 1;
                a.css({ left: c + "px", top: d.top + 40 + "px" });
                a.show(); return false
            }
        });


    $("#shortcut_item li a").tipsy({ gravity: "w" });
    
    $(document).click(function () {
        $(".popup").css("display", "none");
        $(".popup").parent().find("a").removeClass("selected")
    });

    $("table.global tbody tr").mouseenter(function () {
        $(this).css("background", "#f9f9f9")
    });
    $("table.global tbody tr").mouseleave(function () {
        $(this).css("background", "#ffffff")
    });

//    $(window).resize(function () { $(".wysiwyg").css("width", "100%") });
    $("div.option").find("div.text").html($("div.option").find("select:first").val());
    $("div.option").find("select:first").change(function () {
        $(this).parent().find("div.text").html($(this).val())
    });

    $("div.file").find("input:first").change(function () {
        $(this).css("top", "-18px");
        var a = $(this).val().replace(/^.*\\/, "").substr(0, 24); 
        $(this).parent().find("div.text").html(a)
    });

//    $(".media_photos li").mouseenter(function () {
//        $(this).find(".action").css("visibility", "visible")
//    });
//    $(".media_photos li").mouseleave(function () {
//        $(this).find(".action").css("visibility", "hidden")
//    });

    $("#datepicker").datepicker({ nextText: "&raquo;", prevText: "&laquo;", showAnim: "slideDown" });

    $("div.date").find("input:first").change(function () {
        if (BrowserDetect.browser != "Explorer") {
            $(this).css("top", "-21px")
        } else {
            if (BrowserDetect.version > 7) {
                $(this).css("top", "-21px")
            } else {
                $(this).css("top", "-10px")
            } 
        }
        $(this).parent().find("div.text").html($(this).val())
    });

    $(".alert_warning").click(function () { $(this).fadeOut("fast") });

    $(".alert_info").click(function () { $(this).fadeOut("fast") });

    $(".alert_success").click(function () {
        $(this).fadeOut("fast")
    });

    $(".alert_error").click(function () {
        $(this).fadeOut("fast")
    });
        
     //  $(".media_photos li a[rel=slide]").fancybox({ padding: 0, titlePosition: "outside", overlayColor: "#333333", overlayOpacity: 0.2 })
});


     
      $(document).ready(function () {
       // $('input[title!=""]').hint(); setTimeout(function () {
             //$("#graph_data").visualize({ type: "line", width: "760px", height: "240px", colors: ["#26ADE4", "#D1E751"] }).appendTo("#graph_wrapper");
         //    $(".visualize").trigger("visualizeRefresh")
       //  }, 500);
       //  $(".wysiwyg").css("width", "100%") 
      });



     