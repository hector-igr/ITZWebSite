window.consoleITZ = {
    group: function (str, collapse = true) {
        if (collapse) {
            console.groupCollapsed(str);
        }
        else {
            console.group(str);
        }
    },
    groupEnd: function () {
        console.groupEnd();
    },
    log: function (str) {
        console.log(str);
    },
    warning: function (str) {
        console.warn(str);
    },
    error: function (str) {
        console.error(str);
    },
}