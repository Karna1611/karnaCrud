using System.Collections.Generic;
namespace karnaCrud.Models
{
    public class UserFormViewModel
    {
        public UserFormViewModel()
        {
            Countries = new List<Country>();
            States = new List<State>();
            Cities = new List<City>();
        }
        public UserModel User { get; set; }
        public List<Country> Countries { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }

        public class State
        {
            public int Id { get; set; }
            public string Name { get; set; }


            public int CountryId { get; internal set; }
        }

        public class Country
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class City
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int StateId { get; internal set; }
        }
    }
}