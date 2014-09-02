$(function () {
    $(":radio").change(function () {
        var $radioBtnValue = $(this).val();
        var $form = $("form[data-otf-ajax='true']");
        window.location.href = "../questions/" + $radioBtnValue;
    });
});