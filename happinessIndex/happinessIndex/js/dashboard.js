// Load the Visualization API and the corechart package.
google.charts.load('current', { 'packages': ['corechart','controls','gauge','bar'] });

// Set a callback to run when the Google Visualization API is loaded.
google.charts.setOnLoadCallback(buildDashboard);

// this function builds each of the individual graphs needed to create the dashboard
function buildDashboard() {
    avgByDept();
    overallHappiness();
    happinessOverTime();
}

/*function to create a columnchart of average score by department */
function avgByDept() {
    var results = new Array();

    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/GetDepartmentAverage",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
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
                title: 'Average Score By Department',
                width: 800,
                height: 500,
                vAxis: {
                    viewWindow: { max: 100, min: 0 }
                }
            };

            // The chart variable is returned for use in buildDashboard
            var chart = new google.visualization.ColumnChart(document.getElementById('barchart_div'));
            chart.draw(tableData, options);
        },
        error: function (e) {
            alert("Error happens here");
        }
    })
}

function overallHappiness() {
    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/GetOverallHappiness",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var results = Number(data.d);

            var gaugeData = google.visualization.arrayToDataTable([
                ['Label', 'Value'],
                ['Overall Health', results]
            ]);

            var options = {
                width: 600,
                height: 400,
                redFrom: 0,
                redTo: 35,
                greenFrom: 50,
                greenTo: 100,
                yellowFrom: 35,
                yellowTo: 50,
                minorTicks: 5
            };

            var gauge = new google.visualization.Gauge(document.getElementById('gauge_div'));
            gauge.draw(gaugeData, options);
        },
        error: function (e) {
            alert("Error happens here");
        }
    })
}

function happinessOverTime() {
    $.ajax({
        type: "POST",
        url: "../happinessServices.asmx/GetHappinessOverTime",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (data) {
            var results = JSON.parse(data.d);

            /* Google arrayToDataTable is used to put json output into array for line chart */
            var lineData = new google.visualization.arrayToDataTable(results);
            var options = {
                title: 'Department Health Last Two Weeks',
                legend: { position: 'right' },
                vAxis: {
                    title: 'Average Score',
                    viewWindow: { max: 100, min: 0 }
                },
                hAxis: {
                    title: 'Date',
                    slantedText: true,
                    slantedTextAngle: 45
                },
                height: 800,
                width: 1000,
            }

            var lineChart = new google.visualization.LineChart(document.getElementById('line_div'));
            lineChart.draw(lineData, options);
        },
        error: function (e) {
            alert("Error happens here");
        }
    })
}
