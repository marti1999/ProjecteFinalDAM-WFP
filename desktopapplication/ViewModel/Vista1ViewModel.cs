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
using ClassificationRepository = desktopapplication.Model.ClassificationRepository;
using Color = System.Drawing.Color;
using Size = desktopapplication.Model.Size;

namespace desktopapplication.ViewModel
{
    class Vista1ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand requestorReqClickCommand { get; set; }
        public ICommand usersClickCommand { get; set; }
        public ICommand clothesClickCommand { get; set; }
        public ICommand announcementsClickCommand { get; set; }
        public ICommand createAnnouncementCommand { get; set; }
        public ICommand createClothCommand { get; set; }
        public  ICommand clothesSetMaleCommand { get; set; }
        public ICommand clothesSetFemaleCommand { get; set; }
        public ICommand clothesSetOtherCommand { get; set; }
        public ICommand exportDataCommand { get; set; }

        private void restartApp()
        {
            MainWindow main = new MainWindow();
            Application.Current.Windows[0].Close();
            main.ShowDialog();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Vista1ViewModel()
        {
            initCommandsMenu();
            populateDonors();
            populareColorList();

            requestorThings();
            populateAnnouncements();

            populateClassification();
            populateSizes();
        }

        private void requestorThings()
        {
            populateRequestors();
            initRequestorActions();
        }

        private void initCommandsMenu()
        {
            requestorReqClickCommand = new RelayCommand(x => selectHome());
            usersClickCommand = new RelayCommand(x => selectUsers());
            clothesClickCommand = new RelayCommand(x => selectClothes());
            announcementsClickCommand = new RelayCommand(x => selectAnnouncements());
            createAnnouncementCommand = new RelayCommand(x => createAnnouncement());
            createClothCommand = new RelayCommand(x => createCloth());
            clothesSetMaleCommand = new RelayCommand(x => ClothesSetMale());
            clothesSetFemaleCommand = new RelayCommand(x => ClothesSetFemale());
            clothesSetOtherCommand = new RelayCommand(x=> ClothesSetOther());
            exportDataCommand = new RelayCommand(x => restartApp());
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


        #region TabDonors

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

        private List<Donor> _clothesDonors;

        public List<Donor> ClothesDonors
        {
            get { return _clothesDonors; }
            set
            {
                _clothesDonors = value;
                NotifyPropertyChanged();
            }
        }

        private void populateDonors()
        {
            ClothesDonors = new List<Donor>();
            ClothesDonors = donorRepository.getAllDonors();
        }

        private Donor _clothesDonorSelected;

        public Donor ClothnesDonorSelected
        {
            get { return _clothesDonorSelected; }
            set { _clothesDonorSelected = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region TabRecipient

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


        private void populateRecipient()
        {
            Recipients = new ObservableCollection<string>();
            // TODO: en el webservice falta fer el repository i controller de recipient
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

        #endregion

        #region TabColors

        private ObservableCollection<ColorItem> _colorList;
        public ObservableCollection<ColorItem> ColorList
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

        private Model.Color getColorByCode()
        {

            List<Model.Color> lc = colorRepository.getAllColors();
            System.Windows.Media.Color c = SelectedColorRaw;
            string code = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
            Console.WriteLine(code);
            Model.Color c2 = lc.Where(x => x.colorCode.Equals(code)).FirstOrDefault();
            return c2;


        }

        private void populareColorList()
        {
            ColorList = new ObservableCollection<ColorItem>();
            List<Model.Color> lc = colorRepository.getAllColors();
            if (lc != null)
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

        #endregion

        #region TabSizes

        private List<Size> _clothesSizes;

        public List<Size> ClothesSizes
        {
            get { return _clothesSizes; }
            set { _clothesSizes = value; NotifyPropertyChanged(); }
        }

        private void populateSizes()
        {
            ClothesSizes = sizeRepository.getAllSize();
        }

        private Size _clothesSizeSelected;

        public Size ClothesSizeSelected
        {
            get { return _clothesSizeSelected; }
            set { _clothesSizeSelected = value; NotifyPropertyChanged(); }
        }

        #endregion
        
        #region TabClassifications

        private List<Classification> _clotheClassification;

        public List<Classification> ClothesClassification
        {
            get { return _clotheClassification; }
            set
            {
                _clotheClassification = value;
                NotifyPropertyChanged();
            }
        }

        private void populateClassification()
        {
            //TODO: canviar el if un cop estigui implementat el multi idioma
            if (true)
            {
                ClothesClassification = ClassificationRepository.getAllClassifications();

            }
            else
            {
                ClothesClassification = ClassificationRepository.getAllClassificationsLang("ca");

            }

        }

        private Classification _clothesClassificationSelected;

        public Classification ClothesClassificationSelected
        {
            get { return _clothesClassificationSelected; }
            set
            {
                _clothesClassificationSelected = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region TabClothes

        private void createCloth()
        {
            //TODO: No afegeix res a la DB
            Cloth c = new Cloth();
            c.Size = ClothesSizeSelected;
            c.Classification = ClothesClassificationSelected;
            c.Color = getColorByCode();
            c.Gender = ClothesGenderSelected;
            c.Warehouse = ClothesWarehouseSelected;
            //c.Size_Id = 1;
            //c.Classification_Id = 1;
            //c.Color_Id = 9;
            //c.Gender_Id = 1;
            //c.Warehouse_Id = 1;
            c.active = true;
            c.dateCreated = DateTime.Now;
            ClothesRepository.addCloth(c);
        }

        //tab Warehouse;
        private Warehouse _clotheswarehouseselected;

        public Warehouse ClothesWarehouseSelected
        {
            get { return _clotheswarehouseselected; }
            set { _clotheswarehouseselected = value;NotifyPropertyChanged(); }
        }

        private void PopulateClothesWarehouses()
        {
            //TODO: hacer el populate en la lista si hiciera falta;

            ClothesWarehouseSelected = warehouseRepository.getAllWarehouses().FirstOrDefault();
        }
        #endregion

        #region TabGender

        private Gender _clothesGenderSelected;

        public Gender ClothesGenderSelected
        {
            get { return _clothesGenderSelected; }
            set { _clothesGenderSelected = value; NotifyPropertyChanged(); }
        }

        private void ClothesSetMale()
        {
            ClothesGenderSelected = new Gender();
            List<Gender> lg = genderRepository.getAllGenders();
            Gender g = lg.Where(x => x.gender1.ToLower().Equals("male")).FirstOrDefault();
            ClothesGenderSelected = g;
        }

        private void ClothesSetFemale()
        {
            ClothesGenderSelected = new Gender();
            List<Gender> lg = genderRepository.getAllGenders();
            Gender g = lg.Where(x => x.gender1.ToLower().Equals("female")).FirstOrDefault();
            ClothesGenderSelected = g;
        }

        private void ClothesSetOther()
        {
            ClothesGenderSelected = new Gender();
            List<Gender> lg = genderRepository.getAllGenders();
            Gender g = lg.Where(x => x.gender1.ToLower().Equals("other")).FirstOrDefault();
            ClothesGenderSelected = g;
        }

        

        #endregion

        #region TabAnnouncements

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
            Console.WriteLine("announcement added to db");

            populateAnnouncements();
        }

        #endregion

        #region TabTabItems

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

        #endregion

        #region TabRequestors

        public ICommand DenyRequestorChecked { get; set; }
        public ICommand AcceptRequestorChecked { get; set; }
        //TODO: llista amb tots els status disponibles

        public void initRequestorActions()
        {
            DenyRequestorChecked = new RelayCommand(x => denyRequestor(true));
            AcceptRequestorChecked = new RelayCommand(x => denyRequestor(false));
            
            populateStatus();
        }

        private void populateStatus()
        {
            ActionsRequestor = true;
            List<Status> status = requestorRepository.getAllStatus();
            if (status != null)
            {
                StatusDisponibles = status.Where(z => z.reason != "").ToList();

                if (Requestors == null || !Requestors.Any())
                {
                    disableActionsRequestor();
                }
            }


        }

        private void disableActionsRequestor()
        {
            ActionsRequestor = false;
        }

        private void denyRequestor(bool denied)
        {
            Requestor r = SelectedRequestor;
            if (denied)
            {
                Status s = searchByReason(SelectedStatus.reason);
                if (s != null) r.Status = s;
            }
            else if (!denied)
            {
                //TODO: arreglar searchByReason per que busqui sobre tots els status i no nomes sobre els disponibles
                List<Status> allStatus = requestorRepository.getAllStatus();
                if (r != null) r.Status = allStatus.Where(x => x.reason.Equals("") && !x.status1.Equals("Pending")).FirstOrDefault();
            }
            requestorRepository.setRequestors(r.Id, r);
            populateRequestors();
        }

        private Status searchByReason(string statusText)
        {
            Status s = StatusDisponibles.Where(x => x.reason.Equals(statusText)).FirstOrDefault();
            return s;
        }


        public void populateRequestors()
        {
            List<Requestor> requestorList = requestorRepository.getAllRequestors()
                .Where(x => x.Status.status1.Equals("Pending"))
                .Take(10)
                .OrderByDescending(x => x.dateCreated).ToList();
            if (requestorList != null && requestorList.Any())
            {
                Requestors = requestorList;
            }

            SelectedRequestorIndex = 0;
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
        private int _selectedRequestorIndex;
        public int SelectedRequestorIndex
        {
            get { return _selectedRequestorIndex; }
            set
            {
                _selectedRequestorIndex = value;
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
        private List<Status> _statusDisponibles;
        public List<Status> StatusDisponibles
        {
            get { return _statusDisponibles; }
            set
            {
                _statusDisponibles = value;
                NotifyPropertyChanged();
            }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                NotifyPropertyChanged();
            }
        }

        private bool _actionsRequestor;
        public bool ActionsRequestor
        {
            get { return _actionsRequestor; }
            set
            {
                _actionsRequestor = value;
                NotifyPropertyChanged();
            }
        }

        #endregion



    }
}