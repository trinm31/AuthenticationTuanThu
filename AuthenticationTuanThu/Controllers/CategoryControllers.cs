using System.Linq;
using AuthenticationTuanThu.Data;
using AuthenticationTuanThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTuanThu.Controllers
{
    public class CategoriesController : Controller
    {
        // đănt kí thằng db đẻ có mà dùng
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // lấy toàn bộ data từ db lên
            var listAlldata = _db.Categories.ToList();
            return View(listAlldata);
        }
        [Authorize]
        [HttpGet]
        // chức năng này dùng để hiện thị view cho cả insert và update
        public IActionResult Upsert(int? id) // allow null input id ý là truyền vào id thì có nghĩa là update ko truyền thì create
        {
            // trường hợp create thì ko truyền vào id tương ứng vs id = null
            if (id == null)
            {
                // trả về view là một obj rông 
                return View(new Category());
            }

            // trường hợp update thì xuống db tìm kiếm theo id và trả về obj tương ứng
            var categories = _db.Categories.Find(id);
            return View(categories);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Upsert(Category category)
        {
            // kiểm tra điều kiện
            if (ModelState.IsValid)
            {
                // create case
                if (category.Id == 0)
                {
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

                // update case
                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
            
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // get obj cần remove
            var categoryNeedToDelete = _db.Categories.Find(id);
            // remove nó
            _db.Categories.Remove(categoryNeedToDelete);
            // lưu vào db
            _db.SaveChanges();
            // chuyển hướng về lại trang index
            return RedirectToAction(nameof(Index));
        }
    }
}