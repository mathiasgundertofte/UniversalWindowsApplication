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
        private Random random;

        public SecondPage()
        {
            this.InitializeComponent();
            submissionList = new List<Submission>();
            userdataList = new List<UserData>();
            data = new DataPersistance();
            random = new Random();
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

        //Draws a winner from the submission
        private void DrawWinner()
        {
            int winnerIndex = random.Next(0, userdataList.Count);
            
            FillListViewWithUserData(listView_Winner, winnerIndex);
        }

        private void LoadUserdataFromXML()
        {
            userdataList = data.ReadUserDataFromXML("data");
        }

        //Adds the userdata of the selected submission to the list view
        private void FillListViewWithUserData(ListView listview, int index)
        {
            if (userdataList.Count > 0)
            {
                listview.Items.Clear();
                listview.Items.Add("First Name: " + userdataList.ElementAt(index).FirstName);
                listview.Items.Add("Last Name: " + userdataList.ElementAt(index).LastName);
                listview.Items.Add("Phone number: " + userdataList.ElementAt(index).Phone);
                listview.Items.Add("E-mail: " + userdataList.ElementAt(index).Email);
                listview.Items.Add("Birthday: " + userdataList.ElementAt(index).Birthday);
                listview.Items.Add("Unique key: " + userdataList.ElementAt(index).SerialNumber);
            }
        }

        //Changes the view to Main Page
        public void SwitchViewToMainPage()
        {
            Frame.Navigate(typeof(MainPage));
        }


        private void ReturnToMainPage_Button(object sender, RoutedEventArgs e)
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
            int index = listView_submissions.SelectedIndex;
            FillListViewWithUserData(view, index);
        }

        private void DrawWinner_Button_Click(object sender, RoutedEventArgs e)
        {
            DrawWinner();
        }
    }
}
