// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function myFunction() {
    myVar = setTimeout(showPage, 3000);
}

function showPage() {
    document.getElementById("loader").style.display = "none";
    document.getElementById("myDiv").style.display = "block";
}

 
window.PaintRow = function (groupId) {
    //this.console.log("---->" + groupId);

    var favColor = getComputedStyle(document.documentElement)
        .getPropertyValue('--ribbonColor');

    //var parent = document.querySelector('[groupid=' + groupId +']');
    var rows = this.document.getElementsByTagName("tr");

    for (var r = 0; r < rows.length; r++) {
        rows[r].style.background = "white";
        rows[r].style.color = "inherit";
    }
    var parent = this.document.getElementById(groupId);
    if (parent !== this.undefined) {
        //this.console.log(parent);
        parent.style.background = favColor;
        parent.style.color = "white";
    }
}


window.ChangeSelectIndexById = function (id, value) {
    //console.log("ChangeSelectIndexById()");
    var els = this.document.getElementById(id)
    if (els !== this.undefined) {
        els.selectedIndex = value;
    }
}

window.ChangeSelectIndexByClass = function (classId, value) {
    //console.log("ChangeSelectIndex()");
    var selector = "[class^=" + classId + " ]";
    //this.console.log("ChangeSelectIndex " + selector);
    var els = this.document.querySelectorAll(selector);
    for (var i = 0; i < els.length; i++) {
        //this.console.log(els[i]);
        els[i].selectedIndex = value;
    }
}

window.GetSelectedIndex = function (id) {
    var indx = document.getElementById(id).selectedIndex;
    //this.console.log("GetSelectedIndex() " + indx);
    return indx;
}


OnLoadVideo = function (id, dotNetItem) {
    var video = document.getElementById(id);
    if (video !== null) {
        video.onloadeddata = function () {
            result = true;
            if (dotNetItem !== undefined) {
                //dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
                setTimeout(function () {
                    dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
                }, 3000);
            }
        }
    }
    else {
        //dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
        setTimeout(function () {
            dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
        }, 3000);
    }
}

OnAllImageLoaded = function (idContainer, dotNetItem) {
    var container = document.getElementById(idContainer);
    var images = container.getElementsByTagName("img");
    var counter = 0;
    if (images !== null && images.length > 0) {
        for (var i = 0; i < images.length; i++) {
            images[i].onload = function (e) {
                counter++;
                if (counter === images.length) {
                    //console.log("onimagesloaded");
                    //dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
                    setTimeout(function () {
                        dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
                    }, 3000);
                }
            }
        }
    }
    else {
        //dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
        setTimeout(function () {
            dotNetItem.invokeMethodAsync("ChangeMediaStatus", true);
        }, 3000);
    }
}

GetCssRootValue = function (parameter) {
    let v = getComputedStyle(document.documentElement).getPropertyValue(parameter);
    console.log(v);
    //v = hex2rgb(v);
    //console.log(v);
    return v;
}


window.splitters = []
LoadSplitter = function (splitterName, selectorOne, selectorTwo, direction, gutterWidth, iniSizes, dotnetitem) {
    var splt = Split([selectorOne, selectorTwo],
        {
            gutterSize: gutterWidth,
            direction: direction,
            minSize: [0, 0],
            sizes: iniSizes,
            onDragEnd: function (sizes) { console.log('finish resize'); dotnetitem.invokeMethodAsync('UpdateResize', sizes) }
        });
    splitters.push({ name: splitterName, splitter: splt });
    //Split(['.forge_chart', '.forge_table'], { direction: 'vertical', gutterSize: 5 });
}

GetSplitter = function (splitterName) {
    for (var i = 0; i < splitters.length; i++) {
        var record = splitters[i];
        if (record.name === splitterName) {
            return record.splitter;
        }
    }
    return undefined;
}

ChangeSplitterSize = function (splitterName, newSizes) {
    var splitter = GetSplitter(splitterName);
    if (splitter !== undefined) {
        splitter.setSizes(newSizes);
    }
}

SetSplitterToMin = function (splitterName, indexToExpand) {
    var splitter = GetSplitter(splitterName);
    if (splitter !== undefined) {
        sizes = [];
        if (indexToExpand === 0) {
            sizes = [0, 100];
        }
        else {
            sizes = [100, 0];
        }
        splitter.setSizes(sizes);
    }
}








var ganttEl;
var ttask;

LoadJsGantt = function (id, dotnetitem, nowdate) {
    console.log("LoadJsGantt()");
    var g = new JSGantt.GanttChart(document.getElementById(id), 'day');
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
    if (nowdate !== undefined) {

        g.Draw(new Date(nowdate));
    }
    else {
        g.Draw();
    }
    ganttEl = g;
}

LoadJsGanttData = function (json, nowdate) {
    console.log("LoadJsGanttData()");
    JSGantt.parseJSONString(json, ganttEl);
    if (nowdate !== undefined) {

        ganttEl.Draw(new Date(nowdate));
    }
    else {
        ganttEl.Draw();
    }
}

UnloadJsGanttData = function () {
    ganttEl.ClearTasks();
    ganttEl.Draw();
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
                if (dotnetitem !== undefined){
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

        g.Draw(new Date(nowdate));
    }
    else {
        g.Draw();
    }
    
    ganttEl = g;
    //console.log("loaded js gantt");
}


jsGanttResizeHeight = function (height) {
    console.log(ganttEl);
}