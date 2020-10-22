using QuoraApp.ServiceLayer;
using QuoraApp.UI.CustomFilters;
using QuoraApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuoraApp.UI.Controllers
{
    public class HomeController : Controller
    {
        IQuestionsService qs;
        ICategoriesService cs;
        public HomeController(IQuestionsService _qs,ICategoriesService _cs)
        {
            this.qs = _qs;
            this.cs = _cs;
        }

        public ActionResult Index()
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions().Take(10).ToList();
            return View(questions);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [Route("allcategories")]
        [UseAuthorizationFilter]
        public ActionResult Categories()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            return View(categories);
        }
        [Route("allquestions")]
        [UseAuthorizationFilter]
        public ActionResult Questions()
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions();
            return View(questions);
        }
        public ActionResult Search(string str)
        {
            List<QuestionViewModel> questions = this.qs.GetQuestions().Where(x => x.QuestionName.ToLower().Contains(str.ToLower()) || x.Category.CategoryName.ToLower().Contains(str.ToLower())).ToList();
            ViewBag.str = str;
            return View(questions);
        }
    }
}