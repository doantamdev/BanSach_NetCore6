using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Mvc;


namespace WebBanSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        // post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name must not same displayorder");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "Category Create Sucessfully";
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
            //var categoryfromDb=_db.Categories.Find(id);
            var categoryfromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryfromDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryfromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryfromDbFirst);
        }

        // post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Name must not same displayorder");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "Category Update Sucessfully";
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
            var categoryfromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryfromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //var categoryfromDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryfromDb == null)
            {
                return NotFound();
            }

            return View(categoryfromDb);
        }

        // post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryfromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //var categoryfromDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["Sucess"] = "Category Delete Sucessfully";
                return RedirectToAction("index");
            }


            return View(obj);
        }

    }
}
