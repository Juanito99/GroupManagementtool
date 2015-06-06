using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ADgroupobjInfo
/// </summary>
public class ADgroupobjInfo
{
    private string cn = "";
    private string distinguishedName = "";
    private string managedBy = "";
    private string samAccountName = "";
    private string description = "";
    private string notes = "";
    private string displayName = "";
    private string mail = "";
    private List<string> member;
    private List<string> memberOf;

    public string CN
    {
        get
        {
            return cn;
        }
        set
        {
            cn = value;
        }
    }
    public string DistinguishedName
    {
        get
        {
            return distinguishedName;
        }
        set
        {
            distinguishedName = value;
        }
    }
    public string ManagedBy
    {
        get
        {
            return managedBy;
        }
        set
        {
            managedBy = value;
        }
    }
    public string SamAccountName
    {
        get
        {
            return samAccountName;
        }
        set
        {
            samAccountName = value;
        }
    }
    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }
    public string Notes
    {
        get
        {
            return notes;
        }
        set
        {
            notes = value;
        }
    }
    public string DisplayName
    {
        get
        {
            return displayName;
        }
        set
        {
            displayName = value;
        }
    }
    public string Mail
    {
        get
        {
            return mail;
        }
        set
        {
            mail = value;
        }
    }
    public List<string> Member
    {
        get
        {
            return member;
        }
        set
        {
            member = value;
        }
    }
    public List<string> MemberOf
    {
        get
        {
            return memberOf;
        }
        set
        {
            memberOf = value;
        }
    }

	public ADgroupobjInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}