$(document).ready(function () {

    var dataToFindComments = { "singerName": userName, "audioTitle": audioTitle };
    SendAjax("POST", "/relation/FindComments", dataToFindComments, "json", function (comments) {
        var commentsContainer = $(".comments-container");

        for (var i = 0; i < comments.length; i++) {
            var senderName = comments[i].UserName;
            var date = DateCsharpToDateJs(comments[i].Date);
            var text = comments[i].Text;

            var comment = "<div class='comment'><span class='comment-senderName'>" + senderName +
                          "</span><span class='comment-text'>" + text + "</span><span class='comment-date'>" + date + "</span></div>";
            
            commentsContainer.append(comment);
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

    function DateCsharpToDateJs(date) {
        var dateMs = date.replace(/[^0-9 +]/g, '');
        var dateMsInt = parseInt(dateMs);
        var fullDate = new Date(dateMsInt);
        return fullDate.toLocaleString().replace(/,/, '');
    }
})

