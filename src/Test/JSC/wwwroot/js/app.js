/// <reference path="oidc-client.js" />

function log() {
    //document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error：" + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += new Date() + '\r\n' + msg + '\r\n';
    });
}

document.getElementById('login').addEventListener('click', login, false);
document.getElementById('api').addEventListener('click', api, false);
document.getElementById('logout').addEventListener('click', logout, false);

var config = {
    authority: 'http://localhost:9000',
    redirect_uri: 'http://localhost:5002/html/callback.html',
    post_logout_redirect_uri: 'http://localhost:5002/html/index.html',
    client_id: 'JSC_test_00001',
    grant_type: 'authorization_code',
    response_type: 'code',
    scope: 'openid profile SSH',
};
var mgr = new Oidc.UserManager(config);

mgr.getUser().then(function (user) {
    if (user) {
        log('User logged in', user.profile);
    }
    else {
        log('User not logged in');
    }
});

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        console.info(user);
        if (!user) {
            log('getUser failed');
            return;
        }
        var url = 'https://localhost:9003/api/values';
        var xhr = new XMLHttpRequest();
        xhr.open('GET', url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader('Authorization', 'Bearer ' + user.access_token);
        xhr.send();
    })
}

function logout() {
    mgr.signoutRedirect();
}