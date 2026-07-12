
$(function () {
    Public.Routin();
    //بررسی وضعیت تک تک تصاویر پس از بارگذاری کامل صفحه
    $(window).bind('load', function() {
        $('img').each(function() {
            if (!this.complete || (!$.browser.msie && (typeof this.naturalWidth == "undefined" || this.naturalWidth === 0))) {
                errorReplace(this);
            }
        });
    });
});
/*#####################   pblic       ###################*/

var Public = new Object();
Public.Routin = function () {

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-val-number]').each(function () {
        var el = $(this);
        var orig = el.data('val-number');

        var fieldName = orig.replace('The field ', '');
        fieldName = fieldName.replace(' must be a number.', '');

        el.attr('data-val-number', fieldName + ' باید عددی باشد');
    });
    $(".pagination").addClass("pagination-sm").addClass("pull-right");
    $('a.nofollow').attr('rel', 'nofollow');
};



/*####################  Prevent Navigation ###############*/


var warningBeforeLoad = function () {
    var msg = "اطلاعات دخیره نشده ای در این صفحه دارید و با" +
        " هدایت به صفحه بعد این اطلاعات را از دست خواهید داد";
    $('button:button').click(function () {
        msg = null;
    });
    $('input:not(:button,:submit),textarea,select').change(function () {
        window.onbeforeunload = function () {
            if (msg != null)
                return msg;
        };
    });
    $('input:checkbox,input:radio').click(function () {
        window.onbeforeunload = function () {
            if (msg != null)
                return msg;
        };
    });
}

function onComplete(xhr, status) {
    var data = xhr.responseText;
    if (xhr.status == 403) {
        window.location = '/Account/Login';
    }
}

function makeUploadFile(id) {
    $("#" + id).fileinput({
        showUpload: false,
        previewFileType: "image",
        msgInvalidFileType: "از فایل معتبر استفاده کنید",
        //maxFileSize: "10000",
        msgSizeTooLarge: "حجم فایل مورد نظر بیشتر از حجم مورد قبول است",
        browseClass: "btn btn-success",
        browseLabel: "انتخاب تصویر",
        browseIcon: '<i class="glyphicon glyphicon-picture"></i>',
        removeClass: "btn btn-danger",
        removeLabel: "حذف",
        removeIcon: '<i class="glyphicon glyphicon-trash"></i>',
        allowedFileExtensions: ["jpg", "gif", "png"]
    });
}


/*##################### site faveicon  ################*/
function sitesFavicon() {
    $("a").each(function () {
        var $a = $(this);
        var href = $a.attr("href");
        // see if the link is external 
        if (href && href.match(/^http/))
            if (!href.match(document.domain)) {
                var domain = href.replace(/<\S[^><]*>/g, "").split('/')[2];
                var image = '<img src="http://' + domain +
                '/favicon.ico" width="0" ' +
                ' onload="this.width=16;this.height=16;this.style.paddingLeft=\'3px\';this.style.paddingRight=\'1px\';" ' +
                ' style="border:0" ' +
                ' onerror="this.src=\'alternative url\';" />';
                $(this).prepend(image);
            }
    });
}

/*#######################  notify admin for error on load image ####################*/
function errorReplace(arg) {
    //ارسال پیغام خطا
    $.ajax({
        type: "POST",
        url: "api/url",
        data: "{'image': '" + arg.src + "','page':'" + location.href + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });
    //نمایش تصویری دلخواه بجای نمونه مفقود
    $(arg).attr('src', 'alternativeImage');
}

