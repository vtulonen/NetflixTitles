using NetflixClassLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixTitles.Helpers
{
    class DataAccess
    {
        readonly NetflixDBEntities dataEntities = new NetflixDBEntities();

        public ObservableCollection<string> GetUniqueCountries()
        {
            List<string> uniqueCountries = new List<string>();
            var countriesStrings = dataEntities.netflixTitles.Select(x => x.country).Distinct().Where(x => x != null).ToList();

            countriesStrings.ForEach(item => // example item: "Finland, Sweden"
            {
                var countryList = item.Split(',').Select(x => x.Trim()).ToList();
                countryList = countryList.Where(x => !string.IsNullOrEmpty(x)).ToList();
                var notAddedYet = countryList.Where(x => !uniqueCountries.Contains(x)).ToList();
                uniqueCountries.AddRange(notAddedYet);
            });

            uniqueCountries.Sort();

            return ToObservableCollection(uniqueCountries);
        }


        public ObservableCollection<int> GetUniqueYears()
        {
            var years = dataEntities.netflixTitles.Select(x => x.release_year).Distinct().OrderByDescending(x => x);
            return ToObservableCollection(years);
        }

        public ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> enumeration)
        {
            return new ObservableCollection<T>(enumeration);
        }
    }


}
