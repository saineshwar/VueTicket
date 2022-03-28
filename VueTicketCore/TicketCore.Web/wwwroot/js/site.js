

function SessionExpireAlert(timeout) {
    var seconds = timeout / 1000;


    $("#seconds").text(seconds);
    document.getElementsByName("seconds").innerHTML = seconds;
    setInterval(function () {
        seconds--;
        document.getElementById("seconds").innerHTML = seconds;
        $("#seconds").text(seconds);
    }, 1000);
    setTimeout(function () {
        $("#modal-sm").modal('show');

    }, timeout - 20 * 1000);
    setTimeout(function () {
        window.location = "/Error/SessionOut/440";
    }, timeout);
};

function ResetSession() {
    location.reload(true);
}

