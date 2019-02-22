$(document).ready(function () {


    var amountProc = 260;
    var offsetProc = 3;
    var audioList = $(".audio-container");
   
    var canvasWidth = amountProc * offsetProc;
    var canvasHeight = 202;

    var jsonDataToPlayer = {
        "Audio.Src": audioPath, "Audio.UserName": userName, "Audio.Title": audioTitle,
        "Audio.QuantityLikes": quantityLikes, "QuantityComments": quantityComments, "AmountProc": amountProc, "Audio.Listening": listening,
        "Audio.Date": date, "PlayerProperties.CanvasHeight": canvasHeight, "PlayerProperties.CanvasWidth": canvasWidth
    };

    SendAjax("POST", "/sound/Player", jsonDataToPlayer, "json", function (dataPlayer) {

        audioList.append(dataPlayer.Playerhtml);

        var canvas = document.getElementsByTagName('canvas')[0];
        var ctx = canvas.getContext('2d');
        var dataToVisualize = dataPlayer.DataToVisualize;

        var player = $(".player");
        player.attr("audio-road", JSON.stringify(dataToVisualize));

        var roadOffsetX = 0;
        for (var i = 0; i < dataToVisualize.length; i++) {
            DrawRoad(ctx,canvasHeight, dataToVisualize, i, "gray", "silver", roadOffsetX);
            roadOffsetX += offsetProc;
        }

        BindClicksEvents();
    });

      
    


    var audio;
    var dataToVisualize;
    var msOneProc = 0;
    var timer;
    var currentTime = 0;
    var offsetX = 0;
    var currentProc = 0;
    var mousePaintedAudioProc = 0;
    var played = false;
    var canvasBusy = false;
    var audioListening = false;
    var ctx;
    var canvas;

    $(".audio-container").on("mousemove", function (e) {
        if (canvasBusy) {

            var pos = $(this).offset();
            var mouseX = e.pageX - pos.left;
            var mouseY = e.pageY - pos.top;


            var canvasX1 = canvas.position().left;
            var canvasY1 = canvas.position().top;

            var canvasX2 = canvasX1 + canvas.width();
            var canvasY2 = canvasY1 + canvas.height();

            if (mouseX < canvasX1 || mouseX > canvasX2 || mouseY < canvasY1 || mouseY > canvasY2) {
                var localOffsetX = 0;
                for (var i = 0; i < currentProc; i++) {
                    DrawRoad(ctx,canvasHeight, dataToVisualize, i, "DarkOrange", "orange", localOffsetX);
                    localOffsetX += offsetProc;
                }
                for (var i = currentProc; i < mousePaintedAudioProc; i++) {
                    DrawRoad(ctx,canvasHeight, dataToVisualize, i, "gray", "silver", localOffsetX);
                    localOffsetX += offsetProc;
                }
            }
        }
    })

    function BindClicksEvents() {

        $("div[name=mark]").on('click', function (event) {
            var mark = $(this);

            var countLikes = mark.parent().find(".count-likes");
            var currentCountLikes = Number(countLikes.text());

            var isLiked = mark.hasClass("like");
            if (isLiked == true) {
                SendAjax("POST", "/relation/Dislike", { "singerName": userName, "audioTitle": audioTitle }, null, null);
                mark.removeClass("like");
                mark.addClass("unlike");
                countLikes.text(--currentCountLikes);
            }
            else {
                SendAjax("POST", "/relation/Like", { "singerName": userName, "audioTitle": audioTitle }, null, null);
                mark.removeClass("unlike");
                mark.addClass("like");
                countLikes.text(++currentCountLikes);
            }
        });

        $(".play-btn").on('click', function (event) {
            var playBtn = $(this);
            var audioTitle = playBtn.parent().parent().find(".audio-title a").text();
            var singerName = playBtn.parent().parent().find(".singer-name a").text();

      
            if (audioListening == false) {
                SendAjax("POST", "/relation/AddListener", { "singerName": singerName, "audioTitle": audioTitle }, null, null);
                var listening = playBtn.parent().parent().find(".listening-count");

                var currentCountListening = Number(listening.text());
                listening.text(++currentCountListening);
                audioListening = true;
            }

            
         
                if (played == false) {
                    playBtn.html("<span class='fa fa-pause'></span>");
                    playBtn.parent().find("canvas").attr("data-work", "on");
                    InitializationElementsForPlay(playBtn, currentTime);
                }

                else {
                    audio.pause();
                    setTimeout(function () { clearInterval(timer); });

                    played = false;
                    playBtn.html("<span class='fa fa-play'></span>");
                    currentTime = audio.currentTime;
                }
        });


        function InitializationElementsForPlay(playBtn, currentTime) {

            dataToVisualize = JSON.parse(playBtn.parent().parent().attr("audio-road"));

            canvas = playBtn.parent().find("canvas");
            ctx = canvas[0].getContext('2d');

            if (audio == null) {
                audio = new Audio(audioPath);
                audio.currentTime = currentTime;
                audio.addEventListener('loadedmetadata', function () {
                    EventPlay(playBtn);
                });
            }
            else {
                audio.currentTime = currentTime;
                EventPlay(playBtn);
            }


            var oldOfssetX = 0;
            canvas.on("mousemove", function (e) {
                canvasBusy = true;
                var RoadX = e.pageX - $(this).offset().left;

                var lastProc = RoadX / offsetProc;
                var paintedOffsetX = 0;
                for (var i = 0; i < lastProc; i++) {
                    DrawRoad(ctx, canvasHeight, dataToVisualize, i, "#F5D0A9", "#F5D0A9", paintedOffsetX);
                    paintedOffsetX += offsetProc;
                }

                mousePaintedAudioProc = paintedOffsetX / offsetProc;

                if (paintedOffsetX < oldOfssetX) {
                    var offsetX = oldOfssetX;
                    var startProc = RoadX / offsetProc;
                    var endProc = oldOfssetX / offsetProc;
                    for (var i = endProc; i > startProc; i--) {
                        DrawRoad(ctx, canvasHeight, dataToVisualize, i, "gray", "silver", offsetX);
                        offsetX -= offsetProc;
                    }
                }

                oldOfssetX = paintedOffsetX;

            })
        }


        function EventPlay(playBtn) {
            audio.play();
            played = true;

            msOneProc = (audio.duration / amountProc) * 1000; //сколько мс нужно для 1 процента дорожки
            currentProc = Math.round(currentTime / (msOneProc / 1000)); //текущий процент дорожки
            offsetX = currentProc * offsetProc; //текущий отступ дорожки

            //если время за которое проходит 1 процент дорожки меньше 5мс (5 т.к. это мин. число setInterval),
            //то тогда нужно сделать несколько проходов отрисовки дорожки 
            var amountPasses = msOneProc >= 5 ? 1 : Math.ceil(5 / msOneProc);
            var timeForOneProc = msOneProc * amountPasses; // время для 1 процента дорожки с учетом проходов

            timer = setInterval(function () {
                for (var i = 0; i < amountPasses; i++) { //проходы

                    var audioCurTime = audio.currentTime;
                    if (audioCurTime >= audio.duration) {
                        setTimeout(function () { clearInterval(timer); });
                        playBtn.html("<span class='fa fa-play'></span>");
                        played = false;

                        offsetX = 0;
                        currentProc = 0;
                        currentTime = 0;

                        //отрсиовка дорожки в начальный цвет
                        var localOffsetX = 0;
                        for (var i = 0; i < amountProc; i++) {
                            DrawRoad(ctx, canvasHeight, dataToVisualize, i, "gray", "silver", localOffsetX);
                            localOffsetX += offsetProc;
                        }
                        break;
                    }

                    DrawRoad(ctx, canvasHeight, dataToVisualize, currentProc, "DarkOrange", "orange", offsetX);
                    currentProc++;
                    offsetX += offsetProc;

                    currentTime = audioCurTime;

                }

            }, timeForOneProc);
        }



        $("canvas").on('click', function () {
            var working = $(this).attr("data-work");
            var playBtn = $(this).parent().find("input[name=play]");
            if (working == "off") {
                playBtn.click();
                $(this).attr("data-work", "on");
            }
            else {
                var newAudioTime = mousePaintedAudioProc * msOneProc / 1000;
                var localOffsetX = 0;

                for (var i = 0; i < mousePaintedAudioProc; i++) {
                    DrawRoad(ctx, dataToVisualize, i, "DarkOrange", "orange", localOffsetX);
                    localOffsetX += offsetProc;
                }

                audio.currentTime = newAudioTime;
                currentTime = newAudioTime;
                EventPlay(playBtn);
            }
        })
    } //конец инициализации событий



    function DrawRoad(ctx, canvasHeight, data, curProc, color1, color2, offsetX) {
        ctx.lineWidth = "2";
        ctx.beginPath();
        ctx.strokeStyle = color1;

        var startY = (canvasHeight / 2) - 1;
        var endY = startY - data[curProc];
        ctx.moveTo(offsetX, startY);
        ctx.lineTo(offsetX, endY);
        ctx.stroke();

        startY = canvasHeight / 2 + 1;
        endY = startY + data[curProc] / 2;
        ctx.beginPath();
        ctx.strokeStyle = color2;
        ctx.moveTo(offsetX, startY);
        ctx.lineTo(offsetX, endY);
        ctx.stroke();
    }



    function SendAjax(type, url, data, dataType, success) {
        $.ajax({
            type: type,
            url: url,
            dataType: dataType,
            data: data,
            success: success
        });
    }


    var onePixel = 200 / 1400;

    $(window).resize(function () {
        var win = $(this);
        var winWidth = win.width();
        if (winWidth > 500) {

            var roadProcents = Math.round(winWidth * onePixel);
            amountProc = roadProcents;
            var userName = $(".singer-name a").text();
            var audioTitle = $(".audio-title a").text();
            var road = $(".sound-road");

            ChangeAudioRoad(userName, audioTitle, road, roadProcents);
        }
    })



    function ChangeAudioRoad(userName, audioTitle, road, roadProcents) {
      
        var dataToVisualise = { "userName": userName, "audioTitle": audioTitle, "amountProc": roadProcents };
        SendAjax("POST", "/sound/AudioRoad", dataToVisualise, "json", function (newRoad) {

            var oldRoad = road[0].firstElementChild;

            var canvas = document.createElement("canvas");
            canvas.width = roadProcents * offsetProc;
            canvas.height = canvasHeight;

            road[0].replaceChild(canvas, oldRoad);

            var ctx = canvas.getContext('2d');
            var dataToVisualize = newRoad;

            var player = $(".player")[0];
            player.setAttribute("audio-road", JSON.stringify(dataToVisualize));

            var roadOffsetX = 0;
            for (var i = 0; i < dataToVisualize.length; i++) {
                DrawRoad(ctx, canvasHeight, dataToVisualize, i, "gray", "silver", roadOffsetX);
                roadOffsetX += offsetProc;
            } 
       })
    }

})