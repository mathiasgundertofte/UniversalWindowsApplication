using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace ClassLibrary
{
    public class ValidateUserData
    {
        private Popup popup = new Popup();

        //Checks if the userdata has the correct data
        public bool CheckIfValidUserdata(List<String> list, TextBox firstNameTextBox, TextBox lastNameTextBox, TextBox emailTextBox,
            TextBox phoneTextBox, DatePicker datePicker, TextBox serialNumberTextBox, SerialKey serial)
        {

            if (String.IsNullOrEmpty(firstNameTextBox.Text))
            {
                popup.DisplayMessage("You need to enter a first name");
                return false;
            }

            if (String.IsNullOrEmpty(lastNameTextBox.Text))
            {
                popup.DisplayMessage("You need to enter a last name");
                return false;
            }

            if (String.IsNullOrEmpty(emailTextBox.Text))
            {
                popup.DisplayMessage("You need to enter an e-mail address");
                return false;
            }

            if (String.IsNullOrEmpty(phoneTextBox.Text))
            {
                popup.DisplayMessage("You need to enter a phone number");
                return false;
            }

            int error = Regex.Matches(phoneTextBox.Text, @"[a-zA-Z]").Count;
            if (error > 0)
            {
                popup.DisplayMessage("You cant enter letters in the phone number");
                return false;
            }

            if (String.IsNullOrEmpty(datePicker.Date.ToString()))
            {
                popup.DisplayMessage("You need to select a date");
                return false;
            }

            if (String.IsNullOrEmpty(serialNumberTextBox.Text))
            {
                popup.DisplayMessage("You need select a Serial Number from the list");
                return false;
            }

            if (!list.Contains(serialNumberTextBox.Text))
            {
                popup.DisplayMessage("The inserted key does not exist in the system. Check for spelling errors.");
                return false;
            }

            return true;
        }

    }
}

