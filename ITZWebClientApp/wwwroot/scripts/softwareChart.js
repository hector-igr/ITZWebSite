window.SoftwareChart = {


    Ini: function () {

        var values = [6, 9, 9, 6, 7, 7, 9, 3, 3, 3, 5, 5, 9, 7, 3, 3, 1, 7];
        var backgroundColors = [];
        var backgroundBorderColors = [];
        for (i = 0; i < values.length; i++) {
            var val = 200 - ( values[i] * 20)
            backgroundColors[i] = 'rgba(' + val + ',' + val + ',' + val +',0.2)';
            backgroundBorderColors[i] = 'rgba(' + val + ',' + val + ',' + val + ',1.0)';
            //backgroundBorderColors[i] = 'rgba(0,0,0,1.0)';
        }


        var ctx = document.getElementById('softwareChart').getContext('2d');
        if (ctx !== undefined) {
            var softwareChart = new Chart(ctx, {
                type: 'horizontalBar',
                data: {
                    labels: ['AutoCAD', 'Revit ARC', 'Revit STR', 'Revit MEP', 'Inventor', '3ds Max', 'Navisworks', 'Civil 3d', 'City Engine', 'Vray', 'Rhino-Grasshopper', 'Photoshop', 'MS Excel', 'MS Project', 'MS Access', 'After Effects', 'Tekla Structure', 'Lumion'],
                    datasets: [{
                        label: 'Nivel de Experiencia',
                        data: values,
                        backgroundColor: backgroundColors,
                        borderColor: backgroundBorderColors,
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                suggestedMin: 0,
                                beginAtZero: true
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                suggestedMin: 0,
                                suggestedMax: 10,
                                beginAtZero: true,
                                callback: function (value, index, values) {
                                    if (value === 1) {
                                        return "basico"
                                    }
                                    if (value === 4) {
                                        return "medio"
                                    }
                                    if (value === 7) {
                                        return "avanzado"
                                    }
                                    if (value === 10) {
                                        return "experto"
                                    }
                                }
                            }
                        }],
                        labelstring: "Nivel"
                    },
                    responsive: true
                },

            });
        }


        values = [4000, 85, 228, 171, 571, 142, 257, 171, 1428, 228, 228, 285],
        backgroundColors = [];
        backgroundBorderColors = [];
        for (i = 0; i < values.length; i++) {
            var val = 150 - ((values[i] / 400) * 20 )
            backgroundColors[i] = 'rgba(' + val + ',' + val + ',' + val + ',0.2)';
            backgroundBorderColors[i] = 'rgba(' + val + ',' + val + ',' + val + ',1.0)';
            //backgroundBorderColors[i] = 'rgba(0,0,0,1.0)';
            console.log(val)
        }

        var ctx2 = document.getElementById('codingExperience').getContext('2d');
        if (ctx2 !== undefined) {
            var codingExperience = new Chart(ctx2, {
                type: 'horizontalBar',
                data: {
                    labels: ['Revit API C#', 'Autocad API C#', 'Navisworks API C#', 'WPF C#', 'ASP.NET Core C#', 'MS Project VSTO C#', 'MS Excel VSTO C#', 'SQL Server', '3dsMax Maxscript', 'HTML-JavaScript', 'Autodesk Forge JavaScript', 'Grasshopper'],
                    datasets: [{
                        label: 'Horas Aprox de Experiencia',
                        data: values,
                        backgroundColor: backgroundColors,
                        borderColor: backgroundBorderColors,
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                suggestedMin: 0,
                                beginAtZero: true
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                suggestedMin: 0,
                                beginAtZero: true
                            }
                        }],
                        labelstring: "Nivel"
                    },
                    responsive: true
                },

            });
        }
    }
}