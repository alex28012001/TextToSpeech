$(document).ready(function () {

    AudioLib.ViewAudio("/sound/UserAudio", { "userName": singerName });

    SendAjax("POST", "/relation/IsSub", { "singerName": singerName }, "text", function (responce) {
        if (responce == "True") {
            var btnFollow = $("div[name=sub]");
            btnFollow.removeClass("follow");
            btnFollow.addClass("un-follow");
            btnFollow.text("отписаться");
        }
    })

    $("div[name=sub]").click(function () {

        var btnFollow = $("div[name=sub]");
        if (btnFollow.hasClass("follow")) {
            SendAjax("POST", "/relation/Follow", { "singerName": singerName });

            btnFollow.removeClass("follow");
            btnFollow.addClass("un-follow");
            btnFollow.text("отписаться");
        }
        else {
            SendAjax("POST", "/relation/UnFollow", { "singerName": singerName });

            btnFollow.removeClass("un-follow");
            btnFollow.addClass("follow");
            btnFollow.text("подписаться");
        }
    })

    function SendAjax(type, url, data, dataType, success) {
        $.ajax({
            type: type,
            url: url,
            dataType: dataType,
            data: data,
            success: success
        });
    }

})