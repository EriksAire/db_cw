using cw_db.Data;
using cw_db.Interfaces;
using cw_db.Models;
using cw_db.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly IRepository<Order> repository;

        public OrdersController(IOrderService orderService, IProductService productService, IHttpContextAccessor httpContext, UserManager<Customer> userManager, IUnitOfWork unitOfWork, IRepository<Order> repository)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        //[Authorize(Roles = "admin, manager, user")]
        public async Task<IActionResult> Index(string searchstring)
        {
            var model = await orderService.GetAll();
            if (!String.IsNullOrEmpty(searchstring))
            {
                model = unitOfWork.Repo<Order>().Find(s => s.Address.Contains(searchstring));
            }
            var userId = httpContext.HttpContext.User.Identity.Name;
            if (userId == null)
            {
                return View(model);
            }
            
            var user = await userManager.FindByEmailAsync(userId);
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Contains("admin") || roles.Contains("manager"))
            {
                return View(model);
            }
            foreach (var item in model)
            {
                if (item.CustomerId == user.Id)
                {
                    item.customer = user;
                }
            }
            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            //TODO: Rewrite
            order.IssueDate.ToUniversalTime();
            order.IssueDate = order.IssueDate.ToUniversalTime(); 
            var userId = httpContext.HttpContext.User.Identity.Name;
            var user = await userManager.FindByEmailAsync(userId);

            //var supplier = unitOfWork.Repo<Supplier>().Find(s => s.Id == 1).FirstOrDefault();
            //order.Supplier = supplier;
            //order.SupplierId = supplier.Id;

            order.CustomerId = userId;
            order.customer = user;
            order.CompletionDate = null;
            //await orderService.Add(order);
            return RedirectToAction("AddToOrder", order);
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


       // public IActionResult AddToOrder() => View();
        
        //[HttpPost]
        public async Task<IActionResult> AddToOrder(Order order)
        {
            //var order = await orderService.Get(id);

            if (order != null)
            {
                var products = await productService.GetAll();
                var suppliers = await unitOfWork.Repo<Supplier>()
                    .GetAllAsync();
                var supToShow = suppliers.Select(s => s.Category)
                    .Intersect(products.Select(s => s.Category));

                var model = new OrderProductsViewModel
                {
                    Order = order,
                    Products = products,
                    Suppliers = suppliers
                };
                var orderView = new OrderView
                {
                    Address = order.Address,
                    CompletionDate = order.CompletionDate,
                    CustomerId = order.CustomerId,
                    IssueDate = order.IssueDate
                };

                unitOfWork.Repo<OrderView>().Add(orderView);
                await unitOfWork.Repo<OrderView>().SaveChangesAsync();

                model.Id = orderView.Id;

                return View(model);
            }
            return NotFound();
        }

        public async Task<IActionResult> AddOrder(int orderId, List<string>productsNames, string supplierName)
        {
            var orderView = await unitOfWork.Repo<OrderView>().GetByIdAsync(orderId);

            if (orderView != null)
            {
                var Customer = await userManager.FindByEmailAsync(orderView.CustomerId);
                var order = new Order
                {
                    Address = orderView.Address,
                    CompletionDate = orderView.CompletionDate,
                    //customer = orderView.customer,
                    CustomerId = orderView.CustomerId,
                    customer = Customer,
                    Id = orderView.Id,
                    IssueDate = orderView.IssueDate
                };

                var supplier = unitOfWork.Repo<Supplier>().Find(s => s.Name == supplierName).FirstOrDefault();
                var products = new List<Product>();
                foreach (var item in productsNames)
                {
                    products.Add(unitOfWork.Repo<Product>().Find(s=>s.Name.Contains(item)).FirstOrDefault());
                }
                
                order.Products = products;
                order.Supplier = supplier;
                order.SupplierId = supplier.Id;
                await orderService.Add(order);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetProductsInOrder(int id)
        {
            var order = await unitOfWork.Repo<Order>().GetByIdAsync(id);
            var products = repository.GetContext()
                .Products.Include(s => s.Orders)
                .Where(t => t.Orders.Any(d=>d.Id == id))
                .ToList();
                //.AsEnumerable();
            
            
            //var products = await unitOfWork.Repo<Product>().GetAllAsync();
            //products = products.Where(x => x.Orders.Any(c => c.Id == id));

            return View(products);
        }

        public async Task<IActionResult> SetDoneDate(int id)
        {
            var order = await unitOfWork.Repo<Order>().GetByIdAsync(id);

            order.CompletionDate = DateTime.Now;
            await unitOfWork.Repo<Order>().SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
