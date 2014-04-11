/**
 *
 * Author   :   Sumit Nalawade
 * Created  :   26/07/2012
 * Modified :   27/07/2012
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TrialFront
{
    class PayrollValidator
    {
        private XmlDocument doc = new XmlDocument();
        private String path;

        public PayrollValidator()
        /**
         *
         * Constructor for initiation of variables.
         * 
         */
        {
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path = path.Substring(0, path.LastIndexOf("\\", path.LastIndexOf("\\") - 1));
        }
        
        public int validateLogin(String uid, String pass)
        /**
         * 
         * Function for validating user login. Returns int code for success or failure
         * uses 'validate.xml'
         * 
         */
        {
            doc.Load(path+"\\data\\validate.xml");
            XmlNode node = doc.SelectSingleNode("passwords/employee[@id='"+uid+"']");
            if (node != null && pass == node.InnerText)
            {
                String type = node.Attributes["type"].Value;
                if (type == "admin")
                    return 100; // Success: admin
                else
                    return 200; // Success: user
            }
            else
            {
                return 300; // Error: Access Denied
            }
        }

        public int changePassword(String uid, String newPass)
        /**
         * 
         * Function for changing user password. Returns int code indicating success or failure.
         * uses and modifies 'validate.xml'
         * 
         */
        {
            doc.Load(path+"\\data\\validate.xml");
            XmlNode node = doc.SelectSingleNode("passwords/employee[@id='" + uid + "']");
            if (node != null)
            {
                node.InnerText = newPass;
                doc.Save(path + "\\data\\validate.xml");
                return 100; // Success: password changed
            }
            else
                return 200; // Error: Invalid user id
        }
    }
}