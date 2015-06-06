using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;


/// <summary>
/// Getting configuration information from web.config file.
/// </summary>
public class WbGetParams
{
    
    public static string getValue(string webItem)
    {
        string webItemDetails = "";
        webItemDetails = WebConfigurationManager.AppSettings.Get(webItem);               

        if (!string.IsNullOrEmpty(webItemDetails))
        {
            return webItemDetails;
        }
        else
        {
            return "Error!";
        }                
    }

    public static bool getBValue(string webItem)
    {
        string webItemDetails = "";
        webItemDetails = WebConfigurationManager.AppSettings.Get(webItem);

        if (!string.IsNullOrEmpty(webItemDetails))
        {
            return Convert.ToBoolean(webItemDetails);
        }
        else
        {
            return false;
        }
    }       
    
}