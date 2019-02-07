using desktopapplication.Model;
using desktopapplication.Model.objects;
using desktopapplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace desktopapplication.ViewModel
{
    class Vista1ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand homeClickCommand { get; set; }
        public ICommand usersClickCommand { get; set; }
        public ICommand createAnnouncementCommand { get; set; }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Vista1ViewModel()
        {


            homeClickCommand = new RelayCommand(x => selectHome());
            usersClickCommand = new RelayCommand(x => selectUsers());

            populateUsers();

            populateRequestors();
            populateAnnouncements();
        }

        //tab Not used
        private Visibility _homeSelected;
        public Visibility HomeSelected
        {
            get { return _homeSelected; }
            set
            {
                _homeSelected = value;
                NotifyPropertyChanged();

            }
        }





        //tab Donors
        private List<Donor> _donors;
        public List<Donor> Donors
        {
            get { return _donors; }
            set { _donors = value; NotifyPropertyChanged(); }
        }

        private void populateUsers()
        {
            Donors = new List<Donor>();
            Donors = donorRepository.getAllDonors();
        }


        //tab Recipient

        private ObservableCollection<Recipient> _recipients;

        public ObservableCollection<Recipient> Recipients
        {
            get { return _recipients; }
            set { _recipients = value; NotifyPropertyChanged(); }
        }


        //tab Announcements
        private List<Announcement> _announcements;
        public List<Announcement> Announcements
        {
            get { return _announcements; }
            set { _announcements = value; NotifyPropertyChanged(); }
        }

        private string _announcementTitle;
        public string AnnouncementTitle
        {
            get { return _announcementTitle; }
            set { _announcementTitle = value; NotifyPropertyChanged(); }
        }

        private string _announcementMessage;
        public string AnnouncementMessage
        {
            get { return _announcementMessage; }
            set { _announcementMessage = value; NotifyPropertyChanged(); }
        }

        private string _announcementLanguage;
        public string AnnouncementLanguage
        {
            get { return _announcementLanguage; }
            set { _announcementLanguage = value; NotifyPropertyChanged(); }
        }

        private Recipient _announcementRecipient;
        public Recipient AnnouncementRecipient
        {
            get { return _announcementRecipient; }
            set { _announcementRecipient = value; NotifyPropertyChanged(); }
        }



        private void populateAnnouncements()
        {
            Announcements = new List<Announcement>();
            Announcements = announcementRepository.getAllAnnouncements();

        }
        private void createAnnouncement()
        {

        }


        //Tab tabItems
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                NotifyPropertyChanged();
            }
        }
        private void selectHome()
        {
            SelectedTab = 1;
            //HomeSelected = Visibility.Visible;
            //  UsersSelected = Visibility.Hidden;
            Console.WriteLine("HOME SELECTED");
        }

        private void selectUsers()
        {
            SelectedTab = 0;
            // UsersSelected = Visibility.Visible;
            // HomeSelected = Visibility.Hidden;
            Console.WriteLine("users selected");
        }


        //Tab Requestors
        public void populateRequestors()
        {
            Requestors = requestorRepository.getAllRequestors().ToList();
        }

        private List<Requestor> _requestors;
        public List<Requestor> Requestors
        {
            get { return _requestors; }
            set { _requestors = value; NotifyPropertyChanged(); }
        }

    }
}
