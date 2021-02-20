//封装方法
if (!window.location.request) {
    /**
     *
     * 获取URL地址栏参数(/a/b/c?xx=xx&a2=xx&a3=xx) 
     * LCTR 2018-03-21
     *
     * @method request
     * 
     * @param {string} param 参数名称
     *
     */
    window.location.request = function (param) {    /*获取URL的字符串*/    var sSource = String(window.document.location); var sName = param; var sReturn = ""; var sQUS = "?"; var sAMP = "&"; var sEQ = "="; var iPos;    /*获取sSource中的"?"，无则返回 -1*/    iPos = sSource.indexOf(sQUS); if (iPos == -1) return;    /*汲取参数，从iPos位置到sSource.length-iPos的位置，若iPos = -1，则：从-1 到 sSource.length+1  */    var strQuery = sSource.substr(iPos, sSource.length - iPos);    /* alert(strQuery);先全部转换为小写 */    var strLCQuery = strQuery.toLowerCase(); var strLCName = sName.toLowerCase();    /*从子字符串strLCQuery中查找“?”、参数名，以及“=”，即“?参数名=”  */    iPos = strLCQuery.indexOf(sQUS + strLCName + sEQ);    /*如果不存在*/    if (iPos == -1) {        /*继续查找可能的后一个参数，即带“&参数名=”*/        iPos = strLCQuery.indexOf(sAMP + strLCName + sEQ); }    /*判断是否存在参数 */    if (iPos != -1) { sReturn = strQuery.substr(iPos + sName.length + 2, strQuery.length - (iPos + sName.length + 2)); var iPosAMP = sReturn.indexOf(sAMP); if (iPosAMP == -1) { return sReturn; } else { sReturn = sReturn.substr(0, iPosAMP); } } return sReturn; }
}

if (!window.delayedEvent) {
    var delayedEvents = {};

    /**
     *
     * 延时事件 
     * LCTR 2020-12-08
     *
     * @method delayedEvent
     *
     * @param {Function} handler 处理函数
     * @param {number} event 延时(毫秒)(默认800)
     * @param {string} event 事件名称
     * @param {boolean} repeat 禁止重复(默认禁止)
     *
    */
    window.delayedEvent = function (handler, timeout, event, repeat = false) {
        if (!repeat) {
            event ? 1 : event = Date.now();
            if (delayedEvents[event])
                window.clearTimeout(delayedEvents[event]);
        }

        delayedEvents[event] = window.setTimeout(() => { delayedEvents[event] = 0; handler(); }, timeout || 800);
    }
}

if (!window.showDialog) {
    /**
    *
    * 展示对话框
    * LCTR 2020-12-08
    *
    * @method showDialog
    *
    * @param {string} title 标题
    * @param {Array<Array<string>>} content 内容
    * [
    *   ['H5', '标题', '内容']},
    *   ['input', '标题', '内容']},
    *   ['input-readonly', '标题', '内容']},
    *   ['label', '标题', '内容']}, 
    *   ['label', '内容']}
    * ]
    * @param {any} button 操作按钮
    * {
    *   '文本': {
    *       'click': ()=>{ },
    *       'dblclick': ()=>{ },
    *   },
    *   '文本': {
    *       'click': ()=>{ }
    *   },
    *   '文本': [
    *       '提示',{
    *           'click': ()=>{ }
    *       }
    *   ]
    * }
    * @param {any} closeButton 显示关闭按钮(默认显示)
    *
    */
    window.showDialog = (title, content, button, closeButton = true) => {
        var close = () => { body.fadeOut(); },
            body = $('<div id="dialog" class="dialog-ux"><div class="backdrop-ux"></div><div class="modal-ux"><div class="modal-dialog-ux"><div class="modal-ux-inner"><div class="modal-ux-header"><h3>' + title + '</h3></div><div class="modal-ux-content"><div class="auth-container"><div><div><div class="auth-container-items"></div><div class="auth-btn-wrapper"></div></div></div></div></div></div></div></div></div></div>'),
            info = '';

        if (button)
            $.each(button, (key, value) => {
                var title = $.isArray(value) ? value[0] : '',
                    text = key,
                    events = $.isArray(value) ? value[1] : value,
                    btn = $('<button class="btn modal-btn casBtn auth authorize button" title="' + title + '">' + text + '</button>');

                $.each(events, (type, event) => {
                    btn.on(type, event);
                });

                btn.appendTo(body.find('.auth-btn-wrapper'));
            });

        if (content)
            $.each(content, (index, item) => {
                var type = item[0],
                    _title = item.length > 2 ? item[1] : null,
                    _content = item.length > 2 ? item[2] : item[1],
                    _key = 'key_' + index;

                if (_title)
                    info += '<label for="' + _key + '">' + _title + ':</label>';

                switch (type) {
                    case 'H5':
                        info += '<h5>' + _content + '</h5>';
                        break;
                    case 'input':
                    case 'input-readonly':
                        info += '<section class="block-tablet col-10-tablet block-desktop col-10-desktop"><input type="text" ' + (type == 'input-readonly' ? 'readonly="readonly"' : '') + ' class="casInput" id="' + _key + '" data-name="' + _key + '" value="' + _content + '"></section>';
                        break;
                    case 'label':
                    default:
                        info += '<section class="block-tablet col-10-tablet block-desktop col-10-desktop"><label class="casInput" id="' + _key + '" data-name="' + _key + '">' + _content + '</label></section>';
                        break;
                }
            });

        if (closeButton) {
            $('<button class="btn modal-btn casBtn auth btn-done button">关闭</button>')
                .on('click', close)
                .appendTo(body.find('.auth-btn-wrapper'));
            $('<button class="close-modal"><svg width="20" height="20"><use href="#close" xlink:href="#close"></use></svg></button>')
                .on('click', close)
                .appendTo(body.find('.modal-ux-header'));
        }

        body.find('.auth-container-items').append(info);

        body.fadeIn().appendTo($('.scheme-container'));
    };
}

var callback = function () {
    //本土化
    var replacePlaceholder = setInterval(() => { $('.operation-filter-input').length ? ($('.operation-filter-input').attr('placeholder', '接口名称（区分大小写）'), window.clearInterval(replacePlaceholder)) : 0; });
};

document.readyState === "complete" || (document.readyState !== "loading" && !document.documentElement.doScroll) ? callback() : document.addEventListener("DOMContentLoaded", callback);