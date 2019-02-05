using desktopapplication.Model;
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
        }
        //Tab Donors
        private Visibility _homeSelected;
        public Visibility HomeSelected
        {
            get { return _homeSelected; }
            set
            {
                _homeSelected = value;
                NotifyPropertyChanged("Visibility");

            }
        }

        private Visibility _usersSelected;
        public Visibility UsersSelected
        {
            get { return _usersSelected; }
            set
            {
                _usersSelected = value;
                NotifyPropertyChanged("Visibility");

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

        private List<Donor> _users;
        public List<Donor> Users
        {
            get { return _users; }
            set { _users = value; NotifyPropertyChanged(); }
        }

        private void populateUsers()
        {
            Users = new List<Donor>();
            Users = donorRepository.getAllDonors();
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
