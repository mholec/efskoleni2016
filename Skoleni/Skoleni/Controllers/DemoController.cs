using System.Linq;
using System.Web.Mvc;
using Skoleni.Repositories;

namespace Skoleni.Controllers
{
    public class DemoController : Controller
    {
        private readonly Example1Repository _repository;

        public DemoController(Example1Repository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            // var result = _repository.GetBooksByFavoriteCategories("mholec");
            //var result = _repository.GetAllPaperbacks();
            var result = _repository.GetNewBooks();

            return Content("");
        }
    }
}