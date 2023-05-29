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

            string userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order order = _context.Orders
                .Where(o => o.UserId == userIdentity && o.Status == 0)
                .Include(o => o.OrderProducts).FirstOrDefault();
            if (order != null)
            {
                HttpContext.Session.SetInt32("BasketCount", order.OrderProducts.Sum(op => op.Quantity));
            }
            else
            {
                HttpContext.Session.SetInt32("BasketCount", 0);
            }
            return View(_context.Products.ToList());
        }
        public IActionResult ProductsByCategory(long id)
        {
            ViewBag.id = id;
            return View();
        }
        public PartialViewResult PartialProduct(long id, string text)
        {
            var res = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (text != null)
            {
                res = res.Where(p => p.Name.ToLower().Contains(text.ToLower())).ToList();
            }
            return PartialView("SearchProductPartialView", res);
        }

    }
}