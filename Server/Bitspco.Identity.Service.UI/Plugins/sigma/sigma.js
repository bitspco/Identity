
function addHash(hash) {
    if (location.hash.indexOf(hash) < 0) {
        if (location.hash.indexOf('&&') > -1)
            location.hash += '&&' + hash;
        else location.hash = hash;
    }
}
function removeHash(hash) {
    if (location.hash.indexOf('&&') > -1)
        location.hash = location.hash.replace(new Regex(hash + '.*&&'), '');
    else location.hash = '';
}

jQuery.fn.showModal = function (args) {
    var element = this[0];
    if (element) {
        element.classList.add('show');
        if (element.onshow) element.onshow(args);
        addHash(element.id);
        var focusElement = $('[autofocus]', element).get(0);
        if (focusElement) focusElement.focus();
    }
}
jQuery.fn.hideModal = function () {
    var element = this[0];
    if (element) {
        element.classList.remove('show');
        if (element.onhide) element.onhide();
        removeHash(element.id);
    }
}

$(document).ready(function () {

    window.location.prehash = '';
    window.onhashchange = function () {

        var preactions = location.prehash.substr(1).split('&&');
        var actions = location.hash.substr(1).split('&&');

        for (var i = 0; i < preactions.length; i++) {
            const item = preactions[i];
            if (!item) continue;
            if (!actions.find(x => x == item)) {
                var args = item.split('/');
                if (args.length < 1) continue;
                var id = args[0];
                var element = $('#' + id);
                if (element) {
                    if (element.hasClass('modal')) element.hideModal();
                }
            }
        }
        
        for (var i = 0; i < actions.length; i++) {
            const item = actions[i]
            if (!item) continue;
            var args = item.split('/');
            if (args.length < 1) continue;
            var id = args[0];
            var element = $('#' + id);
            args = args.slice(1);
            if (element) {
                if (element.hasClass('modal')) element.showModal(args);
            }
        }
        window.location.prehash = location.hash;
    }
    window.onhashchange();

    $('[modal-toggle]').click(function () {
        console.log('aaa');
        var selector = this.getAttribute('modal-toggle');
        if (selector) $(selector).modalShow();
        else {
            var element = this.parentNode;
            while (element) {
                if (element.classList.contains('modal')) {
                    $(element).modalHide();
                    break;
                }
                element = element.parentNode;
            }
        }
    })
    //================================
    $(window).keyup(function (e) {
        if (e.keyCode == 27) $('.modal.show').hideModal();
    })
})