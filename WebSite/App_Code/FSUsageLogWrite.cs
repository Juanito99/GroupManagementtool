using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Xml;


/// <summary>
/// Summary description for FSUsageLogWrite
/// </summary>
public class FSUsageLogWrite
{
    public string timeStamp;
    public string authUser;
    public string appVersion;
    public string appFeedback;
    public string changedGroup;
    public string groupMemberList;

    private string TimeStamp
    {
        get
        {
            return timeStamp;
        }
        set
        {
            timeStamp = value;
        }
    }

    private string AuthUser
    {
        get
        {
            return authUser;
        }
        set
        {
            authUser = value;
        }
    }

    private string AppVersion
    {
        get
        {
            return appVersion;
        }
        set
        {
            appVersion = value;
        }
    }

    private string AppFeedback
    {
        get
        {
            return appFeedback;
        }
        set
        {
            appFeedback = value;
        }
    }

    private string ChangedGroup
    {
        get
        {
            return changedGroup;
        }
        set
        {
            changedGroup = value;
        }
    }

    private string GroupMemberList
    {
        get
        {
            return groupMemberList;
        }
        set
        {
            groupMemberList = value;
        }
    }

	public FSUsageLogWrite()
	{
        this.TimeStamp = timeStamp;
        this.AuthUser = authUser;
        this.AppVersion = appVersion;
        this.AppFeedback = appFeedback;
        this.ChangedGroup = changedGroup;
        this.GroupMemberList = groupMemberList;
	}

    public void UpdateLog()
    {
        string sourceFile = WbGetParams.getValue("UsageLogFile");        

        try
        {
            string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, sourceFile);

            XmlDocument xUsageLog = new XmlDocument();
            xUsageLog.Load(fileName);

            XmlElement XParentElement = xUsageLog.CreateElement("LogEntry");

            XmlElement XTimeStamp = xUsageLog.CreateElement("TimeStamp");
            XmlElement XAuthUser = xUsageLog.CreateElement("AuthUser");
            XmlElement XAppVersion = xUsageLog.CreateElement("AppVersion");
            XmlElement XAppFeedback = xUsageLog.CreateElement("AppFeedback");
            XmlElement XChangedGroup = xUsageLog.CreateElement("ChangedGroup");
            XmlElement XGroupMemberList = xUsageLog.CreateElement("GroupMemberList");

            if (!String.IsNullOrEmpty(timeStamp))
            {
                XTimeStamp.InnerText = timeStamp;
            }
            else
            {
                XTimeStamp.InnerText = "";
            }                

            if (!String.IsNullOrEmpty(authUser))
            {
                XAuthUser.InnerText = authUser;
            }                
            else
            {
                XAuthUser.InnerText = "";
            }

            if (!String.IsNullOrEmpty(appVersion))
            {
                XAppVersion.InnerText = appVersion;
            }
            else
            {
                XAppVersion.InnerText = "";
            }

            if (!String.IsNullOrEmpty(appFeedback))
            {
                XAppFeedback.InnerText = appFeedback;
            }
            else
            {
                XAppFeedback.InnerText = "";
            }

            if (!String.IsNullOrEmpty(changedGroup))
            {
                XChangedGroup.InnerText = changedGroup;
            }
            else
            {
                XChangedGroup.InnerText = "";
            }

            if (!String.IsNullOrEmpty(groupMemberList))
            {
                XGroupMemberList.InnerText = groupMemberList;
            }
            else
            {
                XGroupMemberList.InnerText = "";
            }

                     
            XParentElement.AppendChild(XTimeStamp);
            XParentElement.AppendChild(XAuthUser);
            XParentElement.AppendChild(XAppVersion);
            XParentElement.AppendChild(XAppFeedback);
            XParentElement.AppendChild(XChangedGroup);
            XParentElement.AppendChild(XGroupMemberList);

            xUsageLog.DocumentElement.AppendChild(XParentElement);
            xUsageLog.Save(fileName);

        }

        catch (Exception ex)
        {
            string retMsg = ex.Message;
        }

    
    
    } // end UpdateLog()



}