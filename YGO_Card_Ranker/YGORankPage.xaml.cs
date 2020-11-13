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
    /// Interaction logic for YGORankPages.xaml
    /// </summary>
    public partial class YGORankPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static readonly DependencyProperty CurCardProperty =
            DependencyProperty.Register(
            "CurCard", typeof(YGOCard),
            typeof(YGORankPage),
            new PropertyMetadata(new YGOCard(), new PropertyChangedCallback(OnCurCardChanged))
        );
        public static readonly DependencyProperty CardImgPathProperty =
            DependencyProperty.Register(
            "CardImgPath", typeof(string),
            typeof(YGORankPage),
            new PropertyMetadata("", new PropertyChangedCallback(OnCurCardChanged))
        );
        public static readonly DependencyProperty CurCardRatingProperty =
            DependencyProperty.Register(
            "CurCardRating", typeof(int),
            typeof(YGORankPage),
            new PropertyMetadata(0)
        );
        public static readonly DependencyProperty SearchResultsProperty =
            DependencyProperty.Register(
            "SearchResults", typeof(List<YGOCard>),
            typeof(YGORankPage),
            new PropertyMetadata(new List<YGOCard>())
        );

        private YGOProDB ydb;
        private YGORankDB yrdb;
        private string galleryDir;
        private string GetCardImgPath()
        {
            if(galleryDir==null || CurCard.CardCode==0)
                return "";
            return galleryDir + "\\" + CurCard.CardCode+".jpg";
        }
        public string CardImgPath
        {
            get
            {
                return (string)GetValue(CardImgPathProperty);
            }
            set
            {
                SetValue(CardImgPathProperty, value);
            }
        }

        public YGOCard CurCard
        {
            get
            {
                return (YGOCard)GetValue(CurCardProperty);
            }
            set
            {
                if (CurCard.CardCode != 0)
                {
                    CurCardRating = yrdb.GetRankByGid(CurCard.CardCode);

                }
                SetValue(CurCardProperty, value);
            }
        }

        public List<YGOCard> SearchResults
        {
            get
            {
                return (List<YGOCard>)GetValue(SearchResultsProperty);
            }
            set
            {
                SetValue(SearchResultsProperty, value);
            }
        }
        private string searchText;


        public static ICommand SearchCmd { get; set; }
        public static ICommand NextUnrankCmd { get; set; }
        public static ICommand PrevUnrankCmd { get; set; }
        public static ICommand GoToSearchResultCmd { get; set; }
        public static ICommand PrevCardCmd { get; set; }
        public static ICommand NextCardCmd { get; set; }
        public static ICommand RankCardCmd { get; set; }
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                if(searchText == null || searchText == "")
                {
                    SearchResults = new List<YGOCard>();
                }
                else
                {
                    SearchResults = ydb.SearchCardByName(searchText);
                }

            }
        }

        public int CurCardRating
        {
            get
            {
                return (int)GetValue(CurCardRatingProperty);
            }
            set
            {
                SetValue(CurCardRatingProperty, value);
            }
        }
        

        public YGORankPage(YGOProDB ydb, YGORankDB yrdb, string galleryDir)
        {
            this.DataContext = this;
            SearchCmd = new RelayCommand(x => CanExecute(), x => SearchCmd_Executed(x));
            NextUnrankCmd = new RelayCommand(x => CanExecute(), x => NextUnrankCmd_Executed());
            PrevUnrankCmd = new RelayCommand(x => CanExecute(), x => PrevUnrankCmd_Executed());
            NextCardCmd = new RelayCommand(x => CanExecute(), x => NextCardCmd_Executed());
            PrevCardCmd = new RelayCommand(x => CanExecute(), x => PrevCardCmd_Executed());
            GoToSearchResultCmd = new RelayCommand(x => CanExecute(), x => GoToSearchResultCmd_Executed(x));
            RankCardCmd = new RelayCommand(x => CanExecute(), x => RankCardCmd_Executed(x));
            InitializeComponent();
            this.ydb = ydb;
            this.yrdb = yrdb;
            this.galleryDir = galleryDir;
            this.CurCard = ydb.GetCardByGid(yrdb.GetNextUnRanked());
            SearchResults = new List<YGOCard>();
            SearchResults.Add(this.CurCard);

        }

        private void RankCardCmd_Executed(object x)
        {
            var rating = Int32.Parse((string)x);
            yrdb.SetRankByGid(CurCard.CardCode, rating);
            CurCard = CurCard;
        }

        private void GoToSearchResultCmd_Executed(object x)
        {
            var cardName = (string)x;
            var card = SearchResults.Find(c => c.CardName == cardName);
            CurCard = card;
        }


        private void PrevUnrankCmd_Executed()
        {
            var gid = yrdb.GetPrevUnRanked(CurCard.CardCode);
            this.CurCard = ydb.GetCardByGid(gid);
        }

        private void NextUnrankCmd_Executed()
        {
            var gid = yrdb.GetNextUnRanked(CurCard.CardCode);
            this.CurCard = ydb.GetCardByGid(gid);
        }

        private void PrevCardCmd_Executed()
        {
            CurCard = ydb.GetPrevCard(CurCard.CardCode);
        }

        private void NextCardCmd_Executed()
        {
            CurCard = ydb.GetNextCard(CurCard.CardCode);
        }

        private bool CanExecute()
        {
            return true;
        }

        private void SearchCmd_Executed(object x)
        {
            uint gid;
            if (UInt32.TryParse((string)x, out gid)){
                this.CurCard = ydb.GetCardByGid(gid);
                this.popup.IsOpen = false;
            }
        }
        private static void OnCurCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as YGORankPage;
            c.CardImgPath = c.GetCardImgPath();
            c.OnPropertyChanged("CurCard");
            c.OnPropertyChanged("CardImgPath");
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
