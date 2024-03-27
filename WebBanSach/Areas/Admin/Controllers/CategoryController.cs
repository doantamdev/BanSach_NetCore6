using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Mvc;
using WebBanSach.Areas.Admin.DecoratorPattern;
using WebBanSach.Areas.IteratorPattern;


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
            //IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();

            IIterator<Category> iterator = new CateIterator(_unitOfWork);
            List<Category> displayedCategories = new List<Category>();

            while (iterator.HasNext())
            {
                displayedCategories.Add(iterator.Next());
            }

            return View(displayedCategories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult Create(Category cate)
        {
            BaseValidate baseValidate = new BaseValidate();
            CateValidateDecorator CateValidator = new CateValidateDecorator(baseValidate);

            if (cate.Name == cate.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name must not same displayorder"); //add validate
            }

            if (ModelState.IsValid && CateValidator.CateValidate(cate)) //thỏa Validate
            {
                _unitOfWork.Category.Add(cate);
                _unitOfWork.Save();
                TempData["Sucess"] = "Category Create Sucessfull";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Failed"] = "Category Create failed!";
                return RedirectToAction("Index");
            }

            return View(cate);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var cateFromDb = _db.Categories.Find(id);
            var categoryfromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (categoryfromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryfromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult Edit(Category cate)
        {
            if (cate.Name == cate.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name must not same displayorder"); //add validate
            }
            if (ModelState.IsValid) //thỏa Validate
            {
                _unitOfWork.Category.Update(cate);
                _unitOfWork.Save();
                TempData["Sucess"] = "Category Create Sucessfull";
                return RedirectToAction("Index");
            }
            return View(cate);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var cateFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (cateFromDb == null)
            {
                return NotFound();
            }
            return View(cateFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult DeletePost(int? id)
        {
            var cateFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (cateFromDb == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Category.Remove(cateFromDb);
                _unitOfWork.Save();
                TempData["Sucess"] = "Delete Category Sucessfull";
                return RedirectToAction("Index");
            }

            return View(cateFromDb);
        }
    }
}
