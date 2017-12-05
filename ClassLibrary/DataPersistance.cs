using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Popups;

namespace ClassLibrary
{
    public class DataPersistance
    {
        private List<UserData> userdata = new List<UserData>();


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
                var dialog = new MessageDialog("Something is wrong with the .xml document or the xml document is empty");
                dialog.ShowAsync();
            }

            return list;
        }


        public async void WriteObjecToXML(object obj, Type type, string filename)
        {
            StorageFolder localFolderPath = ApplicationData.Current.LocalFolder;

            await Task.Run(() =>
            {

                using (var stream = new FileStream(localFolderPath.Path + @"\"+filename+".xml", FileMode.Append))
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
                using (var stream = new FileStream(path.Path + @"\"+filename+".xml", FileMode.Open))
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

    }
}

