﻿using desktopapplication.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace desktopapplication.ViewModel
{
    class loginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand loginCommand { get; set; }

        private string _adminUsername;

        public string AdminUsername
        {
            get { return _adminUsername; }
            set { _adminUsername = value;
                NotifyPropertyChanged();
            }
        }

        private string _adminPassword;

        public string AdminPassword
        {
            get { return _adminPassword; }
            set
            {
                _adminPassword = value;
                NotifyPropertyChanged();
            }
        }

        public loginViewModel()
        {
            loginCommand = new RelayCommand(o => logIn());
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void logIn()
        {
            Administrator a = new Administrator();
            a.email = AdminUsername;
            a.password = AdminPassword;
            
            if (Repository.loginAdministrator(a))
            {
                Console.WriteLine("Login OK");
            } else
            {
                Console.WriteLine("Incorrect email/password");
            }
       //     System.Windows.Forms.Application.Exit();

        }
    }
}
