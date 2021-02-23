/*
*   swagger 新增 SampleAuthentication登录功能
*
*   LCTR 2021-02-20
*/
$(function () {
    /**
    *
    * 检查登录状态
    * LCTR 2021-02-20
    *
    * @method check
    *
    */
    var check = () => {
        $.ajax({
            type: 'POST',
            headers: { 'Content-Type': 'application/json' },
            url: "/sa/authorized",
            dataType: 'json',
            success: function (data) {
                addBtn(data);
            },
            error: function (response) {
                if (response.status == 401)
                    addBtn(false);
                else {
                    window.showDialog(
                        '接口异常',
                        [
                            ['H5', '状态码', response.status],
                            ['label', '输出内容', response.responseText]
                        ]);
                }
            }
        });
    };
    /**
    *
    * 添加操作按钮
    * LCTR 2021-02-20
    *
    * @method addBtn
    *
    * @param {string} data 登录信息
    * 
    */
    var addBtn = (data) => {
        setTimeout(() => {
            if (!$('.scheme-container .schemes').length) {
                addBtn(data);
                return;
            }

            var btns = [];
            if (data) {
                var btn = $('<button class="btn authorize unlocked "><span>SA登录信息</span><svg width="20" height="20"><use href="#unlocked" xlink:href="#unlocked"></use></svg></button>');
                btn.on('click', () => {
                    var content = [];
                    for (var item in data) {
                        content.push(['input-readonly', item, data[item]]);
                    }
                    window.delayedEvent(() => {
                        window.showDialog(
                            'SA登录信息',
                            content,
                            {
                                '注销': ['双击查看对接说明', {
                                    'click': () => {
                                        window.delayedEvent(() => { logout(1); }, 800, 'logout', false);
                                    },
                                    'dblclick': () => {
                                        window.delayedEvent(() => {
                                            window.showDialog(
                                                '注销',
                                                [
                                                    ['label', '注销登录', '跳转至注销地址/sa/logout并附带参数（可选参数, 注销后的重定向地址）'],
                                                    ['label', '未登录', '浏览器重定向至指定地址'],
                                                    ['label', '注销后', '浏览器重定向至指定地址']
                                                ]);
                                        }, 0, 'logout', false);
                                    }
                                }]
                            });
                    }, 0, 'loginInfo', false);
                });
                btns.push(btn);
            } else {
                var btn_1_0 = $('<button class="btn authorize locked " title="双击查看对接说明"><span>SA登录</span><svg width="20" height="20"><use href="#locked" xlink:href="#locked"></use></svg></button>');
                btn_1_0.on('click', () => {
                    window.delayedEvent(() => { login(1); }, 800, 'login', false);
                }).on('dblclick', () => {
                    window.delayedEvent(() => {
                        window.showDialog(
                            '登录',
                            [
                                ['label', '登录验证', 'POST请求/sa/authorized接口'],
                                ['label', '未登录', '接口返回状态码401, 此时应该跳转至登录页面, 之后请求/sa/login接口，并附带参数, 返回成功后跳转至指定页面'],
                                ['label', '已登录', '接口返回状态码200, 以及身份信息']
                            ]);
                    }, 0, 'login', false);
                });
                btns.push(btn_1_0);
            }

            var $wrapper = $('<div class="auth-wrapper"></div>');
            $.each(btns, (index, btn) => {
                btn.appendTo($wrapper);
            });
            $wrapper.appendTo($('.scheme-container .schemes'));
        }, 100);
    };

    /**
    *
    * 登录
    * LCTR 2021-02-20
    *
    * @method login
    *
    * @param {number} mode 方式
    *
    */
    var login = (mode) => {
        switch (mode) {
            case 1:
            default:
                window.showDialog(
                    '验证信息',
                    [
                        ['input', '用户名', ''],
                        ['input', '密码', '']
                    ],
                    {
                        '登录': ['双击查看对接说明', {
                            'click': () => {
                                $.ajax({
                                    type: 'POST',
                                    headers: { 'Content-Type': 'application/json' },
                                    url: "/sa/login",
                                    dataType: 'json',
                                    data: JSON.stringify({
                                        account: $('#key_0').val(),
                                        password: $('#key_1').val()
                                    }),
                                    success: function (data) {
                                        if (data.Success)
                                            location.reload();
                                        else
                                            window.showDialog(
                                                '登录失败',
                                                [
                                                    ['H5', '错误信息', data.Msg]
                                                ]);
                                    },
                                    error: function (response) {
                                        window.showDialog(
                                            '接口异常',
                                            [
                                                ['H5', '状态码', response.status],
                                                ['label', '输出内容', response.responseText]
                                            ]);
                                    }
                                });
                            }
                        }]
                    });
                break;
        }
    };

    /**
    *
    * 注销
    * LCTR 2021-02-20
    *
    * @method logout
    *
    * @param {number} mode 方式
    *
    */
    var logout = (mode) => {
        switch (mode) {
            case 1:
            default:
                location.href = $('[for="servers"] select').val() + '/sa/logout?returnUrl=' + location.href;
                break;
        }
    };

    check();
});