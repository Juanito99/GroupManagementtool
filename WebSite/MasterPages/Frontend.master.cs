using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class MasterPages_Frontend : System.Web.UI.MasterPage
{
    enum mailType
    {
        UserPage = 0,
        InfoPage = 1,
        UsageLogPage = 2       
    };       

    protected void Page_Load(object sender, EventArgs e)
    {
        string appLanguage = Request.UserLanguages[0].Substring(0, 2).ToUpper();
        var appText = new System.Collections.Generic.Dictionary<string, string>();
        string appTextError = "";
        bool foundLangFile = WbVarious.GetAppText(ref appText, ref appTextError, appLanguage);
                
        string  mailSendTxt, reloadPageTxt;

        appText.TryGetValue("MailSend", out mailSendTxt);
        appText.TryGetValue("ReloadPage", out reloadPageTxt);

        lbtnEmail.Attributes.Add("title", mailSendTxt);
        lbtnReload.Attributes.Add("title", reloadPageTxt);
    } // end Page_Load()

   
    protected void lbtnReload_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect(Page.Request.Url.ToString(), false);
    } // end lbtnReload_Click(object sender, ..)

    /// <summary>
    /// Collects information from the GUI which will send out via mail.
    /// Prepares the e-mail text.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbtnEmail_Click(object sender, EventArgs e)
    {
        string appLanguage = Request.UserLanguages[0].Substring(0, 2).ToUpper();
        var appText = new System.Collections.Generic.Dictionary<string, string>();
        string appTextError = "";
        bool foundLangFile = WbVarious.GetAppText(ref appText, ref appTextError, appLanguage);
        var mailChoice = mailType.UserPage;

        string greetingTxt, mailBodyPartATxt, mailBodyPartBTxt, mailBodyEndTxt, groupMemberTxt;

        appText.TryGetValue("Greeting", out greetingTxt);
        appText.TryGetValue("MailBodyPartA", out mailBodyPartATxt);
        appText.TryGetValue("MailBodyPartB", out mailBodyPartBTxt);
        appText.TryGetValue("MailBodyEnd", out mailBodyEndTxt);
        appText.TryGetValue("GroupMembers", out groupMemberTxt);

        string rcpaddr = GetNotifyMailRcpAddr();
        
        var sb = new System.Text.StringBuilder();
        string adGoupName="", authUser="", adGroupNotes ="";

        MasterPage master = Page.Master;
        ContentPlaceHolder mpContentPlaceHolder;
        ListBox mpListBox;
        Label mpLabelUsr, mpLabelGrpNotes;
        DropDownList mpAuthUsrRWGroups;
        Label mLabelcontentTbl;
        string mLabelcontentTblText = "";

        mpContentPlaceHolder = (ContentPlaceHolder)master.FindControl("cpMainContent");
        if (mpContentPlaceHolder != null)
        {
            mpListBox = (ListBox)mpContentPlaceHolder.FindControl("lstGroupMembers");
            mpLabelUsr = (Label)mpContentPlaceHolder.FindControl("lblExistUsrFullName");
            mpLabelGrpNotes = (Label)mpContentPlaceHolder.FindControl("lblADGroupsNotes");
            mpAuthUsrRWGroups = (DropDownList)mpContentPlaceHolder.FindControl("drpdAuthUsrRWGroups");

            mLabelcontentTbl = (Label)mpContentPlaceHolder.FindControl("lblcontentTbl");

            if (mLabelcontentTbl != null)
            {
                mailChoice = mailType.InfoPage;
                mLabelcontentTblText = mLabelcontentTbl.Text;
            }

            if (mpListBox != null)
            {
                for (int i = 0; i < mpListBox.Items.Count; i++)
                {
                    sb.Append(mpListBox.Items[i].ToString()).Append(';');
                }            
            }    
        
            if (mpLabelUsr != null)
            {
                authUser = mpLabelUsr.Text;
            }

            if (mpLabelGrpNotes != null)
            {
                adGroupNotes = mpLabelGrpNotes.Text;
            }

            if (mpAuthUsrRWGroups != null)
            {
                if (mpAuthUsrRWGroups.SelectedValue.Contains(','))
                {
                    string[] tmp = mpAuthUsrRWGroups.SelectedValue.Split(',');
                    string sname = System.Text.RegularExpressions.Regex.Replace(tmp[0], "cn=", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    adGoupName = sname;
                }                
            }

        }

        string groupMemberList = sb.ToString().TrimEnd(';');
        string mailText = "";

        if (mailChoice == mailType.UserPage)
        {
            mailText = "<p>" + greetingTxt + ' ' + authUser +
                "<br /><br />" + mailBodyPartATxt + " <span class=\"othercolor\"> " + adGoupName + "</span>" + ':' +
                "<br />" + groupMemberTxt + " <span class=\"othercolor\"> " + groupMemberList + "</span>" + '.' +
                "<br />" + mailBodyPartBTxt + " <span class=\"othercolor\"> " + adGroupNotes + "</span>" + '.' +
                "<br /><br />" + mailBodyEndTxt + "</p>";
        } 

        if (mailChoice == mailType.InfoPage)
        {
            mailText = "<p>" +
                "<br /><br />" + "<table class=\"tableabout\">" +
                "<tr><th>Group name:</th><th>ManagedBy</th>" +
                "</tr>" + mLabelcontentTblText + "</table> </p>";           
        }               

        SendNotificationMail(rcpaddr, mailText);

    } // end lbtnEmail_Click(object sender, ...)
    
    /// <summary>
    /// Gets the authenticated user's email address from Active Directory.
    /// </summary>
    /// <returns>emailaddress</returns>
    protected string GetNotifyMailRcpAddr()
    {
        string objContentMsg = "";
        ADrwInfo ADrwinfoO = new ADrwInfo();
        ADobjInfo adobjui = new ADobjInfo();
        int retval;        
        string objsearchfilter = "user"; 
        string mailto = "";

        string authUsr = Request.LogonUserIdentity.Name;

        if (!string.IsNullOrEmpty(authUsr))
        {
            if (authUsr.Contains('\\')) 
            {
                string[] tmp = authUsr.Split('\\');
                adobjui.ADUserID = tmp[1];
            }
        }

        var rtnobj = ADrwinfoO.RetObjValues(ref objContentMsg, out retval, objsearchfilter, adobjui);

        if (retval == 0)
        {

            if (!string.IsNullOrEmpty(rtnobj.ADMail))
            {
                mailto = rtnobj.ADMail;
            }
        }

        return mailto;
    } // end GetNotifactionRcpAdr()

    /// <summary>
    /// Calls the method for sending the E-mail to the authenticated user.
    /// </summary>
    /// <param name="rcpaddr"></param>
    /// <param name="notifymsg"></param>
    protected void SendNotificationMail(string rcpaddr, string notifymsg)
    {
        NWvarOps notify = new NWvarOps();
        notify.Notifyrcp = rcpaddr;
        notify.Notifymsg = notifymsg;
        notify.SendMailNotification();
    } // end SendNotificationMail(string rcpaddrs, ...)
}
