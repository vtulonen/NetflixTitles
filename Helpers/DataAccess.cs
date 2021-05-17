using NetflixClassLibrary.EntityModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ObservableCollection<string> GetNetflixTitleNames()
        {
            var allTitleNames = dataEntities.netflixTitles.Select(x => x.title);
            return ToObservableCollection(allTitleNames);
        }

        public netflixTitle GetNetflixTitleData(string title)
        {
            return dataEntities.netflixTitles.First(x => x.title.Equals(title));
        }

        public ObservableCollection<string> GetFilteredNames(string selectedType, string selectedCountry, string selectedYear)
        {
            var allData = dataEntities.netflixTitles.Select(x => x);

            if (selectedType != "All") allData = allData.Where(x => x.type == selectedType);
            if (selectedCountry != null) allData = allData.Where(x => x.country.Contains(selectedCountry));
            if (selectedYear != null) allData = allData.Where(x => x.release_year.ToString() == selectedYear);

            var names = allData.Select(x => x.title);
     
            return ToObservableCollection(names);
        }







    }


}
