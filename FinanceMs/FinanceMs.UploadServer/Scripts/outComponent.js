(function ($) {

    var MDMExcelUrl = {
        postUrlE: '/cwbase/MDMWeb/MDMExporterWeb.ashx',
        postUrlI: '/cwbase/MDMWeb/FinaceImporter.ashx'
    };
    // These methods can be called by adding them as the first argument in the uploadify plugin call
    var methods = {

        //执行Web化导出：内存包
        Export: function (psZdbh, psWhere) {
            var defer = $.Deferred();

            var form = $('<form  action="' + MDMExcelUrl.postUrlE + '" method="POST" name="fileDownload" id="fileDownload" target="_blank"></form>');
            $('<input type="text" name="AppInstanceID" value="' + gsp.rtf.context.get('AppInstanceID') + '">').appendTo(form);
            $('<input type="text" name="UserID" value="' + gsp.rtf.context.get('UserID') + '">').appendTo(form);
            $('<input type="text" name="UserCode" value="' + gsp.rtf.context.get('UserCode') + '">').appendTo(form);
            $('<input type="text" name="ProcessID" value="' + gsp.rtf.context.get('ProcessID') + '">').appendTo(form);
            $('<input type="text" name="BizDate" value="' + gsp.rtf.context.get('BizDate') + '">').appendTo(form);
            $('<input type="text" name="LoginDate" value="' + gsp.rtf.context.get('LoginDate') + '">').appendTo(form);
            $('<input type="text" name="ClientIP" value="' + gsp.rtf.context.get('ClientIP') + '">').appendTo(form);
            $('<input type="text" name="FrameType" value="' + gsp.rtf.context.get('FrameType') + '">').appendTo(form);
            $('<input type="text" name="UserName" value="' + gsp.rtf.context.get('UserName') + '">').appendTo(form);
            $('<input type="text" name="zdbh" value="' + psZdbh + '">').appendTo(form);
            $('<input type="text" name="vsWhere" value="' + psWhere + '">').appendTo(form);
            $(form).appendTo('body');
            form.submit(); //表单提交
            $(form).remove();

            document.characterSet = 'UTF-8';
            document.charset = 'UTF-8';
            defer.resolve();
            return defer;
        },
        Import: function () {
            var defer = $.Deferred();
            var dialogHtml = '<div id="impExcelDialog" class="easyui-dialog" title="帮助"\
    				data-options="closed:true"\
    				style="width:350px; height:150px; overflow:hidden;">\
	            </div>';
            var tableHtml = '<table id="impTable" style="width:100%;height:60%">\
                                <tr>\
                                    <td>请选择需要上传的文件：</td>\
                                </tr>\
                                <tr>\
                                    <td><input id="impFile" class="easyui-filebox"\
                                     data-options="buttonText:\'选择\',multiple:true"\
                                     style="width:300px"></td>\
                                </tr>\
                            </table>';
            var submitBtnHtml = '<table id="impSubmit" style="width:100%;height:40%">\
                                <tr>\
                                    <td><a id="impBtn" href="#" onclick="javascript:$(\'#impForm\').submit()" class="easyui-linkbutton"\
                                     data-options="iconCls:\'icon-Import\'">确定\
                                    </a></td>\
                                </tr>\
                            </table>';
            $('#impExcelDialog').remove(); //清除dialog
            $('#fileDlgContent').remove(); //清除dialog容器
            $('<div id="fileDlgContent"></div>').appendTo($('body')); //添加dialog容器
            $(dialogHtml).appendTo('#fileDlgContent')//添加dialog

            /*********************形成Form表单*******************************/
            var form = $('<form id="impForm"  action="' + MDMExcelUrl.postUrlI + '" method="POST" enctype="multipart/form-data" name="fileImport" id="fileImport" target="_blank"></form>');
            $('<input type="text" style="display:none" name="AppInstanceID" value="' + gsp.rtf.context.get('AppInstanceID') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="UserID" value="' + gsp.rtf.context.get('UserID') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="UserCode" value="' + gsp.rtf.context.get('UserCode') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="ProcessID" value="' + gsp.rtf.context.get('ProcessID') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="BizDate" value="' + gsp.rtf.context.get('BizDate') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="LoginDate" value="' + gsp.rtf.context.get('LoginDate') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="ClientIP" value="' + gsp.rtf.context.get('ClientIP') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="FrameType" value="' + gsp.rtf.context.get('FrameType') + '">').appendTo(form);
            $('<input type="text" style="display:none" name="UserName" value="' + gsp.rtf.context.get('UserName') + '">').appendTo(form);
            $(tableHtml).appendTo(form);
            /****************************************************/

            $('#impExcelDialog').empty(); //清除指定元素下的子元素
            $(form).appendTo('#impExcelDialog'); //将from表单添加到dialog中
            $(submitBtnHtml).appendTo('#impExcelDialog'); //将提交按钮添加到dialog中

            $.parser.parse('#fileDlgContent'); //解析dialog容器中元素
            $.parser.parse('#impTable'); //解析table中元素
            $.parser.parse('#impSubmit'); //解析table中元素

            $('#impExcelDialog').dialog('open');

            document.characterSet = 'UTF-8';
            document.charset = 'UTF-8';
            defer.resolve();
            return defer;
        }
    };

    $.MDMWebExcelHelper = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else {
            $.error('The method ' + method + ' does not exist in $.uploadify');
        }

    }

})($);

jQuery.MDMExcelExport = function (psZdbh, psWhere) {
    return $.MDMWebExcelHelper('Export', psZdbh, psWhere);
}