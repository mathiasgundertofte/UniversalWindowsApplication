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
        //private List<UserData> userdata = new List<UserData>();


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

        public async void WriteObjecToXML(object obj, Type type, string filename)
        {
            StorageFolder localFolderPath = ApplicationData.Current.LocalFolder;

            await Task.Run(() =>
            {

                using (var stream = new FileStream(localFolderPath.Path + @"\" + filename + ".xml", FileMode.Create))
                {
                    var xml = new XmlSerializer(type);
                    xml.Serialize(stream, obj);

                }
            });
        }



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


        //Test

        //public async void TestAsync(Object obj, Type type, string filename, string firstName, string lastName, string email, string phone, string birthday, string serialID)
        //{
        //    StorageFolder path = ApplicationData.Current.LocalFolder;
        //    string fullpath = path.Path + @"\" + filename + ".xml";


        //    //Check if file exists - create file if it does not.
        //    if (!File.Exists(path.Path + @"\" + filename + ".xml"))
        //    {
        //        await Task.Run(() =>
        //        {

        //            using (var stream = new FileStream(path.Path + @"\" + filename + ".xml", FileMode.Create))
        //            {
        //                var doc = new XmlSerializer(type);
        //                doc.Serialize(stream, obj);
        //            }
        //        });
        //    }

        //    else
        //    {
        //        XDocument xml = XDocument.Load(fullpath);
        //        var newElement =
        //        new XElement("first_name", firstName,
        //        new XElement("last_name", lastName),
        //        new XElement("email", email),
        //        new XElement("phone", phone),
        //        new XElement("birthday", birthday),
        //        new XElement("serial_key", serialID));

        //        xml.Element("userdata").Add(newElement);

        //        using (var stream = new FileStream(fullpath, FileMode.Create))
        //        {
        //            xml.Save(stream);
        //        }
        //    }
        //}

        //USING LINQ
        public async void WriteUserdataToXML(string filename, Type type, object obj, string firstName, string lastName, string email, string phone, string birthday, string serialID)
        {

            StorageFolder path = ApplicationData.Current.LocalFolder;

            //Check if file exists - create file if it does not.
            if (!File.Exists(path.Path + @"\" + filename + ".xml"))
            {

                await Task.Run(() =>
                {

                    using (var stream = new FileStream(path.Path + @"\" + filename + ".xml", FileMode.Create))
                    {
                        var xml = new XmlSerializer(type);

                        //removes name spacing
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");

                        //serializes object
                        xml.Serialize(stream, obj, ns);
                    }
                });
            }


            //If the file already exists - append data to the existing file
            else
            {
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
        }


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
}

