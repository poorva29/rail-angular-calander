$(function () {
    $(":radio").change(function () {
        var $textValue = $(this).val();
        var $form = $("form[data-otf-ajax='true']");
        var filter = !($textValue == 'All Questions');
        window.location.href = "../questions/" + filter;
    });
    
});