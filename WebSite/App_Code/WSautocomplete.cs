using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

/// <summary>
/// Summary description for WSautocomplete
/// </summary>
[WebService(Namespace = "")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WSautocomplete : System.Web.Services.WebService {

    public WSautocomplete () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
       
  
    /// <summary>
    /// GetUser retrieves all users from a textfile stores in a list and returns matches after 
    /// recieving key strokes sends them to the jquery-autocomplete for displaying.
    /// </summary>
    /// <param name="userin"></param>
    /// <returns>Array of matched users</returns>

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string[] GetUser(string filterIn)
    {
        int retvalfr;
        string retMsgrf = "";
        
        FSGetFileContent fileCont = new FSGetFileContent();
        fileCont.SourceFile = WbGetParams.getValue("ADUsersFile");
        
        List<string> rtnList = fileCont.rtnValues(out retvalfr, ref retMsgrf, filterIn);
        rtnList.Sort();

        if (retvalfr == 0)
        {            
            return rtnList.ToArray();
        }
        else
        {
            List<string> foundkeys = new List<string>();
            foundkeys.Add("no keys" + ',' + " in cache ");
            return foundkeys.ToArray();
        }

    } //end GetUser
    

}
