using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;

/// <summary>
/// Get the content of a text (csv) file and returns it as a List
/// </summary>
public class FSGetFileContent : System.Web.UI.UserControl
{

    private string sourceFile = "";

    public string SourceFile
    {
        get
        {
            return sourceFile;
        }
        set
        {
            sourceFile = value;
        }
    } 
        
    public List<string> rtnValues(out int retval, ref string objContentMsg)
    {
        retval = 0;
        List<string> rtnLst = new List<string>();        
        string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, sourceFile);

        if (System.IO.File.Exists(fileName))
        {            
            try
            {
                using (FileStream fs = File.OpenRead(fileName))                
                using (TextReader reader = new StreamReader(fs))
                {
                    while (reader.Peek() > -1)
                    {
                        string line = reader.ReadLine();
                        if ((!String.IsNullOrEmpty(line)) && (!String.IsNullOrWhiteSpace(line)))
                        {
                            rtnLst.Add(line);
                        }
                    }
                    reader.Close();
                    fs.Dispose();
                }
                
                retval = 0;
                return rtnLst;
            }
            catch (Exception ex)
            {
                rtnLst.Add(SourceFile + " exception while accessing.");
                objContentMsg = ex.Message + "\n" + ex.StackTrace;
                retval = 1;
                return rtnLst;
            }
        }
        else
        {
            rtnLst.Add(SourceFile + " file not found!");
            retval = 1;
            return rtnLst;
        }
    }

    public List<string> rtnValues(out int retval, ref string objContentMsg, string filterIn)
    {
        retval = 0;
        List<string> rtnLst = new List<string>();
        string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, sourceFile);

        if (System.IO.File.Exists(fileName))
        {
            try
            {
                string regpattern = ".*" + filterIn + ".*";
                var regexACE = new System.Text.RegularExpressions.Regex(regpattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                var tmpMatches = new System.Collections.Generic.List<string>();

                using (FileStream fs = File.OpenRead(fileName))
                using (TextReader reader = new StreamReader(fs))
                {
                    while (reader.Peek() > -1)
                    {
                        string line = reader.ReadLine();
                        if ((!String.IsNullOrEmpty(line)) && (!String.IsNullOrWhiteSpace(line)))
                        {
                            tmpMatches.Clear();
                            tmpMatches.Add(line);

                            var qry = tmpMatches.Where(itm => regexACE.IsMatch(itm));
                            foreach (var item in qry)
                            {
                                rtnLst.Add(item);
                            }                            
                        }
                    }
                    reader.Close();
                    fs.Dispose();
                }

                retval = 0;
                return rtnLst;
                
            }
            catch (Exception ex)
            {
                rtnLst.Add(SourceFile + " exception while accessing.");
                objContentMsg = ex.Message + "\n" + ex.StackTrace;
                retval = 1;
                return rtnLst;
            }
        }
        else
        {
            rtnLst.Add(SourceFile + " file not found!");
            retval = 1;
            return rtnLst;
        }
    }

}