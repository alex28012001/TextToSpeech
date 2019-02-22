$(document).ready(function () {
    var defered = AudioLib.ViewAudio("/account/YourAudio", null);

    defered.done(function () {

        var btnDelete = $("<div>");
        btnDelete.addClass("delete-btn");
        var btnDeleteVal = $("<span>");
        btnDeleteVal.addClass("fa fa-trash-o");
        btnDelete.append(btnDeleteVal);

        $(".sound-road").after(btnDelete);
        $(".audio-date").css({ "right": "40px" });

        $(".delete-btn").click(function () {

            var player = $(this).parent().parent();
            var audioTitle = player.find(".audio-title a").text();
            var youReady = confirm("Вы точно хотите удалить аудиозапись " + audioTitle);

            if (youReady) {
                $.ajax({
                    type: "POST",
                    url: "/account/DeleteAudio",
                    data: { "audioTitle": audioTitle },
                    success: function () {
                        player.remove();
                    }
                })
            }

        })





        var onePixel = 200 / 1400;
        var counterChanger = 0;

        $(window).on('resize', function () {
            var win = $(this);
            var winWidth = win.width();
            if (winWidth > 300 && winWidth < 1000) {

                var roadProcents = winWidth * onePixel;
                var userNames = $(".singer-name");
                var audioTitles = $(".audio-title");
                var players = $(".player");

                ChangeAudioRoad(userNames, audioTitles, players, roadProcents);
            }
        })

        function ChangeAudioRoad(userNames, audioTitles, players, roadProcents) {

            if (counterChanger == audioTitles.length) {
                counterChanger = 0;
                return;
            }

            var promise = new Promise(function (resolve, reject) {

                var userName = userNames[counterChanger].innerText;
                var audioTitle = audioTitles[counterChanger].innerText;
                var dataToVisualise = { "userName": userName, "audioTitle": audioTitle, "amountProc": Math.round(roadProcents) };
                SendAjax("POST", "/sound/AudioRoad", dataToVisualise, "json", function(data) {
                    var b = data;
                    resolve();
                })

            })

            promise.then(function () {
                counterChanger++;
                ChangeAudioRoad(userNames, audioTitles, players, roadProcents);
            })

        }
    })
    
})