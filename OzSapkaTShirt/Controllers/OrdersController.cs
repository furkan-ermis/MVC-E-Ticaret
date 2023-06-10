using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;

namespace OzSapkaTShirt.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Orders.Include(o => o.User).Include(op => op.OrderProducts).ThenInclude(p => p.Product).ThenInclude(c => c.Category);
            return View(await applicationContext.ToListAsync());
        }
        public async Task<IActionResult> Details()
        {
            string userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationContext = _context.Orders.Where(e => e.UserId == userIdentity && e.Status == 1).Include(o => o.User).Include(op => op.OrderProducts).ThenInclude(p => p.Product).ThenInclude(c => c.Category);
            return View(await applicationContext.ToListAsync());
        }
    }
}
