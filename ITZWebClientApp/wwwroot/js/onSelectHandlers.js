function onForgeItemClick(itemId) {
    console.log("ONFORGEITEMCLICK");
    itemId = JSON.stringify(itemId);
    console.log(itemId);
    var elem = document.getElementById("selectedId");
    elem.value = itemId;

    DotNet.invokeMethodAsync("ForgeViewerClient" ,"ChangeIds", itemId);
}