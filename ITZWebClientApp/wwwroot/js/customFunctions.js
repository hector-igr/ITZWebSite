function windowSize() {
    return [window.outerWidth, window.outerHeight];
}

function getElementSize(element) {
    var rect = element.getBoundingClientRect();
    var h = rect.height;
    var w = rect.width;
    return [w,h];
}

function resizeTableBody(tableElId, headerElId, tableBodyElId) {
    var table = document.getElementById(tableElId);
    var header = document.getElementById(headerElId);
    var body = document.getElementById(tableBodyElId);

    if (table && header && body) {
        var h = table.style.offsetHeigth - header.style.offsetHeigth;
        body.style.offsetHeigth = h;
        console.log(table.offsetHeight);
    }

    
}