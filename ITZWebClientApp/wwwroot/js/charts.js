window.chartJS = {
    activeCharts: [],
    selectedIds: [],
    setBarChart: function (chartName, viewElId, barType, keepAspectRatio, dotNetItem) {
        var el = document.getElementById(viewElId);
        if (el === null) {
            console.log("js_charts.setBarChart() ... couldnt find id : " + viewElId);
            return false
        };
        
        var ctx = el.getContext('2d');
        var config = {
            type: barType,
            options: {
                legend: {
                    
                },
                onHover: function (e, elements) {
                    //e.target.style.cursor = 'pointer';
                    if (elements[0]) {
                        e.currentTarget.style.cursor = "pointer";
                    } else {
                        e.currentTarget.style.cursor = "default";
                    }
                },
                scales: {
                    yAxes: [{
                        scaleLabel: {
                            display: true,
                            labelString: 'no unit'
                        },
                        ticks: {
                            beginAtZero: true,
                            callback: function (label, index, labels) {
                                if (!isNaN(label)) {
                                    return String(label.toFixed(0)).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                }
                                return label;
                            }
                        }
                    }],
                    xAxes: [{
                        display: true
                    }]
                },


                onClick: function (ev) {
                    //console.log("js_chartJS.onClick()");
                    var item = this.getElementAtEvent(ev)[0];
                    if (item !== undefined) {
                        //console.log(item);
                        var index = item._index;
                        var ids = this.config.data.datasets[0].ids[index];
                        //console.log(ids);
                        if (dotNetItem !== null) {
                            dotNetItem.invokeMethodAsync("ClickInBar",
                                item._model.label, item._model.backgroundColor, ids);
                        }
                    }
                    else {
                        if (dotNetItem !== null) {
                            dotNetItem.invokeMethodAsync("ClickInEmptySpace");
                        }
                    }
                },
                maintainAspectRatio: keepAspectRatio,
                responsive: true,
                title: {
                    display: false,
                    text: 'units'
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            if (!isNaN(tooltipItem.yLabel)) {
                                return tooltipItem.yLabel.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                            }
                        }
                    }
                }

            }
            //data: {
            //    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            //    datasets: [{
            //        label: '# of Votes',
            //        data: [12, 19, 3, 5, 2, 3],
            //        backgroundColor: [
            //            'rgba(255, 99, 132, 0.2)',
            //            'rgba(54, 162, 235, 0.2)',
            //            'rgba(255, 206, 86, 0.2)',
            //            'rgba(75, 192, 192, 0.2)',
            //            'rgba(153, 102, 255, 0.2)',
            //            'rgba(255, 159, 64, 0.2)'
            //        ],
            //        borderColor: [
            //            'rgba(255, 99, 132, 1)',
            //            'rgba(54, 162, 235, 1)',
            //            'rgba(255, 206, 86, 1)',
            //            'rgba(75, 192, 192, 1)',
            //            'rgba(153, 102, 255, 1)',
            //            'rgba(255, 159, 64, 1)'
            //        ],
            //        borderWidth: 1
            //    }]
            //}
        };
        var found = false;
        for (var i = 0; i < chartJS.activeCharts.length; i++) {
            var name = chartJS.activeCharts[i]["chartName"];
            if (name === chartName) {
                chartJS.activeCharts[i]["myChart"] = new Chart(ctx, config);
                //console.log('js_charts.setBarChart() ... reloaded');
                return true;
            }
        }
        if (found === false) {
            var myChart = new Chart(ctx, config);
            chartJS.activeCharts.push({ viewElId, chartName, myChart });
            //console.log('js_charts.setBarChart() ... loaded');
            return true;
        }

        return false;
    },

    updateChart(chartName, data) {
        //console.log("js_charts.updateChart()");

        //console.log(chartName);
        //console.log(chartJS.activeCharts);
        //console.log(chartJS.activeCharts.length);
        for (var i = 0; i < chartJS.activeCharts.length; i++) {
            var name = chartJS.activeCharts[i]["chartName"];
            //console.log(name);
            if (name === chartName) {
                var chart = chartJS.activeCharts[i]["myChart"];
                var json = JSON.parse(data);
                
                //if (json.data.datasets === undefined) {
                //    chart.config.data.labels = [];
                //    chart.config.data.datasets = [];
                //}
                //else {
                //    chart.config.data = json.data;
                //}

                chart.config.data = json.data;
                if (json.units !== undefined && json.units.length > 0) {
                    chart.config.options.scales.yAxes[0].scaleLabel.labelString = json.units;
                }
                else {
                    chart.config.options.scales.yAxes[0].scaleLabel.labelString = "";
                }
                
                chart.update();
                //postAction(data);
                //idsData = json;
                //chartJS.selectedIds.push({ chartName, data.datasets[0].ids });
                break;
            }
        }
    },

    setMinAndMax(chartName, axy, min, max, beginAtZero, indexAxy, functionStr) {
        //console.log("js_charts.setMinAndMax()");
        for (var i = 0; i < chartJS.activeCharts.length; i++) {
            var name = chartJS.activeCharts[i]["chartName"];
            if (name === chartName) {
                var chart = chartJS.activeCharts[i]['myChart'];
                
                if (axy === 'yAxy') {
                    chart.config.options.scales.yAxes[indexAxy].ticks.suggestedMin = min;
                    chart.config.options.scales.yAxes[indexAxy].ticks.suggestedMax = max;
                    chart.config.options.scales.yAxes[indexAxy].ticks.beginAtZero = beginAtZero;
                    if (functionStr !== undefined) {
                        chart.config.options.scales.yAxes[indexAxy].ticks.callback = function (value, index, values) {
                            //console.log(functionStr);
                            return eval(functionStr)();
                        }
                    }

                }
                else if (axy === 'xAxy') {
                    chart.config.options.scales.xAxes[indexAxy].ticks.suggestedMin = min;
                    chart.config.options.scales.xAxes[indexAxy].ticks.suggestedMax = max;
                    chart.config.options.scales.xAxes[indexAxy].ticks.beginAtZero = beginAtZero;
                    if (functionStr !== null) {
                        chart.config.options.scales.xAxes[indexAxy].ticks.callback = function (value, index, values) {
                            //console.log(functionStr);
                            return eval(functionStr)();
                        }
                    }
                }
                else {
                    console.log('no valid axy');
                }

                
                chart.update();
                break;
            }
        }
    },

    setAspectRatio(chartName, b) {
        //console.log("js_charts.setAspectRatio()");
        for (var i = 0; i < chartJS.activeCharts.length; i++) {
            var name = chartJS.activeCharts[i]["chartName"];
            if (name === chartName) {
                var chart = chartJS.activeCharts[i]['myChart'];
                chart.config.options.maintainAspectRatio = b;
            }
        }
    },

    getSelectedIds(chartName) {
        for (var i = 0; i < selectedIds.length; i++) {
            var name = chartJS.selectedIds[i]["chartName"];
            if (name === chartName) {
                return chartJS.selectedIds[i]["chartName"];
            }
        }
    }
},

window.chartJsPie = {
    activeCharts: [],
    selectedIds: [],
    setPieChart: function (chartName, viewElId, dotNetItem, reset) {
        console.log('js_charts.setPieChart() ... start on ' + viewElId);
        var el = document.getElementById(viewElId);
        
        if (el === null) return false;
        if (reset === true) chartJsPie.activeCharts = [];
        var ctx = el.getContext('2d');
        var config = {
            type: 'doughnut',
            options: {
                onClick: function (e) {
                    console.log("chartJsPie.onClick()");
                    var item = this.getElementAtEvent(e)[0];
                    if (item !== undefined) {
                        var index = item._index;
                        var ids = this.config.data.datasets[0].ids[index];
                        if (dotNetItem !== undefined) {
                            dotNetItem.invokeMethodAsync("OnGraphBarSelection",
                                item._model.label, item._model.backgroundColor, ids);
                        }
                    }
                },
                responsive: true,
                maintainAspectRatio: false,
                title: {
                    display: false,
                    position: 'left',
                    text: 'units'
                },
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var dataset = data.datasets[tooltipItem.datasetIndex];
                            var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                return previousValue + currentValue;
                            });
                            var currentValue = dataset.data[tooltipItem.index];
                            var percentage = Math.floor(((currentValue / total) * 100) + 0.5);
                            return percentage + "%";
                            //return tooltipItem.yLabel.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                        }
                    }
                },
                legend: {
                    fullWidth: false,
                    position: 'left',
                    
                },
                circumference: Math.PI * 2,
                rotation: 0.0,
                plugins: {
                    datalabels: {
                        formatter: (value, ctx) => {
                            let sum = 0;
                            let dataArr = ctx.data.datasets[0].data;
                            dataArr.map(x => {
                                sum += data;
                            });
                            let percentage = (value * 100 / sum).toFixed(2) + "%";
                            return percentage;
                        },
                        color: '#fff'
                    }
                }
            }
            //,
            //data : {
            //    datasets: [{
            //        data: [10, 20, 30]
            //    }],

            //    // These labels appear in the legend and in the tooltips when hovering different arcs
            //    labels: [
            //        'Red',
            //        'Yellow',
            //        'Blue'
            //    ]
            //}
        };
        var found = false;
        for (var i = 0; i < chartJsPie.activeCharts.length; i++) {
            var name = chartJsPie.activeCharts[i]["chartName"];
            if (name === chartName) {
                chartJsPie.activeCharts[i]["myChart"] =  new Chart(ctx, config);
                console.log('js_charts.setPieChart() ... reloaded');
                return true;
            }
        }
        if (found === false) {
            var myChart = new Chart(ctx, config);
            //chartJsPie.activeCharts = [];
            chartJsPie.activeCharts.push({ viewElId, chartName, myChart });
            console.log('js_charts.setPieChart() ... loaded');
            return true;
        }
        
        return false;
    },

    updatePieChart(chartName, data) {
        console.log('js_charts.updatePieChart()');
        for (var i = 0; i < chartJsPie.activeCharts.length; i++) {
            var name = chartJsPie.activeCharts[i]["chartName"];
            if (name === chartName) {
                var chart = chartJsPie.activeCharts[i]['myChart'];
                var json = JSON.parse(data);
                chart.config.data = json.data;
                chart.update();
                console.log('js_charts.updatePieChart() ... updated');
                break;
            }
        }
    }
}