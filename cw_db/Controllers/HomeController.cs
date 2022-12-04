using cw_db.Interfaces;
using cw_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace cw_db.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await productService.GetAll();

            return View("Index", model);
        }

        [Authorize(Roles = "admin, manager")]
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            await productService.Add(product);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, manager")]
        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> EditProduct(int id, [FromBody] Product product)
        {
            if (id == product.Id)
            {
                return BadRequest();
            }
            await productService.Edit(id, product);
            return Ok();
        }

        [Authorize(Roles = "admin, manager")]
        [HttpPut("[action]/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await productService.Delete(id);
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}