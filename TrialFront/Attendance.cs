/*
 * Author   :   umashankar
 * Created  :   26-07-2012
 * modified :   31-07-2012
 */
using System;
using System.Xml;
using System.Xml.XPath;
using System.Linq;


namespace TrialFront
{
    class Attendance
    {
        private XmlDocument activDoc = new XmlDocument();
        private XmlDocument leaveDoc = new XmlDocument();
        private String path;
        public Attendance()
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.LastIndexOf("\\", path.LastIndexOf("\\") - 1));
        }
        public int markAttendance(String EID, String mode, int othours,DateTime date)
        /*
         * it marks appropriate attendance and 
         * uses file attendance.xml,annualleaves.xml,otrecords.xml
         * returns
         * 100 for marked sucess
         * 200 for CL(casual leave) not available for given EID
         * 300 for EL(Earned leave) not available for given EID
         * 400 for SL(sick leave) not available for given EID
         * 500 for HD(half day) will not be allocated
         * 700 for already marked
         * 800 for annual leaves not found
         * 900 for error wrong parameter
         * 404 for inconsistance xml files
         */
        {
            mode=mode.ToUpper();
            try
            {
                leaveDoc.Load(path + "\\data\\annualleaves.xml");
                
            }
            catch (System.Exception)
            {

                return 404;//inconsistance xml file
            }
            
            if (mode.CompareTo("FD") == 0)
            {
                return put(EID, mode,date); // for writing
            }
            else
                if(mode.CompareTo("HD") == 0)
                {
                    XmlNode hdremain=leaveDoc.SelectSingleNode("leaverecords/employee[@id='" + EID + "']/el");
                    if (hdremain == null)
                        return 800; // annual leaves not found
                    float remaining = float.Parse(hdremain.InnerText);
                    if (remaining == 0f)
                        return 500; // HD(half day) will not be allocated
                    else
                    {
                        remaining = remaining - 0.5f;
                        hdremain.InnerText = remaining.ToString();
                        int check = put(EID, mode, date);
                        if (check == 100)
                        {
                            leaveDoc.Save(path + "\\data\\annualleaves.xml");
                            return 100;
                        }
                        else
                            return 700; // already marked
                        
                    }
                }
                if (mode.CompareTo("CL") == 0 || mode.CompareTo("EL") == 0 || mode.CompareTo("SL") == 0)
                {
                    String smallmode = mode.ToLower();
                    XmlNode getleave = leaveDoc.SelectSingleNode("leaverecords/employee[@id='"+EID+"']/"+smallmode);
                    if (getleave != null)
                    {
                        int remainleave = (int)float.Parse(getleave.InnerText);
                        if (remainleave == 0)
                        {
                            if (mode.CompareTo("CL") == 0)
                                return 200; // CL(casual leave) not available for given EID
                            if (mode.CompareTo("EL") == 0)
                                return 300; // EL(Earned leave) not available for given EID
                            if (mode.CompareTo("SL") == 0)
                                return 400; // SL(sick leave) not available for given EID
                            return 900; // error wrong parameter
                        }
                        else
                        {
                            float minusleave = float.Parse(getleave.InnerText);
                            minusleave = minusleave - 1;
                            getleave.InnerText = minusleave.ToString();
                            int check = put(EID, mode, date);
                            if (check == 100)
                            {
                                leaveDoc.Save(path + "\\data\\annualleaves.xml");
                                return 100;
                            }
                            else
                                return 700;
                            
                        }
                    }
                    else
                        
                        return 800; // annual leaves not found
                }
                else
                    if (mode.CompareTo("AB") == 0)
                    {
                        return put(EID, mode, date);

                    }
                    if (mode.CompareTo("OT") == 0)
                    {
                        XmlDocument otdoc = new XmlDocument();
                        otdoc.Load(path + "\\data\\otrecords.xml");
                        XmlNode check=otdoc.SelectSingleNode("otrecords/employee[@id='"+EID+"']");
                        if (check == null)
                        {
                            check = otdoc.SelectSingleNode("otrecords");
                            check.InnerXml = check.InnerXml + "<employee id=\"" + EID + "\">" + othours.ToString() + "</employee>";
                            otdoc.Save(path + "\\data\\otrecords.xml");
                            return 100; // success
                        }
                        else
                        {
                            int othourstotal =int.Parse(check.InnerText)+othours;
                            check.InnerText = othourstotal.ToString();
                            otdoc.Save(path + "\\data\\otrecords.xml");
                            return 100; // success
                        }
                    }
                    else
                        return 900;  // error wrong parameter
        }
        int put(String EID,String attentype,DateTime date)
            /*
             * for writing attendence xml
             * only for my use not public 
             */
        {
            try
            {
                activDoc.Load(path + "\\data\\attendance.xml");
            }
            catch (System.Exception)
            {
                 return 404;//inconsistance xml file
                
            }
            DateTime todayDate = date;
            XmlNode yearnode = activDoc.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']");
            if (yearnode == null)
            {
                XPathNavigator nav = activDoc.CreateNavigator();
                nav.MoveToChild("attendancerecords", "");
                nav.AppendChild("<year value=\"" + todayDate.Year.ToString() + "\"><month value=\"" + todayDate.Month.ToString() + "\" status=\"active\"> <day value=\"" + todayDate.Day.ToString() + "\"><employee id=\"" + EID + "\">" + attentype + "</employee> </day></month></year>");
                activDoc.Save(path + "//data//attendance.xml");
                return 100;  // success
            }
            else
            {
                XmlNode monthnode = activDoc.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']/month[@value='" + todayDate.Month.ToString() + "']");
                if (monthnode == null)
                {
                    XPathNavigator nav = activDoc.CreateNavigator();
                    XPathNavigator map = nav.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']");
                    map.PrependChild("<month value=\"" + todayDate.Month.ToString() + "\" status=\"active\"> <day value=\"" + todayDate.Day.ToString() + "\"><employee id=\"" + EID + "\">" + attentype + "</employee> </day></month>");
                    activDoc.Save(path + "//data//attendance.xml");
                    return 100; // success
                }
                else
                {
                    XmlNode daynode = activDoc.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']/month[@value='" + todayDate.Month.ToString() + "']/day[@value='" + todayDate.Day.ToString() + "']");
                    if (daynode == null)
                    {
                        XPathNavigator nav = activDoc.CreateNavigator();
                        XPathNavigator map = nav.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']/month[@value='" + todayDate.Month.ToString() + "']");
                        map.PrependChild("<day value=\"" + todayDate.Day.ToString() + "\"><employee id=\"" + EID + "\">" + attentype + "</employee> </day>");
                        activDoc.Save(path + "//data//attendance.xml");
                        return 100; // success
                    }
                    else
                    {
                        XPathNavigator nav = activDoc.CreateNavigator();
                        XmlNode presentcheck = activDoc.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']/month[@value='" + todayDate.Month.ToString() + "']/day[@value='" + todayDate.Day.ToString() + "']/employee[@id='" + EID + "']");
                        if (presentcheck == null)
                        {
                            XPathNavigator map = nav.SelectSingleNode("attendancerecords/year[@value='" + todayDate.Year.ToString() + "']/month[@value='" + todayDate.Month.ToString() + "']/day[@value='" + todayDate.Day.ToString() + "']");
                            map.PrependChild("<employee id=\"" + EID + "\">" + attentype + "</employee>");
                            activDoc.Save(path + "//data//attendance.xml");
                            return 100; // success
                        }
                        else
                            return 700; // already marked
                    }
                }
            }    
        }
       
    }
}
 