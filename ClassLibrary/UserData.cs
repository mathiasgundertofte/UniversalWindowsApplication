using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace ClassLibrary
{
    
    public class UserData
    {
        private string firstName, lastName, email, phone, birthday, serialNumber;

        //Empty constructor is mandatory in order to Serialize the object
        public UserData()
        {

        }
        
        public UserData(string firstName, string lastName, string email, string phone, string birthday, string serialNumber)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phone = phone;
            this.birthday = birthday;
            this.serialNumber = serialNumber;
        }

        //Getters & Setters
        [XmlElement("first_name")]
        public string FirstName { get => firstName; set => firstName = value; }

        [XmlElement("last_name")]
        public string LastName { get => lastName; set => lastName = value; }

        [XmlElement("email")]
        public string Email { get => email; set => email = value; }

        [XmlElement("phone")]
        public string Phone { get => phone; set => phone = value; }

        [XmlElement("birthday")]
        public string Birthday { get => birthday; set => birthday = value; }

        [XmlElement("serial_number")]
        public string SerialNumber { get => serialNumber; set => serialNumber = value; }
    }
}
