using desktopapplication.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using desktopapplication.View;

namespace desktopapplication.ViewModel
{
    class loginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       // public logIn loginWindow = new logIn();
        public ICommand loginCommand { get; set; }

        public loginViewModel()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            loginCommand = new RelayCommand(o => logIn());
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ca");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ca");
        }

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
                MainWindow main = new MainWindow();
                Application.Current.Windows[0].Close();
                main.ShowDialog();


            } else
            {
                Console.WriteLine("Incorrect email/password");
            }
            //     System.Windows.Forms.Application.Exit();
        }



    }
}
