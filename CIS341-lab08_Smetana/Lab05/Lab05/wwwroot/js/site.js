// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Responds to navbar button + hover 
$(document).ready(function () {
    $("#menu-toggle, #sidebar").hover(function () {
        $('#sidebar').collapse('show');
    }, function () {
        $('#sidebar').collapse('hide');
    });
});