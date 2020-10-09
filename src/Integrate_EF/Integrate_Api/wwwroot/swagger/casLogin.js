/*
*   swagger 新增 CAS登录功能
*
*   LCTR 2020-03-13
*/

$(function () {
    var addBtn = (data) => {
        setTimeout(() => {
            $('.scheme-container .schemes').length ? 1 : addBtn(data);
            var btn = $('<button class="btn authorize ' + (data ? 'unlocked' : 'locked') + ' "><span>' + (data ? 'CAS登录信息' : 'CAS登录') + '</span><svg width="20" height="20"><use href="#' + (data ? 'unlocked' : 'locked') + '" xlink:href="#' + (data ? 'unlocked' : 'locked') + '"></use></svg></button>');
            $('.scheme-container .schemes')
                .append($('<div class="auth-wrapper"></div>')
                    .append(btn.on('click', () => { data ? loginInfo(data) : login() })));
        }, 100);
    }

    var login = () => {
        location.href = $('[for="servers"] select').val() + '/caslogin?returnUrl=' + location.href;
    }

    var logout = () => {
        location.href = $('[for="servers"] select').val() + '/caslogout?ReturnUrl=' + location.href;
    }

    var loginInfo = (data) => {
        var close = () => { body.fadeOut(); },
            body = $('<div id="dialog" class="dialog-ux"><div class="backdrop-ux"></div><div class="modal-ux"><div class="modal-dialog-ux"><div class="modal-ux-inner"><div class="modal-ux-header"><h3>CAS登录信息</h3></div><div class="modal-ux-content"><div class="auth-container"><div><div><div class="auth-container-items"></div><div class="auth-btn-wrapper"></div></div></div></div></div></div></div></div></div></div>'),
            logoutBtn = $('<button class="btn modal-btn auth authorize button">退出登录</button>'),
            closeBtn_0 = $('<button class="btn modal-btn auth btn-done button">关闭</button>'),
            closeBtn_1 = $('<button class="close-modal"><svg width="20" height="20"><use href="#close" xlink:href="#close"></use></svg></button>'),
            info = '';
        logoutBtn.on('click', logout).appendTo(body.find('.auth-btn-wrapper'));
        closeBtn_0.on('click', close).appendTo(body.find('.auth-btn-wrapper'));
        closeBtn_1.on('click', close).appendTo(body.find('.modal-ux-header'));

        for (var item in data) {
            info += item == 'appName' ? ('<h5>应用:' + data[item] + '</h5>') :
                ('<label for="' + item + '">' + item + ':</label><section class="block-tablet col-10-tablet block-desktop col-10-desktop"><input type="text" id="' + item + '" data-name="' + item + '" value="' + data[item] + '"></section>');
        }

        body.find('.auth-container-items').append(info);

        body.fadeIn().appendTo($('.scheme-container'));
    }

    var check = () => {
        $.ajax({
            type: 'GET',
            headers: { 'Content-Type': 'application/json' },
            url: "/casAuthorized",
            dataType: 'json',
            success: function (data) {
                addBtn(data);
            },
            error: function (text) {
                if (text != null && text.responseText) {
                    console.error(text.responseText);
                }
                addBtn(false);
            }
        });
    }

    check();

    $('.operation-filter-input').attr('placeholder', '筛选标签');
});