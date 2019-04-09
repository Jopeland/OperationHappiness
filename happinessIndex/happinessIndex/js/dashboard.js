// Load the Visualization API and the corechart package.
google.charts.load('current', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
google.charts.setOnLoadCallback(drawChart);

// Callback that creates and populates a data table,
// instantiates the pie chart, passes in the data and
// draws it.
function drawChart() {

    //// Create the data table.
    //var data = new google.visualization.DataTable();
    //data.addColumn('string', 'Topping');
    //data.addColumn('number', 'Slices');
    //data.addRows([
    //    ['Mushrooms', 3],
    //    ['Onions', 1],
    //    ['Olives', 1],
    //    ['Zucchini', 1],
    //    ['Pepperoni', 2]
    //]);

    //// Set chart options
    //var options = {
    //    'title': 'How Much Pizza I Ate Last Night',
    //    'width': 400,
    //    'height': 300
    //};

    //// Instantiate and draw our chart, passing in some options.
    //var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
    //chart.draw(data, options);

    getDeptHealth();
}

function getDeptHealth() {

    /* Getting google charts ready */
    var tableData = new google.visualization.DataTable();
    tableData.addColumn('string', 'Department');
    tableData.addColumn('number', 'Avg Score');

    /* declaring arrays */
    var departments = [];
    var scores = [];

    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/GetDepartments",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            /* loops through data.d to get all array values and puts them into departments */
            for (var i = 0; i < data.d.length; i++) {
                departments[i] = String(data.d[i]);
                console.log(JSON.stringify(data).dataType);
            }
        },
        error: function (e) {
            alert("Error happens here");
        }
    })

    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/GetAverageScores",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.d.length; i++) {
                scores[i] = Number(data.d[i]);
            }
        },
        error: function (e) {
            alert("Error happens here");
        }
    })

    for (var i = 0; i < departments.length; i++) {
        tableData.addRow(departments[i], scores[i]);
        console.log(departments[i]);
        console.log(scores[i]);
    }

    // Set chart options
    var options = {
        'title': 'Average Score By Department',
        'width': 700,
        'height': 500
    };

    // Instantiate and draw our chart, passing in some options.
    var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
    chart.draw(tableData, options);
}