

debugViewer = undefined;
window.forgeViewApp = [];
window.forgeFunctions = {

    initialize: function (viewerId, accesToken, direction, viewguid, dotNetInstance) {
        var viewer;
        var viewerApp;
        var options = {
            'env': 'AutodeskProduction',
            'accessToken': accesToken,
            'api': 'derivativeV2'
        };
        
        var documentId = 'urn:' + direction;
        Autodesk.Viewing.Initializer(options, function onInitialized() {

           
            viewer = forgeFunctions.lookForViewer(viewerId);
            if (viewer !== undefined) {
                console.log("viewerId : " + viewerId + " already exists");
                return;
            }

            //Simple implementation
            //Autodesk.Viewing.Document.load(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);

            var containerId = document.getElementById(viewerId);
            var config3d = {
                'extensions': ['selectionEvent', 'myToolBarExt', 'changeColors']
                //extensions: ['MyAwesomeExtension']
            };
            //v6.0
            //viewerApp = new Autodesk.Viewing.ViewingApplication(containerId);
            //viewerApp.registerViewer(viewerApp.k3D, Autodesk.Viewing.Private.GuiViewer3D, config3d);
            //viewerApp.loadDocument(documentId, onDocumentLoadSuccess, onDocumentLoadFailure);

            //v7.0
            viewer = new Autodesk.Viewing.GuiViewer3D(containerId, config3d);
            viewer.itzdesktop = dotNetInstance;
            function onDocumentLoadFailure_v7() {
                console.error('Failed fetching Forge manifest');
            }

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


            viewer.start();
            Autodesk.Viewing.Document.load(documentId, onDocumentLoadSuccess, onDocumentLoadFailure_v7);
        });

        function onDocumentLoadSuccess(doc) {
            console.log("onDocumentLoadSuccess");

            // Create Viewer instance
            var geo = forgeFunctions.SearchGeometry(doc, viewguid);
            //v6.0
            //var viewerDiv = document.getElementById(viewerId);
            //var config = {
            //    extensions: geo.extensions() || ['selectionEvent', 'myToolBarExt', 'changeColors']
            //};
            //viewer = new Autodesk.Viewing.Private.GuiViewer3D(viewerDiv, config);
            //debugViewer = viewer;
            //// Load the chosen geometry
            //var svfUrl = doc.getViewablePath(geo);
            //var modelOptions = {
            //    sharedPropertyDbPath: doc.getPropertyDbPath()
            //};
            //viewer.start(svfUrl, modelOptions, onLoadModelSuccess, onLoadModelError);

            //v7.0
            viewer.loadDocumentNode(doc, geo);
            console.log("loadDocumentNode .. finish");

            //#HGR CONFIGURATION
            //var modelNodes = viewerApp.bubble.search(Autodesk.Viewing.BubbleNode.MODEL_NODE);
            //viewerApp.selectItem(modelNodes[0].data);
            //var viewer = viewerApp.getCurrentViewer();
            viewer.setEnvMapBackground(false);
            viewer.setBackgroundColor(255, 255, 255, 255, 255, 255);
            viewer.setDisplayEdges(true);
            forgeViewApp.push({ "viewId": viewerId, "viewer": viewer, "doc": doc });

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

    SearchGeometry : function (doc, guid) {
        // A document contains references to 3D and 2D geometries.
        var foundView;
        var geometries = doc.getRoot().search({ 'type': 'geometry' });
        if (geometries.length === 0) {
            console.error('Document contains no geometries.');
            return;
        }
        else {
            //var viewItem = Autodesk.Viewing.Document.getSubItemsWithProperties(doc.getRootItem(), {
            //    'guid': viewguid
            //}, true)
            var viewItem = doc.getRoot().findByGuid(guid).findParentGeom2Dor3D();
            foundView = viewItem;
            //viewer.loadDocumentNode(viewerDocument, viewables[0]);
        }

        var geometry = geometries[0];
        if (foundView !== undefined) {
            geometry = foundView;
        }
        return geometry;
    },

    lookForViewer: function (viewId) {
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                return register["viewer"]
            }
        }
    },

    lookForDocument: function (viewId) {
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                return register["doc"];
            }
        }
    },

    loadModel: function (viewId, guid, dotNetObject) {

        var doc = forgeFunctions.lookForDocument(viewId);
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (doc !== undefined && viewer !== undefined) {
            var geo = forgeFunctions.SearchGeometry(doc, guid);
            var path = doc.getViewablePath(geo);
            viewer.loadModel(path);
            if (dotNetObject !== undefined) {
                var timer = setInterval(function () {
                    console.log("check model is loaded ....");
                    var isDone = viewer.isLoadDone(viewId);
                    if (isDone === true) {
                        console.log("model loaded ... OK");
                        clearInterval(timer);
                        dotNetObject.invokeMethodAsync("IsModelLoaded");
                    }
                }, 3000)
            }
        }
    },

    isLoadDone: function (viewId) {
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (viewer !== undefined) {
            return viewer.isLoadDone();
        }
    },

    tearDown: function (viewId) {
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (viewer !== undefined) {
            viewer.tearDown();
        }
    },

    isolateElements: function (viewId, ids) {
        console.log("js_forge.isolateElements() ");

        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                //v6.0
                //viewer.isolateById(ids);
                viewer.isolate(ids);
                viewer.fitToView(ids);
                console.log("js_forge.isolateElements() ... finish");
            }
        }
    },

    resize: function (viewId) {
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                viewer.resize();
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
        //console.log("js_forge.changeColor() ");
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
    },

    resetOverrideColors: function (viewId) {
        //console.log("js_forge.resetColors() ");
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                viewer.clearThemingColors();
            }
        }
    },

    showAll: function (viewId) {
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                
                viewer.showAll();
            }
        }
    },

    show: function (viewId, ids) {
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (viewer !== undefined) {
            viewer.show(ids);
        }
    },

    hide: function (viewId, ids) {
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (viewer !== undefined) {
            viewer.hide(ids);
        }
    },

    hideAll: function (viewId, ids) {
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (viewer !== undefined) {
            viewer.hide(ids);
        }
    },

    clearColors: function (viewId, ids) {
        var viewer = forgeFunctions.lookForViewer(viewId);
        if (viewer !== undefined) {
            viewer.hide(ids);
        }
    },

    fitToView: function (viewId, ids) {
        for (var i = 0; i < forgeViewApp.length; i++) {
            var register = forgeViewApp[i];
            if (register["viewId"] === viewId) {
                var viewer = register["viewer"];
                viewer.fitToView(ids);
                if (ids !== null) {
                    viewer.fitToView(ids);
                }
                else {
                    viewer.fitToView();
                    
                }
            }
        }
    }
}