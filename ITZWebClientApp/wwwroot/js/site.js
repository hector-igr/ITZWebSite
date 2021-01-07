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
    if (els !== null) {
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

window.DisableById = function (id) {
    document.getElementById(id).disabled = true;
}

window.EnableById = function (id) {
    document.getElementById(id).disabled = false;
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


window.splitters = [];

LoadSplitter = function (splitterName, selectorOne, selectorTwo, direction, gutterWidth, iniSizes, dotnetitem) {
    var splt = Split([selectorOne, selectorTwo],
        {
            gutterSize: gutterWidth,
            direction: direction,
            minSize: [0, 0],
            sizes: iniSizes,
            onDragEnd: function (sizes) { dotnetitem.invokeMethodAsync('UpdateResize', sizes) }
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

ShowGutter = function (splitterName) {
    var splitter = GetSplitter(splitterName);
    if (splitter !== undefined) {
        splitter.pairs[0].gutter.style.display = 'initial';
    }
}

HideGutter = function (splitterName) {
    var splitter = GetSplitter(splitterName);
    if (splitter !== undefined) {
        splitter.pairs[0].gutter.style.display = 'none';
    }
}


window.Popper = {
    logger: [],

    register: function (selector) {
        if (!this.logger.includes(selector)) {
            this.logger.push(selector);
            $(selector).popover()
        }
    },
    show: function (selector) {
        $(selector).popover('enable');
        $(selector).popover('show');
    },
    hide: function (selector) {
        
        $(selector).popover('hide');
        $(selector).popover('disable');
    },
}
