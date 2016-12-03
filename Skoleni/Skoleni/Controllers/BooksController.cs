using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Skoleni.Repositories;
using Skoleni.ViewModels;

namespace Skoleni.Controllers
{
    public class BooksController : Controller
    {
        private readonly Repository _repository;

        public BooksController(Repository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            List<BookViewModel> viewmodel = _repository.GetAllBooks().Select(b => new BookViewModel
            {
                BookPrice = b.Price,
                BookTitle = b.Title,
                CategoryTitle = b.Category.Title
            }).ToList();

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Index(BooksFilter filter)
        {
            List<BookViewModel> viewmodel = _repository.GetAllBooks(filter)
                .Select(b => new BookViewModel
            {
                BookPrice = b.Price,
                BookTitle = b.Title,
                CategoryTitle = b.Category.Title
            }).ToList();

            return View(viewmodel);
        }

        public ActionResult Grouped()
        {
            Dictionary<int, List<BookViewModel>> viewmodel = _repository.GetAllBooksGroupedByCategory()
                .ToDictionary(x => x.Key, x => x.Value.Select(b => new BookViewModel
                {
                    BookPrice = b.Price,
                    BookTitle = b.Title,
                    CategoryTitle = b.Category.Title
                }).ToList());

            return View(viewmodel);
        }
    }
}