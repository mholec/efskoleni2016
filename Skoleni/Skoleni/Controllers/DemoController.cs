using System.Collections.Generic;
using System.Web.Mvc;
using Skoleni.Entities;
using Skoleni.Repositories;

namespace Skoleni.Controllers
{
    public class DemoController : Controller
    {
        private readonly Repository _repository;

        public DemoController(Repository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            // var result = _repository.GetBooksByFavoriteCategories("mholec");
            //var result = _repository.GetAllPaperbacks();
            var result = _repository.GetBooksContainingWord("Amaz", "knih");

            return Content("");
        }
    }
}