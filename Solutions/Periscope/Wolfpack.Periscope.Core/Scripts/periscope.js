var PERISCOPE = (function ($, targetElement) {
    var periscope = {
        Includes: {},
        PanelId: null,
        Start: function () {
            loadPanel();
        }
    };

    function loadPanel() {
        $.ajax({
            type: 'GET',
            url: '/dashboard/panel',
            data: { id: periscope.PanelId },
            contentType: 'application/json; charset=utf-8',
            dataType: 'json'
        }).done(function (data, status, jqXhr) {
            console.log('got panel data...');
            console.log(data);

            displayPanel(data);

        }).fail(function (jqXhr, status, err) {
            console.log('failed to get panel data');
            console.log(err);
        });
    }

    function displayPanel(data) {
        var panel = $(document.createElement("div")).hide()
                    .attr({ id: "loading" })
                    .addClass("periscope-panel")
                    .appendTo($("#" + targetElement));

        $.each(data.widgets, function (index, widget) {
            console.log("Creating widget markup: " + widget.configuration.name);

            //panel.append(widget.Markup);
            var style = "height:" + widget.configuration.height + "px;width:" + widget.configuration.width + "px;";
            $(document.createElement("div"))
                        .hide()
                        .addClass("widget-container")
                        .attr({ id: widget.configuration.name, style: style })
                        .html(widget.markup)
                        .appendTo(panel)
                        .show();
        });

        loadScripts(data, completeLoadingPanel);
    }

    function loadScripts(data, callback) {
        var includes = [];

        $.each(data.includes, function (index, item) {
            if (item.name in periscope.Includes) {
                console.log(item.name + ' already loaded!');
            } else {
                periscope.Includes[item.name] = item.value;
                includes.push(item.value);
            }
        });

        if (includes.length == 0) {
            callback(data);
        } else {
            console.log("loading includes...");
            console.log(includes);

            LazyLoad.js(includes, function () {
                console.log("...done, running callback()...");
                callback(data);
            });
        }
    }

    function completeLoadingPanel(data) {
        $.each(data.widgets, function (index, widget) {
            console.log("Loading widget script: " + widget.configuration.name);
            $(widget.script).appendTo(document.body);
        });

        if (periscope.PanelId != null) {
            console.log("hiding current panel");
            var outPanel = $("#" + periscope.PanelId);
            outPanel.remove();
        }

        periscope.PanelId = data.id.toString();
        var current = $("#loading");
        current.attr({ id: periscope.PanelId }).show();

        setInterval(refreshPanel, data.dwellInSeconds * 1000);
    }

    function refreshPanel() {
        console.log("Refreshing Panel: " + periscope.PanelId);
        //window.location.assign('/?current=' + periscope.PanelId);
    }

    return periscope;
} (jQuery, 'panelcontainer'));
