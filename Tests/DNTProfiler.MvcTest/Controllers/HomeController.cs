using System.Web.Mvc;
using DNTProfiler.ServiceLayer;
using DNTProfiler.ServiceLayer.Contracts;
using DNTProfiler.TestEFContext.DataLayer;
using DNTProfiler.TestEFContext.Domain;

namespace DNTProfiler.MvcTest.Controllers
{
    public class HomeController : Controller
    {
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;
        readonly IUnitOfWork _uow;
        public HomeController(IUnitOfWork uow, IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _uow = uow;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var list = _productService.GetAllProductsIncludeCategory();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoriesList = new SelectList(_categoryService.GetAllCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (this.ModelState.IsValid)
            {
                _productService.AddNewProduct(product);
                _uow.SaveAllChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(Category category)
        {
            if (this.ModelState.IsValid)
            {
                _categoryService.AddNewCategory(category);
                _uow.SaveAllChanges();
            }
            return RedirectToAction("Index");
        }
    }
}