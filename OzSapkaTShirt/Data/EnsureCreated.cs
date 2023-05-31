using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OzSapkaTShirt.Data;
using Microsoft.AspNetCore.Identity;
using OzSapkaTShirt.Models;
using OzSapkaTShirt.Migrations;

namespace OzSapkaTShirt.Data
{

    public class EnsureCreated
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //  ProgramCs içerisine bunları eklemek gerek
        //      UserManager<ApplicationUser> userManager;
        //      RoleManager<IdentityRole> roleManager;
        //                  builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = false; options.Password.RequireNonAlphanumeric = false; })
        //                      .AddEntityFrameworkStores<ApplicationContext>();
        //          context = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetService<ApplicationContext>();
        //          userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
        //          roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();
        //              EnsureCreated ensureCreated = new EnsureCreated(context, userManager, roleManager);

        IdentityRole role;
        public EnsureCreated(ApplicationContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            createGender();
            createCities();
            createCategory();
            createAdminRole();
        }
        public void createAdminRole()
        {
            if (_roleManager != null)
            {
                if (_roleManager.FindByNameAsync("Administrator").Result == null)
                {

                    role = new IdentityRole("Administrator");
                    _roleManager.CreateAsync(role).Wait();
                    ApplicationUser applicationUser = new ApplicationUser();
                    applicationUser.UserName = "Administrator";
                    applicationUser.Name = "Admin";
                    applicationUser.SurName = "--";
                    applicationUser.Address = "---";
                    applicationUser.CityCode = 34;
                    applicationUser.Email = "furkan.ermis@icloud.com";
                    applicationUser.PhoneNumber = "5343656834";
                    applicationUser.PassWord = "ABcd1234";
                    applicationUser.ConfirmPassWord = "ABcd1234";
                    _userManager.CreateAsync(applicationUser, "ABcd1234").Wait();
                    _userManager.AddToRoleAsync(applicationUser, "Administrator").Wait();
                }
            }
        }
        public void createGender()
        {
            if (!_context.Genders.Any())
            {
                var Genders = new List<Gender>
            {
                new Gender { Id=0,Name = "Belirtilmemiş" },
                new Gender { Id=1,Name = "Erkek"},
                new Gender { Id=2,Name = "Kadın"}
            };
                _context.Genders.AddRange(Genders);
                _context.SaveChanges();
            }
        }
        public void createCategory()
        {
            if (!_context.Categories.Any())
            {
                var Categories = new List<Category>
            {
                new Category { Id=0,CategoryName = "Şapka" },
                new Category { Id=1,CategoryName = "T-Shirt"},
            };
                _context.Categories.AddRange(Categories);
                _context.SaveChanges();
            }
        }

        public void createCities()
        {
            if (!_context.Cities.Any())
            {
                var cities = new List<City>
            {
                new City { PlateCode=34,Name = "İstanbul" },
                new City { PlateCode=52,Name = "Ordu" },
                new City { PlateCode=35,Name = "İzmir" },
                new City { PlateCode=59,Name = "Tekirdağ" },
                new City { PlateCode=57,Name = "Sinop" },
                new City { PlateCode=22,Name = "Edirne" },
                new City { PlateCode=28,Name = "Giresun" },
                new City { PlateCode=58,Name = "Sivas" },
                new City { PlateCode=19,Name = "Çorum" },
                new City { PlateCode=06,Name = "Ankara" },
                new City { PlateCode=17,Name = "Çanakkale" },
                new City { PlateCode=10,Name = "Balıkesir" },
            };
                _context.Cities.AddRange(cities);
                _context.SaveChanges();
            }
        }


    }
}
