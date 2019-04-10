// Load the Visualization API and the corechart package.
google.charts.load('current', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.
google.charts.setOnLoadCallback(drawChart);

// Callback that creates and populates a data table,
// instantiates the pie chart, passes in the data and
// draws it.
function drawChart() {
    getDeptHealth();
}

/*functions to get the average scores of each department and draw a bar graph with them */
function getDeptHealth() {

    var results = new Array();

    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/GetDepartmentAverage",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            alert(JSON.parse(data.d));

            results = JSON.parse(data.d);
            console.log(results);

            /* Getting google charts ready */
            var tableData = new google.visualization.DataTable();
            tableData.addColumn('string', 'Department');
            tableData.addColumn('number', 'Avg Score');

            for (var i = 0; i < results.length; i++) {
                tableData.addRow([results[i][0], Number(results[i][1])]);
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
        },
        error: function (e) {
            alert("Error happens here");
        }
    })
}
