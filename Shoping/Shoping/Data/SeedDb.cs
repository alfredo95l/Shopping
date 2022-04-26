using Shoping.Data.Entities;
using Shoping.Enums;
using Shoping.Helpers;

namespace Shoping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriasAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Edwin", "Vicente", "alopez@yopmail.com", "58436020", "Calle Real San Juan Gascon Casa 3a", UserType.Admin);
            await CheckUserAsync("2020", "Paola", "Vicente", "paola@yopmail.com", "58436020", "Calle Real San Juan Gascon Casa 3a", UserType.User);
        }

        private async Task<User> CheckUserAsync(
     string document,
     string firstName,
     string lastName,
     string email,
     string phone,
     string address,
     UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCategoriasAsync()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.Add(new Category { Name = "Tecnologia" });
                _context.Categorias.Add(new Category { Name = "Ropa" });
                _context.Categorias.Add(new Category { Name = "Nutrision" });
                _context.Categorias.Add(new Category { Name = "Rayos x" });
                _context.Categorias.Add(new Category { Name = "Ultrasonido" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Guatemala",
                    States = new List<State>()
                    {
                        new State
                        {
                            Name = "Sacatepequez",
                            Cities = new List<City>()
                            {
                                new City {Name= "La antigua"},
                                new City {Name= "San Lucas Milpas Altas"},
                                new City {Name= "Santa Lucia Milpas Altas"},
                                new City {Name= "Madgalena Milpas Altas"},
                                new City {Name= "San Bartolo Milpas Altas"},
                                new City {Name= "Santiago"},
                                new City {Name= "Ciudad Vieja"},

                            }
                        }
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>() {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>() {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}
