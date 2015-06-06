using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Threading;

/// <summary>
/// Test if can Ping and creates a folder (if does not exist before)
/// </summary>
public class NWvarOps
{

    private string srvName = "";
    private long rspTimeInMs = 0;
    private string notifymsg = "";
    private string notifyrcp = "";

    public string SrvName
    {
        get
        {
            return srvName;
        }
        set
        {
            srvName = value;
        }
    }

    public long RspTimeInMs
    {
        get
        {
            return rspTimeInMs;
        }
        set
        {
            rspTimeInMs = value;
        }
    }

    public string Notifymsg
    {
        get
        {
            return notifymsg;
        }
        set
        {
            notifymsg = value;
        }
    }

    public string Notifyrcp
    {
        get
        {
            return notifyrcp;
        }
        set
        {
            notifyrcp = value;
        }
    }

	public NWvarOps()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public bool ServerReachable(out string retmsg)
    {
        bool retval = false;
        retmsg = "";
        int pingTimeout = 1000;

        try
        {
            Ping cping = new Ping();
            PingReply cpong = cping.Send(srvName, pingTimeout);
            if (cpong.Status == IPStatus.Success)
            {                
                retmsg = srvName + " reached in time: " + cpong.RoundtripTime;
                rspTimeInMs = cpong.RoundtripTime;
                retval = true;
            }                
            else
            {
                retval = false;
            }
        }
        catch (Exception ex)
        {
            retmsg = "Error occured while checking Server. " + srvName + ' ' + ex.StackTrace;
            retval = false;
        }

        return retval;
    }

    

    public void SendMailNotification()
    {
 
        string mailbody = @"
            <style>
                p
                {
                   font-size: 11px;
                   font-family: Verdana, sans-serif;   
                   color: #004595; 
                }
                .othercolor
                {
                    color: #ff7e00;
                }                    
            </style>
        " + notifymsg;

        string mailSubject = "SIG-GroupManagmentTool Notification on: " + DateTime.Now.ToString();        
        MailMessage myMessage = new MailMessage(WbGetParams.getValue("NotifyFromAddress"), Notifyrcp, mailSubject, mailbody);
        myMessage.IsBodyHtml = true;
        if (WbGetParams.getBValue("SendAdminBccNotification")) 
        {
            MailAddress bcc = new MailAddress(WbGetParams.getValue("NotifyAdminToAddress"));
            myMessage.Bcc.Add(bcc);
        }     
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