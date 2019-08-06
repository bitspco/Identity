
const baseUrl = 'http://localhost:13248/';
window.config = {
    api: {
        baseUrl: baseUrl,
        auth: baseUrl + 'Auth/',
        authenticatorApps: baseUrl + 'AuthenticatorApps/',
        claims: baseUrl + 'Claims/',
        events: baseUrl + 'Events/',
        modules: baseUrl + 'Modules/',
        permissions: baseUrl + 'Permissions/',
        positions: baseUrl + 'Positions/',
        questions: baseUrl + 'Questions/',
        roles: baseUrl + 'Roles/',
        thirdPartyAccesses: baseUrl + 'ThirdPartyAccesses/',
        thirdPartyApps: baseUrl + 'ThirdPartyApps/',
        tokens: baseUrl + 'Tokens/',
        users: baseUrl + 'Users/',
    },

    get loginInfo() { return JSON.parse(localStorage.getItem('loginInfo')); },
    set loginInfo(value) {
        if (value) localStorage.setItem('loginInfo', JSON.stringify(value));
        else localStorage.removeItem('loginInfo');
    }
};

$.ajaxSetup({
    global: true,
    contentType: 'application/json',
    beforeSend: function (xhr) {
        var loginInfo = config.loginInfo;
        if (loginInfo) xhr.setRequestHeader('Authorization', `Bearer ${loginInfo.Key}`)
    },
    complete: function (xhr) {
        if (xhr.status == 401) {
            localStorage.removeItem('loginInfo');
            location.href = '/Auth';
        }
    }
});