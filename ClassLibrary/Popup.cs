using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace ClassLibrary
{
    public class Popup
    {

        public async void DisplayMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        public async Task ShowPopUp(ContentDialog dialog)
        {
            var popup = await dialog.ShowAsync();
        }

        //Prints a message of the entered userdata
        public void PrintSuccessfulUserdataMessage(UserData userdata)
        {
            DisplayMessage(
                    "Succes! \nYou entered: \nName: " + userdata.FirstName
                    + "\nLastname: " + userdata.LastName + "\nEmail: " + userdata.Email
                    + "\nPhone: " + userdata.Phone + "\nDate: " + userdata.Birthday
                    + "\nSerial Number: " + userdata.SerialNumber);
        }

    }
}
