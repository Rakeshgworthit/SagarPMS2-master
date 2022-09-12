function KendoFormatDate(e) {
    return kendo.toString(kendo.parseDate(e), GlobalDateFormat)
}

function KendoFormatTime(e) {
    return kendo.toString(kendo.parseDate(e), GlobalTimeFormat)
}

function ValidateImage(e) {
    switch (e) {
        case ".jpg":
        case ".img":
        case ".png":
        case ".gif":
            return "img-file";
        default:
            return "default-file"
    }
}

function OnChangeMenuLink() { }

function DefaultTopMenu() {
    $("#a_ChangeMenu").html("Left Menu"), $("#div_LeftMenu").hide(), $("#div_TopMenu").show(), $(".page-container-header").addClass("page-container-top-menu"), $(".page-content").addClass("page-content-left-menu"), $("#a_ChangeMenu").hide()
}

function BindComboBoxData(e, t, o, a) {
    $.ajax({
        url: e,
        async: !1,
        type: "GET",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            e = JSON.parse(e), null !== e && e.length > 0 ? ($("#" + t).kendoComboBox({
                dataTextField: o,
                dataValueField: a,
                placeholder: "Select..",
                filter: "contains",
                suggest: !0,
                autoBind: !1,
                serverPaging: !0,
                serverFiltering: !0,
                dataSource: e,
                select: function (e) {
                    var o = this.dataItem(e.item.index());
                    void 0 !== o && null !== o && OnSelectComboBox(o, t)
                },
                change: function (e) {
                    var o = this.dataItem(e.item);
                    void 0 !== o && null !== o ? OnSelectComboBox(o, t) : ($("#" + t).data("kendoComboBox").text(""), $("#" + t).data("kendoComboBox").value(""))
                }
            }), SelectedComboBoxValue(t), kendoComboBoxAutoWidth(t)) : ($("#" + t).kendoComboBox({
                dataSource: []
            }), $("#" + t).data("kendoComboBox").text(""))
        },
        error: OnError
    })
}

function kendoComboBoxAutoWidth(e) {
    var t = $("#" + e).data("kendoComboBox"),
        o = t.list.width();
    o > 100 && t.list.width("auto")
}

function kendoMulComboBoxAutoWidth(e) {
    var t = $("#" + e).data("kendoMultiColumnComboBox"),
        o = t.list.width();
    o > 100 && t.list.width("auto")
}

function BindPopupComboBoxData(e, t, o, a) {
    
    $.ajax({
        url: e,
        async: !1,
        type: "GET",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            
            e = JSON.parse(e), null !== e && e.length > 0 ? ($("#" + t).kendoComboBox({
                dataTextField: o,
                dataValueField: a,
                placeholder: "Select..",
                filter: "contains",
                dataSource: e,
                suggest: true,
                select: function (e) {
                    
                    if (e.dataItem == undefined) {
                        void 0 !== 0 && null !== 0 && OnSelectPopupComboBox(0, t)
                    }
                    else {
                        var o = this.dataItem(e.item.index());
                        void 0 !== o && null !== o && OnSelectPopupComboBox(o, t)
                    }
                   
                },
                change: function (e) {
                    
                    var o = this.dataItem(e.item);
                    void 0 !== o && null !== o ? OnSelectPopupComboBox(o, t) : ($("#" + t).data("kendoComboBox").text(""), $("#" + t).data("kendoComboBox").value(""))
                }
            }), SelectedPopupComboBoxValue(t), kendoComboBoxAutoWidth(t)) : ($("#" + t).kendoComboBox({
                dataSource: []
            }), $("#" + t).data("kendoComboBox").text(""), $("#" + t).data("kendoComboBox").value(''))
        },
        error: OnError
    })
}

function BindMultiPopupComboBoxData(e, t, o, a) {
    
    $.ajax({
        url: e,
        async: !1,
        type: "GET",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            
            e = JSON.parse(e), null !== e && e.length > 0 ? ($("#" + t).kendoMultiColumnComboBox({
                dataTextField: o,
                dataValueField: a,
                placeholder: "Select..",
                filter: "contains",
                dataSource: e,
                columns: [
                    { field: "DETAILS_ID", title: "DETAILS_ID" },
                    { field: "SHORT_NAME", title: "Name" },
                    { field: "TYPE", title: "Type" },
                    { field: "REMARKS", title: "Remark" }
                ],
                select: function (e) {
                
                    var o = this.dataItem(e.item.index());
                    void 0 !== o && null !== o && OnSelectPopupComboBox(o, t)
                },
                change: function (e) {
                
                    var o = this.dataItem(e.item);
                    void 0 !== o && null !== o ? OnSelectPopupComboBox(o, t) : ($("#" + t).data("kendoMultiColumnComboBox").text(""), $("#" + t).data("kendoMultiColumnComboBox").value(""))
                }
            }), SelectedPopupComboBoxValue(t), kendoMulComboBoxAutoWidth(t)) : ($("#" + t).kendoMultiColumnComboBox({
                dataSource: []
            }), $("#" + t).data("kendoMultiColumnComboBox").text(""))
        },
        error: OnError
    })
}

//function columnsOptions1(e) {
//    
//    for (var i = 0; i < e.length; i++) {
//        [
//            { field: i[0][0], title:i[0][0]  },
//            { field: a, title: a }
//        ]
//    }
//}

function BindDropdownData(e, t, o, a) {
    $.ajax({
        url: e,
        async: !1,
        type: "GET",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            
            e = JSON.parse(e), null !== e && e.length > 0 ? ($("#" + t).kendoDropDownList({
                dataTextField: o,
                dataValueField: a,
                optionLabel: "Select..",
                filter: "contains",
                dataSource: e,
                select: function (e) {
                    
                    var o = this.dataItem(e.item.index());
                    void 0 !== o && null !== o && OnSelectDropdown(o, t)
                },
                change: function (e) {
                    
                    var o = this.dataItem(e.item);
                    void 0 !== o && null !== o && OnSelectDropdown(o, t)
                }
            }), SelectedDropdownValue(t), kendoDropdownAutoWidth(t)) : ($("#" + t).kendoDropDownList({
                dataSource: []
            }), $("#" + t).data("kendoDropDownList").text(""))
        },
        error: OnError
    })
}

function kendoDropdownAutoWidth(e) {
    var t = $("#" + e).data("kendoDropDownList"),
        o = t.list.width();
    o > 156 && t.list.width("auto")
}

function BindDropdownDataAll(e, t, o, a) {
    $.ajax({
        url: e,
        async: !1,
        type: "GET",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (e) {
            e = JSON.parse(e), null !== e && e.length > 0 ? ($("#" + t).kendoDropDownList({
                dataTextField: o,
                dataValueField: a,
                optionLabel: "All",
                filter: "contains",
                dataSource: e,
                select: function (e) {
                    var o = this.dataItem(e.item.index());
                    void 0 !== o && null !== o && OnSelectDropdownAll(o, t)
                }
            }), SelectedDropdownValueAll(t), kendoDropdownAutoWidthAll(t)) : ($("#" + t).kendoDropDownList({
                dataSource: []
            }), $("#" + t).data("kendoDropDownList").text(""))
        },
        error: OnError
    })
}

function kendoDropdownAutoWidthAll(e) {
    var t = $("#" + e).data("kendoDropDownList"),
        o = t.list.width();
    o > 268 ? t.list.width("auto") : t.list.width("268")
}

function CommonAjaxRequest(e, t, o) {
    ShowLoading(!0), $.ajax({
        url: e,
        type: o,
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: t,
        dataType: "html",
        success: function (e) {
            setTimeout(function () {
                ShowLoading(!1)
            }, 500), $("#Content").html(e), $("html, body").animate({
                scrollTop: 0
            }, "slow"), void 0 !== $("#login-form").html() && null !== $("#login-form").html() ? $("#divReplacement").html(e) : void 0 !== $("#error-form").html() && null !== $("#error-form").html() && $("#divReplacement").html(e)
        },
        error: OnError
    })
}

function ShowLoading(e) {

    if (e)
    {
        var t = $(window).height(),
            o = $(window).width();
        $("#mask").css({
            width: $(window).width(),
            height: $(document).height()
        }), $("#mask").fadeTo("fast", .5), $("#mask").css({
            "z-index": "500"
        }), $("#divShowLoadingText").html(""), $("#divShowLoading").css({
            top: t / 2 - ($("#divShowLoading").height() / 2 + 60),
            left: o / 2 - $("#divShowLoading").width() / 2,
            display: "block",
            width: 200
        }), BringLoadingtoFront_Popup(), $("#divShowLoadingText").show()
    }
    else e ? $("#divShowLoadingText").html(e) : ($("#mask").css({
        display: "none"
    }),
        $("#divShowLoading").css({
        display: "none"
    }))
}

function BringLoadingtoFront_Popup() {
    $("#divShowLoading").css({
        "z-index": "10000"
    }), $("#divShowLoadingText").css({
        "z-index": "10001"
    })
}

function CreateRecordNew(e, t, o, a) {
    
    ShowLoading(!0), $.ajax({
        type: "POST",
        url: e,
        data: t,
        dataType: "json",
        success: function (n) {
            
            n = JSON.parse(n), setTimeout(function () {
                ShowLoading(!1)
            }, 500), "" !== n && null !== n && null !== n.ERROR_MESSAGE && "" !== n.ERROR_MESSAGE ? (setTimeout(function () {
                ShowLoading(!1)
            }, 500), setTimeout(function () {
                n.RESULT > 0 ? (toastr.success(n.ERROR_MESSAGE), 1 === o ? (RedirectActionToList(n), a()) : 2 === o ? RedirectActionToEdit(n) : DefaultSelection()) : toastr.error(n.ERROR_MESSAGE)
            }, 1e3)) : $("#lbl_Message").text("Error occurred in Url=" + e + ", Param" + t)
        },
        error: OnError
    })
}

function UpdateRecordNew(e, t, o) {
    ShowLoading(!0), $.ajax({
        type: "POST",
        url: e,
        data: t,
        dataType: "json",
        success: function (a) {
            a = JSON.parse(a), setTimeout(function () {
                ShowLoading(!1)
            }, 500), "" !== a && null !== a && null !== a.ERROR_MESSAGE && "" !== a.ERROR_MESSAGE ? (setTimeout(function () {
                ShowLoading(!1)
            }, 500), setTimeout(function () {
                a.RESULT > 0 ? (toastr.success(a.ERROR_MESSAGE), 1 === o ? RedirectActionToList(a) : 2 === o ? RedirectActionToEdit(a) : DefaultSelection()) : toastr.error(a.ERROR_MESSAGE)
            }, 1e3)) : $("#lbl_Message").text("Error occurred in Url=" + e + ", Param" + t)
        },
        error: OnError
    })
}

function CreateRecord(e, t, o) {
    ShowLoading(!0), $.ajax({
        type: "POST",
        url: e,
        data: t,
        dataType: "json",
        success: function (a) {
            a = JSON.parse(a), setTimeout(function () {
                ShowLoading(!1)
            }, 500), "" !== a && null !== a ? (null !== a.ERROR_MESSAGE && "" !== a.ERROR_MESSAGE ? (setTimeout(function () {
                ShowLoading(!1)
            }, 500), a.RESULT > 0 ? toastr.success(a.ERROR_MESSAGE) : toastr.error(a.ERROR_MESSAGE)) : $("#lbl_Message").text("Error occurred in Url=" + e + ", Param" + t), setTimeout(function () {
                a.RESULT > 0 && (o ? RedirectAction(a) : DefaultSelection())
            }, 1e3)) : $("#lbl_Message").text("Error occurred in Url=" + e + ", Param" + t)
        },
        error: OnError
    })
}

function SaveRecord(e, t, o) {
    ShowLoading(!0), $.ajax({
        type: "POST",
        url: e,
        data: t,
        dataType: "json",
        success: function (o) {
            o = JSON.parse(o), setTimeout(function () {
                ShowLoading(!1)
            }, 500), "" !== o && null !== o ? (null !== o.ERROR_MESSAGE && "" !== o.ERROR_MESSAGE ? $("#lbl_Message").text(o.ERROR_MESSAGE) : $("#lbl_Message").text("Error occurred in Url=" + e + ", Param" + t), setTimeout(function () {
                o.RESULT > 0 && RedirectSaveRecords(o)
            }, 1e3)) : $("#lbl_Message").text("Error occurred in Url=" + e + ", Param" + t)
        },
        error: OnError
    })
}

function CreatePopupRecord(e, t) {
    ShowLoading(!0), $.ajax({
        async: !1,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: e,
        data: t,
        dataType: "json",
        success: function (o) {
            o = JSON.parse(o), setTimeout(function () {
                ShowLoading(!1)
            }, 500), "" !== o && null !== o ? setTimeout(function () {
                o.RESULT > 0 ? (toastr.success(o.ERROR_MESSAGE), PopupRedirectAction(o)) : toastr.warning(o.ERROR_MESSAGE)
            }, 1e3) : $("#lbl_Message").text("Error occurred in Url=" + e + "Param" + t)
        },
        error: OnError
    })
}

function GetRecordDetails(e, t) {
    $.ajax({
        async: !1,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: e,
        data: t,
        dataType: "json",
        success: function (e) {
            e = JSON.parse(e), RedirectRecordDetails(e)
        },
        error: OnError
    })
}

function GetProjectParamvalue(e, t) {
    $.ajax({
        async: !1,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: e,
        data: t,
        dataType: "json",
        success: function (e) {
            e = JSON.parse(e), ParamRedirectRecordDetails(e)
        },
        error: OnError
    })
}

function GetRecordDetailsNew(e, t) {
    $.ajax({
        async: !1,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: e,
        data: t,
        dataType: "json",
        success: function (e) {
            e = JSON.parse(e), RedirectRecordDetailsNew(e)
        },
        error: OnError
    })
}

function UpdateRecord(e, t, o) {
    ShowLoading(!0), $.ajax({
        type: "POST",
        url: e,
        data: t,
        dataType: "json",
        success: function (a) {
            a = JSON.parse(a), setTimeout(function () {
                ShowLoading(!1)
            }, 500), "" !== a && null !== a ? (null !== a.ERROR_MESSAGE && "" !== a.ERROR_MESSAGE ? $("#lbl_Message").text(a.ERROR_MESSAGE) : $("#lbl_Message").text("Error occurred in Url=" + e + "Param" + t), setTimeout(function () {
                a.RESULT > 0 && (o ? RedirectAction(a) : DefaultSelection())
            }, 1e3)) : $("#lbl_Message").text("Error occurred in Url=" + e + "Param" + t)
        },
        error: OnError
    })
}

function DeleteRecord(e, t, o) {
    ShowLoading(!0), $.ajax({
        url: e,
        type: "POST",
        cache: !1,
        data: t,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (a) {
            a = JSON.parse(a), setTimeout(function () {
                ShowLoading(!1)
            }, 1e3), "" !== a && null !== a ? (null !== a.ERROR_MESSAGE && "" !== a.ERROR_MESSAGE ? $("#lbl_DeletePopupMessage").text(a.ERROR_MESSAGE) : $("#lbl_DeletePopupMessage").text("DeleteFailed"), a.RESULT > 0 && (setTimeout(function () {
                ClosePopup("Popup_Delete")
            }, 1e3), o ? LoadTabsGrid() : DeleteRedirect())) : $("#lbl_Message").text("Error occurred in Url=" + e + "Param" + t)
        },
        error: OnError
    })
}

function DeleteDetailsRecord(e, t, o) {
    ShowLoading(!0), $.ajax({
        url: e,
        type: "POST",
        cache: !1,
        data: t,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (a) {
            a = JSON.parse(a), setTimeout(function () {
                ShowLoading(!1)
            }, 1e3), "" !== a && null !== a ? (null !== a.ERROR_MESSAGE && "" !== a.ERROR_MESSAGE ? $("#lbl_DeletePopupMessage").text(a.ERROR_MESSAGE) : $("#lbl_DeletePopupMessage").text("DeleteFailed"), a.RESULT > 0 && (setTimeout(function () {
                ClosePopup("Popup_Delete")
            }, 1e3), o ? LoadTabsGrid() : DeleteRedirect())) : $("#lbl_Message").text("Error occurred in Url=" + e + "Param" + t)
        },
        error: OnError
    })
}

//*************** GRIDS BLOCK BEGIN ***************

//To CRUD Multiple Purchase Invoice 

function FillRowWiseGrid(e, t, o, a, n) {
    $.ajax({
        url: t,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: o,
        dataType: "json",
        success: function (t) {
            t = JSON.parse(t), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + e).html("");
            var o = $("#li_" + e).html();
            if (o.indexOf(" (") > 0 && (o = o.substring(0, o.indexOf(" ("))), $("#li_" + e).html(o + " (" + t.length + ")"), null !== t && t.length > 0) {
                GenerateGridModelColumns(t[0], e), $("#grid_" + e).kendoGrid({
                    dataSource: {
                        type: "text",
                        data: t,
                        schema: {
                            model: model
                        },
                        pageSize: 20
                    },
                    resizable: !0,
                    selectable: "single",
                    sortable: !0,
                    toolbar: ["create", "save", "cancel"],
                    reorderable: !0,
                    pageable: {
                        refresh: !1,
                        pageSizes: !0
                    },
                    columns: columnsOptions,
                    sortable: !0,
                    resizable: !0,
                    reorderable: !0,
                    pageable: !0,
                    //columnMenu: !0,
                    editable: !0
                });
                for (var r = 0; r <= modelfields.length; r++) $("#grid_" + e + " thead [data-field=" + modelfields[r] + "] .k-link").html(a[r])
            } else $("#grid_" + e).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(e)
}

function FillTabsGrid(e, t, o, a, n) {
    
    $.ajax({
        url: t,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: o,
        dataType: "json",
        success: function (t) {
            
            t = JSON.parse(t), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + e).html("");
            var o = $("#li_" + e).html();
            if (o.indexOf(" (") > 0 && (o = o.substring(0, o.indexOf(" ("))), $("#li_" + e).html(o + " (" + t.length + ")"), null !== t && t.length > 0) {

                
                for (var i = 0; i < t.length; i++) {
                    
                    GenerateGridModelColumns(t[i], e);
                }

                $("#grid_" + e).kendoGrid({
                    dataSource: {
                        type: "text",
                        data: t,
                        schema: {
                            model: model
                        },
                        pageSize: 20
                    },
                    //dataBound: OnGridDataBound,
                    resizable: !0,
                    selectable: "single",
                    sortable: !0,
                    reorderable: !0,
                    pageable: {
                        refresh: !1,
                        pageSizes: !0
                    },
                    columns: columnsOptions,
                    sortable: !0,
                    resizable: !0,
                    reorderable: !0,
                    pageable: !0,
                    //rowTemplate: kendo.template($("#template").html()),
                    //rowTemplate: kendo.template(rowsOptions)
                    columnMenu: !0
                });

                for (var r = 0; r <= modelfields.length; r++) $("#grid_" + e + " thead [data-field=" + modelfields[r] + "] .k-link").html(a[r])
            } else $("#grid_" + e).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(e)
}

function FillTabsGrid_Bill(e, t, o, a, n) {

    $.ajax({
        url: t,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: o,
        dataType: "json",
        success: function (t) {

            t = JSON.parse(t), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + e).html("");
            var o = $("#li_" + e).html();
            if (o.indexOf(" (") > 0 && (o = o.substring(0, o.indexOf(" ("))), $("#li_" + e).html(o + " (" + t.length + ")"), null !== t && t.length > 0) {


                for (var i = 0; i < t.length; i++) {

                    GenerateGridModelColumns(t[i], e);
                }

                $("#grid_" + e).kendoGrid({
                    dataSource: {
                        type: "text",
                        data: t,
                        schema: {
                            model: model
                        },
                        pageSize: 20
                    },
                    dataBound: OnGridDataBound,
                    resizable: !0,
                    selectable: "single",
                    sortable: !0,
                    reorderable: !0,
                    pageable: {
                        refresh: !1,
                        pageSizes: !0
                    },
                    columns: columnsOptions,
                    sortable: !0,
                    resizable: !0,
                    reorderable: !0,
                    pageable: !0,
                    //rowTemplate: kendo.template($("#template").html()),
                    //rowTemplate: kendo.template(rowsOptions)
                    columnMenu: !0
                });

                for (var r = 0; r <= modelfields.length; r++) $("#grid_" + e + " thead [data-field=" + modelfields[r] + "] .k-link").html(a[r])
            } else $("#grid_" + e).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(e)
}

function FillNormalGrid(e, t, o, a, n) {
  
    $.ajax({
        url: e,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: t,
        dataType: "json",
        success: function (e) {
            
            if (e = JSON.parse(e), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + o).html(""), null !== e && e.length > 0) {
                
                GenerateGridModelColumns(e[0], ""),
                    $("#grid_" + o).kendoGrid({
                        dataSource: {
                            type: "text",
                            data: e,
                            schema: {
                                model: model
                            },
                            pageSize: 20
                        },
                        resizable: !0,
                        selectable: "single",
                        sortable: !0,
                        reorderable: !0,
                        pageable: {
                            refresh: !1,
                            pageSizes: !0
                        },
                        columns: columnsOptions,
                        sortable: !0,
                        resizable: !0,
                        reorderable: !0,
                        pageable: !0
                        //columnMenu: !0
                    });
                for (var t = 0; t <= modelfields.length; t++) {
                    
                    $("#grid_" + o + " thead [data-field=" + modelfields[t] + "] .k-link").html(a[t])
                }
            } else $("#grid_" + o).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(o)
}

function FillNormalGrid_2(e, t, o, a, n) {

    $.ajax({
        url: e,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: t,
        dataType: "json",
        success: function (e) {

            if (e = JSON.parse(e), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + o).html(""), null !== e && e.length > 0) {

                GenerateGridModelColumns(e[0], ""),
                    $("#grid_" + o).kendoGrid({
                        dataSource: {
                            type: "text",
                            data: e,
                            schema: {
                                model: model
                            },
                            pageSize: 20
                        },
                        resizable: !0,
                        selectable: "single",
                        sortable: !0,
                        reorderable: !0,
                        pageable: {
                            refresh: !1,
                            pageSizes: !0
                        },
                        columns: columnsOptions,
                        change: onGridRowSelect,
                        sortable: !0,
                        resizable: !0,
                        reorderable: !0,
                        pageable: !0
                        //columnMenu: !0
                    });
                for (var t = 0; t <= modelfields.length; t++) {

                    $("#grid_" + o + " thead [data-field=" + modelfields[t] + "] .k-link").html(a[t])
                }
            } else $("#grid_" + o).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(o)
}

function FillNormalGrid_1(e, t, o, a, n) {

    $.ajax({
        url: e,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: t,
        dataType: "json",
        success: function (e) {
            if (e = JSON.parse(e), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + o).html(""), null !== e && e.length > 0) {
                GenerateGridModelColumns(e[0], ""),
                    $("#grid_" + o).kendoGrid({
                        dataSource: {
                            type: "text",
                            data: e,
                            schema: {
                                model: model
                            },
                            pageSize: 20
                        },
                        resizable: !0,
                        selectable: "single",
                        sortable: !0,
                        reorderable: !0,
                        pageable: {
                            refresh: !1,
                            pageSizes: !0
                        },
                        columns: columnsOptions,
                        sortable: !0,
                        resizable: !0,
                        reorderable: !0,
                        pageable: !0,
                        filterable: {
                            extra: false,
                            operators: {
                                string: {
                                    startswith: "Starts with",
                                    eq: "Is equal to",
                                    neq: "Is not equal to"
                                }
                            }
                        },
                        columnMenu: !0
                    });
                for (var t = 0; t <= modelfields.length; t++) $("#grid_" + o + " thead [data-field=" + modelfields[t] + "] .k-link").html(a[t])
            } else $("#grid_" + o).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(o)
}

function GenerateGridModelColumns(e, t) {
   
    columnsOptions = [], model = {}, modelfields = null, model.id = "ID";
    var o = {};
    modelfields = new Array, 
        GridColumnTemplate(e, t);
    for (var a in e) modelfields.push(a), a.indexOf("_DATE") > 0 || a.indexOf("_Date") > 0 ? (o[a] = {
        type: "date"
    }, columnsOptions.push({
        field: a,
        title: a,
        type: "date",
        width: 120,
        hidden: CheckforHideColumn(a),
        format: "{0:dd MMM yyyy}",
        parseFormats: "{0:dd MMM yyyy}",
        filterable: {
            ui: function (e) {
                e.kendoDatePicker({
                    format: "dd MMM yyyy"
                });
            }
        }
    })) : a.indexOf("_TOTAL") > 0 || a.indexOf("_Total") > 0 ? (o[a] = {
        type: "string"
    }, columnsOptions.push({
        field: a,
        title: a,
        type: "string",
        attributes: {
            style: "text-align: right;"
        },
        width: 80,
        hidden: CheckforHideColumn(a),
        filterable: {
            cell: {
                operator: "contains",
                extra: !1
            },
            ui: function (e) {
                e.kendoAutoComplete({
                    dataSource: e
                });
            }
        }
    })) : a.indexOf("_QTY") > 0 || a.indexOf("_QTY") > 0 ? (o[a] = {
        type: "string"
    }, columnsOptions.push({
        field: a,
        title: a,
        type: "string",
        attributes: {
            style: "text-align: right;"
        },
        width: 40,
        hidden: CheckforHideColumn(a),
        filterable: {
            cell: {
                operator: "contains",
                extra: !1
            },
            ui: function (e) {
                e.kendoAutoComplete({
                    dataSource: e
                });
            }
        }
    })) : a.indexOf("_DES") > 0 || a.indexOf("_DES") > 0 ? (o[a] = {
        type: "string"
    }, columnsOptions.push({
        field: a,
        title: a,
        type: "string",
        width: 200,
        hidden: CheckforHideColumn(a),
        filterable: {
            cell: {
                operator: "contains",
                extra: !1
            },
            ui: function (e) {
                e.kendoAutoComplete({
                    dataSource: e
                });
            }
        }
    })) : (o[a] = {
        type: "string "
    }, columnsOptions.push({
        field: a,
        title: a,
        type: "string",
        width: 150,
        hidden: CheckforHideColumn(a),
        filterable: {
            cell: {
                operator: "contains",
                extra: !1
            },
            ui: function (e) {
                e.kendoAutoComplete({
                    dataSource: e
                });
            }
        }
    }));
    model.fields = o
}

function FillCheckListNormalGrid(e, t, o, a, n, r) {
   
    $.ajax({
        url: e,
        async: !1,
        type: "POST",
        cache: !1,
        contentType: "application/json; charset=utf-8",
        data: t,
        dataType: "json",
        success: function (e) {
            if (e = JSON.parse(e), void 0 !== n && "" !== n && (hidecolumns = n), $("#grid_" + o).html(""), null !== e && e.length > 0) {
                GenerateChecklistGridModelColumns(e[0], "", r), $("#grid_" + o).kendoGrid({
                    dataSource: {
                        type: "text",
                        data: e,
                        schema: {
                            model: model
                        },
                        pageSize: 10
                    },
                    pageable: {
                        refresh: !1,
                        pageSizes: !0
                    },
                    columns: columnsOptions,
                    sortable: !0,
                    resizable: !0,
                    reorderable: !0,
                    pageable: !0,
                    //columnMenu: !0,
                    resizable: !0,
                    selectable: "single",
                    sortable: !0,
                    reorderable: !0
                });
                for (var t = 0; t <= modelfields.length; t++) $("#grid_" + o + " thead [data-field=" + modelfields[t] + "] .k-link").html(a[t])
            } else $("#grid_" + o).html("Data not available")
        },
        error: OnError
    }), KendoGridToolTip(o)
}

function GenerateChecklistGridModelColumns(e, t, o) {
  
    columnsOptions = [], model = {}, modelfields = null, model.id = "ID";
    var a = {};
    modelfields = new Array, "14" === o ? GridColumnTemplateForThiredParty(e, t) : "18" === o ? GridColumnTemplateForPortExpenses(e, t) : "5" === o ? GridColumnTemplateForOtherService(e, t) : "4" === o ? GridColumnTemplateForMedicalAssistance(e, t) : "13" === o ? GridColumnTemplateForLaunchService(e, t) : "8" === o ? GridColumnTemplateForLandingItem(e, t) : "17" === o ? GridColumnTemplateForFreshWater(e, t) : "9" === o ? GridColumnTemplateForEquipmentTimeSheet(e, t) : "21" === o ? GridColumnTemplateForConsignmentExport(e, t) : "16" === o ? GridColumnTemplateForConsignmentImport(e, t) : "22" === o ? GridColumnTemplateForJobCrewHandling(e, t) : "1" === o ? GridColumnTemplateForJobCrewPersonalDetail(e, t) : "2" === o ? GridColumnTemplateForJobCrewSignOffPersonalDetail(e, t) : "3" === o ? GridColumnTemplateForSuperintendent(e, t) : "Customer Port Charges" === o ? GridColumnTemplateForCustomerPortCharges(e, t) : "DSR" === o && GridColumnTemplateForDsr(e, t);
    for (var n in e) modelfields.push(n), n.indexOf("_DATE") > 0 || n.indexOf("_Date") > 0 ? (a[n] = {
        type: "date"
    }, columnsOptions.push({
        field: n,
        title: n,
        type: "date",
        width: 120,
        hidden: CheckforHideColumn(n),
        format: "{0:dd MMM yyyy}",
        parseFormats: "{0:dd MMM yyyy}",
        filterable: {
            ui: function (e) {
                e.kendoDatePicker({
                    format: "dd MMM yyyy"
                })
            }
        }
    })) : n.indexOf("_TOTAL") > 0 || n.indexOf("_Total") > 0 ? (a[n] = {
        type: "string"
    }, columnsOptions.push({
        field: n,
        title: n,
        type: "string",
        attributes: {
            style: "text-align: right;"
        },
        width: 120,
        hidden: CheckforHideColumn(n),
        filterable: {
            cell: {
                operator: "contains",
                extra: !1
            }
        }
    })) : n.indexOf("_DTTIME") > 0 || n.indexOf("_DTTime") > 0 ? (a[n] = {
        type: "date"
    }, columnsOptions.push({
        field: n,
        title: n,
        type: "datetime",
        width: 120,
        hidden: CheckforHideColumn(n),
        format: "{0:dd MMM yyyy hh:mm}",
        parseFormats: "{0:dd MMM yyyy hh:mm}",
        filterable: {
            ui: function (e) {
                e.kendoDatePicker({
                    format: "dd MMM yyyy hh:mm"
                })
            }
        }
    })) : (a[n] = {
        type: "string"
    }, columnsOptions.push({
        field: n,
        title: n,
        type: "string ",
        width: 150,
        hidden: CheckforHideColumn(n),
        filterable: {
            cell: {
                operator: "contains",
                extra: !1
            }
        }
    }));
    model.fields = a
}

function CheckforHideColumn(e) {
    if (null !== hidecolumns && "" !== hidecolumns) {
        for (var t = 0; t < hidecolumns.length; t++)
            if (e === hidecolumns[t]) return !0;
        return !1
    }
    return !1
}

function KendoGridToolTip(e) {
    $("#grid_" + e).kendoTooltip({
        show: function (e) {
            this.content.text().length > 10 && this.content.parent().css("visibility", "visible")
        },
        hide: function (e) {
            this.content.parent().css("visibility", "hidden")
        },
        filter: "td[role=gridcell]",
        content: function (e) {
            var t = e.target;
            if ("" !== t.text()) var o = t.text();
            return o
        },
        position: "bottom"
    }).data("kendoTooltip")
}

function TrimSpace(e) {
    var t = $("#" + e).val().trim();
    $("#" + e).val(t)
}

function uppercase(e) {
    for (var t = e.split(" "), o = [], a = 0; a < t.length; a++) o.push(t[a].charAt(0).toUpperCase() + t[a].slice(1));
    return o.join(" ")
}

function isUpperCase(e) {
    return e === e.toUpperCase()
}

function ConvertCamel(e) {
    var t = $("#" + e).val();
    if (!isUpperCase(t.trim())) {
        var o = uppercase(t.trim());
        $("#" + e).val(o)
    }
}

function StartDateChange() {
    var e = StartDate.value(),
        t = EndDate.value();
    e ? (e = new Date(e), e.setDate(e.getDate()), EndDate.min(e)) : t ? StartDate.max(new Date(t)) : (t = new Date, StartDate.max(t), EndDate.min(t))
}

function EndDateChange() {
    var e = EndDate.value(),
        t = StartDate.value();
    e ? (e = new Date(e), e.setDate(e.getDate()), StartDate.max(e)) : t ? EndDate.min(new Date(t)) : (e = new Date, StartDate.max(e), EndDate.min(e))
}

function PriceFormat(e, t) {
    return t + " " + e.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,")
}

function GetDateFormat(e) {
    var t = new Date(e),
        o = DayofWeek(t.getDay()),
        a = MonthName(t.getMonth() + 1);
    return o + ",  " + t.getDate() + "  " + a + ",  " + t.getFullYear()
}

function DayofWeek(e) {
    switch (e) {
        case 0:
            return "Sunday";
        case 1:
            return "Monday";
        case 2:
            return "Tuesday";
        case 3:
            return "Wednesday";
        case 4:
            return "Thursday";
        case 5:
            return "Friday";
        case 6:
            return "Saturday"
    }
}

function MonthName(e) {
    var t = "";
    switch (e) {
        case 1:
            t = "January";
            break;
        case 2:
            t = "February";
            break;
        case 3:
            t = "March";
            break;
        case 4:
            t = "April";
            break;
        case 5:
            t = "May";
            break;
        case 6:
            t = "June";
            break;
        case 7:
            t = "July";
            break;
        case 8:
            t = "August";
            break;
        case 9:
            t = "September";
            break;
        case 10:
            t = "October";
            break;
        case 11:
            t = "November";
            break;
        case 12:
            t = "December"
    }
    return t
}

function FormatDate(e) {
    return kendo.toString(kendo.parseDate(e), "dd/MM/yyyy")
}

function openNavigator() {
    document.getElementById("mySidenav").style.width = "10%"
}

function closeNavigator() {
    document.getElementById("mySidenav").style.display = "none"
}

function defineUserGuideTour(e) {
    hopscotch.startTour(e)
}