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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using WSRobaSegonaMa.Models;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit;
using Application = System.Windows.Application;
using ClassificationRepository = desktopapplication.Model.ClassificationRepository;
using Color = System.Drawing.Color;
using ColorConverter = System.Drawing.ColorConverter;
using MessageBox = System.Windows.MessageBox;
using Size = desktopapplication.Model.Size;

namespace desktopapplication.ViewModel
{
    class Vista1ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region ICommandGeneral

        public ICommand requestorReqClickCommand { get; set; }
        public ICommand usersClickCommand { get; set; }
        public ICommand rewardsClickCommand { get; set; }
        public ICommand clothesClickCommand { get; set; }
        public ICommand announcementsClickCommand { get; set; }
        public ICommand createAnnouncementCommand { get; set; }
        public ICommand createClothCommand { get; set; }
        public ICommand deleteClothCommand { get; set; }
        public ICommand deleteDonorCommand { get; set; }
        public ICommand DonorUpdateCommand { get; set; }
        public ICommand ClothToRequestor { get; set; }
        public ICommand clothesSetMaleCommand { get; set; }
        public ICommand clothesSetFemaleCommand { get; set; }
        public ICommand clothesSetOtherCommand { get; set; }
        public ICommand exportDataCommand { get; set; }
        public ICommand closeApplication { get; set; }
        public ICommand warehouseEditCommand { get; set; }
        public ICommand warehouseAddCommand { get; set; }
        public ICommand administratorAddCommand { get; set; }
        public ICommand administratorReassignCommand { get; set; }
        public ICommand warehouseClickCommand { get; set; }

        #endregion

        #region WindowManagement

        private void restartApp()
        {
            MainWindow main = new MainWindow();
            Application.Current.Windows[0]?.Close();

            main.Show();
        }

        private void closeApp()
        {
            Application.Current.Windows[0]?.Close();
        }

        #endregion

        #region ChangeLanguages

        public ICommand ChangeLangCmd { get; set; }

        private void initLangChanger()
        {
            ChangeLangCmd = new RelayCommand(x => LangChangerSelector(x));

        }

        private void LangChangerSelector(object o)
        {
            String lang = o as String;
            if (!Thread.CurrentThread.CurrentCulture.IetfLanguageTag.Equals(lang))
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                Properties.Settings.Default.selectedLang = lang;
                Properties.Settings.Default.currentTab = SelectedTab;
                Properties.Settings.Default.Save();

                restartApp();
            }

        }




        #endregion



        public Vista1ViewModel()
        {
            //TODO: fer que nomes super admin pugui accedir a warehouses tab

            setUserLanguageCulture();

            //setColors();

            initCommandsMenu();
            populateDonors();
            populateClothDonors();
            populareColorList();
            PopulateClothesWarehouses();
            IsClothesDonate = true;
            ClothesPopulate();
            PopulateWarehouses();
            WarehouseEditing = new Warehouse();
            AdministratorEditing = new Administrator();
            populateAdministrators();

            rewardsThings();
            populateAnnouncements();

            populateClassification();
            populateSizes();

            initLangChanger();
        }

        private void setUserLanguageCulture()
        {
            int userId = Properties.Settings.Default.remindUser;
            Administrator currentAdministrator = AdministratorRepository.getAdministratorById(userId);


            if (userId != 0 && Properties.Settings.Default.selectedLang.Equals("default"))
            {
                LangChangerSelector(currentAdministrator.Language.code);
            }

            AdministratorRepository.changeLang(userId, Properties.Settings.Default.selectedLang);


            if (Properties.Settings.Default.currentTab != (-1))
            {
                SelectedTab = Properties.Settings.Default.currentTab;
            }
        }

        private void rewardsThings()
        {
            initRewardsActions();
        }

        private void requestorThings()
        {

            populateRequestors();
            populateClothesRequestors();
            initRequestorActions();
        }

        private void setColors()
        {
            List<Hue> primary = new List<Hue>();
            List<Hue> accent = new List<Hue>();
            primary.Add(new Hue("prim1", System.Windows.Media.Color.FromRgb(89, 102, 115), System.Windows.Media.Color.FromRgb(89, 102, 115)));
            primary.Add(new Hue("prim2", System.Windows.Media.Color.FromRgb(89, 102, 115), System.Windows.Media.Color.FromRgb(89, 102, 115)));
            primary.Add(new Hue("prim3", System.Windows.Media.Color.FromRgb(89, 102, 115), System.Windows.Media.Color.FromRgb(89, 102, 115)));
            primary.Add(new Hue("prim3", System.Windows.Media.Color.FromRgb(89, 102, 115), System.Windows.Media.Color.FromRgb(89, 102, 115)));
            accent.Add(new Hue("prim3", System.Windows.Media.Color.FromRgb(115, 51, 38), System.Windows.Media.Color.FromRgb(115, 51, 38)));

            Swatch s = new Swatch("Custom", primary, accent);

            PaletteHelper ph = new PaletteHelper();
            Palette p = new Palette(s, s, 0, 1, 2, 0);
            ph.ReplacePalette(p);
        }

        private void initCommandsMenu()
        {
            requestorReqClickCommand = new RelayCommand(x => selectHome());
            usersClickCommand = new RelayCommand(x => selectUsers());
            rewardsClickCommand = new RelayCommand(x => selectRewards());
            clothesClickCommand = new RelayCommand(x => selectClothes());
            warehouseClickCommand = new RelayCommand(x => selectWarehouse());
            announcementsClickCommand = new RelayCommand(x => selectAnnouncements());
            createAnnouncementCommand = new RelayCommand(x => createAnnouncement());
            createClothCommand = new RelayCommand(x => createCloth());
            deleteClothCommand = new RelayCommand(x => deleteCloth());
            deleteDonorCommand = new RelayCommand(x => DonorDelete());
            DonorUpdateCommand = new RelayCommand(x => DonorUpdate());
            ClothToRequestor = new RelayCommand(x => claimCloth());
            clothesSetMaleCommand = new RelayCommand(x => ClothesSetMale());
            clothesSetFemaleCommand = new RelayCommand(x => ClothesSetFemale());
            clothesSetOtherCommand = new RelayCommand(x => ClothesSetOther());
            exportDataCommand = new RelayCommand(x => restartApp());
            warehouseAddCommand = new RelayCommand(x => warehouseAdd());
            warehouseEditCommand = new RelayCommand(x => warehouseEdit());
            administratorAddCommand = new RelayCommand(x => addAdministrator(x));
            administratorReassignCommand = new RelayCommand(x => assignAdministrator());
            closeApplication = new RelayCommand(x => closeApp());
            DenyRequestorChecked = new RelayCommand(x => denyRequestor(true));
            AcceptRequestorChecked = new RelayCommand(x => denyRequestor(false));
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

        private void DonorDelete()
        {
            Donor d = DonorSelected;
            if (d != null)
            {
                donorRepository.DeleteDonor(d);
                populateDonors();
            }
            else
            {
                MessageBox.Show("Please, select a donor");
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
            Donors = new List<Donor>();
            Donors = donorRepository.getAllDonors();
        }
        private void populateClothDonors()
        {
            ClothesDonors = new List<Donor>();
            ClothesDonors = donorRepository.getAllDonors().Where(x => x.active == true).ToList();
        }

        private Donor _donorSelected;

        public Donor DonorSelected
        {
            get { return _donorSelected; }
            set
            {
                _donorSelected = value; NotifyPropertyChanged();
                if (value != null)
                {
                    DonorName = value.name;
                    DonorLastName = value.lastName;
                    DonorActive = value.active;
                    DonorEmail = value.email;
                    DonorBirthDate = value.birthDate.ToString();
                }
                else
                {
                    DonorName = null;
                    DonorLastName = null;
                    DonorActive = false;
                    DonorEmail = null;
                    DonorBirthDate = null; ;
                }

            }
        }

        public void DonorUpdate()
        {
            if (DonorName != null && DonorLastName != null && DonorEmail != null && DonorBirthDate != null)
            {

                Donor d = DonorSelected;
                d.name = DonorName;
                d.lastName = DonorLastName;
                d.email = DonorEmail;
                d.active = DonorActive;
                d.birthDate = Convert.ToDateTime(DonorBirthDate);


                Donor d2 = donorRepository.updateDonor(d);
                populateDonors();
            }
            else
            {
                MessageBox.Show("Please, fill up all the fields");
            }


        }

        private string _donorName;

        public string DonorName
        {
            get { return _donorName; }
            set
            {
                _donorName = value; NotifyPropertyChanged();
            }
        }

        private string _donorLastName;

        public string DonorLastName
        {
            get { return _donorLastName; }
            set
            {
                _donorLastName = value; NotifyPropertyChanged();
            }
        }

        private string _donorEmail;

        public string DonorEmail
        {
            get { return _donorEmail; }
            set
            {
                _donorEmail = value; NotifyPropertyChanged();
            }
        }

        private bool _donorActive;

        public bool DonorActive
        {
            get { return _donorActive; }
            set { _donorActive = value; NotifyPropertyChanged(); }
        }

        private string _donorBirthDate;

        public string DonorBirthDate
        {
            get { return _donorBirthDate; }
            set { _donorBirthDate = value; NotifyPropertyChanged(); }
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

            //EN PRINCIPI NO ES FA SERVIR, PERO NO ESBORRAR PER SI DE CAS 
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

            try
            {
                List<Model.Color> lc = colorRepository.getAllColors();
                System.Windows.Media.Color c = SelectedColorRaw;
                string code = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
                Console.WriteLine(code);
                Model.Color c2 = lc.Where(x => x.colorCode.Equals(code)).FirstOrDefault();
                return c2;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }




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

            //ClothesClassification = ClassificationRepository.getAllClassifications();
            ClothesClassification = ClassificationRepository.getAllClassificationsLang(Properties.Settings.Default.selectedLang);


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

        private void deleteCloth()
        {
            Cloth c = ClothSelected;
            ClothesRepository.deleteCloth(c);
            ClothesPopulate();
        }
        private void createCloth()
        {
            if (ClothesSizeSelected != null && ClothesClassificationSelected != null && ClothesGenderSelected != null && ClothesWarehouseSelected != null && ClothnesDonorSelected != null)
            {
                Cloth c = new Cloth();
                c.Size_Id = ClothesSizeSelected.Id;

                c.Classification_Id = ClothesClassificationSelected.Id;

                c.Color_Id = getColorByCode().Id;

                if (c.Color_Id != null)
                {
                    c.Gender_Id = ClothesGenderSelected.Id;

                    int userId = Properties.Settings.Default.remindUser;
                    Administrator actualAdmin = AdministratorRepository.getAdministratorById(userId);
                    c.Warehouse_Id = actualAdmin.Warehouse_Id;
                    //c.Warehouse_Id = ClothesWarehouseSelected.Id;

                    c.active = true;
                    c.dateCreated = DateTime.Now;

                    ClothesRepository.addCloth(c);

                    Donor d = ClothnesDonorSelected;

                    d.points += ClothesClassificationSelected.value;
                    d.ammountGiven += 1;

                    donorRepository.updateDonor(d);
                    ClothesPopulate();
                }
                else
                {
                    MessageBox.Show("Please, fill up all the fields.");
                }
            }
            else
            {
                MessageBox.Show("Please, fill up all the fields.");
            }



        }

        private void claimCloth()
        {
            if (ClothesSizeSelected != null && ClothesClassificationSelected != null && ClothesGenderSelected != null && ClothesWarehouseSelected != null && ClothnesDonorSelected != null && getColorByCode() != null)
            {
                List<Cloth> lc = ClothesRepository.getClothes();

                Cloth c = lc.Where(x =>
                    x.Gender_Id == ClothesGenderSelected.Id && x.Size_Id == ClothesSizeSelected.Id &&
                    x.Classification_Id == ClothesClassificationSelected.Id && x.Color_Id == getColorByCode().Id && x.active == true).FirstOrDefault();
                if (c != null)
                {
                    if (ClothesSelectedRequestor.MaxClaim.value - ClothesSelectedRequestor.points >= c.Classification.value)
                    {
                        Order o = new Order();
                        o.Clothes_Id = c.Id;
                        o.Requestor_Id = ClothesSelectedRequestor.Id;
                        o.dateCreated = DateTime.Now;
                        OrderRepository.newOrder(o);
                    }
                    else
                    {
                        MessageBox.Show("Not enough points available");
                    }
                }
                else
                {
                    MessageBox.Show("Cloth not available");
                }
            }
            else
            {
                MessageBox.Show("Please, fill up all the fields.");
            }
        }


        private Warehouse _clotheswarehouseselected;

        public Warehouse ClothesWarehouseSelected
        {
            get { return _clotheswarehouseselected; }
            set { _clotheswarehouseselected = value; NotifyPropertyChanged(); }
        }

        private List<Cloth> _clothsList;

        public List<Cloth> ClothList
        {
            get { return _clothsList; }
            set
            {
                _clothsList = value;
                NotifyPropertyChanged();
            }
        }

        private void ClothesPopulate()
        {
            ClothList = ClothesRepository.getClothes().Where(x => x.active == true).ToList();
            clothesSetFemaleEnabled = true;
            clothesSetMaleEnabled = true;
            clothesSetOtherEnabled = true;
        }

        private Cloth _clothSelected;

        public Cloth ClothSelected
        {
            get { return _clothSelected; }
            set
            {
                _clothSelected = value;
                NotifyPropertyChanged();
            }
        }


        private bool _isClothesClaim;

        public bool IsClothClaim
        {
            get { return _isClothesClaim; }
            set
            {
                _isClothesClaim = value;
                IsClothesDonate = !_isClothesDonate; NotifyPropertyChanged();
            }
        }

        private bool _isClothesDonate;

        public bool IsClothesDonate
        {
            get { return _isClothesDonate; }
            set { _isClothesDonate = value; NotifyPropertyChanged(); }

        }



        private List<Warehouse> _clothesWarehouseList;

        public List<Warehouse> ClothesWarehouseList
        {
            get { return _clothesWarehouseList; }
            set { _clothesWarehouseList = value; NotifyPropertyChanged(); }
        }

        private void PopulateClothesWarehouses()
        {
            ClothesWarehouseList = warehouseRepository.getAllWarehouses();
            ClothesWarehouseSelected = ClothesWarehouseList.FirstOrDefault();
        }
        #endregion

        #region Tabwarehouse

        private List<Warehouse> _warehouseList;

        public List<Warehouse> WarehouseList
        {
            get { return _warehouseList; }
            set { _warehouseList = value; NotifyPropertyChanged(); }
        }

        private void PopulateWarehouses()
        {
            WarehouseList = warehouseRepository.getAllWarehouses();

        }

        private Warehouse _warehouseselected;

        public Warehouse WarehouseSelected
        {
            get { return _warehouseselected; }
            set
            {
                _warehouseselected = value; NotifyPropertyChanged();
                WarehouseEditing = value;
                if (value != null)
                {
                    populateAdministratorsWarehouse();
                }

            }
        }

        private Warehouse _warehouseEditing;

        public Warehouse WarehouseEditing
        {
            get { return _warehouseEditing; }
            set { _warehouseEditing = value; NotifyPropertyChanged(); }
        }

        private void warehouseAdd()
        {
            Warehouse w = new Warehouse();
            w.name = WarehouseEditing.name;
            w.city = WarehouseEditing.city;
            w.number = WarehouseEditing.number;
            w.postalCode = WarehouseEditing.postalCode;
            w.street = WarehouseEditing.street;
            Warehouse w2 = warehouseRepository.addWarehouse(WarehouseEditing);
            PopulateWarehouses();
        }

        private void warehouseEdit()
        {
            try
            {
                if (WarehouseEditing == null)
                {
                    MessageBox.Show("Please, select a warehouse before editing");
                }
                else
                {
                    if (!WarehouseEditing.city.Equals("") && !WarehouseEditing.name.Equals("") && !WarehouseEditing.number.Equals("") && !WarehouseEditing.postalCode.Equals("") && !WarehouseEditing.street.Equals(""))
                    {
                        Warehouse w = warehouseRepository.updateWarehouse(WarehouseEditing);
                        PopulateWarehouses();
                    }
                    else
                    {
                        MessageBox.Show("Please, fill up all fields");

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Please, select a warehouse before editing");
            }






        }




        #endregion

        #region TabAdministrators

        private List<Administrator> _administrators;

        public List<Administrator> Administrators
        {
            get { return _administrators; }
            set { _administrators = value; NotifyPropertyChanged(); }
        }

        private List<Administrator> _administratorsWarehouse;

        public List<Administrator> AdministratorsWarehouse
        {
            get { return _administratorsWarehouse; }
            set { _administratorsWarehouse = value; NotifyPropertyChanged(); }
        }

        private Administrator _administratorSelected;

        public Administrator AdministratorSelected
        {
            get { return _administratorSelected; }
            set { _administratorSelected = value; NotifyPropertyChanged(); }
        }

        private Administrator _administratorComboBoxSelected;

        public Administrator AdministratorComboBoxSelected
        {
            get { return _administratorComboBoxSelected; }
            set { _administratorComboBoxSelected = value; NotifyPropertyChanged(); }
        }

        private Administrator _administratorEditing;

        public Administrator AdministratorEditing
        {
            get { return _administratorEditing; }
            set { _administratorEditing = value; NotifyPropertyChanged(); }
        }

        public void addAdministrator(object parameter)
        {
            if (WarehouseSelected != null)
            {

                //todo check if text boxes are not empty;

                Administrator a = new Administrator();

                a = AdministratorEditing;

                var passwordVar = parameter as PasswordBox;

                a.Warehouse_Id = WarehouseSelected.Id;
                a.lastName = WarehouseSelected.name;
                a.dateCreated = DateTime.Now;
                a.active = true;
                a.Language_Id = 1;


                //var data = Encoding.UTF8.GetBytes(a.password);
                var data = Encoding.UTF8.GetBytes(passwordVar.Password);


                byte[] hash;

                using (SHA512 sha = new SHA512Managed())
                {
                    hash = sha.ComputeHash(data);
                }

                string hashString = System.Text.Encoding.Default.GetString(hash);

                hashString = stringToHex(hashString);
                hashString = hashString.ToLower();

                a.password = hashString;

                //todo check if all fields are filled up;

                AdministratorRepository.add(a);

                populateAdministrators();
                populateAdministratorsWarehouse();

            }
            else
            {
                MessageBox.Show("Please, select a warehouse first");

            }


        }

        public string stringToHex(String text)
        {


            byte[] ba = Encoding.Default.GetBytes(text);
            var hexString = BitConverter.ToString(ba);
            hexString = hexString.Replace("-", "");


            return hexString;
        }

        public void assignAdministrator()
        {
            if (AdministratorComboBoxSelected != null && _warehouseselected != null)
            {
                Administrator a = AdministratorComboBoxSelected;
                a.Warehouse_Id = WarehouseSelected.Id;
                a.lastName = WarehouseSelected.name;
                AdministratorRepository.edit(AdministratorComboBoxSelected);
                populateAdministrators();

            }
            else
            {
                MessageBox.Show("Please, select an administrator from the combo box and a warehouse first");
            }

        }

        public void populateAdministrators()
        {
            Administrators = AdministratorRepository.getAllAdministrators();
        }

        public void populateAdministratorsWarehouse()
        {
            List<Administrator> a = AdministratorRepository.getAllAdministrators();


            AdministratorsWarehouse = a.Where(x => x.Warehouse_Id == WarehouseSelected.Id).ToList();
        }







        #endregion

        #region TabGender

        private bool _clothesSetFemaleEnabled { get; set; }
        public bool clothesSetFemaleEnabled
        {
            get { return _clothesSetFemaleEnabled; }
            set { _clothesSetFemaleEnabled = value; NotifyPropertyChanged(); }
        }
        private bool _clothesSetMaleEnabled { get; set; }
        public bool clothesSetMaleEnabled
        {
            get { return _clothesSetMaleEnabled; }
            set { _clothesSetMaleEnabled = value; NotifyPropertyChanged(); }
        }

        private bool _clothesSetOtherEnabled { get; set; }
        public bool clothesSetOtherEnabled
        {
            get { return _clothesSetOtherEnabled; }
            set { _clothesSetOtherEnabled = value; NotifyPropertyChanged(); }
        }

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
            disableMaleClothes();
        }

        private void ClothesSetFemale()
        {
            ClothesGenderSelected = new Gender();
            List<Gender> lg = genderRepository.getAllGenders();
            Gender g = lg.Where(x => x.gender1.ToLower().Equals("female")).FirstOrDefault();
            ClothesGenderSelected = g;
            disableFemaleClothes();
        }

        private void ClothesSetOther()
        {
            ClothesGenderSelected = new Gender();
            List<Gender> lg = genderRepository.getAllGenders();
            Gender g = lg.Where(x => x.gender1.ToLower().Equals("other")).FirstOrDefault();
            ClothesGenderSelected = g;
            disableOtherClothes();
        }

        private void disableOtherClothes()
        {
            clothesSetFemaleEnabled = true;
            clothesSetMaleEnabled = true;
            clothesSetOtherEnabled = false;
        }
        private void disableMaleClothes()
        {
            clothesSetFemaleEnabled = true;
            clothesSetMaleEnabled = false;
            clothesSetOtherEnabled = true;
        }
        private void disableFemaleClothes()
        {
            clothesSetFemaleEnabled = false;
            clothesSetMaleEnabled = true;
            clothesSetOtherEnabled = true;
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

        private List<Language> _announcementLanguages;

        public List<Language> AnnouncementLanguages
        {
            get { return _announcementLanguages; }
            set { _announcementLanguages = value; NotifyPropertyChanged(); }
        }

        private List<string> _announcementType;

        public List<string> AnnouncementType
        {
            get { return _announcementType; ; }
            set { _announcementType = value; NotifyPropertyChanged(); }
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
            Announcements = announcementRepository.getAllAnnouncements().OrderByDescending(x => x.dateCreated).ToList();
            populateAnnouncementType();
        }

        private string _announcementDestination;
        public string AnnouncementDestination
        {
            get { return _announcementDestination; }
            set { _announcementDestination = value; NotifyPropertyChanged(); }
        }

        public void populateAnnouncementType()
        {
            List<string> l = new List<string>();
            if (Properties.Settings.Default.selectedLang.Equals("en"))
            {

                l.Add("Everyone");
                l.Add("Donors");
                l.Add("Requestors");
            }
            else if (Properties.Settings.Default.selectedLang.Equals("ca"))
            {

                l.Add("Tothom");
                l.Add("Donants");
                l.Add("Demanants");
            }
            else
            {
                l.Add("Todos");
                l.Add("Donantes");
                l.Add("Pedidores");
            }

            AnnouncementType = l;



        }


        private void createAnnouncement()
        {
            if (AnnouncementLanguage != null && AnnouncementMessage != null && SelectedRecipient != null && AnnouncementTitle != null)
            {

                Announcement a = new Announcement();
                a.dateCreated = DateTime.Now;
                a.language = AnnouncementLanguage;
                a.message = AnnouncementMessage;
                a.title = AnnouncementTitle;




                if (SelectedRecipient.ToLower().Equals("Everyone".ToLower()) ||
                    SelectedRecipient.ToLower().Equals("Tothom".ToLower()) ||
                SelectedRecipient.ToLower().Equals("Todos".ToLower()))
                {
                    a.recipient = "Everyone";
                }
                else if (SelectedRecipient.ToLower().Equals("Donors".ToLower()) ||
                         SelectedRecipient.ToLower().Equals("Donants".ToLower()) ||
                         SelectedRecipient.ToLower().Equals("Donantes".ToLower()))
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
            else
            {
                MessageBox.Show("Please, fill up all the fields");
            }

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
            Console.WriteLine("REQUESTOR SELECTED");
            requestorThings();
        }

        private void selectRewards()
        {
            SelectedTab = 4;
            Console.WriteLine("REWARDS SELECTED");
            populateRewards();

        }
        private void selectUsers()
        {
            populateDonors();
            SelectedTab = 0;
            // UsersSelected = Visibility.Visible;
            // HomeSelected = Visibility.Hidden;
            Console.WriteLine("USERS SELECTED");
        }

        private void selectClothes()
        {
            ClothesPopulate();
            SelectedTab = 2;
            populateClothesRequestors();
            //HomeSelected = Visibility.Visible;
            //  UsersSelected = Visibility.Hidden;
            Console.WriteLine("CLOTHES SELECTED");
        }

        private void selectAnnouncements()
        {
            populateAnnouncements();
            SelectedTab = 3;
            //HomeSelected = Visibility.Visible;
            //  UsersSelected = Visibility.Hidden;
            Console.WriteLine("ANNOUNCEMENTS SELECTED");
        }

        private void selectWarehouse()
        {
            int userId = Properties.Settings.Default.remindUser;
            Administrator a = AdministratorRepository.getAdministratorById(userId);
            if (a.isSuper)
            {
                PopulateWarehouses();
                populateAdministrators();
                SelectedTab = 5;
                Console.WriteLine("WAREHOUSES SELECTED");
            }
            else
            {
                MessageBox.Show("Tab only enabled for super adminstrators, please log in as such");
            }

        }

        #endregion

        #region TabRequestors

        public ICommand DenyRequestorChecked { get; set; }
        public ICommand AcceptRequestorChecked { get; set; }

        public void initRequestorActions()
        {
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

                EnableHasErrorRequestor = "HIDDEN";
                Requestor r = SelectedRequestor;
                if (denied && SelectedStatus != null)
                {
                    Status s = searchByReason(SelectedStatus.reason);
                    if (s != null)
                    {
                        r.Status = s;
                        r.Status_Id = r.Status.Id;

                    }
                }
                else if (!denied)
                {
                    List<Status> allStatus = requestorRepository.getAllStatus();
                    if (r != null)
                    {
                        r.Status = allStatus.Where(x => x.reason.Equals("") && !x.status1.Equals("Pending"))
                            .FirstOrDefault();
                        r.Status_Id = r.Status.Id;
                    }
                }
                List<Requestor> requestorList = requestorRepository.getAllRequestors()
                    .Where(x => x.Status_Id == 3)
                    .Take(10)
                    .OrderByDescending(x => x.dateCreated).ToList();
                if (requestorList.Any())
                {
                    requestorRepository.setRequestors(r.Id, r);
                }
                else
                {
                    ActionsRequestor = false;
                    NotifyPropertyChanged();
                }
                NotifyPropertyChanged();
                populateRequestors();
            if (SelectedStatus == null && denied)
            {
                EnableHasErrorRequestor = "VISIBLE";
            }

        }

        private Status searchByReason(string statusText)
        {
            Status s = StatusDisponibles.Where(x => x.reason.Equals(statusText)).FirstOrDefault();
            return s;
        }


        public void populateRequestors()
        {
            EnableHasErrorRequestor = "HIDDEN";
            List<Requestor> requestorList = requestorRepository.getAllRequestors();
            requestorList = requestorList
                .Where(x => x.Status_Id == 3)
                .Take(10)
                .OrderByDescending(x => x.dateCreated).ToList();
            if (requestorList != null)
            {
                Requestors = requestorList;
            }

            SelectedRequestorIndex = 0;
        }

        public void populateClothesRequestors()
        {
            ClothesRequestors = requestorRepository.getAllRequestors();
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
        private string _enableHasErrorRequestor;
        public string EnableHasErrorRequestor
        {
            get { return _enableHasErrorRequestor; }
            set
            {
                _enableHasErrorRequestor = value;
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

        private Requestor _selectedClothesRequestor;
        public Requestor ClothesSelectedRequestor
        {
            get { return _selectedClothesRequestor; }
            set
            {
                _selectedClothesRequestor = value;
                NotifyPropertyChanged();
            }
        }

        private List<Requestor> _clothesrequestors;
        public List<Requestor> ClothesRequestors
        {
            get { return _clothesrequestors; }
            set
            {
                _clothesrequestors = value;
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

        #region TabRewards
        public ICommand ActualitarRewardBtnCmd { get; set; }
        public ICommand InsertRewardBtnCmd { get; set; }
        public ICommand DeleteRewardBtnCmd { get; set; }


        private string _tbPriceRewards;
        public string TbPriceRewards
        {
            get { return _tbPriceRewards; }
            set
            {
                _tbPriceRewards = value;
                NotifyPropertyChanged();
            }
        }

        private string _tbRewardsCA;
        public string TbRewardsCA
        {
            get { return _tbRewardsCA; }
            set
            {
                _tbRewardsCA = value;
                NotifyPropertyChanged();
            }
        }
        private string _tbDescRewardsCA;
        public string TbDescRewardsCA
        {
            get { return _tbDescRewardsCA; }
            set
            {
                _tbDescRewardsCA = value;
                NotifyPropertyChanged();
            }
        }

        private string _tbRewardsEN;
        public string TbRewardsEN
        {
            get { return _tbRewardsEN; }
            set
            {
                _tbRewardsEN = value;
                NotifyPropertyChanged();
            }
        }

        private string _tbDescRewardsEN;
        public string TbDescRewardsEN
        {
            get { return _tbDescRewardsEN; }
            set
            {
                _tbDescRewardsEN = value;
                NotifyPropertyChanged();
            }
        }

        private string _tbRewardsES;
        public string TbRewardsES
        {
            get { return _tbRewardsES; }
            set
            {
                _tbRewardsES = value;
                NotifyPropertyChanged();
            }
        }

        private string _tbDescRewardsES;
        public string TbDescRewardsES
        {
            get { return _tbDescRewardsES; }
            set
            {
                _tbDescRewardsES = value;
                NotifyPropertyChanged();
            }
        }


        private List<Reward> _listRewards;
        public List<Reward> ListRewards
        {
            get { return _listRewards; }
            set
            {
                _listRewards = value;
                NotifyPropertyChanged();
            }
        }

        private Reward _selectedReward;
        public Reward SelectedReward
        {
            get { return _selectedReward; }
            set
            {
                _selectedReward = value;
                populateTextBoxRewards();
                NotifyPropertyChanged();
            }
        }


        private int _selectedIndexReward;
        public int SelectedIndexReward
        {
            get { return _selectedIndexReward; }
            set
            {
                _selectedIndexReward = value;
                populateTextBoxRewards();
                NotifyPropertyChanged();
            }
        }

        private void populateTextBoxRewards()
        {
            emptyFields();
            try
            {
                TbPriceRewards = SelectedReward.neededPoints.ToString();
                List<RewardInfoLang> listRewardInfo = RewardInfoLangRepository.getRewardInfoLangFromReward(SelectedReward.Id);
                if (listRewardInfo != null)
                {
                    foreach (RewardInfoLang rewardInfo in listRewardInfo)
                    {
                        if (rewardInfo.Language.code.ToLower().Equals("es"))
                        {
                            TbRewardsES = rewardInfo.title;
                            TbDescRewardsES = rewardInfo.description;
                        }
                        else if (rewardInfo.Language.code.ToLower().Equals("ca"))
                        {
                            TbRewardsCA = rewardInfo.title;
                            TbDescRewardsCA = rewardInfo.description;
                        }
                        else if (rewardInfo.Language.code.ToLower().Equals("en"))
                        {
                            TbRewardsEN = rewardInfo.title;
                            TbDescRewardsEN = rewardInfo.description;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void emptyFields()
        {
            TbPriceRewards = "0";
            TbRewardsES = "";
            TbDescRewardsES = "";
            TbRewardsCA = "";
            TbDescRewardsCA = "";
            TbRewardsEN = "";
            TbDescRewardsEN = "";
        }

        public void initRewardsActions()
        {
            ActualitarRewardBtnCmd = new RelayCommand(x => updateReward());
            InsertRewardBtnCmd = new RelayCommand(x => insertReward());
            DeleteRewardBtnCmd = new RelayCommand(x => deleteReward());
            populateRewards();
        }

        private void populateRewards()
        {
            ListRewards = RewardRepository.getAllReward().OrderByDescending(x => x.dateCreated).ToList();
            emptyFields();
        }

        private void updateReward()
        {
            Reward reward = SelectedReward;

            if (reward == null)
            {
                MessageBox.Show("Select a reward first, please.");
                return;
            }

            if (TbPriceRewards != "")
            {
                reward.neededPoints = int.Parse(TbPriceRewards);
                reward.active = true;
                reward.dateCreated = DateTime.Now;

                List<RewardInfoLang> listInfo = new List<RewardInfoLang>();
                RewardInfoLang rwInfo = new RewardInfoLang();

                rwInfo.title = TbRewardsES;
                rwInfo.description = TbDescRewardsES;
                rwInfo.Language_Id = getLanguageId("es");
                listInfo.Add(rwInfo);

                rwInfo = new RewardInfoLang();
                rwInfo.title = TbRewardsCA;
                rwInfo.description = TbDescRewardsCA;
                rwInfo.Language_Id = getLanguageId("ca");
                listInfo.Add(rwInfo);

                rwInfo = new RewardInfoLang();
                rwInfo.title = TbRewardsEN;
                rwInfo.description = TbDescRewardsEN;
                rwInfo.Language_Id = getLanguageId("en");
                listInfo.Add(rwInfo);


                reward.RewardInfoLangs = listInfo;
                RewardRepository.setRewardWithLang(SelectedReward.Id, reward);
                populateRewards();
            }
        }
        private void insertReward()
        {
            Reward reward = new Reward();
            var preuReward = 0;
            try
            {

                preuReward = int.Parse(TbPriceRewards);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            if (TbPriceRewards != "" && preuReward >= 0)
            {
                reward.neededPoints = int.Parse(TbPriceRewards);
                reward.active = true;
                reward.dateCreated = DateTime.Now;

                RewardInfoLang rwInfo = new RewardInfoLang();

                rwInfo.title = TbRewardsES;
                rwInfo.description = TbDescRewardsES;
                rwInfo.Language_Id = getLanguageId("es");
                reward.RewardInfoLangs.Add(rwInfo);

                rwInfo = new RewardInfoLang();
                rwInfo.title = TbRewardsCA;
                rwInfo.description = TbDescRewardsCA;
                rwInfo.Language_Id = getLanguageId("ca");
                reward.RewardInfoLangs.Add(rwInfo);

                rwInfo = new RewardInfoLang();
                rwInfo.title = TbRewardsEN;
                rwInfo.description = TbDescRewardsEN;
                rwInfo.Language_Id = getLanguageId("en");
                reward.RewardInfoLangs.Add(rwInfo);


                RewardRepository.insertRewardWithLang(reward);
                populateRewards();
            }
        }
        private void deleteReward()
        {
            if (SelectedReward != null)
            {
                RewardRepository.deactivateReward(SelectedReward.Id);
                populateRewards();
            }

        }

        private int getLanguageId(String lang)
        {
            Language first = null;
            foreach (var language in LanguageRepository.getLanguageByCode(lang))
            {
                first = language;
                break;
            }

            return first.Id;
        }

        #endregion

    }
}