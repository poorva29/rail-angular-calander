function shareOnFacebook(question, answer, id, url, appkey) {
    FB.init({
        appId: appkey,
    });
    var logourl = url + '/Content/image/logoImage.png';
    if (id == 0) {
        LinkUrl =  url;
    }
    else {
        LinkUrl = url + 'Patients/PatientQuestionDetails/' + id;
    }
    FB.ui(
    {
        method: 'feed',
        name: unescape(question),
        description: (
          unescape(answer)
        ),
        link: LinkUrl,
        picture: logourl
    },
    function (response) {
        if (response && response.post_id) {
            alert('Post was published.');
        } else {
            alert('Post was not published.');
        }
    }
  );
}

if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        if (this === undefined || this === null) {
            throw new TypeError('"this" is null or not defined');
        }

        var length = this.length >>> 0; // Hack to convert object.length to a UInt32

        fromIndex = +fromIndex || 0;

        if (Math.abs(fromIndex) === Infinity) {
            fromIndex = 0;
        }

        if (fromIndex < 0) {
            fromIndex += length;
            if (fromIndex < 0) {
                fromIndex = 0;
            }
        }

        for (; fromIndex < length; fromIndex++) {
            if (this[fromIndex] === searchElement) {
                return fromIndex;
            }
        }

        return -1;
    };
}

function thankToDoctor(userid, answerid, button, lastname, mobileno, emailid, questiontext) {
    button.disabled = true;
    if (button.childNodes[0].data != 'Is it useful?') {
        $(div).getElementsByTagName('a').onclick = '';
    }
    button.innerHTML = '<div class="custom-green-text"><img src="../Content/image/thanks.png"/> Thanks its useful</div>';
    var div = "#" + answerid + "ans";
    div = div.replace(/ /g, '');
    var thanxcount = $(div).get(0).childNodes[0].innerHTML;
    thanxcount = parseInt(thanxcount);
    thanxcount++;
    $(div).get(0).childNodes[0].innerHTML = " " + thanxcount;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: '{"userid":"' + userid + '","answerid":"' + answerid + '","lastname":"' + lastname + '","emailid":"' + emailid + '","mobileno":"' + mobileno + '","questiontext":"' + questiontext + '","thanxcount":"' + thanxcount + '"}',
        url: "../Services/UserService.svc/thanktodoctor",
        success: function (response) {
            response = JSON.parse(response);
        },
        error: function (e) {
        }
    });
    return false;
}


function MinLength(sender, args) {
    args.IsValid = (args.Value.length >= 6);
}
function isValidDateofBirth(args) {
    var today = new Date();
    var date = today.getDate();
    var month = today.getMonth();
    var year = today.getFullYear();
    var todayDate = new Date(year, month, date);
    //  date validations starts here
    var inputDate = document.getElementById("txtDob").value;
    inputDate = isValidDate(inputDate);
    if (inputDate != '') {
        if (inputDate == null) {
            alert('Please enter valid date.');
            document.getElementById("txtDob").value = "";
            args.IsValid = false;
            return;
        }
        var startdate_day = inputDate.substring(0, 2);
        var startdate_month = inputDate.substring(3, 5);
        var startdate_year = inputDate.substring(6, 10);
        var startdt = new Date(startdate_year, startdate_month - 1, startdate_day);
        if (startdt > todayDate) {
            alert('Date should not be greater than current date.');
            document.getElementById("txtDob").value = "";
            args.IsValid = false;
            return;
        }
        else {
            document.getElementById("txtDob").value = inputDate
            args.IsValid = true;
        }
    }
}
function endorseToDoctor(userid, answerid, button, lastname, mobileno, Email, answerreplyedby,questiontext, endorsecount) {
    button.disabled = true;
    if (button.children[0].innerHTML != 'Want to endorse?') {
        $(div).getElementsByTagName('a').onclick = '';
    }
    var div = "#" + answerid + "ans";
    div = div.replace(/ /g, '');
    var endorsecount = $(div).get(0).childNodes[1].innerHTML;
    endorsecount = parseInt(endorsecount);
    endorsecount++;
    var space = ' ' + endorsecount + ' ';
    var jsondata = '{ "userid": "' + userid + '", "answerid": "' + answerid + '", "lastname": "' + lastname + '", "Email": "' + Email + '", "answerreplyedby": "' + answerreplyedby + '", "mobileno": "' + mobileno + '", "questiontext": "' + questiontext + '", "endorsecount": "' + endorsecount + '" }';
    $(div).get(0).childNodes[1].innerHTML = space;
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: '{"userid":"' + userid + '","answerid":"' + answerid + '","lastname":"' + lastname + '","Email":"' + Email + '","answerreplyedby":"' + answerreplyedby + '","mobileno":"' + mobileno + '","questiontext":"' + questiontext + '","endorsecount":"' + endorsecount + '"}',
        url: "../Services/UserService.svc/endorsetodoctor",
        success: function (response) {
            button.innerHTML = '<img src="../Content/image/thanks.png"/><div class="inline custom-green-text">Endorsed</div>';
            response = JSON.parse(response);
        },
        error: function (e) {

        }
    });
    return false;
}

function bookaptclicked(docid) {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: '{"userid":\"' + docid + '"\}',
        url: "../../Services/UserService.svc/IncrementAppointmentHitCnt",
        success: function (response) {
            response = JSON.parse(response);
            console.log(response);
        },
        error: function (e) {

        }
    });
}
