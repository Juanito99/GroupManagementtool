<%@ Application Language="C#" %>
<%@ Import NameSpace="System.Net.Mail" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        if (WbGetParams.getBValue("SendMailOnError"))
        {
            if (HttpContext.Current.Server.GetLastError() != null)
            {
                Exception myException = HttpContext.Current.Server.GetLastError();
                string mailSubject = "Error in page " + Request.Url.ToString() + DateTime.Now.ToString();
                string message = string.Empty;
                message += "<strong>Message</strong><br/>" + myException.StackTrace + "<br />";
                message += "<strong>Query String</strong><br />" + Request.QueryString.ToString() + "<br />";                
                MailMessage myMessage = new MailMessage(WbGetParams.getValue("NotifyFromAddress"), WbGetParams.getValue("NotifyToAddress"), mailSubject, message);
                myMessage.IsBodyHtml = true;                             
                SmtpClient mySmtpClient = new SmtpClient(WbGetParams.getValue("NotifyExchangeServer"));

                try
                {
                    mySmtpClient.Send(myMessage);
                }
                catch
                {
                    string foo = "bar";
                }                                        
            }
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
