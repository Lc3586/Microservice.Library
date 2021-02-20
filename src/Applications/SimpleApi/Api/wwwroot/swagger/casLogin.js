/*
*   swagger 新增 CAS登录功能
*
*   LCTR 2020-03-13
*/
$(function () {
    /**
    *
    * 检查登录状态
    * LCTR 2020-12-07
    *
    * @method check
    *
    */
    var check = () => {
        //方式一
        $.ajax({
            type: 'POST',
            headers: { 'Content-Type': 'application/json' },
            url: "/cas/authorized",
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

        //方式二
        //var casInfo = window.location.request('casInfo');
        //if (casInfo)
        //    addBtn(JSON.parse(decodeURI(casInfo).replace(/%3A/g, ':')));
        //else
        //    addBtn(false);
    };
    /**
    *
    * 添加操作按钮
    * LCTR 2020-12-07
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
                var btn = $('<button class="btn authorize unlocked "><span>CAS登录信息</span><svg width="20" height="20"><use href="#unlocked" xlink:href="#unlocked"></use></svg></button>');
                btn.on('click', () => {
                    var content = [];
                    for (var item in data) {
                        content.push(['input-readonly', item, data[item]]);
                    }
                    window.delayedEvent(() => {
                        window.showDialog(
                            'CAS登录信息',
                            content,
                            {
                                '注销（方式一）': ['双击查看对接说明', {
                                    'click': () => {
                                        window.delayedEvent(() => { logout(1); }, 800, 'logout', false);
                                    },
                                    'dblclick': () => {
                                        window.delayedEvent(() => {
                                            window.showDialog(
                                                '注销方式一',
                                                [
                                                    ['label', '注销登录', '跳转至注销地址/cas/logout?returnUrl=，并附带参数（登录后的重定向地址）'],
                                                    ['label', '未登录', '浏览器重定向至指定地址'],
                                                    ['label', '注销后', '浏览器重定向至指定地址'],
                                                    ['label', '如果要单点注销', '附加参数logoutCAS=true']
                                                ]);
                                        }, 100, 'logout', false);
                                    }
                                }],
                                '注销（方式二）': ['双击查看对接说明', {
                                    'click': () => {
                                        window.delayedEvent(() => { logout(2); }, 800, 'logout', false);
                                    },
                                    'dblclick': () => {
                                        window.delayedEvent(() => {
                                            window.showDialog(
                                                '注销方式二',
                                                [
                                                    ['label', '注销登录', 'POST请求/cas/logout接口'],
                                                    ['label', '未登录', '接口返回状态码401'],
                                                    ['label', '注销后', '接口返回状态码200']
                                                ]);
                                        }, 100, 'logout', false);
                                    }
                                }]
                            });
                    }, 100, 'loginInfo', false);
                });
                btns.push(btn);
            } else {
                var btn_1_0 = $('<button class="btn authorize locked " title="双击查看对接说明"><span>CAS登录（方式一）</span><svg width="20" height="20"><use href="#locked" xlink:href="#locked"></use></svg></button>');
                btn_1_0.on('click', () => {
                    window.delayedEvent(() => { login(1); }, 800, 'login', false);
                }).on('dblclick', () => {
                    window.delayedEvent(() => {
                        window.showDialog(
                            '登录方式一',
                            [
                                ['label', '登录验证', 'POST请求/cas/authorized接口'],
                                ['label', '未登录', '接口返回状态码401, 此时应该跳转至登录地址/cas/login?returnUrl=，并附带参数（登录后的重定向地址）'],
                                ['label', '已登录', '接口返回状态码200, 以及身份信息']
                            ]);
                    }, 100, 'login', false);
                });
                btns.push(btn_1_0);

                var btn_2_0 = $('<button class="btn authorize locked " title="双击查看对接说明"><span>CAS登录（方式二）</span><svg width="20" height="20"><use href="#locked" xlink:href="#locked"></use></svg></button>');
                btn_2_0.on('click', () => {
                    window.delayedEvent(() => { login(2); }, 800, 'login', false);
                }).on('dblclick', () => {
                    window.delayedEvent(() => {
                        window.showDialog(
                            '登录方式二',
                            [
                                ['label', '登录验证', '跳转至验证地址/cas/authorize?returnUrl=，并附带参数（登录后的重定向地址）'],
                                ['label', '未登录', '浏览器重定向至登录地址'],
                                ['label', '登录后', '浏览器重定向指指定地址，地址栏附带身份信息'],
                                ['label', '获取身份信息', 'JSON.parse(decodeURI(window.location.request("casInfo")).replace(/%3A/g, ":"))'],
                                ['label', '注释', 'window.location.request()是本页面的一个封装方法']
                            ]);
                    }, 100, 'login', false);
                });
                btns.push(btn_2_0);
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
    * LCTR 2020-12-07
    *
    * @method login
    *
    * @param {number} mode 方式
    *
    */
    var login = (mode) => {
        switch (mode) {
            case 2:
                location.href = "/cas/authorize?returnUrl=" + location.href;
                break;
            case 1:
            default:
                location.href = $('[for="servers"] select').val() + '/cas/login?returnUrl=' + location.href;
                break;
        }
    };

    /**
    *
    * 注销
    * LCTR 2020-12-07
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
                var done = (logoutCAS) => {
                    location.href = $('[for="servers"] select').val() + '/cas/logout?returnUrl=' + location.href + '&logoutCAS=' + logoutCAS;
                };

                window.showDialog(
                    '是否单点注销',
                    [
                        ['label', '单点注销', '当前登录的所有应用都会注销']
                    ],
                    {
                        '是': {
                            'click': () => {
                                done('true');
                            }
                        },
                        '否': {
                            'click': () => {
                                done('false');
                            }
                        }
                    },
                    false);
                break;
            case 2:
                $.ajax({
                    type: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    url: "/cas/logout",
                    dataType: 'json',
                    success: function (data) {
                        window.location.reload();
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
                break;
        }
    };

    check();
});