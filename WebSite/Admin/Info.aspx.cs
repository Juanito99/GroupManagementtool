using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Info : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
                 
            var aDGroupsStatic = new System.Collections.Generic.Dictionary<string, string>();                                
            bool aDGroupsExist = ADGroups_Load(ref aDGroupsStatic);                    
            var sb = new System.Text.StringBuilder();
            
            if (aDGroupsExist)
            {                
                var items = from pair in aDGroupsStatic
                            orderby pair.Value ascending
                            select pair;
                
                foreach (KeyValuePair<string, string> pair in items)
                {                    
                    string[] tmp = pair.Key.Split(',');
                    string gname = System.Text.RegularExpressions.Regex.Replace(tmp[0], "cn=", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    sb.Append("<tr><td>").Append(gname).Append("</td><td>").Append(pair.Value).Append("</td></tr>");
                }
            }

            lblcontentTbl.Text = sb.ToString();
    }

    /// <summary>
    /// Getting all groups which have managedBy set out of a text file and storing it in a Dictionary.
    /// </summary>
    /// <param name="aDGroupsStatic">D</param>
    protected bool ADGroups_Load(ref Dictionary<string, string> aDGroupsStatic)
    {

        int retval;
        string objContentMsg = "";

        FSGetFileContent fileCont = new FSGetFileContent();
        fileCont.SourceFile = WbGetParams.getValue("ADGroupsFile");
                
        List<string> rtnList = fileCont.rtnValues(out retval, ref objContentMsg);               

        if (retval == 0)
        {
            foreach (var itm in rtnList)
            {
                string[] itms = itm.Split('#');
                aDGroupsStatic.Add(itms[0], itms[1]);
            }
            return true;
        }
        else
        {
            return false;
        }

    } // end ADGroups_Load()


}