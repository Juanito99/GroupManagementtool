using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for WbVarious
/// </summary>
public class WbVarious
{
     

    /// <summary>
    /// Getting the text for the UserInterface from XML file.
    /// </summary>
    /// <param name="appText"></param>
    /// <param name="errorDetails"></param>
    /// <param name="appLanguage"></param>
    /// <returns>Dictionary containing the items in the detected language.</returns>
    public static bool GetAppText(ref Dictionary<string, string> appText, ref string errorDetails, string appLanguage)
    {
        string[] arrlangs = WbGetParams.getValue("AppLanguages").Split(',');

        if (!arrlangs.Contains(appLanguage))
        {
            appLanguage = "EN";
        }

        bool retval = false;
        var xmlSrc = new FSReadXML()
        {
            NameSpace = "ns",
            QueryParam = @"Language='" + appLanguage + @"'",
            SourceFile = WbGetParams.getValue("AppTextFile"),
            XmlChildName = "AppText"
        };

        bool hasContent = xmlSrc.ReadSingleNode(ref appText, ref errorDetails);
        if (hasContent)
        {
            retval = true;
        }

        return retval;
    } // end GetAppText(ref Dictionary<string, string> appText, ref string errorDetails, string appLanguage)   


}

