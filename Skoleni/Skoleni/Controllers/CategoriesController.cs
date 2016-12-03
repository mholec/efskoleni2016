using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skoleni.Repositories;
using Skoleni.ViewModels;

namespace Skoleni.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly Example2Repository _repository;

        public CategoriesController(Example2Repository repository)
        {
            this._repository = repository;
        }

        public ActionResult Index()
        {
            var model = _repository.GetAll();
            var viewModel = model.Select(c => new CategoryViewModel()
            {
                Id = c.CategoryId,
                Title = c.Title
            }).ToList();

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        public ActionResult Create(CategoryViewModel category)
        {
            return null;
        }

        public ActionResult Edit(int id)
        {
            var model = _repository.GetById(id);
            var viewModel = new CategoryViewModel()
            {
                Title = model.Title
            };

            return View(viewModel);
        }

        public ActionResult Edit(CategoryViewModel category)
        {
            return null;
        }

        public ActionResult Delete(int categoryId)
        {
            return null;
        }
    }
}