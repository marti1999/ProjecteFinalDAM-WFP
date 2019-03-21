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
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ca");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ca");



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

        private async void logIn(object parameter)
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
            Console.WriteLine(hashString);

            hashString = hashString.Substring(0, 32);
            Console.WriteLine(hashString);

            AdminPassword = passwordVar.Password;
            a.password = hashString;

            if (Repository.loginAdministrator(a))
            {
                Administrator currenAdministrator = AdministratorRepository.getAdministratorByEmail(a.email);
                Properties.Settings.Default.remindUser = currenAdministrator.Id;
                Properties.Settings.Default.Save();

                Console.WriteLine("Login OK");
                MainWindow main = new MainWindow();
                Application.Current.Windows[0].Close();
                main.ShowDialog();
            }
            else
            {
                Console.WriteLine("Incorrect email/password");
            }
            //     System.Windows.Forms.Application.Exit();
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
