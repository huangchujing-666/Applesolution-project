$(window).resize(function(){
      $(".product").each(function(){
         var tt ="";
         tt=$(".product").width(); 
         $(".product_img").width(tt);
      })             
         })

$(document).ready(function(){
  
       $(".product").each(function(){
         var tt ="";
         tt=$(".product").width(); 
         $(".product_img").width(tt);
      })    

      $(".pages_ul2 a").click(function(){
          var index = $(this).index();
          $(".pages_ul2 a").children().removeClass("active");
          $(".pages_ul2 a").eq(index).children().addClass("active");
      })

      

    
})
