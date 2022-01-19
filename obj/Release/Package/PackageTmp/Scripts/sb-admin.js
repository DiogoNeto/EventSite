$(function() {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
$(function () {
    $(window).bind("load resize", function () {
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.sidebar-collapse').addClass('collapse')
        } else {
            $('div.sidebar-collapse').removeClass('collapse')
        }
    })
});


//$(function () {
//    var url = window.location.href;
//    if (url.indexOf("EventAux") > -1) {
//        //$("#info-expand").parent("li").addClass("active");
//        $("#info-expand").trigger("click");
        
//    }
//    else if (url.indexOf("Map") > -1) {
//        //$("#map-expand").parent("li").addClass("active");
//        $("#map-expand").trigger("click");
        
//    }
//    else if (url.indexOf("Notification") > -1) {
//        //$("#notif-expand").parent("li").addClass("active");
//        $("#notif-expand").trigger("click");
        
//    }
//});
