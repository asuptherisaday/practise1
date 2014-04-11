/**
 *
 * Author   :   Sumit Nalawade
 * Created  :   26/07/2012
 * Modified :   31/07/2012
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TrialFront
{
    class PayrollReportGenerator
    {
        String path;
        private XmlDocument doc = new XmlDocument();

        public PayrollReportGenerator()
        /**
         *
         * Constructor for initiation of variables.
         * 
         */
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.LastIndexOf("\\", path.LastIndexOf("\\") - 1));
        }

        public String[] getDesignations()
        /**
         * 
         * Function for populating dropdown of update payroll scheme. Returns String array of designation names
         * uses 'payrollscheme.xml'
         * 
         */
        {
            doc.Load(path+"\\data\\payrollscheme.xml");
            XmlNodeList list = doc.GetElementsByTagName("designation");
            String[] desList = new String[list.Count];
            for (int i = 0; i < list.Count; i++)
                desList[i] = list[i].Attributes["type"].Value;
            return desList;
        }

        public String[] getPayrollScheme(String designation)
        /**
         * 
         * Function for populating textfiels of update payroll scheme. Returns String array of payroll scheme as per selected designation
         * uses 'payrollscheme.xml'
         * 
         */
        {
            doc.Load(path+"\\data\\payrollscheme.xml");
            XmlNode node = doc.SelectSingleNode("schemerecords/designation[@type='" + designation + "']");
            XmlNodeList list = node.ChildNodes;
            String[] scheme = new String[list.Count];
            for (int i = 0; i < list.Count; i++)
                scheme[i] = list[i].InnerText;
            return scheme;
        }

        public int updatePayrollScheme(String designation, String[] scheme)
        /**
         * 
         * Function for updating payroll scheme. Returns String array of payroll scheme as per selected designation
         * uses and modifies 'payrollscheme.xml'
         * 
         */
        {
            doc.Load(path + "\\data\\payrollscheme.xml");
            XmlNode node = doc.SelectSingleNode("schemerecords/designation[@type='" + designation + "']");
            if (node != null)
            {
                XmlNodeList list = node.ChildNodes;
                if (scheme.Length == list.Count)
                {
                    for (int i = 0; i < list.Count; i++)
                        list[i].InnerText = scheme[i];
                    doc.Save(path + "\\data\\payrollscheme.xml");
                    return 100; // Success
                }
                else
                    return 200; //Error : Invalid string array
            }
            else
                return 300; //Error : Invalid designation
        }

        public string[] calculatePayroll(String empid,DateTime date)
        /**
         * 
         * Function for calculating payroll upto current date. Returns String array of payroll for perticular employee.
         * uses 'attendance.xml', 'payrollscheme.xml', 'employeeinfo.xml', 'otrecords.xml'
         * 
         */
        {
            //Getting required data for calculation
            doc.Load(path + "\\data\\employeeinfo.xml");
            
            String designation = doc.SelectSingleNode("employeerecords/employee[@id='" + empid + "']/designation").InnerText;
            String pf = doc.SelectSingleNode("employeerecords/employee[@id='" + empid + "']/pf").InnerText;
            String[] scheme = getPayrollScheme(designation);

            doc.Load(path + "\\data\\otrecords.xml");

            String otHours = doc.SelectSingleNode("otrecords/employee[@id='" + empid + "']") == null ? "0" : doc.SelectSingleNode("otrecords/employee[@id='" + empid + "']").InnerText;

            doc.Load(path + "\\data\\attendance.xml");
            //String day = DateTime.Today.Day.ToString();
            String month = date.Month.ToString();
            String year = date.Year.ToString();

            //Processing attendance details
            int FD, HD, CL, SL, EL, AB;
            FD = HD = CL = SL = EL = AB = 0;

            XmlNode nodeActiveMonth;
            XmlNodeList entryList;
            try
            {
                nodeActiveMonth = doc.SelectSingleNode("attendancerecords/year[@value='" + year + "']/month[@value='" + month + "']");
                entryList = nodeActiveMonth.SelectNodes("day/employee[@id='" + empid + "']");
            }
            catch (Exception)
            {
                return new String[] { "NULL" };
            }
            foreach (XmlNode entryNode in entryList)
            {
                switch (entryNode.InnerText)
                {
                    case "FD": FD++;
                        break;
                    case "HD": HD++;
                        break;
                    case "CL": CL++;
                        break;
                    case "SL": SL++;
                        break;
                    case "EL": EL++;
                        break;
                    case "AB": AB++;
                        break;
                    default: Console.WriteLine("Invalid Entry");
                        break;
                }
            }

            double basic, da, hra, ma, ot, tax;
            basic = Double.Parse(scheme[0]);
            da = Double.Parse(scheme[1]);
            hra = Double.Parse(scheme[2]);
            ma = Double.Parse(scheme[3]);
            ot = Double.Parse(scheme[4]);
            tax = Double.Parse(scheme[5]);

            //Calculation Starts Here

            int workingDays = FD + HD + CL + SL + EL + AB;
            basic = basic - (basic / workingDays * AB);
            da = basic * da / 100;
            hra = basic * hra / 100;
            ma = basic * ma / 100;
            ot = ot * Double.Parse(otHours);
            double creditTotal = basic + da + hra + ma + ot;
            tax = creditTotal * tax / 100;
            double debitTotal = tax + Double.Parse(pf);
            double grossTotal = creditTotal - debitTotal;
            
            //See the order of return values here.

            String[] retString = { basic.ToString(), da.ToString(), hra.ToString(), ma.ToString(), ot.ToString(), tax.ToString(), pf, grossTotal.ToString(), FD.ToString(), HD.ToString(), CL.ToString(), SL.ToString(), EL.ToString(), AB.ToString()};

            return retString;
        }

        public int generateReport(String empid,DateTime date)
        /**
         * 
         * This function will store payroll report of perticular employee to a file as historical information.
         * Stores data in xml file as \reports\<year>-<month>-<empid>.xml
         * 
         */
        {
            String year = date.Year.ToString();
            String month = date.Month.ToString();
            String day = date.Day.ToString();
            
            String[] payrollInfo = calculatePayroll(empid,date);
            if (payrollInfo[0] == "NULL") return 200;//Error
            updatePfrecord(empid, payrollInfo[6]);
            resetOtrecords(empid);
            //String[] employeeInfo = new PayrollEmployee().getEmpInfo(empid); Shikha is gonna give me these values
            
            XmlTextWriter writer = new XmlTextWriter(path + "\\data\\reports\\"+year+"-"+month+"-"+empid+".xml", Encoding.UTF8);
            
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("info");
            writer.WriteElementString("date", day+"-"+month+"-"+year);
            
            //employee deatils to write here

            writer.WriteElementString("basic",payrollInfo[0]);
            writer.WriteElementString("da", payrollInfo[1]);
            writer.WriteElementString("hra", payrollInfo[2]);
            writer.WriteElementString("ma", payrollInfo[3]);
            writer.WriteElementString("ot", payrollInfo[4]);
            writer.WriteElementString("tax", payrollInfo[5]);
            writer.WriteElementString("pf", payrollInfo[6]);
            writer.WriteElementString("total", payrollInfo[7]);
            writer.WriteElementString("fd", payrollInfo[8]);
            writer.WriteElementString("hd", payrollInfo[9]);
            writer.WriteElementString("cl", payrollInfo[10]);
            writer.WriteElementString("sl", payrollInfo[11]);
            writer.WriteElementString("el", payrollInfo[12]);
            writer.WriteElementString("ab", payrollInfo[13]);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return 100;
        }

        public String[] showReport(String year, String month, String empid)
        /**
         * 
         * This function returns all data about perticular report.
         * 
         */
        {
            try
            {
                doc.Load(path + "\\data\\reports\\" + year + "-" + month + "-" + empid + ".xml");
            }
            catch (Exception)
            {

                return new String[]{"NULL"};
            }
            XmlNode root = doc.SelectSingleNode("info");
            XmlNodeList list = root.ChildNodes;
            String[] retString = new String[list.Count];
            for(int i = 0; i<list.Count;i++)
            {
                retString[i] = list[i].InnerText;
            }
            return retString;
        }

        public void updatePfrecord(String empid, String amount)
        {
            doc.Load(path + "\\data\\pfrecords.xml");
            XmlNode node = doc.SelectSingleNode("pfrecords/employee[@id='"+empid+"']");
            if (node != null)
            {
                int existingPf = int.Parse(node.InnerText);
                node.InnerText = (existingPf + int.Parse(amount)).ToString();
                doc.Save(path + "\\data\\pfrecords.xml");
            }
            else
            {
                addPfrecord(empid);
                updatePfrecord(empid,amount);
            }    
        }

        public void addPfrecord(String empid)
        {
            doc.Load(path + "\\data\\pfrecords.xml");
            XmlNode node = doc.SelectSingleNode("pfrecords");
            XmlNode entry = doc.CreateNode(XmlNodeType.Element, "employee", "");
            entry.InnerText = "0";
            XmlAttribute attrib = doc.CreateAttribute("id");
            attrib.Value = empid;
            entry.Attributes.Append(attrib);
            node.AppendChild(entry);
            doc.Save(path + "\\data\\pfrecords.xml");
        }

        public void resetOtrecords(String empid)
        {
            doc.Load(path + "\\data\\otrecords.xml");
            XmlNode node = doc.SelectSingleNode("otrecords/employee[@id='" + empid + "']");
            if (node != null)
            {
                node.InnerText = "0";
                doc.Save(path + "\\data\\otrecords.xml");
            }
        }
    }
}
