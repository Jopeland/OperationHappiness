window.onload = function () {
    var webMethod = "../happinessServices.asmx/ViewAccounts";
    var parameters = "{\"admin\":\"true\"}";

    $.ajax({
        type: "POST",
        data: parameters,
        url: webMethod,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var returnData = data.d;

            if (returnData != "") {
                $("#tableData").append(returnData);
            }

            else {
                alert("Error. Please reload the page.");
            }

        },
        error: function (e) {
            alert("Error happens here");
        }
    })


}