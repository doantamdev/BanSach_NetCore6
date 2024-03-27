using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanSach.DesignPattern_Tam.Command;
using WebBanSach.DesignPattern_Tam.Observer;


namespace WebBanSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private static Stack<IUndoItem> _undoItem = new Stack<IUndoItem>();
        private readonly CategoryObserver _publisher;
        public CategoryController(IUnitOfWork unitOfWork, Stack<IUndoItem> undoItem,ApplicationDbContext db, CategoryObserver categoryObserver)
        {
            _unitOfWork = unitOfWork;
            _undoItem = undoItem;
            _db = db;   
            _publisher = categoryObserver;
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
        [HttpPost]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult Create(Category cate)
        {
            if (cate.Name == cate.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Name must not same displayorder"); //add validate
            }
            if (ModelState.IsValid) //thỏa Validate
            {
                _unitOfWork.Category.Add(cate);
                _unitOfWork.Save();
                TempData["Sucess"] = "Category Create Sucessfull";
                _publisher.Notify("New category added: " + cate.Name);
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

            var command = new DeleteCommand<Category>(_db, cateFromDb.Id, _unitOfWork);
            _undoItem.Push(command);
            command.Execute();

            return RedirectToAction("Index");
        }


        // Áp dụng Command Pattern để xoá
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //chống giả mạo method
        public IActionResult DeletePost(int? id)
        {
            var cateFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (cateFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(cateFromDb);
            _unitOfWork.Save();
            TempData["Sucess"] = "Delete Category Sucessfull";

            return RedirectToAction("Index");
        }



        // Áp dụng Command Pattern để Undo
        public IActionResult Undo()
        {
            if (_undoItem.Count > 0)
            {
                var undoCommand = _undoItem.Pop();
                // Chuyển đối số DbContextOptions vào constructor của ApplicationDbContext
                using (var undo = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()))
                {
                    undoCommand.Undo();
                    TempData["Success"] = "Undo operation successful.";
                }
            }
            else
            {
                TempData["Error"] = "No undo operations available.";
            }

            return RedirectToAction("Index");
        }

    }
}
