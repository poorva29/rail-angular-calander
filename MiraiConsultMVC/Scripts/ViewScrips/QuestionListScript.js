$(function () {
    $(":radio").change(function () {
        var $textValue = $(this).val();
        var $form = $("form[data-otf-ajax='true']");
        window.location.href = "../questions/" + $textValue;
    });
});