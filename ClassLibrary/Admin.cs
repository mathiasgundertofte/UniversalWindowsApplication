using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClassLibrary
{
    public class Admin
    {
        [XmlElement]
        private string username;
        [XmlElement]
        private string password;

        public Admin()
        {

        }

        public Admin(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
