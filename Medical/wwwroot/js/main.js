jQuery(document).ready(function($){

    updateCartInformation();

    //Search Product
    $('#search-product').keypress(function (e) {
        if (e.which == 13) {
            searchProduct();
        }
    });

    $('.product-carousel').owlCarousel({
        loop:false,
        nav:true,
        margin: 20,
        responsiveClass:true,
        responsive: {
            200: {
                items:1,
            },
            400:{
                items:2,
            },
            600: {
                items: 3,
            },
            800:{
                items:4,
            },
            1000:{
                items:5,
            }
        }
    });  


    $('.product-carousel-home').owlCarousel({
        loop: false,
        nav: true,
        margin: 20,
        responsiveClass: true,
        responsive: {
            700: {
                items: 1,
            },
            900: {
                items: 2,
            },
            1100: {
                items: 3,
            },
            1200: {
                items: 4,
            }
        }
    });  
    
    // Bootstrap Mobile Menu fix
    $(".navbar-nav li a").click(function(){
        $(".navbar-collapse").removeClass('in');
    });    
    
    // jQuery Scroll effect
    $('.navbar-nav li a, .scroll-to-up').bind('click', function(event) {
        var $anchor = $(this);
        var headerH = $('.header-area').outerHeight();
        $('html, body').stop().animate({
            scrollTop : $($anchor.attr('href')).offset().top - headerH + "px"
        }, 1200, 'easeInOutExpo');

        event.preventDefault();
    });    
    
    // Bootstrap ScrollPSY
    $('body').scrollspy({ 
        target: '.navbar-collapse',
        offset: 95
    })      
});

  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-10146041-21', 'auto');
  ga('send', 'pageview');

function addToCart(selector) {
    $.ajax(
        {
            type: "POST",
            url: '/Cart/AddToCart',
            data: {
                cart_product: $("#" + selector + " #cart_product").val(),
                cart_product_count: $("#" + selector + " #cart_amount").val()
            },
            success: function (response) {
                if (response === true) {
                    $.bootstrapGrowl("Added to cart successfully", {
                        type: 'info',
                        delay: 2000,
                    });
                    updateCartInformation();
                } else {
                    $.bootstrapGrowl("Failed", {
                        type: 'error',
                        delay: 2000,
                    });
                }
            }
        });
}

function updateCartInformation() {
    $.ajax(
        {
            type: "POST",
            url: '/Cart/GetInformation',
            success: function (result) {
                if (result) {
                    $("#cart-information").html(result.item + "Item - RS" + result.amount);
                    if ($("#checkout-total-price")) {
                        $("#checkout-total-price").html(result.amount);
                    }
                }
            }
        });
}

// search product with name
function searchProduct() {
    const key = $("#search-product").val();
    document.location.href = "/Product?key=" + key;
}

function ChoosePrescriptionImage() {
    const isLogin = $("#logged-in").val();
    if (isLogin) {
        $("#prescription-modal").modal();
    } else {
        document.location.href = "/Account/Login";
    }
}

function UploadPrescription() {
    if (!$("#prescription_deliver_address").val() || !$("#prescription_image").val()) {
        $.bootstrapGrowl("Please fill all data.", {
            type: 'info',
            delay: 2000,
        });
    }
    let formData = new FormData(document.getElementById('prescription-form'));
    $.ajax({
        url: '/Prescription/Create',
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (response) {
            if (response) {
                $.bootstrapGrowl("Uploaded new prescription successfully.", {
                    type: 'info',
                    delay: 2000,
                });
                $("#prescription-modal").modal("hide");
            }
        }
    });
}