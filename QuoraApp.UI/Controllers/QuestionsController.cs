using QuoraApp.ServiceLayer;
using QuoraApp.UI.CustomFilters;
using QuoraApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace QuoraApp.UI.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionsService qs;
        IAnswersService asw;
        ICategoriesService cs;
        public QuestionsController(IQuestionsService qs,IAnswersService asw,ICategoriesService cs)
        {
            this.qs = qs;
            this.asw = asw;
            this.cs = cs;
        }

        public ActionResult View(int id)
        {
            this.qs.UpdateQuestionViewsCount(id, 1);
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(id, uid);
            return View(qvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UseAuthorizationFilterAttribute]
        public ActionResult AddAnswer(NewAnswerViewModel navm)
        {
            navm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
            navm.AnswerDateAndTime = DateTime.Now;
            navm.VotesCount = 0;
            if (ModelState.IsValid)
            {
                this.asw.InsertAnswer(navm);
                return RedirectToAction("View", "Questions", new { id = navm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                QuestionViewModel qvm = this.qs.GetQuestionByQuestionID(navm.QuestionID, navm.UserID);
                return View(qvm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAnswer(EditAnswerViewModel eavm)
        {
            if (ModelState.IsValid)
            {
                eavm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.asw.UpdateAnswer(eavm);
                return RedirectToAction("View", new { id = eavm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return RedirectToAction("View", new { id = eavm.QuestionID });
            }           
        }
        public ActionResult Create()
        {
            List<CategoryViewModel> categories = this.cs.GetCategories();
            ViewBag.categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UseAuthorizationFilter]
        public ActionResult Create(NewQuestionViewModel qvm)
        {
            if (ModelState.IsValid)
            {
                qvm.AnswersCount = 0;
                qvm.ViewsCount = 0;
                qvm.VotesCount = 0;
                qvm.QuestionDateAndTime = DateTime.Now;
                qvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.qs.InsertQuestion(qvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View();
            }
        }
    }
}