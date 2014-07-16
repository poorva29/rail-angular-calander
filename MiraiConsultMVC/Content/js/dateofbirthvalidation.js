// this file does date valiadtion and if valid date converted into e.g 12/03/2013 form
//['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
var months = [' ', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
var monthsInSmall = [' ', 'jan', 'feb', 'mar', 'apr', 'may', 'jun', 'jul', 'aug', 'sep', 'oct', 'nov', 'dec'];
var dtCh = "/";
var minYear = 1900;
var maxYear = 2100;

String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); };

function isValidDate(enterDate) {

    var selectDate = enterDate.toString();
     selectDate=selectDate.trim();
    if (isDate(selectDate)) {
        var dayStr = day.toString();
        if (dayStr.length == 1)
            dayStr = "0" + day;
        var monthStr = monthVal.toString();
        if (monthStr.length == 1)
            monthStr = "0" + monthVal;
        var correctDate = dayStr + "/" + monthStr + "/" + year;
        return correctDate;
    }
    else {
        return null;
    }
}

function isInteger(s) {
    var i;
    for (i = 0; i < s.length; i++) {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag) {
    var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++) {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary(year) {
    // February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
}
function DaysArray(n) {
    for (var i = 1; i <= n; i++) {
        this[i] = 31
        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
        if (i == 2) { this[i] = 29 }
    }
    return this
}

function checkPertMonth(dtStr) {
    var serMo = "";
    var strMonthSmall = dtStr.toLowerCase();
    for (var count = 1; count < monthsInSmall.length; count++) {  //alert(months[count]);
        serMo = strMonthSmall.indexOf(monthsInSmall[count]);
        if (serMo != -1) {
            return serMo;
        }
    }
    return false;
}
function formatMonth(strMonth) {
    var serMo = "";
    var strMonthSmall = strMonth.toLowerCase();
    for (var count = 1; count < monthsInSmall.length; count++) {  //alert(months[count]);
        serMo = strMonthSmall.indexOf(monthsInSmall[count]);
        if (serMo != -1) {
            return count.toString();
        }
    }
    return strMonth;
}
function isDate(dtStr) {

        if (dtStr.match(".")) {
            dtCh = ".";
        }
        if (dtStr.match(" ")) {
            dtCh = " ";
        }
        if (dtStr.match("-")) {
            dtCh = "-";
        }
        if (dtStr.match(":")) {
            dtCh = ":";
        }

        if (dtStr.match("/")) {
            dtCh = "/";
        }
        var daysInMonth = DaysArray(12)
        var pos1 = dtStr.indexOf(dtCh)
        var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
        if (pos2 == -1) {
            pos2 = dtStr.length;
        }
        var strDay;
        var strMon;
        var monthBool = checkPertMonth(dtStr);
        strDay = dtStr.substring(0, pos1)
        strMon = dtStr.substring(pos1 + 1, pos2)
        var strMonth = formatMonth(strMon);
        var strYear = dtStr.substring(pos2 + 1)
        strYr = strYear
        if (strYr == '') {
            var date = new Date();
            strYr = date.getFullYear().toString();
        }
        if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
        if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
        for (var i = 1; i <= 3; i++) {
            if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
        }
        monthVal = parseInt(strMonth)
        day = parseInt(strDay)
        year = parseInt(strYr)
        if (strMonth.length < 1 || monthVal < 1 || monthVal > 12) {
            return false
        }
        if (strDay.length < 1 || day < 1 || day > 31 || (monthVal == 2 && day > daysInFebruary(year)) || day > daysInMonth[monthVal]) {
            return false
        }
        if (strYr.length != 4 || year == 0 || year < minYear || year > maxYear) {
            return false
        }
        if (isNaN(day) || isNaN(monthVal) || isNaN(year)) {
            return false;
        }
    return true
}

var timeFormatters = {
    mm: function (d) { return zeroPadTotime(d.getMinutes()) },
    HH: function (d) { return zeroPadTotime(d.getHours()) },
};

function zeroPadTotime(n) {
	return (n < 10 ? '0' : '') + n;
}

function formatTime(date,format) {
	var	i, len = format.length, c,i2, formatter,res = '';
    for (i = 0; i < len; i++) {
        c = format.charAt(i);
        if(c!=':')
        {
            for (i2 = len; i2 > i; i2--) {
                if (formatter = timeFormatters[format.substring(i, i2)]) {
                    if (date) {
                        res += formatter(date);
                    }
                    i = i2 - 1;
                    break;
                }
            }
            if (i2 == i) {
                if (date) {
                    res += c;
                }
            }
         }
      }
    return res;
};
         
         
         
    