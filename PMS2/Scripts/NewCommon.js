
function onSelectBillingUOM(e) {
    // debugger
    var detailGridWrapper = this.wrapper;
    var parentTR = detailGridWrapper.closest("tr.k-grid-edit-row");
    var value = e.dataItem.description;

    var price = parentTR.find("td[data-container-for='Price'] input[data-role='numerictextbox']").data("kendoNumericTextBox");
    var qty = parentTR.find("td[data-container-for='Qty'] input[data-role='numerictextbox']").data("kendoNumericTextBox");
    var uomDesc = parentTR.find("td[data-container-for='UOM'] input[data-role='dropdownlist']").data("kendoDropDownList");
    var amount = parentTR.find("td[data-container-for='Amount'] input[data-role='numerictextbox']").data("kendoNumericTextBox");

    if (value == "FOC" || value == "Inclusive" || value == "Complementary") {
        price.enable(false);
        qty.enable(false);
        uomDesc.enable(false);
        amount.readonly();

        //to get total row values in a grid
        var grid = e.sender.element.closest(".k-grid").data("kendoGrid");
        var row = e.sender.element.closest("tr");
        var dataItem = grid.dataItem(row);
        //  debugger
        console.log(dataItem);
        dataItem.set('Price', 0);
        dataItem.set('Qty', 0);
        dataItem.UOM.set('uom_description', '');
        dataItem.UOM.set('uom_id', 0);
        dataItem.set('Amount', 0);

        //qty.value('');
        //price.value('');
        uomDesc.value('');


        //parentTR.find("td[data-container-for='Amount'] .totalSpan").html('');
    } else {
        debugger
        //to get total row values in a grid
        var grid = e.sender.element.closest(".k-grid").data("kendoGrid");
        var row = e.sender.element.closest("tr");
        var dataItem = grid.dataItem(row);
        // debugger
        //console.log(dataItem);
        //debugger
        var ItemId = dataItem.Item.item_id;
        GetItemDetails(e, ItemId);
        price.enable(true);
        qty.enable(true);
        uomDesc.enable(true);
        amount.enable(true);
    }

};

function onSelectItem(e) {
    debugger
    var ItemId = e.dataItem.item_id;
    GetItemDetails(e, ItemId);
};

function onSelectUOM(e) {
    //debugger
    var detailGridWrapper = this.wrapper;
    var parentTR = detailGridWrapper.closest("tr.k-grid-edit-row");
    var value = e.dataItem.uom_description;

    var price = parentTR.find("td[data-container-for='Price'] input[data-role='numerictextbox']").data("kendoNumericTextBox");
    var qty = parentTR.find("td[data-container-for='Qty'] input[data-role='numerictextbox']").data("kendoNumericTextBox");
    //var uomDesc = parentTR.find("td[data-container-for='UOM'] input[data-role='dropdownlist']").data("kendoDropDownList");
    var amount = parentTR.find("td[data-container-for='Amount'] input[data-role='numerictextbox']").data("kendoNumericTextBox");

    if (value == "Lumpsum") {
        price.enable(false);
        qty.enable(false);
        //uomDesc.enable(false);
        amount.enable(true);

        //to get total row values in a grid
        var grid = e.sender.element.closest(".k-grid").data("kendoGrid");
        var row = e.sender.element.closest("tr");
        var dataItem = grid.dataItem(row);
        //debugger
        //console.log(dataItem);
        dataItem.set('Price', 0);
        dataItem.set('Qty', 0);
        dataItem.set('Amount', 0);

    } else {
        var grid = e.sender.element.closest(".k-grid").data("kendoGrid");
        var row = e.sender.element.closest("tr");
        var dataItem = grid.dataItem(row);

        var ItemId = dataItem.Item.item_id;
        GetItemDetails(e, ItemId);
        price.enable(true);
        qty.enable(true);
        //uomDesc.enable(true);
        amount.enable(true);
    }

};

function GetItemDetails(e, ItemId) {
    $.ajax({
        url: '@Url.Action("GetItemByItemId", "Master")?ItemId=' + ItemId,
        type: "POST",
        dataType: "json",
        //data: { JsonPackageDetails: JSON.stringify(options.data.models[0]) },
        success: function (result) {
            debugger
            //to get total row values in a grid
            var grid = e.sender.element.closest(".k-grid").data("kendoGrid");
            var row = e.sender.element.closest("tr");
            var dataItem = grid.dataItem(row);
            dataItem.set('Price', result.data.Items[0].price);
            dataItem.set('Qty', result.data.Items[0].default_qty);
            dataItem.set('Amount', result.data.Items[0].price * result.data.Items[0].default_qty);
            dataItem.set('UOM', result.data.Items[0].UOM);
            //var uomDesc = parentTR.find("td[data-container-for='UOM'] input[data-role='dropdownlist']").data("kendoDropDownList");
            //uomDesc.value(result.data.Items[0].uom.uom_id);
        },
        error: function (result) {
            //   debugger

        }
    });
}