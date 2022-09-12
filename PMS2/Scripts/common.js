function openModelpop(pageName, fieldname, fieldval) {
    //$.loader({ content: "<img src='/Content/img/Preloader_3.gif' />" });
    //$("#modalcontent").html("<img src='/Content/img/Preloader_3.gif' />");
    $('#mainModel').modal({ backdrop: 'static', keyboard: false });
    pageName = pageName + "?" + fieldname + "=" + fieldval;
    $("#modalcontent").load(pageName);
    $.loader('close');
}

function openModelpopup(pageName, fieldname, fieldval, fieldName2, fieldval2) {
    //$.loader({ content: "<img src='/Content/img/Preloader_3.gif' />" });
    //$("#modalcontent").html("<img src='/Content/img/Preloader_3.gif' />");
    debugger;
    $('#mainModel').modal({ backdrop: 'static', keyboard: false });    
    pageName = pageName + "?" + fieldname + "=" + fieldval + "&" + fieldName2 + "=" + fieldval2;
    $("#modalcontent").load(pageName);    
    $.loader('close');
}

function notify(msg, type) {
    notif({
        msg: msg,
        type: type,
        position: "center",
        opacity: 0.9,
        timeout: 2000
    });
}

function reloadGrid(gridname) {
    MVCGrid.reloadGrid("" + gridname + "");
}


function DeleteConfirm(pageName, fieldname, fieldval) {

    //$.loader({ content: "<img src='/Content/img/Preloader_3.gif' />" });

    $('#confirmModel').modal({ backdrop: 'static', keyboard: false });
    pageName = pageName + "?" + fieldname + "=" + fieldval;
    $(".Deletebtn").attr("onclick", "window.location.href='" + pageName + "'");
    $.loader('close');


}


function formatCurrency(total) {
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-$" : '$') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

$('.decimal-restrict').keypress(function (event) {
    var $this = $(this);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
        ((event.which < 48 || event.which > 57) &&
            (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(this).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }

    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which != 0 && event.which != 8) &&
        ($(this)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
});

$('.decimal-restrict').bind("paste", function (e) {
    var text = e.originalEvent.clipboardData.getData('Text');
    if ($.isNumeric(text)) {
        if ((text.substring(text.indexOf('.')).length > 3) && (text.indexOf('.') > -1)) {
            e.preventDefault();
            $(this).val(text.substring(0, text.indexOf('.') + 3));
        }
    }
    else {
        e.preventDefault();
    }
});