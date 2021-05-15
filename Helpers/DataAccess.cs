using NetflixClassLibrary.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixTitles.Helpers
{
    class DataAccess
    {
        readonly NetflixDBEntities dataEntities = new NetflixDBEntities();

        public List<string> GetUniqueCountries()
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

            return uniqueCountries;
        }


        public List<int> GetUniqueYears()
        {
            return dataEntities.netflixTitles.Select(x => x.release_year).Distinct().OrderByDescending(x => x).ToList();
        }
    }


}
