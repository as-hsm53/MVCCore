using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration;
using MyFirstMVC.Models;
using Rotativa.AspNetCore;
using System.Drawing;


namespace MyFirstMVC.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        // GET: AdminController
        private readonly UserContext _context;
        private readonly IEmailSender emailSender;
        private readonly IHttpContextAccessor contextAccessor;

        public AdminController(UserContext context, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.emailSender = emailSender;
            contextAccessor = httpContextAccessor;
        }
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Admin admin)
        {
            var admins = _context.Admins.Where(x =>
            x.Username == admin.Username && x.Password == admin.Password).FirstOrDefault();
            if (admins == null)
            {
                ViewBag.Message = "Invalid Credentials";
                return View(admin);
            }
            else
            {
                contextAccessor.HttpContext.Session.SetInt32("Admin", admins.Id);
                return RedirectToAction("Index", "Admin");
            }
        }
        public class UsersAndOrdersViewModel
        {
            public List<User> Users { get; set; }
            public List<Order> Orders { get; set; }
            public decimal TotalOrderPrice { get; set; }
            public int OrderCount { get; set; }
            public int UserCount { get; set; }
        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var adminId = HttpContext.Session.GetInt32("Admin");

            if (adminId == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                int userCount = await _context.Users.CountAsync();
                int orderCount = await _context.Orders.CountAsync();
                decimal totalOrderPrice = await _context.Orders.SumAsync(x => x.TotalPrice);

                var viewModel = new UsersAndOrdersViewModel
                {
                    Users = await _context.Users.ToListAsync(),
                    Orders = await _context.Orders.ToListAsync(),
                    UserCount = userCount,
                    OrderCount = orderCount,
                    TotalOrderPrice = totalOrderPrice
                };
                return View(viewModel);

            }

        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        // GET: AdminController/Details/5

        public class OrdersAndUsersViewModel
        {
            public Order Order { get; set; }
            public User User { get; set; }
        }
        

        [Route("Orders")]
        public async Task<IActionResult> Orders()
        {
            var adminId = HttpContext.Session.GetInt32("Admin");

            if (adminId == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var ordersWithUsers = await (from order in _context.Orders
                                             join user in _context.Users on order.UserID equals user.Id
                                             select new OrdersAndUsersViewModel
                                             {
                                                 Order = order,
                                                 User = user
                                             })
                                             .ToListAsync();

                return View(ordersWithUsers);

            }

        }

        public class OrderAndUserViewModel
        {
            public Order Order { get; set; }
            public User User { get; set; }
        }
        private OrderAndUserViewModel GetDataForOrder(int orderId)
        {
            var orderWithUser = (from order in _context.Orders
                                 join user in _context.Users on order.UserID equals user.Id
                                 where order.Id == orderId
                                 select new OrderAndUserViewModel
                                 {
                                     Order = order,
                                     User = user
                                 }).FirstOrDefault();

            return orderWithUser;
        }

        [Route("GenerateInvoice")]
        public IActionResult GenerateInvoice(int orderId)
        {
            OrderAndUserViewModel orderAndUser = GetDataForOrder(orderId);

            if (orderAndUser == null)
            {
                // Handle case where order is not found
                return NotFound();
            }

            return new ViewAsPdf("InvoiceTemplate", orderAndUser)
            {
                FileName = "Invoice.pdf"
            };
        }
        [Route("InvoiceTemplate")]
        public ActionResult InvoiceTemplate(int id)
        {
            id = 2;
            OrderAndUserViewModel orderAndUser = GetDataForOrder(id);
            return View(orderAndUser);
        }
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
