window.onload = function () {
    $('.main').scroll(function () {
        $(this).find('.sticky').css('left', $(this).scrollLeft());
    });

    var webMethod = "../happinessServices.asmx/ViewFeedback";
    var parameters = "{\"type\":\"load\",\"choice\":\"\"}";

    $.ajax({
        type: "POST",
        data: parameters,
        url: webMethod,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var returnData = data.d;

            if (returnData != "") {
                $("#tableBody").append(returnData);
            }

            else {
                alert("Error. Please reload the page.");
            }

        },
        error: function (e) {
            alert("Error happens here");
        }
    })

    document.getElementById("csvButton").addEventListener("click", saveCSV);

}

function saveCSV() {
    var department = $('#depSelect').val();
    var parameters = "{\"type\":\"\",\"choice\":\"" + encodeURI(department) + "\"}";

    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/ViewFeedback",
        data: parameters,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data.d);

            var hiddenElement = document.createElement('a');
            hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(data.d);
            hiddenElement.target = '_blank';
            hiddenElement.download = department + 'Feedback.csv';
            hiddenElement.click();
        },
        error: function (e) {
            alert(data);
        }
    })
}

