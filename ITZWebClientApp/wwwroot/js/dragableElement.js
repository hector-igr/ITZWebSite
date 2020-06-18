window.dragableElement = {

    ini: function (id) {
        console.log("dragableElement.ini()");
        var pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0;
        var el = document.getElementById(id);
        if (el !== null) {
            el.onmousedown = dragMouseDown;
            console.log("dragableElement.ini() register OK");
        }
        else {
            console.log("dragableElement.ini() register FAILED");
        }

        function dragMouseDown(e) {
            e = e || window.event;
            e.preventDefault();

            // get the mouse cursor position at startup:
            pos3 = e.clientX;
            pos4 = e.clientY;

            document.onmouseup = closeDragElement;
            // call a function whenever the cursor moves:
            document.onmousemove = elementDrag;
        }

        function elementDrag(e) {
            e = e || window.event;
            e.preventDefault();

            // calculate the new cursor position:
            pos1 = pos3 - e.clientX;
            pos2 = pos4 - e.clientY;
            pos3 = e.clientX;
            pos4 = e.clientY;

            // set the element's new position:
            console.log("dragableElement.ini() drag");
            el.style.top = (el.offsetTop - pos2) + "px";
            el.style.left = (el.offsetLeft - pos1) + "px";
        }

        function closeDragElement() {
            console.log("dragableElement.ini() close");
            document.onmouseup = null;
            document.onmousemove = null;
        }
    }

}