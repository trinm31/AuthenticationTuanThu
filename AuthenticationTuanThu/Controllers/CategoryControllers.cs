﻿using System.Linq;
using AuthenticationTuanThu.Data;
using AuthenticationTuanThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationTuanThu.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var listAlldata = _db.Categories.ToList();
            return View(listAlldata);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if (id == null)
            {
                return View(new Category());
            }

            var categories = _db.Categories.Find(id);
            return View(categories);
        }
        
        [HttpPost]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _db.Categories.Add(category);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }

                _db.Categories.Update(category);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(category);
            
        }
        
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var categoryNeedToDelete = _db.Categories.Find(id);
            _db.Categories.Remove(categoryNeedToDelete);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}