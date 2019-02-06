using desktopapplication.Model;
using desktopapplication.Model.objects;
using System;
using System.Collections.Generic;
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
        //Tab Donors
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

        private Visibility _usersSelected;
        public Visibility UsersSelected
        {
            get { return _usersSelected; }
            set
            {
                _usersSelected = value;
                NotifyPropertyChanged();

            }
        }

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

        private List<Donor> _donors;
        public List<Donor> Donors
        {
            get { return _donors; }
            set { _donors = value; NotifyPropertyChanged(); }
        }

        private List<Announcement> _announcements;
        public List<Announcement> Announcements
        {
            get { return _announcements; }
            set { _announcements = value; NotifyPropertyChanged(); }
        }

        private void populateUsers()
        {
            Donors = new List<Donor>();
            Donors = donorRepository.getAllDonors();
        }

        private void populateAnnouncements()
        {
            Announcements = new List<Announcement>();
            Announcements = announcementRepository.getAllAnnouncements();

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
