using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for FSReadXML
/// </summary>
public class FSReadXML
{
      
    private string sourceFile;
    private string nameSpace;
    private string queryParam;
    private string xmlChildName;

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

    public string NameSpace
    {
        get
        {
            return nameSpace;
        }
        set
        {
            nameSpace = value;
        }            
    }

    public string QueryParam
    {
        get
        {
            return queryParam;
        }
        set
        {
            queryParam = value;
        }
    }

    public string XmlChildName
    {
        get
        {
            return xmlChildName;
        }
        set
        {
            xmlChildName = value;
        }
    }
        
	public FSReadXML()
	{
        nameSpace = "";
	}

    public bool ReadSingleNode(ref Dictionary<string, string> rtnDict, ref string errorDetails)
    {
        
        bool rsltState = false; 
        errorDetails = "";

        try
        {
            
            string fileName = Path.Combine(HttpRuntime.AppDomainAppPath, sourceFile);
            if (System.IO.File.Exists(fileName))
            {
                bool hasSchema = false;
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNode root = doc.DocumentElement;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                
                string urn = "";
                string objMsg = "";

                hasSchema = GetUrnFromFile(fileName, ref urn, ref objMsg);
                
                if (!hasSchema)                
                {
                    urn = "defaults";
                }

                nsmgr.AddNamespace(nameSpace, "urn:" + urn);

                string qryStr = "descendant::" + nameSpace + ":" + xmlChildName + "[" + nameSpace + ":" + queryParam + "]";
                XmlNode node = root.SelectSingleNode(qryStr, nsmgr);               
                                
                // only flat xml-files are supported no childitem can have childitems!
                if(node.ChildNodes.Count > 0)
                {                   
                    foreach (XmlNode cn in node.ChildNodes)
                    {
                        rtnDict.Add(cn.Name, cn.InnerText);                        
                    }
                    rsltState = true;
                }
                else
                {
                    errorDetails = "Error, No childelements found in xml.";                    
                }                
            }
            else
            {
                throw new FileNotFoundException(fileName);
            }

        }
        catch (Exception ex)
        {            
            errorDetails += ex.StackTrace + ' ' + ex.Message; 
        }

        return rsltState;

    } // end ReadSingleNode(out Dictionary<string, string> rtnDict)

    private bool GetUrnFromFile(string fileName, ref string fUrn, ref string objMsg)
    {
        bool hasSchema = false;
        fUrn = "";
        objMsg = "no errrs in GetUrnFromFile";
                        
        string nsfile = File.ReadLines(fileName).Skip(1).Take(1).First();
        if (nsfile.Contains("urn:"))
        {
            string regP = @"(?<=urn:)[a-zA-Z-]+(?="">)";
            var m = Regex.Match(nsfile, regP);
            try
            {
                if (m.Success)
                {
                    fUrn = m.Value;
                    hasSchema = true;
                }
                else
                {                    
                    hasSchema = false;
                }
            }
            catch (Exception ex)
            {                
                objMsg = "Regerror: " + ex.StackTrace + ' ' + ex.Message;
            }
        }
        else
        {            
            hasSchema = false;
            objMsg = "Regerror: No schemaFound.";
        }             

        return hasSchema;
    } // end bool GetUrnFromFile(string fileName, string ref fUrn)

}