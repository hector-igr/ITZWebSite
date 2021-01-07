var ganttEl;
var ttask;
var tempData;
window.JsGantt = {
    Load: function (id, dotnetitem, nowdate) {
        //console.log("LoadJsGantt()");
        var g = new JSGantt.GanttChart(document.getElementById(id), 'week');
        g.setOptions({
            vCaptionType: 'Complete',  // Set to Show Caption : None,Caption,Resource,Duration,Complete,     
            vQuarterColWidth: 36,
            vDateTaskDisplayFormat: 'day dd month yyyy', // Shown in tool tip box
            vDayMajorDateDisplayFormat: 'mon yyyy - Week ww',// Set format to display dates in the "Major" header of the "Day" view
            vWeekMinorDateDisplayFormat: 'dd mon', // Set format to display dates in the "Minor" header of the "Week" view
            //vLang: lang,
            vAdditionalHeaders: { // Add data columns to your table
                //category: {
                //    title: 'Category'
                //},
                //sector: {
                //    title: 'Sector'
                //}
                wbs: {
                    title: 'wbs'
                }
            },
            vColumnOrder: ['vAdditionalHeaders','vShowRes', 'vShowDur', 'vShowComp', 'vShowStartDate', 'vShowEndDate', 'vShowPlanStartDate', 'vShowPlanEndDate', 'vShowCost',  'vShowAddEntries'],
            vShowTaskInfoLink: 1, // Show link in tool tip (0/1)
            vShowEndWeekDate: 0,  // Show/Hide the date for the last day of the week in header for daily view (1/0)
            vUseSingleCell: 10000, // Set the threshold at which we will only use one cell per table row (0 disables).  Helps with rendering performance for large charts.
            vFormatArr: ['Day', 'Week', 'Month', 'Quarter'], // Even with setUseSingleCell using Hour format on such a large chart can cause issues in some browsers
            vScrollTo: new Date(),
            vTotalHeight: "100%",
            // EVENTS

            // OnChangee
            vEventsChange: {
                taskname: console.log,
                res: console.log,
            },
            // EventsClickCell
            vEvents: {
                taskname: (x) => {
                    if (dotnetitem !== undefined) {
                        ttask = x;
                        dotnetitem.invokeMethodAsync('SelectedRow', ttask.getOriginalID())
                    }
                },
                res: console.log,
                dur: console.log,
                comp: console.log,
                start: console.log,
                end: console.log,
                planstart: console.log,
                planend: console.log,
                cost: console.log,
                //beforeDraw: () => console.log('before draw listener'),
                //afterDraw: () => console.log('before after listener')
            },
            vEventClickRow: console.log,
            vEventClickCollapse: console.log
        });
        g.setUseSort(0);
        
        g.setDateTaskTableDisplayFormat("mm/dd/yyyy");
        if (nowdate !== undefined) {
            g.todayDate = new Date(nowdate);
        }
        g.setShowRes(0);//hide resources
        g.Draw();
        //console.log("finish draw");
        ganttEl = g;
    },
    LoadData: function (json, nowdate) {
        tempData = json;
        //console.log("Load()");
        ganttEl.ClearTasks();
        ganttEl.Draw();
        JSGantt.parseJSONString(json, ganttEl);
        if (nowdate !== undefined) {
            ganttEl.todayDate = new Date(nowdate);
        }
        ganttEl.Draw();
        //console.log("Load() ... redraw");
    },
    UnloadData : function () {
        ganttEl.ClearTasks();
        ganttEl.Draw();
    },
    ChangeDate: function (nowDate) {
        //console.log("ChangeDate()");
        //console.log(nowDate);

        // keep vertical position
        let top = document.querySelector("#GanttChartDIVgchartbody")?.scrollTop;
        let left = document.querySelector("#GanttChartDIVgchartbody")?.scrollLeft;
        ganttEl.todayDate = new Date(nowDate);
        //ganttEl.vScrollTo = new Date(nowDate);
        ganttEl.Draw();

        if (top !== null) {
            $("#GanttChartDIVgchartbody").scrollTop(top);
            $("#GanttChartDIVgchartbody").scrollLeft(left);
        }
        //console.log("ChangeDate() ... redraw");
    },
    ResizeHeight: function (height) {
        //console.log(ganttEl);
    }
}

LoadJsGanttOriginal = function (id, json, dotnetitem, nowdate) {
    console.log("NOWDATE");
    console.log(nowdate);
    var g = new JSGantt.GanttChart(document.getElementById(id), 'day');
    // passing object
    //g.AddTaskItemObject({
    //    pID: 1,
    //    pName: "Define Chart <strong>API</strong>",
    //    pStart: "2017-02-25",
    //    pEnd: "2017-03-17",
    //    pPlanStart: "2017-04-01",
    //    pPlanEnd: "2017-04-15 12:00",
    //    pClass: "ggroupblack",
    //    pLink: "",
    //    pMile: 0,
    //    pRes: "Brian",
    //    pComp: 0,
    //    pGroup: 0,
    //    pParent: 0,
    //    pOpen: 1,
    //    pDepend: "",
    //    pCaption: "",
    //    pCost: 1000,
    //    pNotes: "Some Notes text",
    //    pBarText: "ex. bar text",
    //    category: "My Category",
    //    sector: "Finance"
    //});
    //g.addLang('en2', { 'format': 'Select', 'comp': 'Complete' });
    g.setOptions({
        vCaptionType: 'Complete',  // Set to Show Caption : None,Caption,Resource,Duration,Complete,     
        vQuarterColWidth: 36,
        vDateTaskDisplayFormat: 'day dd month yyyy', // Shown in tool tip box
        vDayMajorDateDisplayFormat: 'mon yyyy - Week ww',// Set format to display dates in the "Major" header of the "Day" view
        vWeekMinorDateDisplayFormat: 'dd mon', // Set format to display dates in the "Minor" header of the "Week" view
        //vLang: lang,
        vAdditionalHeaders: { // Add data columns to your table
            category: {
                title: 'Category'
            },
            sector: {
                title: 'Sector'
            }
        },
        vShowTaskInfoLink: 1, // Show link in tool tip (0/1)
        vShowEndWeekDate: 0,  // Show/Hide the date for the last day of the week in header for daily view (1/0)
        vUseSingleCell: 10000, // Set the threshold at which we will only use one cell per table row (0 disables).  Helps with rendering performance for large charts.
        vFormatArr: ['Day', 'Week', 'Month', 'Quarter'], // Even with setUseSingleCell using Hour format on such a large chart can cause issues in some browsers
        vScrollTo: new Date(),
        vTotalHeight: "100%",
        // EVENTS

        // OnChangee
        vEventsChange: {
            taskname: console.log,
            res: console.log,
        },
        // EventsClickCell
        vEvents: {
            taskname: (x) => {
                if (dotnetitem !== undefined) {
                    ttask = x;
                    dotnetitem.invokeMethodAsync('SelectedRow', ttask.getOriginalID())
                }
            },
            res: console.log,
            dur: console.log,
            comp: console.log,
            start: console.log,
            end: console.log,
            planstart: console.log,
            planend: console.log,
            cost: console.log,
            beforeDraw: () => console.log('before draw listener'),
            afterDraw: () => console.log('before after listener')
        },
        vEventClickRow: console.log,
        vEventClickCollapse: console.log
    });
    g.setUseSort(0);
    g.setDateTaskTableDisplayFormat("mm/dd/yyyy");
    //JSGantt.parseJSON('./models_metadata/EST BenitoJuarez_Schedule.json', g);
    JSGantt.parseJSONString(json, g);
    if (nowdate !== undefined) {
        g.todayDate = new Date(nowdate)
    }
    g.Draw();
    console.log("Gantt draw Finish");
    ganttEl = g;
    //console.log("loaded js gantt");
}

