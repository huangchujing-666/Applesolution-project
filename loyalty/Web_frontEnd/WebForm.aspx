<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="Palmary.Loyalty.Web_frontend.WebForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FrontEndWeb</title>
     <script src="/Scripts/jquery-2.0.3.js"></script>
    
     <script>
         window.fbAsyncInit = function () {
             FB.init({
                 appId: '612989558734123', // App ID
                 channelUrl: '<%=fronEndStartURL%>', // Channel File
                 status: true, // check login status
                 cookie: true, // enable cookies to allow the server to access the session
                 xfbml: true  // parse XFBML
             });
            
             // Here we subscribe to the auth.authResponseChange JavaScript event. This event is fired
             // for any authentication related change, such as login, logout or session refresh. This means that
             // whenever someone who was previously logged out tries to log in again, the correct case below 
             // will be handled. 
             FB.Event.subscribe('auth.authResponseChange', function (response) {
                 // Here we specify what we do with the response anytime this event occurs. 
                 if (response.status === 'connected') {
                     // The response object is returned with a status field that lets the app know the current
                     // login status of the person. In this case, we're handling the situation where they 
                     // have logged in to the app.
                     testAPI();
                 } else if (response.status === 'not_authorized') {
                     // In this case, the person is logged into Facebook, but not into the app, so we call
                     // FB.login() to prompt them to do so. 
                     // In real-life usage, you wouldn't want to immediately prompt someone to login 
                     // like this, for two reasons:
                     // (1) JavaScript created popup windows are blocked by most browsers unless they 
                     // result from direct interaction from people using the app (such as a mouse click)
                     // (2) it is a bad experience to be continually prompted to login upon page load.
               
                     FB.login();
                 } else {
                     // In this case, the person is not logged into Facebook, so we call the login() 
                     // function to prompt them to do so. Note that at this stage there is no indication
                     // of whether they are logged into the app. If they aren't then they'll see the Login
                     // dialog right after they log in to Facebook. 
                     // The same caveats as above apply to the FB.login() call here.
              
                     FB.login();
                 }
             });
             FB.getLoginStatus(function (response) {
                 if (response.status === 'connected') {
                     // the user is logged in and has authenticated your
                     // app, and response.authResponse supplies
                     // the user's ID, a valid access token, a signed
                     // request, and the time the access token 
                     // and signed request each expire
                     var uid = response.authResponse.userID;
                     var accessToken = response.authResponse.accessToken;

                 } else if (response.status === 'not_authorized') {

                     // the user is logged in to Facebook, 
                     // but has not authenticated your app
                 } else {

                     // the user isn't logged in to Facebook.
                 }
             });
         };

         // Load the SDK asynchronously
        (function (d) {
             var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
             if (d.getElementById(id)) { return; }
             js = d.createElement('script'); js.id = id; js.async = true;
             js.src = "//connect.facebook.net/en_US/all.js";
             ref.parentNode.insertBefore(js, ref);
            } (document));

         // Here we run a very simple test of the Graph API after login is successful. 
         // This testAPI() function is only called in those cases. 
            function testAPI() {
            console.log('Welcome!  Fetching your information.... ');
            FB.api('/me', function (response) {

                 console.log('Good to see you, ' + response.id);
                 console.log('name, ' + response.name + '.');
                 console.log('first_name, ' + response.first_name);
                 console.log('middle_name, ' + response.middle_name);
                 console.log('last_name, ' + response.last_name);
                 console.log('gender, ' + response.gender);
                 console.log('locale, ' + response.locale);
                 console.log('languages, ' + response.languages);
                 console.log('link, ' + response.link);
                 console.log('username, ' + response.username);
                 console.log('age_range, ' + response.age_range);
                 console.log('link, ' + response.link);

                 console.log('email, ' + response.email);
                 console.log('mobile, ' + response.mobile);
                 console.log('birthday, ' + response.birthday);
                 console.log('relationship_status, ' + response.relationship_status);

                 //                 $.post("http://127.0.0.1:54925/MemberLogin.ashx", {
                 //                     login_type: "fb",
                 //                     fbid: response.id,
                 //                     username: response.username,
                 //                     email: response.email,
                 //                     birthday: response.birthday,
                 //                     gender: response.gender
                 //                 });

                 //                 posting.done(function (data) {
                 //                     alert("DONE: "+$(data));
                 ////                     var content = $(data).find('#content');
                 ////                     $("#result").empty().append(content);
                 //                 });

                 var jqxhr = $.post(
                            "<%=fronEndURL%>/fbLogin.ashx",
                             {
                                 login_type: "fb",
                                 fbid: response.id,
                                 fblogin_id: response.username,
                                 name: response.first_name,
                                 middlename: response.middle_name,
                                 lastname: response.last_name,
                                 email: response.email,
                                 birthday: response.birthday,
                                 gender: response.gender
                             },
                            function (data) {$("#member_status").html("<span class='red'>"+data+"</span>");}
                        )
                        .done(function () {
                            //alert("second success"); 
                        })
                        .fail(function () {
                            //alert("error"); 
                        })
                        .always(function () {
                            //alert("finished"); 
                        });
                }
            );
        };

        //lougout
        function logout() {
            FB.logout(function (response) {
                // user is now logged out
                window.location.replace("<%=fronEndStartURL%>");
            });
        };
    </script>
</head>
<body>
    <B>Login: (MemberLogin.ashx)</B><br />
    <form id="form1" runat="server">
    <div>
         Login ID: <input type="text" name="login_id" value="userleo" runat="server" id="login_id">(or: member_no, mobile_no, email)<br>
         Password: <input type="text" name="password" value="123456" runat="server" id="password"><br>
         <asp:Button ID="btn_login" runat="server" Text="Login" onclick="btn_login_Click" />
    </div>
    </form><br />

    <B>Get Member Detail: (MemberDetail.ashx)</B><br />
    <form id="form2" action="<%=apiURL%>/MemberDetail.ashx" method="POST">
    <div>
         Member ID: <input type="text" name="member_id" value="1"><br>
         Session: <input type="text" runat="server" id="session"  name="session" style="width:600px"/><br>
         <input type="submit" value="Submit">
    </div>
    </form><br />
    
    <B>Facebook Login:</B><br />
    <div id="fb-root"></div>
    <fb:login-button show-faces="true" width="200" max-rows="1" autologoutlink="true" perms="email, user_birthday"></fb:login-button>
    <br /><br />
   <div id="member_status">Please login.</div>
   
          <input type="button" value="Logout" onclick="logout()" >


    <br /><br /><br />
    <B>Registered Passcode: (PasscodeRegistry.ashx)</B><br />
    <form id="form3" action="<%=apiURL%>/PasscodeRegistry.ashx" method="POST">
    <div>
         Member ID: <input type="text" name="member_id" value="1"><br>
        
         <input type="submit" value="Submit">
    </div>
    </form><br />
</body>
</html>
