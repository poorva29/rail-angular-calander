$("#txtsearch").autocomplete({
    source: function (request, response) {
        $.ajax({
            type: 'POST',
            url: "../../Services/UserService.svc/searchQuestions",
            data: '{"searchstr":"' + $('#txtsearch').val() + '"}',
            dataType: "json",
            contentType: 'application/json',
            success: function (data) {
                data = JSON.parse(data);
                response($.map(data, function (item) {
                    var temp = document.createElement("pre");
                    temp.innerHTML = item.questiontext;
                    return {
                        label: temp.firstChild.nodeValue,
                        value: "../patient/questiondetails.aspx?questionid=" + item.questionid
                    }
                }));
            },
            error: function (data) {
                console.log('Error in ajax request');
            }
        });
    },
    select: function (event, ui) {
        window.location.href = ui.item.value;

        this.value = "";
        return false;
    },
    open: function () {
        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
    },
    close: function () {
        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
    },
    focus: function (event, ui) {
        this.value = ui.item.label;
        return false;
    }
});