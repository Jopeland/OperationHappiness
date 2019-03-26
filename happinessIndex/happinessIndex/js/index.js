window.onload = function () {
    document.getElementById("loginButton").addEventListener("click", ValidateLogin);
}

function ValidateLogin() {

    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    console.log("Input received. Attempting to validate credentials.");

    var webMethod = "../happinessServices.asmx/VerifyCredentials";
    var parameters = "{\"username\":\"" + encodeURI(username) + "\",\"password\":\"" + encodeURI(password) + "\"}";

    $.ajax({
        type: "POST",
        data: parameters,
        url: webMethod,
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var returnData = data.d;

            if (returnData == "True") {
                document.location.href = "html/account.html";
                sessionStorage.setItem('admin', 'true');
            }

            else {
                alert("Invalid credentials. Please try again.");
                document.getElementById("username").value = "";
                document.getElementById("password").value = "";

                document.getElementById("username").focus();
            }

        },
        error: function (e) {
            alert("Error happens here");
        }
    })
}

