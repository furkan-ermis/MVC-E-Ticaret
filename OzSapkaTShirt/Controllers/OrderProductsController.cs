using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;

namespace OzSapkaTShirt.Controllers
{
    [Authorize]
    public class OrderProductsController : Controller
    {
        private readonly ApplicationContext _context;

        public OrderProductsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: OrderProducts
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.OrderProducts.Include(o => o.Order).Include(o => o.Product);
            return View(await applicationContext.ToListAsync());
        }

        // GET: OrderProducts/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // GET: OrderProducts/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Color");
            return View();
        }

        // POST: OrderProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {

                _context.Add(orderProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Color", orderProduct.ProductId);
            return View(orderProduct);
        }

        // GET: OrderProducts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts.FindAsync(id);
            if (orderProduct == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Color", orderProduct.ProductId);
            return View(orderProduct);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, OrderProduct orderProduct)
        {
            if (id != orderProduct.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderProductExists(orderProduct.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", orderProduct.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Color", orderProduct.ProductId);
            return View(orderProduct);
        }

        // GET: OrderProducts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.OrderProducts == null)
            {
                return NotFound();
            }

            var orderProduct = await _context.OrderProducts
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderProduct == null)
            {
                return NotFound();
            }

            return View(orderProduct);
        }

        // OrderProducts/UpdateBasket/5&1/&false
        public async Task<ActionResult> Approve(long? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = 1;
            order.OrderDate = DateTime.Now;
            _context.Update(order);
            _context.SaveChanges();
            return View(order);
        }

        public Order UpDateBasket(long id, int quantity, bool delete)
        {
            Order? order;
            OrderProduct? orderProduct;
            string userIdentity = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Product product = _context.Products.Find(id);


            order = _context.Orders
                            .Where(o => o.UserId == userIdentity && o.Status == 0)
                            .Include(o => o.OrderProducts).FirstOrDefault();
            if (order == null)
            {
                order = new Order();
                order.UserId = userIdentity;
                order.OrderDate = DateTime.Today;
                order.TotalPrice = 0;
                order.Status = 0;
                order.OrderProducts = new List<OrderProduct>();
                _context.Add(order);
                _context.SaveChanges();
            }


            orderProduct = order.OrderProducts.Find(op => op.ProductId == id);
            if (orderProduct == null)
            {
                orderProduct = new OrderProduct();
                orderProduct.OrderId = order.Id;
                orderProduct.ProductId = id;
                orderProduct.Quantity = quantity;
                orderProduct.Price = product.Price;
                orderProduct.Total = product.Price;

                order.OrderProducts.Add(orderProduct);


            }
            else
            {
                orderProduct.Quantity += quantity;
                orderProduct.Total += quantity * product.Price;

                if (orderProduct.Quantity == 0)
                {
                    order.OrderProducts.Remove(orderProduct);
                    if (order.OrderProducts.Sum(op => op.Quantity) == 0)
                    {
                        _context.Remove(order);
                        HttpContext.Session.SetInt32("BasketCount", order.OrderProducts.Sum(op => op.Quantity));
                        _context.SaveChanges();
                        return null;
                    }
                }
                if (delete)
                {
                    quantity = -quantity;
                    order.OrderProducts.Remove(orderProduct);
                    if (order.OrderProducts.Sum(op => op.Quantity) == 0)
                    {
                        _context.Remove(order);
                        HttpContext.Session.SetInt32("BasketCount", order.OrderProducts.Sum(op => op.Quantity));
                        _context.SaveChanges();
                        return null;
                    }
                }
                _context.SaveChanges();


            }
            order.Clicked = id;
            order.TotalPrice += product.Price * quantity;
            _context.Update(order);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("BasketCount", order.OrderProducts.Sum(op => op.Quantity));
            return order;
            // (byte)order.OrderProducts.Sum(op=>op.Quantity)
        }



        private bool OrderProductExists(long id)
        {
            return (_context.OrderProducts?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
























    }
}
