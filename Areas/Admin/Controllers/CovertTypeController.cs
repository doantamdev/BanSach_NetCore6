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
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.covertype.GetAll();
            return View(objCoverTypeList);
        }

        public IActionResult Create()
        {
            return View();
        }

        // post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.covertype.Add(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "CoverType Create Sucessfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }  
            
            var covertTypefromDbFirst = _unitOfWork.covertype.GetFirstOrDefault(u => u.Id == id);
          
            if (covertTypefromDbFirst == null)
            {
                return NotFound();
            }

            return View(covertTypefromDbFirst);
        }

        // post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {           
            if (ModelState.IsValid)
            {
                _unitOfWork.covertype.Update(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "Cover Type Update Sucessfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypefromDb = _unitOfWork.covertype.GetFirstOrDefault(u => u.Id == id);
           
            if (coverTypefromDb == null)
            {
                return NotFound();
            }

            return View(coverTypefromDb);
        }

        // post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.covertype.GetFirstOrDefault(u => u.Id == id);
          
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.covertype.Remove(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "Covert Type Delete Sucessfully";
                return RedirectToAction("index");
            }


            return View(obj);
        }

    }
}
