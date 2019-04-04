using desktopapplication.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using desktopapplication.View;

using MaterialDesignThemes.Wpf;

namespace desktopapplication.ViewModel
{
    class loginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand loginCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public loginViewModel()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            loginCommand = new RelayCommand(o => logIn(o));
            Progress = "HIDDEN";
            _dialogOpen = false;
            Opacity = 1;
            _panelEnabled = true;


        }

        private string _adminUsername;

        public string AdminUsername
        {
            get { return _adminUsername; }
            set
            {
                _adminUsername = value;
                NotifyPropertyChanged();
            }
        }

        private double _opacity;

        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; NotifyPropertyChanged(); }
        }

        private bool _panelEnabled;
        public bool PanelEnabled
        {
            get { return _panelEnabled; }
            set { _panelEnabled = value; NotifyPropertyChanged(); }
        }

        private bool _dialogOpen;
        public bool DialogOpen
        {
            get { return _dialogOpen; }
            set { _dialogOpen = value; NotifyPropertyChanged(); }
        }


        private string _progress;

        public string Progress
        {
            get { return _progress; }
            set { _progress = value; NotifyPropertyChanged(); }
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

        private  void loginAnimation()
        {

            Opacity = 0.3;
            Progress = "VISIBLE";
            PanelEnabled = false;


            
            //CommandManager.InvalidateRequerySuggested();
        }

        private  void logIn(object parameter)
        {
            loginAnimation();
            bool isLogin;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                
                logInActions(parameter);
            }).Start();
            
            
           

        }

        private async void logInActions(object parameter)
        {
            var passwordVar = parameter as PasswordBox;

            Administrator a = new Administrator();
            a.email = AdminUsername;
            string passwordRaw = passwordVar.Password;




            var data = Encoding.UTF8.GetBytes(passwordRaw);


            byte[] hash;

            using (SHA512 sha = new SHA512Managed())
            {
                hash = sha.ComputeHash(data);
            }

            string hashString = System.Text.Encoding.Default.GetString(hash);

            //hashString = hashString.Substring(0, 32);
            Console.WriteLine(hashString);

            AdminPassword = passwordVar.Password;
            a.password = hashString;

            if (Repository.loginAdministrator(a))
            {
                Thread.Sleep(1500);
                Administrator currenAdministrator = AdministratorRepository.getAdministratorByEmail(a.email);
                Properties.Settings.Default.remindUser = currenAdministrator.Id;
                Properties.Settings.Default.currentTab = -1;
                Properties.Settings.Default.Save();

                Console.WriteLine("Login OK");
                Application.Current.Dispatcher.Invoke((Action)delegate {
                    
                    MainWindow main = new MainWindow();
                    Application.Current.Windows[0].Close();

                    main.ShowDialog();
                });
             
            }
            else
            {

                Console.WriteLine("Incorrect email/password");
                MessageBox.Show("Incorrect email/password");


                //DialogOpen = true;


                Progress = "HIDDEN";
                Opacity = 1;
                PanelEnabled = true;
               
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }



    }
}
