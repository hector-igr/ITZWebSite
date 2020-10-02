window.forgeViewApp = [];
window.forgeFunctions = {

    initialize: function (viewerId, accesToken, direction, dotNetInstance) {
        var viewer;
        var viewerApp;
        var options = {
            'env': 'AutodeskProduction',
            'accessToken': accesToken,
            'api': 'derivativeV2'
        };
        
        var documentId = 'urn:' + direction;
        Autodesk.Viewing.Initializer(options, function onInitialized() {

            //Simple implementation
            //Autodesk.Viewing.Document.load(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);

            var containerId;
            var config3d = {
                'extensions': ['selectionEvent', 'myToolBarExt', 'changeColors']
            };
            viewerApp = new Autodesk.Viewing.ViewingApplication(containerId);
            viewerApp.registerViewer(viewerApp.k3D, Autodesk.Viewing.Private.GuiViewer3D, config3d);
            viewerApp.loadDocument(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);

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

                    if (dotNetInstance !== undefined) {
                        dotNetInstance.invokeMethodAsync("OnViewerChangedSelection", currentSelection);
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
                if (this.viewer.toolbar) {
                    this.createUI();
                }
                else {
                    this.onToolbarCreatedBinded = this.onToolbarCreated.bind(this);
                    this.viewer.addEventListener(av.TOOLBAR_CREATED_EVENT, this.onToolbarCreatedBinded);
                }
                return true;
            };

            ToolbarExtension.prototype.onToolbarCreated = function () {
                this.viewer.removeEventListener(av.TOOLBAR_CREATED_EVENT, this.onToolbarCreatedBinded);
                this.onToolbarCreatedBinded = null;
                this.createUI();
            };

            ToolbarExtension.prototype.createUI = function () {
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

                viewer.toolbar.addControl(this.subToolbar);
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

            ////#Isolate
            //function isolateElements(ids) {
            //    var viewer = viewerApp.getCurrentViewer();
            //    viewer.isolateById(ids);
            //    viewer.fitToView(ids);
            //}

            ////#Hide
            //Autodesk.Viewing.View3D.prototype.turnOff = function (dbIds) {
            //    var node;
            //    var viewer = viewerApp.getViewer();
            //    if (Array.isArray(dbIds)) {
            //        for (var i = 0; i < dbIds.length; i++) {
            //            var id = dbIds[i];
            //            node = viewer;
            //        }
            //        viewer.impl.visibilityManager.setNodeOff(node, true);
            //    }
            //    else {
            //        node = viewer.model.getData().instanceTree.dbIdToNode[dbIds];
            //        viewer.impl.visibilityManager.setNodeOff(node, true);
            //    }
            //}

            ////One Element Click
            //function onElementClicked(ev) {
            //    var item = myChart.getElementAtEvent(ev)[0];
            //    if (item !== undefined) {
            //        var index = item._index;
            //        isolateElements(elems[index]);
            //    }
            //}
            
        });


        function onDocumentLoadSuccess(doc) {
            // A document contains references to 3D and 2D geometries.
            var geometries = doc.getRoot().search({ 'type': 'geometry' });
            if (geometries.length === 0) {
                console.error('Document contains no geometries.');
                return;
            }

            // Choose any of the avialable geometries
            var initGeom = geometries[0];

            // Create Viewer instance
            var viewerDiv = document.getElementById(viewerId);
            var config = {
                extensions: initGeom.extensions() || ['selectionEvent', 'myToolBarExt', 'changeColors']
            };
            viewer = new Autodesk.Viewing.Private.GuiViewer3D(viewerDiv, config);
            //console.log(viewer);

            // Load the chosen geometry
            var svfUrl = doc.getViewablePath(initGeom);
            var modelOptions = {
                sharedPropertyDbPath: doc.getPropertyDbPath()
            };
            viewer.start(svfUrl, modelOptions, onLoadModelSuccess, onLoadModelError);


            //#HGR CONFIGURATION
            //var modelNodes = viewerApp.bubble.search(Autodesk.Viewing.BubbleNode.MODEL_NODE);
            //viewerApp.selectItem(modelNodes[0].data);
            //var viewer = viewerApp.getCurrentViewer();
            viewer.setEnvMapBackground(false);
            viewer.setBackgroundColor(255, 255, 255, 255, 255, 255);
            viewer.setDisplayEdges(true);

            forgeViewApp.push({ "viewId" : viewerId, "viewer": viewer });
            //console.log("FORGE DIC");
            //console.log(viewer);

            //DotNet.invokeMethodAsync("ITZWebClientApp", "OnLoadSuccess");
            if (dotNetInstance !== undefined) {
                dotNetInstance.invokeMethodAsync("OnLoadInstanceSuccess");
            };
        }

        function onDocumentLoadFailure(viewerErrorCode) {
            console.error('onDocumentLoadFailure() - errorCode:' + viewerErrorCode);
        }

        function onLoadModelSuccess(model) {
            console.log('js_forge.onLoadModelSuccess()!');
            console.log('   Validate model loaded: ' + (viewer.model === model));
            //console.log(model);
        }

        function onLoadModelError(viewerErrorCode) {
            console.error('onLoadModelError() - errorCode:' + viewerErrorCode);
        }
    },

    isolateElements: function (viewId, ids) {
        console.log("js_forge.isolateElements() ");
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                viewer.isolateById(ids);
                viewer.fitToView(ids);
            }
        }
    },

    changeRGBAStringToThreeVector4: function (backgroundColor) {
        var color = backgroundColor.replace("rgba(", "");
        color = color.replace(")", "");
        color = color.split(",");
        var threeVector = new THREE.Vector4(Number(color[0]) / 255, Number(color[1]) / 255, Number(color[2]) / 255, Number(color[3]));
        return threeVector;
    },

    changeColor: function (viewId, ids, rgbString) {
        console.log("js_forge.changeColor() ");
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                var color = forgeFunctions.changeRGBAStringToThreeVector4(rgbString);
                for (var c = 0; c < ids.length; c++) {
                    viewer.setThemingColor(ids[c], color);
                }
            }
        }
    }
}