using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Mvc;


namespace WebBanSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult Create(CoverType obj)
        {
            
            if (ModelState.IsValid) //thỏa Validate
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "CoverType Create Sucessfull";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var cateFromDb = _db.Categories.Find(id);
            var coverTypefromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypefromDbFirst == null)
            {
                return NotFound();
            }
            return View(coverTypefromDbFirst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid) //thỏa Validate
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "Cover Type Create Sucessfull";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            { 
                return NotFound();
            }
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult DeletePost(int? id)
        {
            var coverTypeFromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.CoverType.Remove(coverTypeFromDb);
                _unitOfWork.Save();
                TempData["Sucess"] = "CoverType Delete Sucessfull";
                return RedirectToAction("Index");
            }

            return View(coverTypeFromDb);
        }
    }
}
