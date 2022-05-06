using Microsoft.EntityFrameworkCore;
using Shoping.Data.Entities;
using Shoping.Enums;
using Shoping.Helpers;

namespace Shoping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriasAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Edwin", "Vicente", "alopez@yopmail.com", "58436020", "Calle Real San Juan Gascon Casa 3a", "EdwinNo1.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Paola", "Vicente", "paola@yopmail.com", "58436020", "Calle Real San Juan Gascon Casa 3a", "PaolaNo1.jpg", UserType.User);
            await CheckProductsAsync();
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Adidas Barracuda", 270000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "playera.jfif" });
                await AddProductAsync("Adidas Superstar", 250000M, 12F, new List<string>() { "Calzado", "Deportes" }, new List<string>() { "playera.jfif" });
                await AddProductAsync("AirPods", 1300000M, 12F, new List<string>() { "Tecnología", "Apple" }, new List<string>() {"telefono mac.png" });
                await AddProductAsync("Audifonos Bose", 870000M, 12F, new List<string>() { "Tecnología" }, new List<string>() { "audiInalambrico.jfif" });
                await AddProductAsync("Camisa Cuadros", 56000M, 24F, new List<string>() { "Ropa" }, new List<string>() { "playera.jfif" });
                await AddProductAsync("iPad", 2300000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "telefono mac.png" });
                await AddProductAsync("Mac Book Pro", 12100000M, 6F, new List<string>() { "Tecnología", "Apple" }, new List<string>() { "telefono mac.png" });
                await AddProductAsync("Teclado Gamer", 67000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "Dall.jpg" });
                await AddProductAsync("TV 8K", 980000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "TV 8K.jfif" });
                await AddProductAsync("Memoria RAM", 132000M, 12F, new List<string>() { "Gamer", "Tecnología" }, new List<string>() { "ddr4.jfif" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.Add(new Category { Name = "Tecnología" });
                _context.Categorias.Add(new Category { Name = "Ropa" });
                _context.Categorias.Add(new Category { Name = "Gamer" });
                _context.Categorias.Add(new Category { Name = "Belleza" });
                _context.Categorias.Add(new Category { Name = "Nutrición" });
                _context.Categorias.Add(new Category { Name = "Calzado" });
                _context.Categorias.Add(new Category { Name = "Deportes" });
                _context.Categorias.Add(new Category { Name = "Mascotas" });
                _context.Categorias.Add(new Category { Name = "Apple" });
            }

            await _context.SaveChangesAsync();
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (string? category in categories)
            {
                prodcut.ProductCategories.Add(new ProductCategory { Category = await _context.Categorias.FirstOrDefaultAsync(c => c.Name == category) });
            }


            foreach (string? image in images)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{image}", "products");
                prodcut.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(prodcut);
        }


        private async Task<User> CheckUserAsync(
             string document,
             string firstName,
             string lastName,
             string email,
             string phone,
             string address,
             string image,
             UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
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
                    ImageId = imageId,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

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
