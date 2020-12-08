window.LoadGantt = function (r) {
    console.log("js_LoadGantt()");

    let rows = r;
    function daysToMilliseconds(days) {
        return days * 24 * 60 * 60 * 1000;
    }
    
    function drawChart() {
        //console.log(rows);

        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Task ID');
        data.addColumn('string', 'Task Name');
        data.addColumn('date', 'Start Date');
        data.addColumn('date', 'End Date');
        data.addColumn('number', 'Duration');
        data.addColumn('number', 'Percent Complete');
        data.addColumn('string', 'Dependencies');

        let r = eval("new Array(" + rows + ")");
        //let r = "new Array(" + rows + ")";
        //let r = Array.from(rows);
        console.log(r);
        data.addRows(r);
        var trackHeight = 20;
        var options = {
            height: (data.getNumberOfRows() + 1) * trackHeight,
            gantt: {
                trackHeight: trackHeight,
                sortTasks: false,
                barHeight: trackHeight / 2,
                barCornerRadius: 4,
            }
        };
        var div = document.getElementById('chart_div');
        //function resizeCharts() {
        //    // redraw charts, dashboards, etc here
        //    console.log("Resizing gantt");
        //    chart.draw(data, options);
        //}
        //if (div.addEventListener) {
        //    console.log("resize div 1");
        //    div.addEventListener("resize", resizeCharts);
        //} else if (div.attachEvent) {
        //    console.log("resize div 2");
        //    div.attachEvent("onresize", resizeCharts);
        //} else {
        //    console.log("resize div 3");
        //    div.onresize = resizeCharts;
        //}

        var chart = new google.visualization.Gantt(div);
        chart.draw(data, options);

        function resizeCharts() {
            // redraw charts, dashboards, etc here
            console.log("resizing");
            chart.draw(data, options);
        }
        if (window.addEventListener) {
            window.addEventListener("resize", resizeCharts);
        } else if (window.attachEvent) {
            window.attachEvent("onresize", resizeCharts);
        } else {
            window.onresize = resizeCharts;
        }

    }


    google.charts.load('current', { 'packages': ['gantt'] });
    google.charts.setOnLoadCallback(drawChart);


}


//window.LoadGantt = function (r) {

//    google.charts.load('current', {
//        'packages': ['gantt']
//    });
//    google.charts.setOnLoadCallback(drawChart);

//    function toMilliseconds(minutes) {
//        return minutes * 60 * 1000;
//    }

//    function drawChart() {

//        var otherData = new google.visualization.DataTable();
//        otherData.addColumn('string', 'Task ID');
//        otherData.addColumn('string', 'Task Name');
//        otherData.addColumn('string', 'Resource');
//        otherData.addColumn('date', 'Start');
//        otherData.addColumn('date', 'End');
//        otherData.addColumn('number', 'Duration');
//        otherData.addColumn('number', 'Percent Complete');
//        otherData.addColumn('string', 'Dependencies');

//        otherData.addRows([
//            ['toTrain', 'Walk to train stop', 'walk', null, null, toMilliseconds(5), 80, null],
//            ['music', 'Listen to music', 'music', null, null, toMilliseconds(400), 100, null],
//            ['wait', 'Wait for train', 'wait', null, null, toMilliseconds(10), 100, 'toTrain'],
//            ['train', 'Train ride', 'train', null, null, toMilliseconds(45), 75, 'wait'],
//            ['toWork', 'Walk to work', 'walk', null, null, toMilliseconds(10), 0, 'train'],
//            ['work', 'Sit down at desk', null, null, null, toMilliseconds(2), 0, 'toWork'],

//        ]);

//        var options = {

//            height: 275,
//            gantt: {
//                defaultStartDateMillis: new Date(2015, 3, 28)
//            }
//        };
//        div = document.getElementById('chart_div');
//        chart = new google.visualization.Gantt(div);
//        chart.draw(otherData, options);


//        function resizeCharts() {
//            // redraw charts, dashboards, etc here
//            console.log("resize charts");
//            chart.draw(otherData, options);
//        }

//        if (window.addEventListener) {
//            window.addEventListener("resize", resizeCharts);
//        } else if (window.attachEvent) {
//            window.attachEvent("onresize", resizeCharts);
//        } else {
//            window.onresize = resizeCharts;
//        }


//        if (div.addEventListener) {
//            div.addEventListener("resize", resizeCharts);
//        } else if (div.attachEvent) {
//            div.attachEvent("onresize", resizeCharts);
//        } else {
//            div.onresize = resizeCharts;
//        }
//    }
//}

