$(document).ready(function () {
  
    var url = this.location.pathname;
    var tab = $('.navbar-nav li a[href="' + url + '"]').parent();
    tab.addClass('active');

    $("span[name=search-audio-btn]").click(function () {
        var audioTitle = $("input[name=search-audio-area]").val();
        document.location.href = "/sound/search/" + audioTitle;  
    })
}) 