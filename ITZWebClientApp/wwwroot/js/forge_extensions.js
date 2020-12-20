//Examples
///////////////////////////////

function MyAwesomeExtension(viewer, options) {
    Autodesk.Viewing.Extension.call(this, viewer, options);

    // Preserve "this" reference when methods are invoked by event handlers.
    this.lockViewport = this.lockViewport.bind(this);
    this.unlockViewport = this.unlockViewport.bind(this);
}

MyAwesomeExtension.prototype = Object.create(Autodesk.Viewing.Extension.prototype);
MyAwesomeExtension.prototype.constructor = MyAwesomeExtension;

MyAwesomeExtension.prototype.lockViewport = function () {
    this.viewer.setNavigationLock(true);
};

MyAwesomeExtension.prototype.unlockViewport = function () {
    this.viewer.setNavigationLock(false);
};

MyAwesomeExtension.prototype.load = function () {
    // alert('MyAwesomeExtension is loaded!');

    this._lockBtn = document.getElementById('MyAwesomeLockButton');
    this._lockBtn.addEventListener('click', this.lockViewport);

    this._unlockBtn = document.getElementById('MyAwesomeUnlockButton');
    this._unlockBtn.addEventListener('click', this.unlockViewport);

    return true;
};

MyAwesomeExtension.prototype.unload = function () {
    // alert('MyAwesomeExtension is now unloaded!');

    if (this._lockBtn) {
        this._lockBtn.removeEventListener('click', this.lockViewport);
        this._lockBtn = null;
    }

    if (this._unlockBtn) {
        this._unlockBtn.removeEventListener('click', this.unlockViewport);
        this._unlockBtn = null;
    }

    return true;
};

Autodesk.Viewing.theExtensionManager.registerExtension('MyAwesomeExtension', MyAwesomeExtension);

////////////////////////////////////




//#onSelectionChanged

function SelectionChangedExtension(viewer, options) {
    Autodesk.Viewing.Extension.call(this, viewer, options);
    //this.handlers = eval(onSelectHandlers);
}

SelectionChangedExtension.prototype = Object.create(Autodesk.Viewing.Extension.prototype);
SelectionChangedExtension.prototype.constructor = SelectionChangedExtension;

SelectionChangedExtension.prototype.onSelectionEvent = function (event) {
    var currentSelection = this.viewer.getSelection();
    if (currentSelection.length > 0) {
        console.log("js_forgeFunctions.SelectionChangedExtension");
        //console.log(currentSelection);
        //console.log(onSelectHandlers);

        //var array = onSelectHandlers.split(",");
        //for (var i = 0; i < array.length; i++) {

        //    var cmd = array[i] + "([" + currentSelection + "])";
        //    console.log(cmd);
        //    eval(cmd);
        //}

        if (this.viewer.itzdesktop !== undefined) {
            this.viewer.itzdesktop.invokeMethodAsync("OnViewerChangedSelection", currentSelection);
        }
        //viewer.isolateById(currentSelection);
        //viewer.fitToView(currentSelection);
    }
    //if (currentSelection !== 0) {
    //    onSelectionChanged(currentSelection);
    //    var node = this.viewer.fitToView(currentSelection);
    //}
};

SelectionChangedExtension.prototype.load = function () {
    this.onSelectionBinded = this.onSelectionEvent.bind(this);
    this.viewer.addEventListener(Autodesk.Viewing.SELECTION_CHANGED_EVENT, this.onSelectionBinded);
    return true;
};

SelectionChangedExtension.prototype.unload = function () {
    this.viewer.removeEventListener(Autodesk.Viewing.SELECTION_CHANGED_EVENT, this.onSelectionBinded);
    this.onSelectionEvent = null;
    return true;
};
Autodesk.Viewing.theExtensionManager.registerExtension('selectionEvent', SelectionChangedExtension);


//#Add Extra Toolbar

function ToolbarExtension(viewer, options) {
    Autodesk.Viewing.Extension.call(this, viewer, options);
}
ToolbarExtension.prototype = Object.create(Autodesk.Viewing.Extension.prototype);
ToolbarExtension.prototype.constructor = ToolbarExtension;

ToolbarExtension.prototype.load = function () {

    //V6.0
    //if (this.viewer.toolbar) {
    //    this.createUI();
    //}
    //else {
    //    this.onToolbarCreatedBinded = this.onToolbarCreated.bind(this);
    //    this.viewer.addEventListener(av.TOOLBAR_CREATED_EVENT, this.onToolbarCreatedBinded);
    //}
    return true;
};

//v6.0
//ToolbarExtension.prototype.onToolbarCreated = function () {
//    this.viewer.removeEventListener(av.TOOLBAR_CREATED_EVENT, this.onToolbarCreatedBinded);
//    this.onToolbarCreatedBinded = null;
//    this.createUI();
//};

//ToolbarExtension.prototype.createUI = function () {
//    var viewer = this.viewer;
//    var button1 = new Autodesk.Viewing.UI.Button('changeColors-button');
//    var b = false;
//    button1.onClick = function (e) {
//        console.log("GHOSTING");
//        //if (idsData !== undefined) { //check idsdata implementation

//        //}
//        viewer.setGhosting(b);
//        b = !b;
//    };
//    button1.addClass('changeColors-button');
//    button1.setToolTip('Show/Hide Wireframe');

//    var button2 = new Autodesk.Viewing.UI.Button('resetColors-button');
//    button2.onClick = function (e) {
//        var changeColors = new ChangeColors(viewer, null);
//        changeColors.ResetColors();
//    };
//    button2.addClass('resetColors-button');
//    button2.setToolTip('Reset Colors');

//    this.subToolbar = new Autodesk.Viewing.UI.ControlGroup('hgr-toolbar');
//    this.subToolbar.addControl(button1);
//    this.subToolbar.addControl(button2);

//    viewer.toolbar.addControl(this.subToolbar);
//};



ToolbarExtension.prototype.onToolbarCreated = function (toolbar) {
    var viewer = this.viewer;
    var button1 = new Autodesk.Viewing.UI.Button('changeColors-button');
    var b = false;
    button1.onClick = function (e) {
        console.log("GHOSTING");
        //if (idsData !== undefined) { //check idsdata implementation

        //}
        viewer.setGhosting(b);
        b = !b;
    };
    button1.addClass('changeColors-button');
    button1.setToolTip('Show/Hide Wireframe');

    var button2 = new Autodesk.Viewing.UI.Button('resetColors-button');
    button2.onClick = function (e) {
        var changeColors = new ChangeColors(viewer, null);
        changeColors.ResetColors();
    };
    button2.addClass('resetColors-button');
    button2.setToolTip('Reset Colors');

    this.subToolbar = new Autodesk.Viewing.UI.ControlGroup('hgr-toolbar');
    this.subToolbar.addControl(button1);
    this.subToolbar.addControl(button2);

    toolbar.addControl(this.subToolbar);
};

ToolbarExtension.prototype.unload = function () {
    this.viewer.toolbar.removeControl(this.subToolbar);
    return true;
};

Autodesk.Viewing.theExtensionManager.registerExtension('myToolBarExt', ToolbarExtension);

//#Change Colors

function ChangeColors(viewer, options) {
    Autodesk.Viewing.Extension.call(this, viewer, options);
}

ChangeColors.prototype = Object.create(Autodesk.Viewing.Extension.prototype);
ChangeColors.prototype.constructor = ChangeColors;

ChangeColors.prototype.ChangeColors = function (ids, color) {
    for (var i = 0; i < ids.length; i++) {
        this.viewer.setThemingColor(ids[i], color);
    }
};
ChangeColors.prototype.ResetColors = function (ids, color) {
    this.viewer.clearThemingColors();
};
ChangeColors.prototype.load = function () {
    return true;
};
ChangeColors.prototype.unload = function () {
    alert('change color unloaded');
};
Autodesk.Viewing.theExtensionManager.registerExtension('changeColors', ChangeColors);


/////////////////////////////////////////////