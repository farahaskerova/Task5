using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Task.Controllers.Admin
{

    [Route("admin/catigories")]
    public class CatigoriesController : Controller
    {
        private readonly TaskDbContext _taskDbContext;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            TaskDbContext TaskDbContext,
            ILogger<TaskController> logger)
        {
            _taskDbContext = TaskDbContext;
            _logger = logger;
        }

        #region Categories

        [HttpGet] //admin/Categories
        public IActionResult Categories()
        {
            var Categories = _taskDbContext.Categories
                .Include(p => p.Category)
                .ToList();

            return View("Views/Admin/Category/Categories.cshtml", Categories);
        }

        #endregion

        #region Add

        [HttpGet("add")]
        public IActionResult Add()
        {
            var model = new ProductAddResponseViewModel
            {
                Categories = _taskDbContext.Categories.ToList(),
                Colors = _taskDbContext.Colors.ToList()
            };

            return View("Views/Admin/Category/ProductAdd.cshtml", model);
        }

        [HttpPost("add")]
        public IActionResult Add(ProductAddRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return PrepareValidationView("Views/Admin/Category/ProductAdd.cshtml");

            if (model.CategoryId != null)
            {
                var category = _taskDbContext.Categories.FirstOrDefault(c => c.Id == model.CategoryId.Value);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Category doesn't exist");

                    return PrepareValidationView("Views/Admin/Category/ProductAdd.cshtml");
                }
            }

            try
            {
                var Category = new Category
                {
                    Name = model.Name,
                    CategoryId = model.CategoryId,
                };

                _taskDbContext.Categories.Add(Category);

                _taskDbContext.SaveChanges();

            }
            catch (PostgresException e)
            {
                _logger.LogError(e, "Postgresql Exception");

                throw e;
            }

            return RedirectToAction("Categories");
        }

        #endregion

        #region Edit

        [HttpGet("edit")]
        public IActionResult Edit(int id)
        {
            Category Category = _taskDbContext.Categories
                .Include(p => p.ProductColors)
                .FirstOrDefault(p => p.Id == id);

            if (Category == null)
                return NotFound();

            var model = new TaskUpdateResponseViewModel
            {
                Id = Category.Id,
                Name = Category.Name,
                Categories = _taskDbContext.Categories.ToList(),
                CategoryId = Category.CategoryId,              
            };

            return View("Views/Admin/Category/ProductEdit.cshtml", model);
        }

        [HttpPost("edit")]
        public IActionResult Edit(TaskUpdateRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return PrepareValidationView("Views/Admin/Category/ProductEdit.cshtml");

            if (model.CategoryId != null)
            {
                var category = _taskDbContext.Categories.FirstOrDefault(c => c.Id == model.CategoryId.Value);
                if (category == null)
                {
                    ModelState.AddModelError("CategoryId", "Category doesn't exist");

                    return PrepareValidationView("Views/Admin/Category/ProductAdd.cshtml");
                }
            }

            Category Category = _taskDbContext.Categories
                .Include(p => p.ProductColors)
                .FirstOrDefault(p => p.Id == model.Id);

            if (Category == null)
                return NotFound();

            try
            {
                Category.Name = model.Name;
                Category.CategoryId = model.CategoryId;

                _taskDbContext.SaveChanges();
            }
            catch (PostgresException e)
            {
                _logger.LogError(e, "Postgresql Exception");

                throw e;
            }


            return RedirectToAction("Categories");
        }

        #endregion

        #region Delete

        [HttpGet("delete")]
        public IActionResult Delete(int id)
        {
            Category Category = _taskDbContext.Categories
                .FirstOrDefault(p => p.Id == id);
            if (Category == null)
            {
                return NotFound();
            }

            _taskDbContext.Remove(Category);
            _taskDbContext.SaveChanges();


            return RedirectToAction("Categories");
        }

        #endregion

        private IActionResult PrepareValidationView(string viewName)
        {
            var responseViewModel = new TaskAddResponseViewModel
            {
                Categories = _taskDbContext.Categories.ToList(),
                Colors = _taskDbContext.Colors.ToList()
            };

            return View(viewName, responseViewModel);
        }
    }

}
