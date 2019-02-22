$(document).ready(function () {
    
    var rusLetters = "абвгдеёжзийклмнопрстуфхцчшщыэюя";
    var rusSounds = ["а", "б", "бь", "в", "вь", "г", "гь", "д", "дь", "е", "ё", "ж", "з", "зь", "и", "й", "к", "кь", "л", "ль", "м", "мь", "н", "нь", "о", "п", "пь", "р", "рь", "с", "сь", "т", "ть", "у", "ф", "фь", "х", "хь", "ц", "ч", "ш", "щ", "ы", "э", "ю", "я"];
    var sellsLetters = $(".letters-box");
 
    for (var i = 0; i < rusSounds.length; i++) {
        var cell = "<div class='letter'><div class='record-signal'><span class='fa fa-circle'><span></div>"+
        "<div class='letter-status'><span class='fa fa-times'></span></div>" +
        "<div class='letter-name'><span>" + rusSounds[i] + "</span></div><div class='rec-button'>" +
        " <span class='fa fa-microphone'></span></div><div class='stop-button'>" +
        "<span class='fa fa-stop-circle-o'></span></div></div>";

        sellsLetters.append(cell);
    }

    var counterLetters = 0;
    IsRecord();

    function IsRecord() {
        var promise = new Promise(function (resolve, reject) {
            if (counterLetters == rusSounds.length) {
                return;
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/record/letterIsRecord",
                    data: { "letterName": rusSounds[counterLetters] },
                    success: function (letterIsRecord) {
                        if (letterIsRecord == true) {

                            var curElem = sellsLetters.find(".letter-status span")[counterLetters];
                            var newElem = document.createElement('span');
                            newElem.className = "fa fa-check";
                            newElem.style.color = "#40FF00";

                            var parentNode = curElem.parentNode;
                            parentNode.replaceChild(newElem, curElem);
                        }
                        counterLetters++;
                        resolve();
                    }
                })
            }
        })
        promise.then(function () {
            IsRecord();
        })
    }


    var gumStream; //stream from getUserMedia()
    var rec; //Recorder.js object
    var input; //MediaStreamAudioSourceNode we'll be recording
    var audioContext = new AudioContext();
    var cellLetter;
    var countChannels = 2;

    $(".rec-button").click(function () {
        navigator.mediaDevices.getUserMedia({ audio: true }).then(function (stream) {
            gumStream = stream;
            input = audioContext.createMediaStreamSource(stream);
            rec = new Recorder(input, { numChannels: countChannels });
            rec.record();
        })
        cellLetter = $(this).parent();
        cellLetter.find(".record-signal").fadeTo("fast", 0.9);
    })

    $(".stop-button").click(function () {
        if (rec != undefined && gumStream != undefined) {
            rec.stop();
            gumStream.getAudioTracks()[0].stop();
            rec.exportWAV(DowloadAudio);

            cellLetter.find(".record-signal").fadeTo("fast", 0);
        }
    })

    function DowloadAudio(blob) {
        var formData = new FormData();
        formData.append("waveFile", blob);
        
        var word = cellLetter.find(".letter-name").text();
        formData.append("word", word);
        formData.append("sampleRate", audioContext.sampleRate);
        formData.append("channels", countChannels);
 
        $.ajax({
            type: "POST",
            url: "/record/saveLetter",
            data: formData,
            contentType: false,
            processData: false,
            dataType: "json",
            success: function (successed) {
                if (successed == true) {
                    cellLetter.find(".letter-status span").replaceWith("<span class='fa fa-check'></span>");
                    cellLetter.find(".letter-status span").css("color", "#40FF00");
                }
                else
                    alert("Говорите громче и внятней");
            }
        })
    }

})