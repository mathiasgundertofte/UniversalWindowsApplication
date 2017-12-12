using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Popups;

namespace ClassLibrary
{
    public class DataPersistance
    {

        //Serializes a list and saves it as an xml file
        public async void SerializeList<T>(List<T> list, string filename)
        {
            StorageFolder localFolderPath = ApplicationData.Current.LocalFolder;

            await Task.Run(() =>
            {

                using (var stream = new FileStream(localFolderPath.Path + @"\" + filename + ".xml", FileMode.Create))
                {
                    var xml = new XmlSerializer(typeof(List<T>));
                    xml.Serialize(stream, list);
                }
            });
        }

        //Deserializes a list from a given xml file
        public List<T> DeserialiseListFromXML<T>(List<T> list, string filename)
        {

            StorageFolder path = ApplicationData.Current.LocalFolder;

            try
            {
                using (var stream = new FileStream(path.Path + @"\" + filename + ".xml", FileMode.Open))
                {
                    var xml = new XmlSerializer(typeof(List<T>));

                    list = (List<T>)xml.Deserialize(stream);

                }
            }

            catch (Exception e)
            {
                Debug.Write(e);
            }

            return list;
        }


        //returns an Admin object from the XML file
        public Admin GetAdminObjectFromXML(string filename)
        {

            StorageFolder path = ApplicationData.Current.LocalFolder;

            try
            {
                using (var stream = new FileStream(path.Path + @"\" + filename + ".xml", FileMode.Open))
                {
                    var xml = new XmlSerializer(typeof(Admin));

                    return (Admin)xml.Deserialize(stream);
                }
            }

            catch (Exception e)
            {

            }

            return null;
        }



        //Writes the UserData to .xml using XmlSerializer to create the file and LINQ to append to it.
        public async void WriteUserdataToXML(string filename, Type type, object obj, string firstName, string lastName, string email, string phone, string birthday, string serialID)
        {

            StorageFolder path = ApplicationData.Current.LocalFolder;
            string fullpath = path.Path + @"\" + filename + ".xml";

            //Check if file exists - create file if it does not.
            if (!File.Exists(fullpath))
            {

                await Task.Run(() =>
                {

                    using (var stream = new FileStream(path.Path + @"\" + filename + ".xml", FileMode.Create))
                    {
                        var xmlSerializer = new XmlSerializer(type);

                        //removes name spacing
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        //serializes object
                        xmlSerializer.Serialize(stream, obj, ns);
                    }
                });
            }


            //If the file already exists - append data to the existing file 
            //This will add the first object twice, but it will only be read out once as the first is not listed as "UserData"
            //should probably be fixed
            XElement xml = XElement.Load(path.Path + @"\" + filename + ".xml");
            

                xml.Add(
                    new XElement("UserData",
                    new XElement("first_name", firstName),
                    new XElement("last_name", lastName),
                    new XElement("email", email),
                    new XElement("phone", phone),
                    new XElement("birthday", birthday),
                    new XElement("serial_number", serialID)
                    ));

                using (var stream = new FileStream(path.Path + @"\" + filename + ".xml", FileMode.Create))
                {
                    xml.Save(stream);
                }
        }


        //Read the UserData in from xml file and store it as a list
        public List<UserData> ReadUserDataFromXML(string filename)
        {
            List<UserData> list = new List<UserData>();
            StorageFolder path = ApplicationData.Current.LocalFolder;
            string fullpath = path.Path + @"\" + filename + ".xml";


            try
            {
                XElement xml = XElement.Load(fullpath);

                list = (from data in xml.Elements("UserData")
                        select new UserData()
                        {

                            FirstName = (string)data.Element("first_name").Value,
                            LastName = (string)data.Element("last_name").Value,
                            Email = (string)data.Element("email").Value,
                            Phone = (string)data.Element("phone").Value,
                            Birthday = (string)data.Element("birthday").Value,
                            SerialNumber = (string)data.Element("serial_number").Value
                        }).ToList();
            }
            catch (Exception e)
            {

                Debug.Write(e);
            }



            return list;
        }

    }


    //UNUSED - could prove helpful later.

    //public async void WriteObjecToXML(object obj, Type type, string filename)
    //{
    //    StorageFolder localFolderPath = ApplicationData.Current.LocalFolder;

    //    await Task.Run(() =>
    //    {

    //        using (var stream = new FileStream(localFolderPath.Path + @"\" + filename + ".xml", FileMode.Create))
    //        {
    //            var xml = new XmlSerializer(type);
    //            xml.Serialize(stream, obj);

    //        }
    //    });
    //}
}

