using System.Collections.Generic;
namespace karnaCrud.Models
{
    public class UserFormViewModel
    {
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
