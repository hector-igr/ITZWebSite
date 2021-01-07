let tree = debugViewer.model.getData().instanceTree

function findNodeIdbyName(name) {
    let nodeList = Object.values(tree.nodeAccess.dbIdToIndex);
    for (let i = 0, len = nodeList.length; i < len; ++i) {
        if (tree.getNodeName(nodeList[i]) === name) {
            return nodeList[i];
        }
    }
    return null;
};

tree.enumNodeFragments(nodeId, function (frag) {
    let fragProxy = debugViewer.impl.getFragmentProxy(debugViewer.model, frag);
    console.log(fragProxy);


});

