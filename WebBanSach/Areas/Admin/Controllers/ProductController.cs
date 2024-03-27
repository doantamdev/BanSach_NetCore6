using BanSach.DataAccess.Data;
using BanSach.DataAccess.Repository.IRepository;
using BanSach.Models;
using BanSach.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using WebBanSach.Areas.IteratorPattern;



namespace WebBanSach.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM();

            productVM.product = new Product();

            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem
                {
                    Text=u.Name,
                    Value=u.Id.ToString()
                }
                );
            productVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() }
                );


            if (id == null || id == 0)
            {
                // Create product              
                return View(productVM);
            }
            else
            {
                // update product              
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            }


            return View(productVM);
        }

        // post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // upload images
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file!=null)
                {
                    string fileName = Guid.NewGuid().ToString(); //random tên image
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName); //lấy định dạng
                    if (obj.product.ImageUrl!=null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\')); //lấy hình ảnh và xóa ảnh cũ
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.product.ImageUrl = @"images\products\"+ fileName + extension;

                }
                if (obj.product.Id==0)
                {
                    _unitOfWork.Product.Add(obj.product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);
                }

                _unitOfWork.Save();
                TempData["Sucess"] = "Sucessfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

            IIterator<Product> iterator = new ProductIterator(_unitOfWork);
            List<Product> displayed = new List<Product>();

            while (iterator.HasNext())
            {
                displayed.Add(iterator.Next());
            }

            return Json(new { data = displayed });
        }

        [HttpDelete]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                if (obj.ImageUrl != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Product.Remove(obj);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Delete Successful" });
            }


            return View(obj);
        }

        #endregion
    }
}
