// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


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


window.ChangeSelectIndex = function (id, value) {
    var selector = "[class^=" + id + " ]";
    //this.console.log("ChangeSelectIndex " + selector);
    var els = this.document.querySelectorAll(selector);
    for (var i = 0; i < els.length; i++) {
        //this.console.log(els[i]);
        els[i].selectedIndex = value;
    }
    
}
//ResetSelect("qry_Groups-0", "-1")
//ResetSelect("qry_Parameters-1", "-1")