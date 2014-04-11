/*
 * Author   :   Shikha Gadodiya
 * Created  :   26-07-2012
 * modified :   02-08-2012
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace TrialFront
{
    
    class Employee
        /*
         * Storing Employee details within function calls
         */
    {
        private String path;
        public String fname, mname, lname,id;
        public Char gender;
        public String mobile, email, bankaccount, designation, address, pf;
        public DateTime dob, doj;
        XmlDocument empdoc = new XmlDocument();
        public Employee()
        /**
         *
         * Constructor for initiation of variables.
         * 
         */
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.LastIndexOf("\\", path.LastIndexOf("\\") - 1));
        }
        public Employee(String fname, String mname, String lname, Char gender, DateTime dob, DateTime doj, String mobile, String email, String bankaccount, String designation, String address,String pf)
        /**
         *
         * Constructor for initiation of variables with the class.
         * 
         */
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.LastIndexOf("\\", path.LastIndexOf("\\") - 1));
            this.fname = fname;
            this.mname = mname;
            this.lname = lname;
            this.gender = gender;
            this.dob = dob;
            this.doj = doj;
            this.mobile = mobile;
            this.email = email;
            this.bankaccount = bankaccount;
            this.designation = designation;
            this.address = address;
            this.pf = pf;
        }
        public int addEmployee()
        /*
         * for adding new employee to file
         * uses annualleaves.xml,leavescheme.xml,employeeinfo.xml
         * returns employee_id for new employee using this object
         * returns 404 for error
         */
        {
            id = doj.Year.ToString() + doj.Month + RandomNumber(101, 999);
            EmployeeList e = new EmployeeList();
            String[] eid = e.retriveEIDs();
            int i = 0;
            while (i < eid.Length)
            {
                if (id.CompareTo(eid[i]) == 0)
                {
                    id = doj.Year.ToString() + doj.Month + RandomNumber(101, 999);
                    i = 0;
                }
                else
                    i++;
            }
            try
            {
                empdoc.Load(path + "\\Data\\employeeinfo.xml");
            }
            catch (Exception)
            {
                return 404; // inconsistence XML file
            }
            XmlNode node = empdoc.SelectSingleNode("employeerecords");
            XmlNode emp = empdoc.CreateNode(XmlNodeType.Element, "employee", "");
            XmlAttribute attid = empdoc.CreateAttribute("id");
            attid.Value = id.ToString();
            emp.Attributes.Append(attid);
            emp.AppendChild(createnode("fname", fname));
            emp.AppendChild(createnode("mname", mname));
            emp.AppendChild(createnode("lname", lname));
            emp.AppendChild(createnode("gender", gender + ""));
            XmlNode dobnode = empdoc.CreateNode(XmlNodeType.Element, "dob", "");
            dobnode.AppendChild(createnode("date", dob.Day.ToString()));
            dobnode.AppendChild(createnode("month", dob.Month.ToString()));
            dobnode.AppendChild(createnode("year", dob.Year.ToString()));
            emp.AppendChild(dobnode);
            XmlNode dojnode = empdoc.CreateNode(XmlNodeType.Element, "doj", "");
            dojnode.AppendChild(createnode("date", doj.Day.ToString()));
            dojnode.AppendChild(createnode("month", doj.Month.ToString()));
            dojnode.AppendChild(createnode("year", doj.Year.ToString()));
            emp.AppendChild(dojnode);
            emp.AppendChild(createnode("mobile", mobile));
            emp.AppendChild(createnode("email", email));
            emp.AppendChild(createnode("bankaccount", bankaccount));
            emp.AppendChild(createnode("designation", designation));
            emp.AppendChild(createnode("address", address));
            emp.AppendChild(createnode("pf", pf));
            node.AppendChild(emp);
            XmlDocument annualleavedoc = new XmlDocument();
            XmlDocument leaveschemadoc = new XmlDocument();
            XmlDocument validatedoc = new XmlDocument();
            try
            {
                annualleavedoc.Load(path + "\\Data\\annualleaves.xml");
                leaveschemadoc.Load(path + "\\Data\\leavescheme.xml");
                validatedoc.Load(path + "\\Data\\validate.xml");
                XmlNode leavenode = leaveschemadoc.SelectSingleNode("schemerecords/designation[@type='" + designation + "']");
                XmlNode annualleave = annualleavedoc.SelectSingleNode("leaverecords");
                annualleave.InnerXml = annualleave.InnerXml + "<employee id=\"" + id + "\">" + leavenode.InnerXml + "</employee>";
                XmlNode empvalidate = validatedoc.CreateNode(XmlNodeType.Element, "employee", "");
                XmlAttribute valid = validatedoc.CreateAttribute("id");
                valid.Value = id;
                XmlAttribute type = validatedoc.CreateAttribute("type");
                type.Value = "user";
                empvalidate.Attributes.Append(valid);
                empvalidate.Attributes.Append(type);
                empvalidate.InnerText = id;
                XmlNode validateput = validatedoc.SelectSingleNode("passwords");
                validateput.AppendChild(empvalidate);
                validatedoc.Save(path + "\\Data\\validate.xml");
                annualleavedoc.Save(path + "\\Data\\annualleaves.xml");
                empdoc.Save(path + "\\Data\\employeeinfo.xml");
            }
            catch (Exception)
            {
                return 404;
            }
            return int.Parse(id); // success
        }
        public int updateEmployee(String id, String fname, String mname, String lname, Char gender, DateTime dob, DateTime doj, String mobile, String email, String bankaccount, String designation, String address, String pf)
        /*
         * for adding updating employee info in file
         * uses employeeinfo.xml
         * parameters are new info of employee
         * returns 100 for success update
         * returns 200 if employee_id not found
         * returns 404 for error
         */
        
        {
            this.id = id;
            this.fname = fname;
            this.mname = mname;
            this.lname = lname;
            this.gender = gender;
            this.dob = dob;
            this.doj = doj;
            this.mobile = mobile;
            this.email = email;
            this.bankaccount = bankaccount;
            this.designation = designation;
            this.address = address;
            this.pf = pf;
            try
            {
                empdoc.Load(path + "\\Data\\employeeinfo.xml");
            }
            catch (Exception)
            {
                return 404;
            }
            
            XmlNode emp = empdoc.SelectSingleNode("employeerecords/employee[@id='"+id+"']");
            if (emp != null)
            {
                emp.RemoveAll();
                XmlAttribute attid = empdoc.CreateAttribute("id");
                attid.Value = id.ToString();
                emp.Attributes.Append(attid);
                emp.AppendChild(createnode("fname", fname));
                emp.AppendChild(createnode("mname", mname));
                emp.AppendChild(createnode("lname", lname));
                emp.AppendChild(createnode("gender", gender + ""));
                XmlNode dobnode = empdoc.CreateNode(XmlNodeType.Element, "dob", "");
                dobnode.AppendChild(createnode("date", dob.Day.ToString()));
                dobnode.AppendChild(createnode("month", dob.Month.ToString()));
                dobnode.AppendChild(createnode("year", dob.Year.ToString()));
                emp.AppendChild(dobnode);
                XmlNode dojnode = empdoc.CreateNode(XmlNodeType.Element, "doj", "");
                dojnode.AppendChild(createnode("date", doj.Day.ToString()));
                dojnode.AppendChild(createnode("month", doj.Month.ToString()));
                dojnode.AppendChild(createnode("year", doj.Year.ToString()));
                emp.AppendChild(dojnode);
                emp.AppendChild(createnode("mobile", mobile));
                emp.AppendChild(createnode("email", email));
                emp.AppendChild(createnode("bankaccount", bankaccount));
                emp.AppendChild(createnode("designation", designation));
                emp.AppendChild(createnode("address", address));
                emp.AppendChild(createnode("pf", pf));
                Console.WriteLine(emp.InnerXml);
                try
                {
                    empdoc.Save(path + "\\Data\\employeeinfo.xml");
                }
                catch (Exception)
                {
                    return 404;
                }
                return 100; // success
            }
            else
            {
                return 200;// employee id not found
            }
        }
        public Employee retriveEmployee(String eid)
        {
            /*
             * for retriving employee info
             * uses employeeinfo.xml
             * parameters are employee_id for particular employee
             * returns Employee object for given employee_id
             * returns NULL for error
             */
            
            try
            {
                empdoc.Load(path + "\\Data\\employeeinfo.xml");
            }
            catch (Exception)
            {
                return null;
            }
            XmlNode emp = empdoc.SelectSingleNode("employeerecords/employee[@id='"+eid+"']");
            if (emp != null)
            {
                this.id = eid;
                XmlNodeList list= emp.ChildNodes;
                this.fname = list[0].InnerText;
                this.mname = list[1].InnerText;
                this.lname = list[2].InnerText;
                this.gender = list[3].InnerText[0];
                int day, month, year;
                day =int.Parse(list[4].ChildNodes[0].InnerText);
                month = int.Parse(list[4].ChildNodes[1].InnerText);
                year = int.Parse(list[4].ChildNodes[2].InnerText);
                DateTime localdob = new DateTime(year, month, day);
                this.dob = localdob;
                day = int.Parse(list[5].ChildNodes[0].InnerText);
                month = int.Parse(list[5].ChildNodes[1].InnerText);
                year = int.Parse(list[5].ChildNodes[2].InnerText);
                DateTime localdoj = new DateTime(year, month, day);
                this.doj = localdoj;
                this.mobile = list[6].InnerText;
                this.email = list[7].InnerText;
                this.bankaccount = list[8].InnerText;
                this.designation = list[9].InnerText;
                this.address = list[10].InnerText;
                this.pf = list[11].InnerText;
                return this;
            }
            else
                return null;
        }
        public int removeEmployee(String eid)
        /*
        * for removing employee info from file
        * uses employeeinfo.xml
        * parameters are employee_id
        * returns 100 for success removal
        * returns 200 if employee_id not found
        * returns 404 for error
        */
        {
            XmlDocument validatedoc=new XmlDocument();
            XmlDocument leavedoc =new XmlDocument();

            try
            {
                empdoc.Load(path + "\\Data\\employeeinfo.xml");
                validatedoc.Load(path + "\\Data\\validate.xml");
                leavedoc.Load(path + "\\Data\\annualleaves.xml");
            }
            catch (Exception)
            {
                return 404;
            }
            XmlNode emp = empdoc.SelectSingleNode("employeerecords/employee[@id='"+eid+"']");
            XmlNode leave = leavedoc.SelectSingleNode("leaverecords/employee[@id='" + eid + "']");
            XmlNode validte = validatedoc.SelectSingleNode("passwords/employee[@id='" + eid + "']");
            if (emp != null && leave!=null && validte !=null)
            {
                emp.ParentNode.RemoveChild(emp);
                leave.ParentNode.RemoveChild(leave);
                validte.ParentNode.RemoveChild(validte);
                try
                {
                    empdoc.Save(path + "\\Data\\employeeinfo.xml");
                    validatedoc.Save(path + "\\Data\\validate.xml");
                    leavedoc.Save(path + "\\Data\\annualleaves.xml");
                }
                catch (Exception)
                {
                    return 404;
                }
                return 100;
            }
            else
                return 200;// not found
        }
        private XmlNode createnode(String nodename, String nodetext)
            /*
             * internal use
             * for creating xmlnode element fast
             */
        {
            XmlNode data=empdoc.CreateNode(XmlNodeType.Element,nodename,"");
            data.InnerText = nodetext;
            return data;
        }
        private int RandomNumber(int min, int max)
        /*
         * internal use
         * generate random  number between two parameter
         */
        {
            Random random = new Random();
            return random.Next(min, max);
        }

    }
}
