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

    document.getElementById("feedbackButton").addEventListener("click", sendFeedback);
    document.getElementById("emailtodb").addEventListener("click", saveEmail);
    document.getElementById("searchButton").addEventListener("click", scoreSearch);

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

function scoreSearch() {
    /* Deletes all table rows except for the header */
    $("#tableBody tr").remove();
    $("#error p").remove();

   /* variables to store all of the inputs */
    /* if input is null value is changed to "#" otherwise input is given the value of the field*/
    if (document.getElementById("searchbar").value === "") {
        input = "#";
    }
    else {
        var input = document.getElementById("searchbar").value;
    }
    var minScore = document.getElementById("minScore").value;
    var maxScore = document.getElementById("maxScore").value;
    console.log(input);

    /* for the ordering filter the chosen sort by and order are concatenated into a string */
    var select = document.getElementById("sortFilter");
    var category = select.options[select.selectedIndex].value;
    var order = document.querySelector('input[name=order]:checked').value;

    var orderBy = category + " " + order;

    /* parameters are set */
    var parameters = "{\"input\":\"" + encodeURI(input) + "\",\"minScore\":\"" + minScore + "\",\"maxScore\":\"" + maxScore + "\",\"order\":\"" + orderBy + "\"}"


    $.ajax({
        type: "POST",
        data: parameters,
        url: "../happinessServices.asmx/SearchEmployees",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var returnData = data.d;

            if (returnData != null) {
                $("#tableBody").append(returnData);
                console.log("success");
            }

            else {
                $("#error").append("<p>No records found</p>");
                console.log("success");
            }

        },
        error: function (e) {
            alert("Error happens here");
        }
    })
}