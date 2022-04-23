function selectAll() {
    var all = $('#all');
    var sn = document.getElementsByName("ids");
    if (all.is(":checked")) {
        for (var i = 0; i < sn.length; i++) {
            sn[i].checked = true;
        }
    } else {
        for (var i = 0; i < sn.length; i++) {
            sn[i].checked = false;
        }
    }
}

function deleteIt(action, extraId, extraValue) {

    var sn = document.getElementsByName("ids");
    var count = 0;
    var IDS = [];
    for (var i = 0; i < sn.length; i++) {
        if (sn[i].checked == true) {
            IDS.push(sn[i])
            count++;
        }
    }
    if (count == 0) {
        alert("請勾選刪除項目!");
        return false;
    } else {
        if (confirm("你確定要刪除選取資料?")) {
            //$.blockUI({ message: '<h1><img src="' + processGif + '" /></h1>' });
            alert(IDS.length)
            document.forms['uForm'].action = action;
            document.forms['uForm'].submit();
        }
    }
}

function update(STATUS, action, extraId, extraValue) {
    var sn = document.getElementsByName("ids");
    var count = 0;
    for (var i = 0; i < sn.length; i++) {
        if (sn[i].checked == true) {
            count++;
        }
    }
    if (count == 0) {
        alert("請勾選更新項目!");
        return false;
    } else {
        if (confirm("你確定要更新選取資料?")) {
            // $.blockUI({ message: '<h1><img src="' + processGif + '" /></h1>' });
            document.getElementById("STATUS").value = STATUS;
            document.forms['uForm'].action = action;
            document.forms['uForm'].submit();
        }
    }
}

function formSubmit() {
    //$.blockUI({ message: '<h1><img src="' + processGif + '" /></h1>' });
    $('#sForm').find('input').prop('disabled', false);//解除 disabled 讓參數可順利送出
    document.forms['sForm'].submit();
}

function drawImage(ImgD, FitWidth, FitHeight) {
    var image = new Image();
    image.src = ImgD.src;
    if (image.width > 0 && image.height > 0) {
        if (image.width / image.height >= FitWidth / FitHeight) {
            if (image.width > FitWidth) {
                ImgD.width = FitWidth;
                ImgD.height = (image.height * FitWidth) / image.width;
            } else {
                ImgD.width = image.width;
                ImgD.height = image.height;
            }
        } else {
            if (image.height > FitHeight) {
                ImgD.height = FitHeight;
                ImgD.width = (image.width * FitHeight) / image.height;
            } else {
                ImgD.width = image.width;
                ImgD.height = image.height;
            }
        }
    }
    try {
        ImgD.style.display = "";
    } catch (e) {
    }
}

function getFileName(url) {
    var strobj = url;
    if (strobj == '') {
        return "";
    } else {
        var file_value = strobj.toUpperCase();
        var index = file_value.lastIndexOf('/', file_value.length);
        var file_attribute = file_value.substr(index + 1);
        return file_attribute;
    }
}

function hasChecked(name) {
    var chkObj = document.getElementsByName(name);
    if (chkObj[0]) {
        for (var i = 0; i < chkObj.length; ++i) {
            if (chkObj[i].checked) {
                return true;
            }
        }
    } else if (chkObj) {
        return chkObj.checked;
    }
    return false;
}

function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

function isAscii(pattern) {
    for (var i = 0; i < pattern.length; i++) {
        if (pattern.charCodeAt(i) > 127) {
            return false;
        }
    }
    return true;
}
function getCookie(name) {
    var start = document.cookie.indexOf(name + "=");
    var len = start + name.length + 1;
    if ((!start) && (name != document.cookie.substring(0, name.length))) {
        return null;
    }
    if (start == -1)
        return null;
    var end = document.cookie.indexOf(";", len);
    if (end == -1)
        end = document.cookie.length;
    return unescape(document.cookie.substring(len, end));
}

function up(obj) {
    var objParentTR = $(obj).parent().parent();
    var prevTR = objParentTR.prev();
    if (prevTR.length > 0) {
        prevTR.insertAfter(objParentTR);
    }
}
function down(obj) {
    var objParentTR = $(obj).parent().parent();
    var nextTR = objParentTR.next();
    if (nextTR.length > 0) {
        nextTR.insertBefore(objParentTR);
    }
}
function updateSeq(id, seq, action, extraId, extraValue) {
    $.blockUI({ message: '<h1><img src="' + processGif + '" /></h1>' });
    document.uForm.id.value = id;
    document.uForm.seq.value = seq;
    if (extraId) {
        document.getElementById(extraId).value = extraValue;
    }
    document.forms['uForm'].action = action;
    document.forms['uForm'].submit();
}

function dateFormat(date, yearAdd, type) {
    var day = date.getDate();
    var month = date.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }
    var year = date.getFullYear() + yearAdd;
    var h = !(type) ? date.getHours() : (type == 1) ? "00" : 23;
    var m = !(type) ? date.getMinutes() : (type == 1) ? "00" : 59;
    var s = !(type) ? date.getSeconds() : (type == 1) ? "00" : 59;
    return year + "/" + month + "/" + day + " " + h + ":" + m + ":" + s;
}

$('#uForm input[name=ids]').change(function () {
    var allChecked = $('#uForm input[name=ids]').length == $('#uForm input[name=ids]:checked').length;
    $('#uForm #all').prop('checked', allChecked);
});

$('.file_click').click(function () {
    $(this).find('input[type=file]')[0].click();
});

$('input[type=file]').change(function () {
    $(this).parent().parent().find('.file_name').text($(this).val().split('\\').pop());
});

$('#PageSize').keypress(function () {
    $('#indexListPage').submit();
})
$(document).ready(function () {
    $(function () {
        $('#START_TIME').on('focus', function (e) {
            e.preventDefault();
            $(this).datepicker('show');
            $(this).datepicker('widget').css('z-index', 1051);
        });
        $('#END_TIME').on('focus', function (e) {
            e.preventDefault();
            $(this).datepicker('show');
            $(this).datepicker('widget').css('z-index', 1051);
        });
        $('#SearchParameter_StartTime').on('focus', function (e) {
            e.preventDefault();
            $(this).datepicker('show');
            $(this).datepicker('widget').css('z-index', 1051);
        });
        $('#SearchParameter_EndTime').on('focus', function (e) {
            e.preventDefault();
            $(this).datepicker('show');
            $(this).datepicker('widget').css('z-index', 1051);
        });
    });
});