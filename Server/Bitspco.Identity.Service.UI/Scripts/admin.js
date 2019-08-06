$(document).ready(function () {
    setTimeout(function () {
        document.body.classList.add('loaded');
    }, 100);

    $('main nav .menu-toggle a').click(function () {
        if (document.body.classList.contains('menu-open')) document.body.classList.remove('menu-open');
        else document.body.classList.add('menu-open');
    });
    $('main nav .help a').click(function () {
        var helpBox = document.getElementById('help-box')
        if (helpBox.classList.contains('show')) document.body.classList.remove('show');
        else helpBox.classList.add('show');
    });
    $('main nav .full-screen a').click(function () {
        document.body.classList.add('full-screen');
        document.body.requestFullscreen();
    });
    $('main nav .full-screen-exit a').click(function () {
        document.body.classList.remove('full-screen');
        document.exitFullscreen()
    });
    $('.menu a.open').click(function () {
        this.parentNode.classList.add('open')
    });
    $('.menu a.close').click(function () {
        this.parentNode.parentNode.parentNode.classList.remove('open')
    });
    $(`.menu a[href~="${location.pathname}"]`).each(function () {
        var element = this;
        while (element) {
            if (element.tagName == 'LI') element.classList.add('open');
            if (element.classList.contains('menu')) break;
            element = element.parentNode;
        }
    });
})

function logout() {
    config.loginInfo = null;
    location.href = '/Auth';
}
function lock() {
    var loginInfo = config.loginInfo;
    loginInfo.Key = null;
    config.loginInfo = loginInfo;
    location.href = '/Auth/Lock';
}