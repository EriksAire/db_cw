using cw_db.Interfaces;
using cw_db.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace cw_db.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<Customer> userManager;
        private readonly IHttpContextAccessor httpContext;

        public FeedbackController(IUnitOfWork unitOfWork, UserManager<Customer> userManager, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.httpContext = httpContext;
        }

        public async Task<IActionResult> Index(string searchstring)
        {
            var model = await unitOfWork.Repo<Feedback>().GetAllAsync();

            if (!String.IsNullOrEmpty(searchstring))
            {
                model = unitOfWork.Repo<Feedback>().Find(s => s.Title.Contains(searchstring));
            }
            foreach (var item in model)
            {
                var userId = httpContext.HttpContext.User.Identity.Name;
                var user = await userManager.FindByIdAsync(item.CustomerId);
                item.customer = user;
            }
            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> AddFeedback(Feedback feedback)
        {
            var userId = httpContext.HttpContext.User.Identity.Name;
            var user = await userManager.FindByEmailAsync(userId);
            feedback.customer = user;
            feedback.CustomerId = userId;
            feedback.date = DateTime.Now;
            unitOfWork.Repo<Feedback>().Add(feedback);
            await unitOfWork.Repo<Feedback>().SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
