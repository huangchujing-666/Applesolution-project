
function checkSession() {

    var defaultRedirectURL = "../Login/Index";
    var loginPageContainWords = "Login Page Layout";

    $.ajax({
        type: "POST",
        url: "../User/SessionCheck",
        data: "",
        async: false,
        success: function (data) {

            //alert('checkSession index: ' + data.indexOf(loginPageContainWords));
           
            // Login Page contains wording to confirm has not been redirected
            if (data.indexOf(loginPageContainWords) == -1) // guarantee session check URL has not been redirected
            {
                //alert('will parseJSON');
                var obj = jQuery.parseJSON(data);

                if (obj.result != "true")
                    window.location = obj.returnURL;
            }
            else
            {
                // checking URL is redirected by server to Login page
                window.location = defaultRedirectURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("REQUEST SESSION ERROR. Network Unstable.");
            window.location = defaultRedirectURL;
        }
    });
}