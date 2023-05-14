using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_InterViewTest.Models;
using static Web_InterViewTest.Models.DataBaseManager;

namespace Web_InterViewTest.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult View_VoteClient()
        {
            var VoteRecord_Item = _dbContext.SP_GetVoteRecordItemCount();
            return View("View_VoteClient", VoteRecord_Item);
        }

        [HttpPost]
        public IActionResult SubmitSelections(string[] selectedItems)
        {
            

            return RedirectToAction("View_VoteClient");
        }
    }
}