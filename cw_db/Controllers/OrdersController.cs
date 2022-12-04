﻿using cw_db.Interfaces;
using cw_db.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cw_db.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;

        public OrdersController(IOrderService orderService, IProductService productService)
        {
            this.orderService = orderService;
            this.productService = productService;
        }

        [Authorize(Roles = "admin, manager, user")]
        public async Task<IActionResult> Index()
        {
            var model = await orderService.GetAll();

            return View("Index", model);
        }

        public async IActionResult Create()
        {
            return View();
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
            //TODO: Redo like in roles controller
            var viewModel = new OrderProductsViewModel();
            
            return View(model);
        }

        public async Task AddToOrder(int id, Product product)
        {
            await orderService.AddProductToOrder(id, product);
        }

        public async Task<IActionResult> AddOrder(Order order)
        {
            await orderService.Add(order);
            return RedirectToAction("Index");
        }
        
        //TODO: CWDB: Add edit functions
    }
}
