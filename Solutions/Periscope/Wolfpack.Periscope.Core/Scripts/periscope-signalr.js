
jQuery(function () {
    // Proxy created on the fly   
    jQuery.connection.hub.url = 'http://localhost:805/signalr/';
    var dashboard = jQuery.connection.dashboard;

    // Declare functions on the dashboard hub so the server can invoke them
    dashboard.client.clockTick = function (datepoint) {
        //console.log(datepoint);
    };

    dashboard.client.panelChanged = function (metadata) {
        //payload.Content.Id, payload.Content.Name, payload.Content.DwellInSeconds
        console.log(metadata);
    };

    // Declare a function on the dashboard hub so the server can invoke it          
    dashboard.client.widgetUpdate = function (update) {
        //console.log(update);

        var target = update.Target + "Update(" + update.Payload + ");";
        eval(target);
    };

    // Start the connection
    jQuery.connection.hub.start();
});
