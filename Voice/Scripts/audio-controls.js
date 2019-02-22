$(document).ready(function () {
    var audio = null;
    var dataToVisualize = null;
    var seek = null;
    var seconeproc = 0;
    var timer = null;
    var time = null;
    var offsetX = 3;
    var currentProc = 0;
    var amountProc = 200;

    var ctx = null;
   
    var inputs = $("input");
    var a = document.documentElement.outerHTML;

    $("input[name=play]").on('click', function (e) {
        e.preventDefault();
        var pathToAudio = $(this).parent().attr("data-audio");
        dataToVisualize = JSON.parse($(this).parent().attr("data-road"));
        var canvas = $(this).parent().find("canvas");
        ctx = canvas[0].getContext('2d');

        seek = $(this).parent().find("input[type=range]");
        time = $(this).parent().find("input[name=time]");

        audio = new Audio(pathToAudio);
        audio.play();

        $(this).replaceWith("<input type='submit' id='Pause' value='Pause' class'btn-default'/>");
        seek.attr("max", audio.duration);

        seconeproc = audio.duration / amountProc;

        timer = setInterval(function () {

             seek.val(audio.currentTime);
             time.val(audio.currentTime.toString());

            DrawRoad(currentProc, "DarkOrange", "orange", offsetX);
            currentProc++;
            offsetX += 3;
        }, seconeproc * 1000);
    });


    //$("#Pause").on('click', function (e) {
    //    e.preventDefault();
    //    audio.pause();
    //    setTimeout(function () { clearInterval(timer); });

    //    $(this).replaceWith("<input type='submit' id='Play' value='Play' class'btn-default'/>");
    //});



    //seek.bind("change", function () {
    //    var tempCurrentTime = audio.currentTime
    //    audio.currentTime = $(this).val();
    //    seek.attr('max', audio.duration);

    //    var movedProc = $(this).val() / seconeproc;
    //    if (movedProc > tempCurrentTime / seconeproc) { //если значение ползунка(сек) больше чем текущее время аудио, т.е проматываем
    //        for (var i = currentProc; i < movedProc; i++) { //от текущего процента дорожки до процента ползунка
    //            DrawRoad(i, "DarkOrange", "orange", offsetX);
    //            offsetX += 3;
    //            currentProc++;
    //        }
    //    }
    //    else { //если значение ползунка(сек) меньше тек. времени аудио , т.е отматываем
    //        for (var i = currentProc; i > movedProc; i--) {
    //            DrawRoad(i, "gray", "silver", offsetX);
    //            offsetX -= 3;
    //            currentProc--;
    //        }
    //    }
    //});



    function DrawRoad(curProc, color1, color2, offsetX) {
        ctx.lineWidth = "2";
        //   offsetX = offsetX + 0.5;
        ctx.beginPath();
        ctx.strokeStyle = color1;
        ctx.moveTo(offsetX, 50);
        ctx.lineTo(offsetX, 50 - dataToVisualize[curProc] / 2);
        ctx.stroke();

        ctx.beginPath();
        ctx.strokeStyle = color2;
        ctx.moveTo(offsetX, 52);
        ctx.lineTo(offsetX, 52 + dataToVisualize[curProc] / 3);
        ctx.stroke();
    }
})
                
            
        
    
