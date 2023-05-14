using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

        //---------------------------------------------------------------------------------------------------------------------------
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
        public IActionResult SubmitSelections(string user,string[] selectedItems)
        {
            if (!string.IsNullOrWhiteSpace(user) && user.Length <= 50)
            {
                for (int i = 0; i < selectedItems.Length; i++)
                {
                    var VoteRecord_Item = _dbContext.SP_GetVoteItemSnByItem(selectedItems[i]);
                    if (VoteRecord_Item.Count() > 0)
                    {
                        foreach (var item in VoteRecord_Item)
                        {
                            if (_dbContext.SP_CheckVoteRecordExist(user, int.Parse(item.Sn.ToString())).Count() == 0)
                            {
                                _dbContext.SP_SetVoteRecord(user, int.Parse(item.Sn.ToString()));
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "使用者已在此項目投票過了";
                            }
                        }
                    }
                    
                }
            }
            else 
            {
                TempData["ErrorMessage"] = "使用者名稱無效";
            }
            return RedirectToAction("View_VoteClient");
        }
        //---------------------------------------------------------------------------------------------------------------------------
        public IActionResult View_VoteManager()
        {
            var VoteItem = _dbContext.SP_GetVoteItem();
            return View("View_VoteManager", VoteItem);
        }
        public IActionResult EditItem(int itemId)
        {
            if (itemId == null)
            {
                //TempData["ErrorMessage"] = "使用者名稱無效";
                return RedirectToAction("View_VoteManager");
            }
           

            var VoteItem = _dbContext.SP_GetVoteItemItemBySn(itemId);
            return View("View_EditItem", VoteItem);
        }
        [HttpPost]
        public IActionResult AddNewItem(string newItem)
        {
            if (string.IsNullOrEmpty(newItem))
            {
                TempData["ErrorMessage"] = "投票項目必須填寫";
                return RedirectToAction("View_VoteManager");
            }
            else if (newItem.Length > 50)
            {
                TempData["ErrorMessage"] = "投票項目長度過大";
                return RedirectToAction("View_VoteManager");
            }
            
            if (_dbContext.SP_CheckVoteItemExist(newItem).Count() == 0)
            {
                _dbContext.SP_SetVoteItem(newItem);
            }
            else
            {
                TempData["ErrorMessage"] = "投票項目已存在";
            }

            return RedirectToAction("View_VoteManager");
        }
        [HttpPost]
        public IActionResult UpdateItem(int itemId, string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                TempData["ErrorMessage"] = "投票項目必須填寫";
                return RedirectToAction("View_VoteManager");
            }
            else if (itemName.Length > 50) 
            {
                TempData["ErrorMessage"] = "投票項目長度過大";
                return RedirectToAction("View_VoteManager");
            }
            if (_dbContext.SP_CheckVoteItemExist(itemName).Count() == 0)
            {
                _dbContext.SP_UpdateVoteItem(itemId, itemName);
            }
           
            return RedirectToAction("View_VoteManager");
        }

        [HttpPost]
        public IActionResult DeleteItem(int itemId)
        {
            if (itemId == null)
            {
                return RedirectToAction("View_VoteManager");
            }
            _dbContext.SP_DeleteVoteRecord(itemId);
            _dbContext.SP_DeleteVoteItem(itemId);
            
            return RedirectToAction("View_VoteManager");
        }
    }
}