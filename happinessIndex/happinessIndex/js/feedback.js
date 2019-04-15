window.onload = function() {
    document.getElementById("submitFeedback").addEventListener("click", SendFeedback);
    document.getElementById("thankYou").style.visibility = "hidden";
}

function SendFeedback() {
    var email = document.getElementById("email").value;
    var mRecs = document.getElementById("open1").value;
    var mChange = document.getElementById("open2").value;
    var dFixed = document.getElementById("open3").value;
    var hComms = document.getElementById("open4").value;
    var comments = document.getElementById("open5").value;

    if (email == "" || mRecs == "" || mChange == "" || dFixed == "" || hComms == "" || comments == "") {
        window.alert("Please fill out all fields");
        break;
    }
    else {
        var parameters = "{\"email\":\"" + encodeURI(email) + "\",\"mRecs\":\"" + encodeURI(mRecs) + "\",\"mChange\":\"" + encodeURI(mChange) + "\",\"dFixed\":\"" + encodeURI(dFixed) + "\",\"hComms\":\"" + encodeURI(hComms) + "\",\"comments\":\"" + encodeURI(comments) + "\"}";
    }

    $.ajax({
        type: "POST",
        data: parameters,
        url: "../happinessServices.asmx/CollectFeedback",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data.d)
            $("textarea").prop("disabled", true);
            $("button").prop("disabled", true);
            $("#thankYou").show();
        },
        error: function (e) {
            window.alert("Something went wrong");
        }
    })
}