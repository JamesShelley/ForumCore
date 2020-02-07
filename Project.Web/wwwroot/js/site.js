// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(".ui.toggle.button").click(function () {
        $(".mobile.only.grid .ui.vertical.menu").toggle(100);
    });

    // Delete Forum Models
    $("#deleteForumButton").click(function () {
        $(".deleteForum").modal('show');
    });
    $(".deleteForum").modal({
        closable: true
    });

    // Post Modals

    $("#deletePostButton").click(function(){
        $(".deletePost").modal('show');
    });
    $(".deletePost").modal({
        closable: true
    });
    $("#editPostButton").click(function () {
        $(".editPost").modal('show');
    });
    $(".editPost").modal({
        closable: true
    });

    // Post Reply Modals
    $(".deleteReplyButton").click(function () {
        $(".deleteReply").modal('show');
    });
    $(".deleteReply").modal({
        closable: true
    });
    $(".editReplyButton").click(function () {
        $(".editReply").modal('show');
    });
    $(".editReply").modal({
        closable: true
    });
});

$('.ui.accordion')
    .accordion()
    ;

$('.ui.modal')
    .modal()
    ;
$('.ui.dropdown')
    .dropdown()
;
$('.special.cards .image').dimmer({
    on: 'hover'
});