using cw_db.Interfaces;
using cw_db.Models;
using cw_db.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace cw_db.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<Customer> userManager;
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IOrderService orderService, IProductService productService, IHttpContextAccessor httpContext, UserManager<Customer> userManager, IUnitOfWork unitOfWork)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "admin, manager, user")]
        public async Task<IActionResult> Index()
        {
            var model = await orderService.GetAll();

            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            //TODO: Rewrite
            order.IssueDate = order.IssueDate.ToUniversalTime();
            var userId = httpContext.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(userId);

            var supplier = unitOfWork.Repo<Supplier>().Find(s => s.Id == 1).FirstOrDefault();
            order.Supplier = supplier;
            order.SupplierId = supplier.Id;

            Console.WriteLine(user);

            order.CustomerId = userId;
            order.customer = user;
            await orderService.Add(order);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = orderService.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> AddToOrder(int id)
        {
            var product = await productService.GetAll();

            return PartialView(product);
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsInOrder(int id)
        {
            var order = await orderService.Get(id);

            return PartialView(order.Products);
        }

        //public async Task AddToOrder(int id, Product product)
        //{
        //    await orderService.AddProductToOrder(id, product);
        //}
        //
        //public async Task<IActionResult> AddOrder(Order order)
        //{
        //    await orderService.Add(order);
        //    return RedirectToAction("Index");
        //}
        
        //TODO: CWDB: Add edit functions
    }
}
