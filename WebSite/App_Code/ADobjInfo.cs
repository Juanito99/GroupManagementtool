using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This class is used to create user objects which are used as container
/// to read/write information in Active Directory
/// </summary>
public class ADobjInfo
{
    private string aDUserID = "";
    private string aDfirstName = "";
    private string aDfamilyName = "";
    private string aDfullName = "";
    private string aDdistinguishedName = "";
    private string aDcompanyKey = "";
    private string aDpersonnelNumber = "";
    private string aDstreetAddress = "";
    private string aDpostalCode = "";
    private string aDtelelphoneNumber = "";
    private string aDCity = "";
    private string aDSkype = "";
    private string aDMail = "";
    private string aDPwd = "";
    private string aDExDB = "";
    private string aDExMBRetPol = "";
    private string aDExOwaPol = "";
    private string aDSID = "";
    private string aDHomeDirve = "";
    private double aDTimeOffset = 0;
    private DateTime aDExperiationDate;
    private string aDproxyAddresses = "";
    private List<string> aDMemberOf;

    public string ADUserID
    {
        get
        {
            return aDUserID;
        }
        set
        {
            aDUserID = value;
        }
    } 

    public string ADfirstName
    {
        get
        {
            return aDfirstName;
        }
        set
        {
            aDfirstName = value;
        }
    }

    public string ADfamilyName
    {
        get
        {
            return aDfamilyName; 
        }
        set
        {
            aDfamilyName = value;
        }
    }
    
    public string ADfullName
    {
        get
        {
            return aDfullName;
        }
        set
        {
            aDfullName = value;
        }
    }
        
    public string ADdistinguishedName
    {
        get
        {
            return aDdistinguishedName;
        }
        set
        {
            aDdistinguishedName = value;
        }
    }

    public string ADcompanyKey
    {
        get
        {
            return aDcompanyKey;
        }
        set
        {
            aDcompanyKey = value;
        }
    }
        
    public string ADpersonnelNumber
    {
        get
        {
            return aDpersonnelNumber;
        }
        set
        {
            aDpersonnelNumber = value;
        }
    }

    public string ADstreetAddress
    {
        get
        {
            return aDstreetAddress;
        }
        set
        {
            aDstreetAddress = value;
        }
    }

    public string ADpostalCode
    {
        get
        {
            return aDpostalCode;
        }
        set
        {
            aDpostalCode = value;
        }
    }

    public string ADCity
    {
        get
        {
            return aDCity;
        }
        set
        {
            aDCity = value;
        }
    }

    public string ADSkype
    {
        get
        {
            return aDSkype;
        }
        set
        {
            aDSkype = value;
        }
    }

    public string ADMail
    {
        get
        {
            return aDMail;
        }
        set
        {
            aDMail = value;
        }
    }

    public string ADPwd
    {
        get
        {
            return aDPwd;
        }
        set
        {
            aDPwd = value;
        }
    }

    public string ADtelelphoneNumber
    {
        get
        {
            return aDtelelphoneNumber;
        }
        set
        {
            aDtelelphoneNumber = value;
        }
    }

    public string ADExDB
    {
        get
        {
            return aDExDB;
        }
        set
        {
            aDExDB = value;
        }
    }

    public string ADExOwaPol
    {
        get
        {
            return aDExOwaPol;
        }
        set
        {
            aDExOwaPol = value;
        }
    }

    public string ADExMBRetPol
    {
        get
        {
            return aDExMBRetPol;
        }
        set
        {
            aDExMBRetPol = value;
        }
    }

    public string ADSID
    {
        get
        {
            return aDSID;
        }
        set
        {
            aDSID = value;
        }
    }

    public string ADHomeDirve
    {
        get
        {
            return aDHomeDirve;
        }
        set
        {
            aDHomeDirve = value;
        }
    }

    public double ADTimeOffset
    {
        get
        {
            return aDTimeOffset;
        }
        set
        {
            aDTimeOffset = value;
        }
    }

    public DateTime ADExperiationDate
    {
        get
        {
            return aDExperiationDate;
        }
        set
        {
            aDExperiationDate = value;
        }
    }

    public List<string> ADmemberOf
    {
        get
        {
            return aDMemberOf;
        }
        set
        {
            aDMemberOf = value;
        }
    }

    public string ADproxyAddresses
    {
        get
        {
            return aDproxyAddresses;
        }
        set
        {
            aDproxyAddresses = value;
        }
    }

	public ADobjInfo()
	{
		
	}
}