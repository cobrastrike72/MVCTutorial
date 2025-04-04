// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$.validator.unobtrusive.adapters.addSingleVal("myvalidator", "minage");

$.validator.addMethod("myvalidator", function (value, element, minAge) {
    return parseInt(value, 10) >= parseInt(minAge, 10);
}, function (params, element) {
    return $(element).attr("data-val-myvalidator");
});
