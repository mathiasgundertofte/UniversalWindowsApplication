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
        private string username, password;
        private bool loginSuccesfully;
        private int index;
        private Popup popup;
        private ValidateUserData validate;
        private SerialKey serial;
        private UserData userdata;
        private DataPersistance data;
        private List<UserData> userdataList;
        private List<String> keysList;
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
        private void submission_button_Click(object sender, RoutedEventArgs e)
        {
            if (validate.CheckIfValidUserdata(keysList, firstNameTextBox, lastNameTextBox, emailTextBox, phoneTextBox, 
                datePicker, serialNumberTextBox, serial))
            {
                userdata = new UserData(firstNameTextBox.Text, lastNameTextBox.Text, emailTextBox.Text, 
                                        phoneTextBox.Text, datePicker.Date.ToString(), serialNumberTextBox.Text);
                popup.PrintSuccessfulUserdataMessage(userdata);
                SaveUserdataToXML(userdata);
                ClearTextBoxes();
                //RemoveSerialKey();
            }
        }

        private void DeserializeKeys()
        {
            keysList = data.DeserialiseListFromXML(keysList, "keys");
        }

        private void SaveSerialKeysToXML()
        {
            keysList = new List<String>();
            serial.GenerateMultipleKeys(keysList, 100);
            data.SerializeList(keysList, "keys");
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
                foreach (String key in keysList)
                {
                    listView.Items.Add(key);
                }
            }

            catch(Exception e)
            {
                popup.DisplayMessage("No keys found. Please Generate keys.");
            }
            
        }

        //When a key is selected in the listview it is inserted into the serial number textbox
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = listView.SelectedIndex;
            
            serialNumberTextBox.Text = keysList.ElementAt(index);
        }

        
        private void SwitchViewToSecondPage()
        {
            Frame.Navigate(typeof(SecondPage));
        }


        //Changes the view to Second Page
        private void view_submission_button_Click(object sender, RoutedEventArgs e)
        {
            popup.ShowPopUp(loginDialog);
            //loginDialog.PrimaryButtonClick += LoginDialog_PrimaryButtonClick;
                
            
        }

        //When the popup button "Login" is pressed
        //private void LoginDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        //{
        //    adminList = data.DeserialiseListFromXML(adminList, "admin");

        //    foreach (Admin adm in adminList)
        //    {

        //        if (adm.Username.Equals(AdminUsername) && adm.Password.Equals(AdminPassword))
        //        {
        //            loginSuccesfully = true;
        //            popup.DisplayMessage("Login succesful!");
        //        }
        //    }

        //    if (loginSuccesfully)
        //    {
        //        SwitchViewToSecondPage();
        //    }
        //}

        

        private void ClearTextBoxes()
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            phoneTextBox.Text = "";
            serialNumberTextBox.Text = "";
        }

        private void RemoveSerialKey()
        {
            keysList.ElementAt(index).Remove(index);
            listView.Items.RemoveAt(index);
        }

        private void Generate_keys_button_Click(object sender, RoutedEventArgs e)
        {
            SaveSerialKeysToXML();
            popup.DisplayMessage("100 keys very succesfully generated and stored in keys.xml \nClick Load Keys to add them to the list and select a key to add it to the submission");
            
        }

        private void Load_keys_button_Click(object sender, RoutedEventArgs e)
        {
            DeserializeKeys();
            AddKeysToListView();
        }

       

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            popup.ShowPopUp(Admin_Dialog);

        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {

        //    adminList = data.DeserialiseListFromXML(adminList, "admin");

        //    foreach(Admin adm in adminList) {
                

        //        if (adm.Username.Equals(AdminUsername) && adm.Password.Equals(AdminPassword))
        //        {
        //            loginSuccesfully = true;
        //        }

        //    }

        //    textBlock1.Text = loginSuccesfully.ToString();
        }

        private void Admin_Dialog_CreateButton_click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            username = AdminUsername.Text;
            password = AdminPassword.Text;
           
            adminList.Add(new Admin(username, password));
            data.SerializeList(adminList, "admin");
        }

        private void LoginButtonClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            adminList = data.DeserialiseListFromXML(adminList, "admin");

            foreach (Admin adm in adminList)
            {

                if (adm.Username == Username.Text && adm.Password == Password.Text)
                {
                    loginSuccesfully = true;
                    popup.DisplayMessage("Login succesful!");
                }

                else
                {
                    loginSuccesfully = false;
                    popup.DisplayMessage("Wrong username or password");
                }
            }

            if (loginSuccesfully)
            {
                SwitchViewToSecondPage();
            }
        }
    }
}
