function show_letter()
{
    $('#letter-button').fadeOut(300, function () {
        $('#letter-content').fadeIn(800);
    });
}

function show_signees()
{
    $.getJSON( "./signees.php", function( data ) {
        var counter = 0;
        var items = [];
        items.push("<div class='row'>");
        $.each( data, function( key, val ) {
            if (!val.vorname)
            {
                //if (counter % 3 == 0) items.push("</div>");
                return;
            }
            //if (counter % 3 == 0) items.push("<div class='row'>");
            items.push( "<div class='col-md-4 border border-primary'>"
                + val.prefix
                + " "
                + val.vorname
                + " "
                + val.nachname
                + "<br />"
                + val.hs
                + "</div>");
            counter++;
            //if (counter % 3 == 0) items.push("</div>");
        });
        items.push("</div>");
        $("#signees-content").replaceWith(items.join(''));
    });


    $('#signees-button').fadeOut(300, function () {
        $('#signees-content').fadeIn(800);
    });
}



function scroll_to_top()
{
    scroll_to_px = $('.intro-container').offset().top - $('nav').outerHeight();
    $('html, body').stop().animate({scrollTop: scroll_to_px}, 1000);
}

function scroll_to(clicked_link, nav_height) {
    var element_class = clicked_link.attr('href').replace('#', '.');
    var scroll_to = 0;
    if(element_class != '.top-content') {
        element_class += '-container';
        scroll_to = $(element_class).offset().top - nav_height;
    }
    if($(window).scrollTop() != scroll_to) {
        $('html, body').stop().animate({scrollTop: scroll_to}, 1000);
    }
}

jQuery(document).ready(function() {

    // adding waypoints to  navbar
    $('.top-content .text').waypoint(function() {
        $('nav').toggleClass('navbar-no-bg');
    });
    // adding scroll functions
    $('a.scroll-link').on('click', function(e) {
        e.preventDefault();
        scroll_to($(this), $('nav').outerHeight());
    });
	new WOW().init();
});