using System.Collections.Generic;
using karnaCrud.Models;
using static karnaCrud.Models.UserFormViewModel;

namespace karnaCrud.Services
{
    public class StaticDataService
    {
        public List<Country> GetCountries()
        {
            return new List<Country>
            {
                new Country {Id=1,Name="India"},
                new Country {Id=2,Name="Canada"}
            };
        }
        public List<State> GetStates()
        {
            return new List<State>
            {
                new State { Id = 1, Name = "Gujarat", CountryId = 1 },
                new State { Id = 2, Name = "Rajasthan", CountryId = 1 },
                new State { Id = 3, Name = "Ontario", CountryId = 2 }
            };

        }

        public List<City> GetCities()
        {
            return new List<City>
            {
                new City { Id = 1, Name = "Ahmedabad", StateId = 1 },
                new City { Id = 2, Name = "Gandhinagar", StateId = 1 },
                new City { Id = 3, Name = "Jaipur", StateId = 2 },
                new City { Id = 4, Name = "Udaipur", StateId = 2 },
                new City { Id = 5, Name = "Toronto", StateId = 3 },
                new City { Id = 6, Name = "Ottawa", StateId = 3 }
            };
        }
    }
}