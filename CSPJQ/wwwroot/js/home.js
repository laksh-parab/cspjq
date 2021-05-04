$(function () {
    $("#btnGetContent").click(function () {
        $.ajax({
            type: "GET",
            url: "/home/getpartialviewcontent",
            processData: true,
            cache: false
        })
            .done(function (response, textStatus, jqXHR) {
                $("#result").html(response);
            })
    })
})