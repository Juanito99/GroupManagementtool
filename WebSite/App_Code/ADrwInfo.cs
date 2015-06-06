using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Text.RegularExpressions;


/// <summary>
/// Read and write operations with Actie Directory
/// </summary>
public class ADrwInfo
{
    const string AccountDisabledFlag = @"1.2.840.113556.1.4.803:=2";        
    private string rootDSE = "";
    private string defaultnc = "";
    private string upnSuffix = "";

	public ADrwInfo()
	{
        using (DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE"))
        {
            rootDSE = "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
            defaultnc = de.Properties["defaultNamingContext"][0].ToString();
            upnSuffix = Regex.Replace(defaultnc, "dc=", "", RegexOptions.IgnoreCase);
            upnSuffix = upnSuffix.Replace(',', '.');
        }
    }

    public string UpnSuffx
    {
        get
        {
            return upnSuffix;
        }
    }    

    /// <summary>
    /// Multi purpose query of specified AD objects and some of their properties.
    /// </summary>
    /// <param name="objContentMsg"></param>
    /// <param name="retval"></param>
    /// <param name="objsearchfilter"></param>
    /// <param name="adobjin"></param>
    /// <returns>Object that reflects the queried information</returns>
    public ADobjInfo RetObjValues(ref string objContentMsg, out int retval, string objsearchfilter, ADobjInfo adobjin)
    {

        retval = 0;
        string objfilter = "";        

        switch (objsearchfilter)
        {
            case "userdn":
                objfilter = "(&(objectClass=User)(!(userAccountControl:" + AccountDisabledFlag + "))(distinguishedName=" + adobjin.ADdistinguishedName + "))";
                break;
            case "user":
                objfilter = "(&(objectClass=User)(!(userAccountControl:" + AccountDisabledFlag + "))(samAccountName=" + adobjin.ADUserID + "))";
                break;
            case "mail":
                objfilter = "(&(objectClass=User)(!(userAccountControl:" + AccountDisabledFlag + "))(proxyAddresses=smtp:" + adobjin.ADMail + "))";
                break;
            case "personnelno":
                objfilter = "(&(objectClass=User)(!(userAccountControl:" + AccountDisabledFlag + "))(extensionAttribute5=" + adobjin.ADpersonnelNumber.ToString() + "))";
                break;
            case "name":
                objfilter = "(&(objectClass=User)(!(userAccountControl:" + AccountDisabledFlag + "))(name=" + adobjin.ADfullName + "))";
                break;
            case "firstfamilyname":
                objfilter = "(|" +
                        "(&(objectClass=Contact)(givenName=" + adobjin.ADfirstName + ")(sn=" + adobjin.ADfamilyName + "))" +
                        "(&(objectClass=User)(!(userAccountControl:" + AccountDisabledFlag + "))(givenName=" + adobjin.ADfirstName + ")(sn=" + adobjin.ADfamilyName + "))" +
                        ")";
                break;
        }

        ADobjInfo ADObj = new ADobjInfo();

        try
        {                       

            string[] myProps = new string[] { "distinguishedName", "extensionAttribute1", "sn", "proxyAddresses", "mail",
                "name", "givenName", "memberOf", "l", "postalCode", "samAccountName", "streetAddress", "extensionAttribute5" };

            using (DirectoryEntry entry = new DirectoryEntry(rootDSE))
            using (DirectorySearcher mySearcher = new DirectorySearcher(entry, objfilter, myProps))
            {
                using (SearchResultCollection result = mySearcher.FindAll())
                {
                    if (result.Count > 0)
                    {
                        string propertyName = "memberOf";                        

                        foreach (SearchResult rs in result)
                        {                            

                            ResultPropertyValueCollection valueCollection = rs.Properties[propertyName];
                            ADObj.ADmemberOf = new List<string>();

                            ADObj.ADdistinguishedName = rs.Properties["distinguishedName"][0].ToString();

                            if (rs.Properties["samAccountName"].Count > 0)
                                ADObj.ADUserID = rs.Properties["samAccountName"][0].ToString();
                            else
                                ADObj.ADUserID = "";

                            if (rs.Properties["name"].Count > 0)
                                ADObj.ADfullName = rs.Properties["name"][0].ToString();
                            else
                                ADObj.ADfullName = "";

                            if (rs.Properties["givenName"].Count > 0)
                                ADObj.ADfirstName = rs.Properties["givenName"][0].ToString();
                            else
                                ADObj.ADfirstName = "";

                            if (rs.Properties["sn"].Count > 0)
                                ADObj.ADfamilyName = rs.Properties["sn"][0].ToString();
                            else
                                ADObj.ADfamilyName = "";

                            if ((String.IsNullOrEmpty(valueCollection.ToString())))
                            {
                                ADObj.ADmemberOf.Add("");
                            }
                            else
                            {
                                if (valueCollection.Count > 0)
                                {
                                    for (int j = 0; j < valueCollection.Count - 1; j++)
                                    {
                                        ADObj.ADmemberOf.Add(valueCollection[j].ToString());
                                    }
                                }
                                else
                                {
                                    ADObj.ADmemberOf.Add("");
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["extensionAttribute1"].ToString())))
                            {
                                ADObj.ADcompanyKey = "";
                            }
                            else
                            {
                                if (rs.Properties["extensionAttribute1"].Count > 0)
                                {
                                    ADObj.ADcompanyKey = rs.Properties["extensionAttribute1"][0].ToString();
                                }
                                else
                                {
                                    ADObj.ADcompanyKey = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["extensionAttribute5"].ToString())))
                            {
                                ADObj.ADpersonnelNumber = "";
                            }
                            else
                            {
                                if (rs.Properties["extensionAttribute5"].Count > 0)
                                {
                                    ADObj.ADpersonnelNumber = rs.Properties["extensionAttribute5"][0].ToString();
                                }
                                else
                                {
                                    ADObj.ADpersonnelNumber = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["streetAddress"].ToString())))
                            {
                                ADObj.ADstreetAddress = "";
                            }
                            else
                            {
                                if (rs.Properties["streetAddress"].Count > 0)
                                {
                                    ADObj.ADstreetAddress = rs.Properties["streetAddress"][0].ToString();
                                }
                                else
                                {
                                    ADObj.ADstreetAddress = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["postalCode"].ToString())))
                            {
                                ADObj.ADpostalCode = "";
                            }
                            else
                            {
                                if (rs.Properties["postalCode"].Count > 0)
                                {
                                    ADObj.ADpostalCode = rs.Properties["postalCode"][0].ToString();
                                }
                                else
                                {
                                    ADObj.ADpostalCode = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["l"].ToString())))
                            {
                                ADObj.ADCity = "";
                            }
                            else
                            {
                                if (rs.Properties["l"].Count > 0)
                                {
                                    ADObj.ADCity = rs.Properties["l"][0].ToString();
                                }
                                else
                                {
                                    ADObj.ADCity = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["mail"].ToString())))
                            {
                                ADObj.ADMail = "";
                            }
                            else
                            {
                                if (rs.Properties["mail"].Count > 0)
                                {
                                    ADObj.ADMail = rs.Properties["mail"][0].ToString();
                                }
                                else
                                {
                                    ADObj.ADMail = "";
                                }
                            }
                            
                        }
                        
                        objContentMsg = "ok";
                    }
                    else
                    {
                        objContentMsg = "Searched object not found: " + adobjin.ADUserID + " " + adobjin.ADfirstName + " " + adobjin.ADfamilyName;                        
                    }
                }
            }

            retval = 0;
        }

        catch (Exception ex)
        {
            objContentMsg += ex.Message + "\n" + ex.StackTrace;            
            retval = 1;
        }

        return ADObj;

    } // end RetObjValues()

    /// <summary>
    /// Query of group objects and its properties
    /// </summary>
    /// <param name="objContentMsg"></param>
    /// <param name="retval"></param>
    /// <param name="objContentNO"></param>
    /// <param name="adgrpin"></param>
    /// <returns>object that reflects the queried group object</returns>
    public ADgroupobjInfo RetGroupValues(ref string objContentMsg, out int retval, out int objContentNO, ADgroupobjInfo adgrpin)
    {

        retval = 0;
        string objfilter = "";
        var adgrp = new ADgroupobjInfo();

        objfilter = "(&(objectClass=group)(distinguishedName=" + adgrpin.DistinguishedName + "))";
        
        try
        {

            string[] myProps = new string[] { "distinguishedName", "cn", "managedBy", "mail", "samAccountName",
                "description", "notes", "memberOf", "displayName ", "member", "memberOf"};

            using (DirectoryEntry entry = new DirectoryEntry(rootDSE))
            using (DirectorySearcher mySearcher = new DirectorySearcher(entry, objfilter, myProps))
            {
                using (SearchResultCollection result = mySearcher.FindAll())
                {
                    if (result.Count > 0)
                    {                        

                        foreach (SearchResult rs in result)
                        {
                            
                            ResultPropertyValueCollection memberCollection = rs.Properties["member"];
                            adgrp.Member = new List<string>();

                            if ((String.IsNullOrEmpty(memberCollection.ToString())))
                            {
                                adgrp.Member.Add("");
                            }
                            else
                            {
                                if (memberCollection.Count > 0)
                                {
                                    for (int j = 0; j < memberCollection.Count; j++)
                                    {
                                        adgrp.Member.Add(memberCollection[j].ToString());
                                    }
                                }
                                else
                                {
                                    adgrp.Member.Add("");
                                }
                            }

                            ResultPropertyValueCollection memberOfCollection = rs.Properties["memberOf"];
                            adgrp.MemberOf = new List<string>();

                            if ((String.IsNullOrEmpty(memberOfCollection.ToString())))
                            {
                                adgrp.MemberOf.Add("");
                            }
                            else
                            {
                                if (memberOfCollection.Count > 0)
                                {
                                    for (int j = 0; j < memberOfCollection.Count - 1; j++)
                                    {
                                        adgrp.MemberOf.Add(memberOfCollection[j].ToString());
                                    }
                                }
                                else
                                {
                                    adgrp.MemberOf.Add("");
                                }
                            }

                            adgrp.DistinguishedName = rs.Properties["distinguishedName"][0].ToString();
                            adgrp.SamAccountName = rs.Properties["SamAccountName"][0].ToString();
                            adgrp.CN = rs.Properties["cn"][0].ToString();

                            if ((String.IsNullOrEmpty(rs.Properties["description"].ToString())))
                            {
                                adgrp.Description = "";
                            }
                            else
                            {
                                if (rs.Properties["description"].Count > 0)
                                {
                                    adgrp.Description = rs.Properties["description"][0].ToString();
                                }
                                else
                                {
                                    adgrp.Description = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["notes"].ToString())))
                            {
                                adgrp.Notes = "";
                            }
                            else
                            {
                                if (rs.Properties["notes"].Count > 0)
                                {
                                    adgrp.Notes = rs.Properties["notes"][0].ToString();
                                }
                                else
                                {
                                    adgrp.Notes = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["displayName"].ToString())))
                            {
                                adgrp.DisplayName = "";
                            }
                            else
                            {
                                if (rs.Properties["displayName"].Count > 0)
                                {
                                    adgrp.DisplayName = rs.Properties["displayName"][0].ToString();
                                }
                                else
                                {
                                    adgrp.DisplayName = "";
                                }
                            }  

                            if ((String.IsNullOrEmpty(rs.Properties["mail"].ToString())))
                            {
                                adgrp.Mail = "";
                            }
                            else
                            {
                                if (rs.Properties["mail"].Count > 0)
                                {
                                    adgrp.Mail = rs.Properties["mail"][0].ToString();
                                }
                                else
                                {
                                    adgrp.Mail = "";
                                }
                            }

                            if ((String.IsNullOrEmpty(rs.Properties["managedBy"].ToString())))
                            {
                                adgrp.ManagedBy = "";
                            }
                            else
                            {
                                if (rs.Properties["managedBy"].Count > 0)
                                {
                                    adgrp.ManagedBy = rs.Properties["managedBy"][0].ToString();
                                }
                                else
                                {
                                    adgrp.ManagedBy = "";
                                }
                            } 
                            
                        }

                        objContentNO = 1;
                        objContentMsg = "ok";

                    }
                    else
                    {
                        objContentMsg = "Searched object not found: " + adgrpin.DistinguishedName;
                        objContentNO = 0;
                    }
                }
            }

            retval = 0;
        }

        catch (Exception ex)
        {
            objContentMsg += ex.Message + "\n" + ex.StackTrace;
            objContentNO = 0;
            retval = 1;
        }

        return adgrp;

    } // end  public List<ADgroupobjInfo> RetGroupValues            

    /// <summary>
    /// Method to add users to the selected AD group, membership is cleared firstly, only enabled users are added back.
    /// </summary>
    /// <param name="objContentMsg"></param>
    /// <param name="retval"></param>
    /// <param name="adGrpDNIn"></param>
    /// <param name="adGrpMembersIn"></param>
    public void AddUsersToGroup(out string objContentMsg, out int retval, string adGrpDNIn, List<string> adGrpMembersIn)
    {
        retval = 0;
        objContentMsg = "";

        try
        {                       
            if ((adGrpMembersIn.Count > 0) && (!String.IsNullOrEmpty(adGrpDNIn)))
            {
                if (ADObjCheckWritePermission("LDAP://" + adGrpDNIn))
                {   
                    using (DirectoryEntry adentry = new DirectoryEntry("LDAP://" + adGrpDNIn))
                    {
                        adentry.Properties["member"].Clear();
                        adentry.CommitChanges();
                        foreach (string memberDN in adGrpMembersIn)
                        {                        
                            adentry.Properties["member"].Add(memberDN);                            
                        }
                        adentry.CommitChanges();
                        adentry.Close();                                        
                    }
                    retval = 0;
                }
                else
                {
                    objContentMsg += "Not having write access to group: " + adGrpDNIn + "<br />";
                    retval = 1; 
                }             
            } // end-if (adGrpMembersIn.Count ...)            
        }
        catch (Exception ex)
        {
            objContentMsg += ex.Message + ' ' + ex.StackTrace;
            retval = 1;
        }

    } // end AddUsersToGroup(out string ...)

    /// <summary>
    /// Checks if the logged on user has write permissions to the AD object ( the group ).
    /// </summary>
    /// <param name="distingusihedname"></param>
    /// <returns> true or false </returns>
    public bool ADObjCheckWritePermission(string distingusihedname)
    {
        using (DirectoryEntry adentry = new DirectoryEntry(distingusihedname))
        {
            try
            {
                string[] properties = { "member" };
                adentry.RefreshCache(new string[] { "allowedAttributesEffective" });
                return properties.All(property => adentry.Properties["allowedAttributesEffective"].Contains(property));
                //return adentry.Properties["allowedAttributesEffective"].Value != null;
            }
            catch
            {
                return false;
            }
        }
    } // end ADObjCheckWritePermissions()

    /// <summary>
    /// Checks if an specified object exists in Active Directory ( not used presently )
    /// </summary>
    /// <param name="distinguishedname"></param>
    /// <returns></returns>
    public bool ADObjExists(string distinguishedname)
    {
        bool found = false;
        if (DirectoryEntry.Exists(distinguishedname))
            found = true;
        return found;
    }     //end ADObjExists


}