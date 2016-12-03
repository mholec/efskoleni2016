﻿using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using Newtonsoft.Json;
using Skoleni.Entities;
using Skoleni.Extras;
using Skoleni.ViewModels;

namespace Skoleni.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppContext _db;

        public CategoriesController(AppContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            var model = _db.Categories.ToList();
            var viewModel = model.Select(c => new CategoryViewModel()
            {
                Id = c.CategoryId,
                Title = c.Title
            }).ToList();

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View("Form", new CategoryViewModel());
        }

        /// <summary>
        /// Př.: Chceme vytvořit zcela nový objekt
        /// </summary>
        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            Category category = new Category
            {
                Title = model.Title
            };

            _db.Categories.Add(category);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var model = _db.Categories.Find(id);
            var viewModel = new CategoryViewModel()
            {
                Id = model.CategoryId,
                Title = model.Title
            };

            return View("Form", viewModel);
        }

        /// <summary>
        /// Př.: Chceme aktualizovat konkrétní objekt
        /// </summary>
        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            Category category = _db.Categories.Find(model.Id);
            category.Title = model.Title;

            // Pokud nechci preloadovat
            //Category category = new Category
            //{
            //    CategoryId = model.Id,
            //    Title = model.Title
            //};
            //_db.Entry(category).State = EntityState.Modified;

            //_db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Př.: Chceme odstranit konkrétní objekt
        /// </summary>
        public ActionResult Delete(int id)
        {
            Category category = _db.Categories.Find(id);
            //category.ParentCategory = null; // Nutné odstranit reference, pokud nemám cascade delete
            //category.Books.ToList().ForEach(b => b.CategoryId = 4);
            _db.Categories.Remove(category);

            // Pokud nechci preloadovat
            //Category category = new Category() {CategoryId = id};
            //_db.Entry(category).State = EntityState.Deleted;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Aktualizace object graph
        /// Př.: Chci vložit kategorii i s nadřazenou kategorií a knihami
        /// </summary>
        public ActionResult Extras_AddManyObjects()
        {
            // vytvoření kompletního stromu
            var category = new Category()
            {
                Title = "Povídky",
                ParentCategory = new Category()
                {
                    Title = "Zlevněné knihy"
                },
                Books = new List<Book>()
                {
                    new Paperback() {Title = "Povídka o černém notebooku", Description = "Nová knížka", Size = PaperbackSize.A4, Price = new Price() {BasePrice = 0, VatRate = 0} },
                    new Paperback() {Title = "Humoreska", Description = "Trochu jiná kniha", Size = PaperbackSize.A4, Price = new Price() {BasePrice = 50, VatRate = 0.15M } }
                }
            };
            _db.Categories.Add(category);
            _db.SaveChanges();

            return View("Index");
        }

        /// <summary>
        /// Change Tracker API
        /// Př.: Chci ci auditovat změny na nějaké kolekci, třeba kategorie
        /// </summary>
        public ActionResult Extras_ChangeTrackerApi()
        {
            var category = _db.Categories.Include(x => x.Books).FirstOrDefault(b => b.CategoryId == 4);
            category.Title = "Naučte se C# za 21 hodin";
            category.Books.FirstOrDefault().Title = "Super kniha";
            category.ParentCategory = null;

            var result = new Auditor(_db.ChangeTracker).AuditChanges(true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}