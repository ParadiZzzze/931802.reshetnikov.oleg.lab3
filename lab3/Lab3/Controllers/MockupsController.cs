using System;
using Lab3.Models;
using Lab3.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers {
    public class MockupsController : Controller {
        private int QuestionCount {
            get {
                return HttpContext.Session.Get<int>(nameof(QuestionCount)) switch {
                    < 0 => throw new Exception("Invalid question count"),
                    { } count => count
                };
            }
        }

        private QuizQuestionModel NextQuestion {
            get {
                var question = QuizQuestionModel.RandomQuizQuestion;

                var count = QuestionCount;
                HttpContext.Session.Set($"Question{count}", question);
                count += 1;
                HttpContext.Session.Set(nameof(QuestionCount), count);

                return question;
            }
        }

        private QuizQuestionModel LastQuestion {
            get {
                var count = QuestionCount - 1;
                return HttpContext.Session.Get<QuizQuestionModel>($"Question{count}");
            }
        }

        private QuizResultModel Result {
            get {
                var result = new QuizResultModel {Questions = new()};
                for (var i = 0; i < QuestionCount; i++) {
                    var question = HttpContext.Session.Get<QuizQuestionModel>($"Question{i}");
                    result.Questions.Add(question);
                }

                return result;
            }
        }

        private void SaveAnswer(int? answer) {
            var lastQuestion = LastQuestion;
            lastQuestion.UserAnswer = 0;
            if (answer.HasValue) lastQuestion.UserAnswer = answer;
            HttpContext.Session.Set($"Question{QuestionCount - 1}", lastQuestion);
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Quiz() {
            var question = QuestionCount switch {
                0 => NextQuestion,
                _ => LastQuestion
            };
            ViewBag.Question = question.ToString();
            return View();
        }

        [HttpPost]
        public IActionResult Quiz(QuizAnswerModel answerModel, string action) {
            if (ModelState.IsValid) {
                if (answerModel.Answer < -10) {
                    ModelState.AddModelError("Answer", $"  {answerModel.Answer} слишком маленькое число для этого сайта" );
                    ViewBag.Question = LastQuestion;
                    return View();
                }
                SaveAnswer(answerModel.Answer);

                if (action == "Next") {
                    ViewBag.Question = NextQuestion;
                    return RedirectToAction("Quiz");
                }
                else {
                    return RedirectToAction("QuizResult");
                }
            }
            else {
                ViewBag.Question = LastQuestion;
                return View();
            }
        }
        public IActionResult QuizResult() {
            var result = Result;

            HttpContext.Session.Clear();

            return View(result);
        }
    }
}
