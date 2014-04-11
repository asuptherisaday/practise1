/*
 * Author   :   umashankar
 * Created  :   26-07-2012
 * modified :   27-07-2012
 */
using System;
using System.Xml;

namespace TrialFront
{
    class EmployeeList
    {
        XmlDocument doc = new XmlDocument();
        private String path;
        public EmployeeList()
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.LastIndexOf("\\", path.LastIndexOf("\\") - 1));
        }
         public String[]  retriveEIDs()
        /*
         * returns array of String of all Employee_id in employee records
         * using file "employeeinfo.xml"
         */
        {
           
            //path = ;
            doc.Load(path + "\\Data\\employeeinfo.xml");
            XmlNodeList nodelist = doc.SelectNodes("employeerecords/employee");
             String[] eid=new String[nodelist.Count];
             for (int i=0; i < nodelist.Count; i++)
             {
                 XmlNode node = nodelist.Item(i);
                 eid[i]= node.Attributes["id"].Value;
             }
             return eid;
        }
        public Boolean resetAnnualLeaves()
         /*
          * reset annaulleaves counter of all employes
          * uses file annualleaves.xml,leavescheme.xml,employeeinfo.xml
          * return
          * true for success
          * false for failure
          */
         {
             try
             {
             XmlDocument annleavedoc = new XmlDocument();
             XmlDocument empdoc = new XmlDocument();
             XmlDocument leavesheme = new XmlDocument();
             empdoc.Load(path + "\\Data\\employeeinfo.xml");
             annleavedoc.Load(path + "\\Data\\annualleaves.xml");
             leavesheme.Load(path + "\\Data\\leavescheme.xml");
             XmlNode wholeleave = annleavedoc.SelectSingleNode("leaverecords");
             wholeleave.InnerXml = "";
             String[] eid=retriveEIDs();
             for (int i = 0; i < eid.Length; i++)
             {
                 String designation = empdoc.SelectSingleNode("employeerecords/employee[@id='" + eid[i] + "']/designation").InnerText;
                 XmlNode leave = leavesheme.SelectSingleNode("schemerecords/designation[@type='"+designation+"']");
                 wholeleave.InnerXml = wholeleave.InnerXml + "<employee id=\""+eid[i]+"\">"+leave.InnerXml+"</employee>";
             }
             annleavedoc.Save(path + "\\Data\\annualleaves.xml");
                 return true; // success
             }
             catch (Exception)
             {
                 return false; //failure
             }
         }
    }
}
