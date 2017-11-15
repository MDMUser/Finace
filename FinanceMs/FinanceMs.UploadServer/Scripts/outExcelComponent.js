(function ($) {

    var postUrl = '/cwbase/MDMWeb/FinaceExporter.ashx';
    var methods = {

        //执行Web化导出：内存包
        Export: function (psZdbh, psWhere) {
            var defer = $.Deferred();

            var form = $('<form  action="' + postUrl + '" method="POST" name="fileDownload" id="fileDownload" target="_blank"></form>');
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
        }
    };

    $.FinaceExcelHelper = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else {
            $.error('The method ' + method + ' does not exist in $.uploadify');
        }

    }

})($);

jQuery.FinaceExcelExport = function (psZdbh, psWhere) {
    return $.FinaceExcelHelper('Export', psZdbh, psWhere);
}