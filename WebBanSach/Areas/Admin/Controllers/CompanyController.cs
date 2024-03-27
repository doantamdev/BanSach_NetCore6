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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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
            Company company = new Company();


            if (id == null || id == 0)
            {
                // Create product              
                return View(company);
            }
            else
            {
                // update product              
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

            }


            return View(company);
        }

        // post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id==0)
                {
                    _unitOfWork.Company.Add(obj);
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                }

                _unitOfWork.Save();
                return RedirectToAction("index");
            }
            return View(obj);
        }

        #region API_CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            //var companyList = _unitOfWork.Company.GetAll();

            IIterator<Company> iterator = new CompanyIterator(_unitOfWork);
            List<Company> companyList = new List<Company>();

            while (iterator.HasNext())
            {
                companyList.Add(iterator.Next());
            }

            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            else
            {

                _unitOfWork.Company.Remove(obj);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Delete Successful" });
            }


            return View(obj);
        }
        #endregion
    }
}
