
using NetflixClassLibrary.EntityModel;
using NetflixTitles.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
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

namespace NetflixTitles
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Access db table netflixTitles with dataEntties.netflixTitles
        /// </summary>
        readonly NetflixDBEntities dataEntities = new NetflixDBEntities();
        public event PropertyChangedEventHandler PropertyChanged;
        DataAccess da = new DataAccess();
        private ObservableCollection<string> _names;

        public ObservableCollection<string> Countries { get; set; } // Bound to ComboBoxCountry
        public ObservableCollection<int> Years { get; set; } // Bound to ComboBoxYear
        public ObservableCollection<string> Names // Bound to ListBoxNames
        {
            get { return _names; }
            set
            {
                _names = value;
                RaisePropertyChanged("Names");
            }
        }

        public string SelectedType { get; set; }
        public string SelectedCountry { get; set; }
        public string SelectedYear { get; set; }

        public MainWindow()
        {

            Countries = da.GetUniqueCountries();
            Years = da.GetUniqueYears();
            Names = da.ToObservableCollection(dataEntities.netflixTitles.Select(x => x.title));
            InitializeComponent();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        ///     Shows the selected netflixTitles description in a messagebox
        /// </summary>
        /// 
        // TODO: display more data in different view
        private void ListBoxNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxNames.SelectedItem != null)
            {
                string selected = ListBoxNames.SelectedItem.ToString();

                var data = dataEntities.netflixTitles.First(x => x.title.Equals(selected));

                if (data != null) MessageBox.Show(data.description.ToString());
            }
        }

        /// <summary>
        ///     Updates ListBoxNames based on selected filters
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var allTitles = dataEntities.netflixTitles.Select(x => x);

            if (SelectedType != "All") allTitles = allTitles.Where(x => x.type == SelectedType);
            if (SelectedCountry != null) allTitles = allTitles.Where(x => x.country.Contains(SelectedCountry));
            if (SelectedYear != null) allTitles = allTitles.Where(x => x.release_year.ToString() == SelectedYear);

            var names = allTitles.Select(x => x.title);
            Names = da.ToObservableCollection(names);

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.IsChecked.Value)
            {
                SelectedType = rb.Content.ToString();
            }
        }

        private void ComboBoxCountry_DropDownClosed(object sender, EventArgs e)
        {
            SelectedCountry = ComboBoxCountry.Text;
        }

        private void ComboBoxYear_DropDownClosed(object sender, EventArgs e)
        {
            SelectedYear = ComboBoxYear.Text;
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            RadioButtonAll.IsChecked = true;
            SelectedCountry = null;
            SelectedYear = null;
            ComboBoxCountry.Text = null;
            ComboBoxYear.Text = null;
            Update_Click(null, null);
        }
    }

}
