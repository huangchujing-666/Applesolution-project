var maxHeight = 400;

$(function(){

    $(".dropdown > li").hover(function() {
    
         var $container = $(this),
             $list = $container.find("ul"),
             $anchor = $container.find("a"),
             height = $list.height() * 1.2,       // make sure there is enough room at the bottom
             multiplier = height / maxHeight;     // needs to move faster if list is taller
        
        // need to save height here so it can revert on mouseout            
        $container.data("origHeight", $container.height());
        
        // so it can retain it's rollover color all the while the dropdown is open
        $anchor.addClass("hover");
        
        // make sure dropdown appears directly below parent list item    
        $list
            .show()
            .css({
                paddingTop: $container.data("origHeight")+20			
            });
        
        // don't do any animation if list shorter than max
        if (multiplier > 1) {
            $container
                .css({
                    height: maxHeight,
                    overflow: "hidden"
                })
                .mousemove(function(e) {
                    var offset = $container.offset();
                    var relativeY = ((e.pageY - offset.top) * multiplier) - ($container.data("origHeight") * multiplier);
                    if (relativeY > $container.data("origHeight")) {
                        $list.css("top", -relativeY + $container.data("origHeight"));
                    };
                });
        }
        
    }, function() {
    
        var $el = $(this);
        
        // put things back to normal
        $el
            .height($(this).data("origHeight"))
            .find("ul")
            .hide()
            .end()
            .find("a")
            .removeClass("hover");
    
    });
    
    // Add down arrow only to menu items with submenus
//    $(".dropdown > li:has('ul')").each(function() {
//        $(this).find("a:first").append("<img src='images/down-arrow.png' />");
//    });
    
    
});

function clickfaqbtn(id) {
	if ($('#faq_content_'+id).css('display') == 'none') {
		$('#faq_content_'+id).show();
		$('#faq_btn_'+id).attr("src", $('#faq_btn_'+id).attr("src").replace("up", "down"));
	}
	else {
		$('#faq_content_'+id).hide();
		$('#faq_btn_'+id).attr("src", $('#faq_btn_'+id).attr("src").replace("down", "up"));
	}
	
	return false;
}



