
using NetflixTitles.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace NetflixTitles
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        ///     DataAccess class provides data with it's methods by querying dataEntities.netflixTitles table in DB
        /// </summary>
        readonly DataAccess da = new DataAccess();
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<string> _names;

        public ObservableCollection<string> Countries { get; set; } // Bound to ComboBoxCountry
        public ObservableCollection<int> Years { get; set; }        // Bound to ComboBoxYear
        public ObservableCollection<string> Names                   // Bound to ListBoxNames
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
            Names = da.GetNetflixTitleNames();
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
        private void ListBoxNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxNames.SelectedItem != null)
            {
                string selectedTitle = ListBoxNames.SelectedItem.ToString();

                var data = da.GetNetflixTitleData(selectedTitle);

                if (data != null) MessageBox.Show(data.description.ToString());
            }
        }

        /// <summary>
        ///     Updates ListBoxNames based on selected filters
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Names = da.GetFilteredNames(SelectedType, SelectedCountry, SelectedYear);
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
