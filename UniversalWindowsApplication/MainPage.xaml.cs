using ClassLibrary;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.Storage;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace UWP
{

    public sealed partial class MainPage : Page
    {
        private string enteredUsername, enteredPassword;
        private bool loginSuccesfully;
        private int index;
        private Popup popup;
        private ValidateUserData validate;
        private SerialKey serial;
        private UserData userdata;
        private DataPersistance data;
        private List<UserData> userdataList;
        private List<String> availableKeysList, usedKeysList;
        private List<Admin> adminList;

        public MainPage()
        {
            this.InitializeComponent();

            //Keeps data in listview etc. when changing back and fourth in pages
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Initialize();
        }

        //Initializes objects, generates keys and adds to listview
        private void Initialize()
        {
            userdataList = new List<UserData>();
            serial = new SerialKey();
            data = new DataPersistance();
            validate = new ValidateUserData();
            adminList = new List<Admin>();
            usedKeysList = new List<String>();
            popup = new Popup();
            loginSuccesfully = false;
        }

        //Serializes the userdata list to an xml document
        private void SaveUserdataToXML(UserData data)
        {
            userdataList.Add(data);
            this.data.SerializeList(userdataList, "data");
        }


        //Creates a new Userdata object when button is clicked
        private void Submission_Button_Click(object sender, RoutedEventArgs e)
        {

            //Checks if data entered is correct
            if (validate.CheckIfValidUserdata(availableKeysList, usedKeysList, firstNameTextBox, lastNameTextBox, 
                emailTextBox, phoneTextBox, datePicker, serialNumberTextBox, serial))
            {
                userdata = new UserData(firstNameTextBox.Text, lastNameTextBox.Text, emailTextBox.Text,
                                        phoneTextBox.Text, datePicker.Date.ToString(), serialNumberTextBox.Text);

                //Adds used key to keep track if available keys
                usedKeysList.Add(userdata.SerialNumber);
                

                //Prints a success message
                popup.PrintSuccessfulUserdataMessage(userdata);

                //SaveUserdataToXML(userdata);
                data.WriteUserdataToXML("data", typeof(UserData), userdata, userdata.FirstName, userdata.LastName,
                userdata.Email, userdata.Phone, userdata.Birthday, userdata.SerialNumber);



                ClearTextBoxes();
                //RemoveSerialKey();
            }
        }

        private void DeserializeKeys()
        {
            availableKeysList = data.DeserialiseListFromXML(availableKeysList, "keys");
        }

        private void SaveSerialKeysToXML()
        {
            availableKeysList = new List<String>();
            serial.GenerateMultipleKeys(availableKeysList, 100);
            data.SerializeList(availableKeysList, "keys");
        }

        //Adds all the serial keys to the listview
        private void AddKeysToListView()
        {
            if (listView.Items.Count > 0)
            {
                listView.Items.Clear();
            }

            try
            {
                foreach (String key in availableKeysList)
                {
                    listView.Items.Add(key);
                }
            }

            catch (Exception e)
            {
                popup.DisplayMessage("No keys found. Please Generate keys.");
            }

        }

        //When a key is selected in the listview it is inserted into the serial number textbox
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = listView.SelectedIndex;
            serialNumberTextBox.Text = availableKeysList.ElementAt(index);
        }


        private void SwitchViewToSecondPage()
        {
            Frame.Navigate(typeof(SecondPage));
        }


        //Changes the view to Second Page
        private void View_Submission_Button_Click(object sender, RoutedEventArgs e)
        {
            popup.ShowPopUp(loginDialog);
        }

        private void ClearTextBoxes()
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";
            serialNumberTextBox.Text = "";
        }

        private void Generate_Keys_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSerialKeysToXML();
            popup.DisplayMessage("100 keys very succesfully generated and stored in keys.xml \nClick Load Keys to add them to the list and select a key to add it to the submission");

        }

        private void Load_Keys_Button_Click(object sender, RoutedEventArgs e)
        {
            DeserializeKeys();
            AddKeysToListView();
        }



        private void CreateAdminProfile_Button_Click(object sender, RoutedEventArgs e)
        {
            popup.ShowPopUp(Admin_Dialog);

        }


        private void Admin_Dialog_CreateButton_click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            enteredUsername = AdminUsername.Text;
            enteredPassword = AdminPassword.Text;

            if (String.IsNullOrEmpty(enteredUsername) || String.IsNullOrEmpty(enteredPassword))
            {
                popup.DisplayMessage("You need to enter a username and a password");
            }

            else
            {
                adminList.Add(new Admin(enteredUsername, enteredPassword));
                data.SerializeList(adminList, "admin");
            }
        }



        private void LoginButtonClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            adminList = data.DeserialiseListFromXML(adminList, "admin");

            if (adminList.Count == 0)
            {
                popup.DisplayMessage("You need to create an admin profile");
            }

            foreach (Admin adm in adminList)
            {

                if (adm.Username == Username.Text && adm.Password == Password.Text)
                {
                    loginSuccesfully = true;
                    popup.DisplayMessage("Login succesful!");
                }

                else if (String.IsNullOrEmpty(Username.Text) || String.IsNullOrEmpty(Password.Text))
                {
                    popup.DisplayMessage("You need to enter a username and a password");
                }

                else
                {
                    loginSuccesfully = false;
                    popup.DisplayMessage("Wrong username or password.\nYou can create an admin profile by clicking on 'Create Admin Profile'");
                }
            }

            if (loginSuccesfully)
            {
                SwitchViewToSecondPage();
            }
        }
    }
}

