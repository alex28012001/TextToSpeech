$(document).ready(function () {
    $("input[name=search-audio-area]").val(audioTitle);
    AudioLib.ViewAudio("/sound/FindAudio", { "audioTitle": audioTitle });
})