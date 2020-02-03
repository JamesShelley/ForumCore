// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(".ui.toggle.button").click(function () {
        $(".mobile.only.grid .ui.vertical.menu").toggle(100);
    });
    $("#test").click(function(){
        $(".test").modal('show');
    });
    $(".test").modal({
        closable: true
    });
});

$('.ui.accordion')
    .accordion()
;