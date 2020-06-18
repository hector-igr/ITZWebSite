window.tableExpander = {
    contractGroup: function (event, groupId) {
        var status = event.currentTarget.getAttribute('isExpanded');
        var spn = event.currentTarget.getElementsByTagName('span')[0];
        if (status === 'true') {
            tableExpander.hide(groupId);
            event.currentTarget.setAttribute('isExpanded', 'false');
            if (spn !== undefined) spn.setAttribute('class', 'fa fa-chevron-down');
        }
        else {
            tableExpander.show(groupId);
            event.currentTarget.setAttribute('isExpanded', 'true');
            if (spn !== undefined) spn.setAttribute('class', 'fa fa-chevron-up');
        }
    },

    hide: function (groupId) {
        var query = '[groupId="' + groupId + '"]';
        var containers = document.querySelectorAll(query);
        for (var i = 0; i < containers.length; i++) {
            var cont = containers[i];
            cont.setAttribute('hidden', 'hidden');
        }
    },

    show: function (groupId) {
        var query = '[groupId="' + groupId + '"]';
        var containers = document.querySelectorAll(query);
        for (var i = 0; i < containers.length; i++) {
            var cont = containers[i];
            cont.removeAttribute('hidden');
        }
    },

    expand: function (rowId) {

    },

    collapse: function (rowId) {
        document.getElementById(rowId).innerHTML = "";
        cont.setAttribute('isExpanded', 'false');
    }
}