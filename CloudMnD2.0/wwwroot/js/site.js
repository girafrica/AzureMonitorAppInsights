function SetAppRoleName() {
    var name = $('#roleName').val();
    if (name.trim() === "") {
        return false;
    }
    $.ajax({
        url: "/Application/SetAppRoleName/?roleName=" + name,
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
        }
    });
}

getLogs();
checkState();
setInterval(getLogs, 10000);

function getLogs() {
    $.ajax({
        url: "/Application/GetLogs",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#logs').text(result);
        }
    });
}

function checkState() {
    if ($("#availabilityState").is(":checked")) {
        $('#btnChangeAppModal').attr('disabled', true);
    }
}

$("#availabilityState").change(function () {
    if (this.checked) {
        $.ajax({
            url: '/Application/StartGenerationAvailability/',
            type: 'post',
            beforeSend: function () {
                $('#loading-overlay').show();
            },
            success: function (response) {
            },
            complete: function (data) {
                $('#loading-overlay').hide();
            }
        });
        $('#btnChangeAppModal').attr('disabled', true);
    }
    else {
        $.ajax({
            url: '/Application/StopGenerationAvailability/',
            type: 'post',
            beforeSend: function () {
                // Show image container
                $('#loading-overlay').show();
            },
            success: function (response) {
            },
            complete: function (data) {
                $('#loading-overlay').hide();
            }
        });
        $('#btnChangeAppModal').attr('disabled', false);
    }
    getLogs();
});

$("#requestState").change(function () {
    if (this.checked) {
        $.ajax({
            url: '/Application/StartRequestSender/',
            type: 'post',
            beforeSend: function () {
                $('#loading-overlay').show();
            },
            success: function (response) {
            },
            complete: function (data) {
                $('#loading-overlay').hide();
            }
        });
        $('#btnChangeAppModal').attr('disabled', true);
    }
    else {
        $.ajax({
            url: '/Application/StopRequestSender/',
            type: 'post',
            beforeSend: function () {
                // Show image container
                $('#loading-overlay').show();
            },
            success: function (response) {
            },
            complete: function (data) {
                $('#loading-overlay').hide();
            }
        });
        $('#btnChangeAppModal').attr('disabled', false);
    }
    getLogs();
});

$(document).ready(function () {
    $("#btnChangeAppModal").click(function () {
        $("#newAppNameModal").modal('show');
    });

    $("#btnHideAppNameModal").click(function () {
        $("#newAppNameModal").modal('hide');
    });
});

$(document).ready(function () {
    $("#generateExceptionBtn").click(function () {
        var numberOfExceptions = $("#numberOfExceptions").val();
        $.ajax({
            url: '/Application/GenerateException/',
            type: 'POST',
            data: { numberOfExceptions: numberOfExceptions },
            beforeSend: function () {
                $('#loading-overlay').show();
            },
            success: function (response) {
                $('#infoModal .modal-body').empty();
                $('#infoModal .modal-body').append(response).append("exception(s) were generated:");
            },
            complete: function (data) {
                $('#loading-overlay').hide();
                $('#infoModal').modal('show');
            }
        });
    });
});

$(document).ready(function () {
    $('#generateExceptionBtn').attr('disabled', true);

    $('#numberOfExceptions').on('input', function () {
        var inputValue = $("#numberOfExceptions").val();

        if (inputValue !== '') {
            $('#generateExceptionBtn').attr('disabled', false);
        } else {
            $('#generateExceptionBtn').attr('disabled', true);
        }
    });
});

$(document).ready(function () {
    $("#cleanLogsBtn").click(function () {
        $.ajax({
            url: '/Application/CleanLogs/',
            type: 'post',
            beforeSend: function () {
                // Show image container
                $('#loading-overlay').show();
            },
            success: function (response) {
                $('#infoModal .modal-body').empty();
                $('#infoModal .modal-body').append("Logs were cleaned");
            },
            complete: function (data) {
                $('#loading-overlay').hide();
                getLogs();
                $('#infoModal').modal('show');
            }
        });

    });
});

