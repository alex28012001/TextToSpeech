$(document).ready(function () {
    //--------------------------------------------------------------------------
    var audioContext = new AudioContext(); //new audio context to help us record
    var audioStream = null;

    var isVoiceRecorded = false;

    SendAjax("POST", "/buildSound/IsVoiceRecorded", { "language": lang }, "json", function (successed) {
        if (successed) {
            isVoiceRecorded = true;
        }
        else {
           alert("У вас не записаны все буквы выбранного языка!");
        }
    });


    $("#play").click(function () {
        var text = $("#audioText").val();
        var validateMsg = $(".text-validate-msg");
        
        if (!isVoiceRecorded) {
            alert("У вас не записаны все буквы выбранного языка!"); r
            return;
        }

        if (ValidateText(text, lang)) {
            if (validateMsg.hidden == false) {
                validateMsg.hidden = true;
                validateMsg.hide();
            }
        }
        else {
            validateMsg.show();
            validateMsg.hidden = false;
            validateMsg.text("текст должен содержать хотя-бы 1 алфавитно-символьный символ");
            return;
        }

          
        var request = new XMLHttpRequest();
        request.open("POST", "/buildSound/create", true);
        request.responseType = "blob";
        request.onload = function () {
            var responseType = this.response.type;

            if (responseType == "audio/wav") {
                audioStream = this.response;
                var url = window.URL.createObjectURL(audioStream);
                $("#audioSrc").attr("src", url);
            }         
        }

        var percentVolume = $("input[type=range]").val();

        var data = new FormData();
        data.append("Text", text);
        data.append("Language", lang);
        data.append("PercentVolume", percentVolume);
        data.append("SampleRate", audioContext.sampleRate);
        
        request.send(data);
        
    })


    $("#saveAudio").click(function () {
        var audioTitle = $("#titleAudio").val();
        var savedLink = $("#savedLink");
        var text = $("#audioText").val()

        if (audioStream != null) {
            var formData = new FormData();
            
            formData.append("waveFile", audioStream);
            formData.append("title", audioTitle);
            formData.append("text", text);
            formData.append("Language", lang);

            savedLink.attr("href", URL.createObjectURL(audioStream));
            savedLink.attr("download", audioTitle);
            

            $.ajax({
                type: "POST",
                url: "/buildSound/saveAudioByStream",
                data: formData,
                contentType: false,
                processData: false,
                dataType: "text",
                success: function (resultStr) {
                    $(".audio-saved").text(resultStr);
                }
            })
        }
        else {

            var dataToSaveByText = { "text": text, "title": audioTitle, "Language": lang, "sampleRate": audioContext.sampleRate };

            SendAjax("POST", "/buildSound/saveAudioByText", dataToSaveByText, "text", function (resultStr) {
                $(".audio-saved").text(resultStr);
            })

        }
    })

    function ValidateText(text, language) {
        if (text == null || text == "")
            return false;

        var pattern;
        switch (language) {
            case "rus": pattern = /[а-яА-Я0-9]+/; break;
            case "en": pattern = /[a-zA-Z0-9]+/; break;
        }

        return pattern.test(text);
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
})