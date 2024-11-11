

$(document).ready(function () {
    $('#genty').submit(function (e) {
        e.preventDefault();
        var $form = $(this);
        $.post($form.attr("action"), $form.serialize()).done(function (res) {
            alert(res.message);
            window.top.location.reload();
        });
    });
    var url = "";
    $("#views").dialog({
        title: 'Update MCS Result',
        autoOpen: false,
        width: 1000,
        height: 600,
        modal: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $(this).load(url);
        },
        buttons: {
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#NewView").dialog({
        title: 'Update MCS Result',
        autoOpen: false,
        width: 1000,
        height: 600,
        modal: true,
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $(this).load(url);
        },
        buttons: {
            "Cancel": function () {
                $(this).dialog("close");
                window.top.location.reload();
            }
        }
    });



    $(".gethist").click("click", function (e) {
        url = $(this).attr('href');
        $(".ui-dialog-title").html("");
        $(".ui-dialog-titlebar").css({
            'background-color': '#395c82',
            'color': 'white'
        });
        $("#views").dialog("open");
        return false;
    });
    $(".createnew").click("click", function (e) {
        url = $(this).attr('href');
        $(".ui-dialog-title").html("");
        $(".ui-dialog-titlebar").css({
            'background-color': '#395c82',
            'color': 'white'
        });
        $("#NewView").dialog("open");
        return false;
    });



    $(".oh").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/hospital_patients/widal/',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            $("#hfCustomer").val(i.item.val);
        },
        minLength: 1
    });

});