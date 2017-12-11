using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace UWP
{

    public sealed partial class SecondPage : Page
    {

        private List<UserData> userdataList;
        private List<Submission> submissionList;
        private DataPersistance data;

        public SecondPage()
        {
            this.InitializeComponent();
            submissionList = new List<Submission>();
            userdataList = new List<UserData>();
            data = new DataPersistance();
            LoadUserdataFromXML();
            AddSubmissionsToList();
            AddSubmissionsToListView();
        }

        //Creates submission objects and adds them to the submission list
        private void AddSubmissionsToList()
        {
            for (int i = 0; i < userdataList.Count; i++)
            {
                Submission submission = new Submission("Submission " + (i + 1), userdataList.ElementAt(i));
                submissionList.Add(submission);
            }
        }

        private void LoadUserdataFromXML()
        {

            //userdataList = data.DeserialiseListFromXML(userdataList, "data");
            userdataList = data.ReadUserDataFromXML("data");
        }

        //Adds the userdata of the selected submission to the list view
        private void FillUserDataListView()
        {
            int index = listView_submissions.SelectedIndex;

            if (userdataList.Count > 0)
            {
                listView_userdata.Items.Clear();
                listView_userdata.Items.Add("First Name: " + userdataList.ElementAt(index).FirstName);
                listView_userdata.Items.Add("Last Name: " + userdataList.ElementAt(index).LastName);
                listView_userdata.Items.Add("Phone number: " + userdataList.ElementAt(index).Phone);
                listView_userdata.Items.Add("E-mail: " + userdataList.ElementAt(index).Email);
                listView_userdata.Items.Add("Birthday: " + userdataList.ElementAt(index).Birthday);
                listView_userdata.Items.Add("Unique key: " + userdataList.ElementAt(index).SerialNumber);
            }
        }

        //Changes the view to Main Page
        public void SwitchViewToMainPage()
        {
            Frame.Navigate(typeof(MainPage));
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SwitchViewToMainPage();
        }

        //Adds every submission to the list view
        private void AddSubmissionsToListView()
        {
                foreach(Submission sub in submissionList) {
                
                listView_submissions.Items.Add(sub.Name);
            } 
        }

        //Adds the userdata to the listview when a submission is selected
        private void listView_submissions_itemclicked(object sender, SelectionChangedEventArgs e)
        {
            
            FillUserDataListView();
        }
    }
}
