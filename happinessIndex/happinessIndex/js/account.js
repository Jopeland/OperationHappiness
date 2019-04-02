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

    document.getElementById("feedbackButton").addEventListener("click", sendFeedback);
    document.getElementById("emailtodb").addEventListener("click", saveEmail);

    //$(".data tr").each(function () {
    //   if(this.id != ""){
    //    var score = $("#score").val();
    //    this.insertAdjacentHTML('beforeend',"<td><svg  xmlns='http://www.w3.org/2000/svg' height='200' width='200' viewBox='0 0 200 200' data-value='"+score+"'><path class='bg' stroke='#ccc' d='M41 149.5a77 77 0 1 1 117.93 0' fill='none' /><path class='meter' stroke='#09c' d='M41 149.5a77 77 0 1 1 117.93 0' fill='none' stroke-dasharray='350' stroke-dashoffset='350' /></svg ></td>");
    //   }
    //   else
    //    this.insertAdjacentHTML('beforeend',"<th>Status</th>")
    //})

    //// Get all the Meters
    //const meters = document.querySelectorAll('svg[data-value] .meter');

    //meters.forEach((path) => {
    //   // Get the length of the path
    //   let length = path.getTotalLength();
    //   // console.log(length) once and hardcode the stroke-dashoffset and stroke-dasharray in the SVG if possible 
    //   // or uncomment to set it dynamically
    //   // path.style.strokeDashoffset = length;
    //   // path.style.strokeDasharray = length;

    //   // Get the value of the meter
    //   let value = parseInt(path.parentNode.getAttribute('data-value'));
    //   // Calculate the percentage of the total length
    //   let to = length * ((100 - value) / 100);
    //   // Trigger Layout in Safari hack https://jakearchibald.com/2013/animated-line-drawing-svg/
    //   path.getBoundingClientRect();
    //   // Set the Offset
    //   path.style.strokeDashoffset = Math.max(0, to);
    //});

}

function sendFeedback() {
    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/SendFeedbackEmail",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log("Sending feedback emails.")
        },
        error: function (e) {
            alert("Something went wrong");
        }
    })
}

function saveEmail() {
    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/RetrieveEmails",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data)
        },
        error: function (e) {
            alert(data);
        }
    })
}