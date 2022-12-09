using cw_db.Interfaces;
using cw_db.Models;
using Microsoft.AspNetCore.Mvc;

namespace cw_db.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await supplierService.GetAll();

            return View(model);
        }

        [Obsolete]
        public IActionResult Create() => View();

        
        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {

            await supplierService.Add(supplier);
            return RedirectToAction("Index");
        }


    }
}
