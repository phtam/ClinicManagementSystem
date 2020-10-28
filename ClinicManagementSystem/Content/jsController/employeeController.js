﻿$(document).ready(function () {
    $("#txtUsername").keyup(function () {
        $.ajax({
            type: "POST",
            url: '/Values/EmpUsernameValidation',
            data: {
                username: $(this).val()
            },
            cache: false,
            datatype: "json",
        }).done(function (data) {
            $("#notice_invalid_username").empty();
            $("#notice_invalid_username").append(data);
        });
    });
});

$(document).ready(function () {
    $("#txtEmail").keyup(function () {
        $.ajax({
            type: "POST",
            url: '/Values/CheckEmpEmail',
            data: {
                emailAddress: $(this).val()
            },
            cache: false,
            datatype: "json",
        }).done(function (data) {
            $("#notice_invalid_email").empty();
            $("#notice_invalid_email").append(data);
        });
    });
});
