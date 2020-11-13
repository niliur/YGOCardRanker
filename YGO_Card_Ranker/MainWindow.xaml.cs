using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YGO_Card_Ranker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private YGOProDB ydb;
        private YGORankDB yrdb;
        private string galleryDir;
        public event PropertyChangedEventHandler PropertyChanged;


        public static readonly DependencyProperty CardDbStatusProperty =
            DependencyProperty.Register(
            "CardDbStatus", typeof(string),
            typeof(MainWindow),
            new PropertyMetadata("", new PropertyChangedCallback(OnCardDbStatusChanged))
        );
        public static readonly DependencyProperty RankDbStatusProperty =
            DependencyProperty.Register(
            "RankDbStatus", typeof(string),
            typeof(MainWindow),
            new PropertyMetadata("", new PropertyChangedCallback(OnRankDbStatusChanged))
        );
        public static readonly DependencyProperty GalleryStatusProperty =
            DependencyProperty.Register(
            "GalleryStatus", typeof(string),
            typeof(MainWindow),
            new PropertyMetadata("", new PropertyChangedCallback(OnGalleryStatusChanged))
        );

        public static readonly DependencyProperty ContinueVisibilityProperty =
            DependencyProperty.Register(
            "ContinueVisibility", typeof(Visibility),
            typeof(MainWindow),
            new PropertyMetadata(Visibility.Hidden, new PropertyChangedCallback(OnContinueVisibilityChanged))
        );

        public string CardDbStatus { 
            get { return (string) GetValue(CardDbStatusProperty);
            }
            set
            {
                UpdateContinueNav();
                SetValue(CardDbStatusProperty, value);
            }
        }
        public string RankDbStatus
        {
            get
            {
                return (string)GetValue(RankDbStatusProperty);
            }
            set
            {
                UpdateContinueNav();
                SetValue(RankDbStatusProperty, value);
            }
        }
        public string GalleryStatus
        {
            get
            {
                return (string)GetValue(GalleryStatusProperty);
            }
            set
            {
                UpdateContinueNav();
                SetValue(GalleryStatusProperty, value);
            }
        }
        public Visibility ContinueVisibility
        {
            get
            {
                return (Visibility)GetValue(ContinueVisibilityProperty);
            }
            set
            {
                SetValue(ContinueVisibilityProperty, value);
            }
        }

        public static ICommand GoToRankWindowCmd { get; set; }
        public static ICommand OpenCardDbCmd { get; set; }
        public static ICommand ChooseImgDirCmd { get; set; }
        public static ICommand OpenRankDbCmd { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CardDbStatus = Properties.Resources.MissingCardDBPrompt;
            RankDbStatus = Properties.Resources.MissingRankingDBPrompt;
            GalleryStatus = Properties.Resources.MissingPicDirPrompt;
            

            GoToRankWindowCmd = new RelayCommand(x => GoToRankWindow_CanExecute(), x=> GoToRankWindow_Executed());
            OpenCardDbCmd = new RelayCommand(x => OpenCardDbCmd_CanExecute(), x => OpenCardDbCmd_Executed());
            OpenRankDbCmd = new RelayCommand(x => OpenRankDbCmd_CanExecute(), x => OpenRankDbCmd_Executed());
            ChooseImgDirCmd = new RelayCommand(x => ChooseImgDirCmd_CanExecute(), x => ChooseImgDirCmd_Executed());


            //var cards = db.SearchCardByName("battery");
            var mainPage = new MainPage();
            mainPage.DataContext = this;
            _NavigationFrame.Navigate(mainPage);
        }

        private bool GoToRankWindow_CanExecute()
        {
            return true;
        }

        private void GoToRankWindow_Executed()
        {
            var mainPage = new YGORankPage(ydb, yrdb,galleryDir);
            _NavigationFrame.Navigate(mainPage);
        }

        private bool OpenCardDbCmd_CanExecute()
        {
            return ydb==null;
        }

        private void OpenCardDbCmd_Executed()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filename;
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                try
                {
                    ydb = new YGOProDB(filename);

                    CardDbStatus = Properties.Resources.FoundCardDBPrompt;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Could not load card DB! Error msg: {e}", "YGO Ranker");
                }
            }
        }
        private bool OpenRankDbCmd_CanExecute()
        {
            return yrdb == null;
        }

        private void OpenRankDbCmd_Executed()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filename;
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                try
                {
                    yrdb = new YGORankDB(filename);

                    RankDbStatus = Properties.Resources.FoundRankingDBPrompt;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Could not load card DB! Error msg: {e}", "YGO Ranker");
                }
            }
        }
        private bool ChooseImgDirCmd_CanExecute()
        {
            return galleryDir == null;
        }

        private void ChooseImgDirCmd_Executed()
        {

            var openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = openFolderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                galleryDir = openFolderDialog.SelectedPath;
                GalleryStatus = Properties.Resources.HasPicDirPrompt;
            }
        }


        private static void OnCardDbStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MainWindow;
            c.OnPropertyChanged("CardDbStatus");
        }

        private static void OnRankDbStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MainWindow;
            c.OnPropertyChanged("RankDbStatus");
        }

        private static void OnGalleryStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MainWindow;
            c.OnPropertyChanged("GalleryStatus");
        }
        private static void OnContinueVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as MainWindow;
            c.OnPropertyChanged("ContinueVisibility");
        }
        private void UpdateContinueNav()
        {
            if(yrdb == null || ydb == null)
            {
                ContinueVisibility = Visibility.Hidden;
            }
            else
            {
                ContinueVisibility = Visibility.Visible;
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
