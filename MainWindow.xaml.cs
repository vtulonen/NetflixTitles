
using NetflixClassLibrary.EntityModel;
using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
        readonly NetflixDBEntities dataEntities = new NetflixDBEntities();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Series_Click(object sender, RoutedEventArgs e)
        {
            var query =
                from netflixTitle in dataEntities.netflixTitles
                where netflixTitle.type == "TV Show"
                select netflixTitle.title;

            ListBoxNames.ItemsSource = query.ToList();
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            var query =
                from netflixTitle in dataEntities.netflixTitles
                where netflixTitle.type == "Movie"
                select netflixTitle.title;

            ListBoxNames.ItemsSource = query.ToList();
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            var query =
                from netflixTitle in dataEntities.netflixTitles
                select netflixTitle.title;

            ListBoxNames.ItemsSource = query.ToList();
        }

        private void ListBoxNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxNames.SelectedItem != null)
            {
                string selected = ListBoxNames.SelectedItem.ToString();
                var data = from netflixTitle in dataEntities.netflixTitles
                                    where netflixTitle.title == selected
                                    select netflixTitle;

                var data2 = dataEntities.netflixTitles.First(x => x.title.Equals(selected));


                if (data != null)
                {
                    MessageBox.Show(data2.description.ToString());

                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var query =
                from netflixTitle in dataEntities.netflixTitles
                select netflixTitle.title;

            ListBoxNames.ItemsSource = query.ToList();
        }
    }

}
