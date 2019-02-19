using desktopapplication.Model;
using desktopapplication.Model.objects;
using desktopapplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Color = System.Drawing.Color;

namespace desktopapplication.ViewModel
{
    class Vista1ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand homeClickCommand { get; set; }
        public ICommand usersClickCommand { get; set; }
        public ICommand clothesClickCommand { get; set; }
        public ICommand announcementsClickCommand { get; set; }
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
            clothesClickCommand = new RelayCommand(x => selectClothes());
            announcementsClickCommand = new RelayCommand(x => selectAnnouncements());
            createAnnouncementCommand = new RelayCommand(x => createAnnouncement());

            populateUsers();
            populareColorList();
            populateRequestors();
            setDefaultOptionsRequestor();
            initRequestorCommands();
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
            set
            {
                _donors = value;
                NotifyPropertyChanged();
            }
        }

        private void populateUsers()
        {
            Donors = new List<Donor>();
            Donors = donorRepository.getAllDonors();
        }


        //tab Recipient

        private ObservableCollection<string> _recipients;

        public ObservableCollection<string> Recipients
        {
            get { return _recipients; }
            set
            {
                _recipients = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Xceed.Wpf.Toolkit.ColorItem> _colorList;

        public ObservableCollection<Xceed.Wpf.Toolkit.ColorItem> ColorList
        {
            get { return _colorList; }
            set
            {
                _colorList = value;
                NotifyPropertyChanged();
            }
        }

        private System.Windows.Media.Color _selectedColorRaw;
            
        public System.Windows.Media.Color SelectedColorRaw
        {
            get { return _selectedColorRaw; }
            set { _selectedColorRaw = value; NotifyPropertyChanged(); }
        }

        private Model.Color _selectedColor;

        public Model.Color SelectetColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; NotifyPropertyChanged(); }
        }

        private void populareColorList()
        {
            ColorList = new ObservableCollection<Xceed.Wpf.Toolkit.ColorItem>();
            List<Model.Color> lc = colorRepository.getAllColors();
            foreach (Model.Color item in lc)
            {
                System.Drawing.Color c = new System.Drawing.Color();
                c = ColorTranslator.FromHtml(item.colorCode);
                //int r = Convert.ToInt16(c.R);
                //int g = Convert.ToInt16(c.G);
                //int b = Convert.ToInt16(c.B);

                //System.Windows.Media.Color c2 = new System.Windows.Media.Color.FromAR
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);

                /**
                 * check this website for help
                 * http://jkshay.com/configuring-the-extended-wpf-toolkits-colorpicker-color-palette/
                 *
                 */


                ColorList.Add(new ColorItem(newColor, item.name));
            }
        }

        private void populateRecipient()
        {
            Recipients = new ObservableCollection<string>();
            //TODO: en el webservice falta fer el repository i controller de recipient
            // Recipients = announcementRepository.getRecipients();

            Recipients.Add("Everyone");
            Recipients.Add("Donors");
            Recipients.Add("Requestors");
        }



        private string _selectedRecipient;

        public string SelectedRecipient
        {
            get { return _selectedRecipient; }
            set
            {
                _selectedRecipient = value;
                NotifyPropertyChanged();
            }
        }


        //tab Announcements
        private List<Announcement> _announcements;

        public List<Announcement> Announcements
        {
            get { return _announcements; }
            set
            {
                _announcements = value;
                NotifyPropertyChanged();
            }
        }

        private string _announcementTitle;

        public string AnnouncementTitle
        {
            get { return _announcementTitle; }
            set
            {
                _announcementTitle = value;
                NotifyPropertyChanged();
            }
        }

        private string _announcementMessage;

        public string AnnouncementMessage
        {
            get { return _announcementMessage; }
            set
            {
                _announcementMessage = value;
                NotifyPropertyChanged();
            }
        }

        private string _announcementLanguage;

        public string AnnouncementLanguage
        {
            get { return _announcementLanguage; }
            set
            {
                _announcementLanguage = value;
                NotifyPropertyChanged();
            }
        }

        private Recipient _announcementRecipient;

        public Recipient AnnouncementRecipient
        {
            get { return _announcementRecipient; }
            set
            {
                _announcementRecipient = value;
                NotifyPropertyChanged();
            }
        }


        private void populateAnnouncements()
        {
            Announcements = new List<Announcement>();
            Announcements = announcementRepository.getAllAnnouncements();
        }

        private void createAnnouncement()
        {
            Announcement a = new Announcement();
            a.dateCreated = DateTime.Now;
            a.language = AnnouncementLanguage;
            a.message = AnnouncementMessage;
            a.title = AnnouncementTitle;

            if (SelectedRecipient.ToLower().Equals("Everyone".ToLower()) ||
                SelectedRecipient.ToLower().Equals("Tothom".ToLower()))
            {
                a.recipient = "Everyone";
            }
            else if (SelectedRecipient.ToLower().Equals("Donors".ToLower()) ||
                     SelectedRecipient.ToLower().Equals("Donants".ToLower()))
            {
                a.recipient = "Donors";
            }
            else
            {
                a.recipient = "Requestors";
            }

            announcementRepository.addAnnouncement(a);
            Console.WriteLine("announcement added to db"); //TODO: per ara no s' afegeix a la base de dades, mirar WS

            populateAnnouncements();
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

        private void selectClothes()
        {
            SelectedTab = 2;
            //HomeSelected = Visibility.Visible;
            //  UsersSelected = Visibility.Hidden;
            Console.WriteLine("CLOTHES SELECTED");
        }

        private void selectAnnouncements()
        {
            SelectedTab = 3;
            //HomeSelected = Visibility.Visible;
            //  UsersSelected = Visibility.Hidden;
            Console.WriteLine("ANNOUNCEMENTS SELECTED");
        }


        //Tab Requestors
        public ICommand BtnSaveRequestorsCmd { get; set; }

        public void initRequestorCommands()
        {
            BtnSaveRequestorsCmd = new RelayCommand(x => applyRequest());
        }

        private void applyRequest()
        {
            foreach (var req in Requestors)
            {
                requestorRepository.setRequestors(req.Id, req);
            }
        }

        public void populateRequestors()
        {
            Requestors = requestorRepository.getAllRequestors()
                .Where(x => !x.Status.status1.Equals("Pending"))
                .Take(15).OrderBy(x=> x.dateCreated).ToList();
        }

        public void setDefaultOptionsRequestor()
        {
            PendingRequestorChecked = true;
        }


        private Requestor _selectedRequestor;
        public Requestor SelectedRequestor
        {
            get { return _selectedRequestor; }
            set
            {
                _selectedRequestor = value;
                NotifyPropertyChanged();
            }
        }

        private List<Requestor> _requestors;

        public List<Requestor> Requestors
        {
            get { return _requestors; }
            set
            {
                _requestors = value;
                NotifyPropertyChanged();
            }
        }


        private bool _denyRequestorChecked;

        public bool DenyRequestorChecked
        {
            get { return _denyRequestorChecked; }
            set
            {
                _denyRequestorChecked = value;
                NotifyPropertyChanged();
            }
        }

        private bool _acceptRequestorChecked;

        public bool AcceptRequestorChecked
        {
            get { return _acceptRequestorChecked; }
            set
            {
                _acceptRequestorChecked = value;
                NotifyPropertyChanged();
            }
        }

        private bool _pendingRequestorChecked;

        public bool PendingRequestorChecked
        {
            get { return _pendingRequestorChecked; }
            set
            {
                _pendingRequestorChecked = value;
                NotifyPropertyChanged();
            }
        }
    }
}