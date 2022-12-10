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
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.productService = productService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string searchstring)
        {
            var model = await productService.GetAll();

            if (!String.IsNullOrEmpty(searchstring))
            {
                model = _unitOfWork.Repo<Product>().Find(s => s.Name.Contains(searchstring));
            }

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
        //[HttpPut("[action]/{id}")]
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