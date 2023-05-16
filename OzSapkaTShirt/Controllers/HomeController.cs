using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace OzSapkaTShirt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View(_context.Products.ToList());
        }
        public IActionResult ProductsByCategory(long id)
        {
            return View(_context.Products.Where(p => p.CategoryId == id).ToList());
        }
        
        //public  IActionResult Gender(long id)
        //{
        //        return View(_context.Products.Where(p=>p.Gender==id).ToList());
        //}
    }
}