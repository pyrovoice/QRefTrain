﻿using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class QuestionSuiteController : Controller
    {
        // GET: QuestionSuite
        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            User currentUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            QuestionSuiteViewModel qsvm = new QuestionSuiteViewModel()
            {
                PreviouslyCreatedQuestionSuiteCode = null,
                UserQuestionSuites = Dal.Instance.GetQuestionSuiteByUser(currentUser)
            };
            return View(qsvm);
        }

        [HttpPost]
        public ActionResult MoveToQuestionSuite()
        {
            return View("CreateQuestionSuite", Dal.Instance.getAllQuestionsExceptRetired());
        }

        [HttpPost]
        public ActionResult CreateQuestionSuite(String name, List<string> questionIds, string timeLimit)
        {

            //Check at least one question was selected and no more than 50
            if (questionIds.Count < 1 || questionIds.Count > 50)
            {
                ViewBag.MailError = QRefResources.Resource.QuestionSuite_ErrorNbrQuestion;
                return View("CreateQuestionSuite", Dal.Instance.getAllQuestionsExceptRetired());
            }
            //Check time limit is valid. If empty, set a default time limit
            if (String.IsNullOrEmpty(timeLimit))
            {
                timeLimit = questionIds.Count.ToString();
                
            }else if(timeLimit.Length > 2)
            {
                timeLimit = "90";
            }
            else if (!Int32.TryParse(timeLimit, out int test))
            {
                ViewBag.MailError = QRefResources.Resource.QuestionSuite_ErrorTime;
                return View("CreateQuestionSuite", Dal.Instance.getAllQuestionsExceptRetired());
            }
            //Check name is valid. If empty, set a default name
            if (String.IsNullOrEmpty(name))
            {
                name = "MyCustomExam";
            }
            else if (name.Length > 20)
            {
                name = name.Substring(0, 19);
            }

            //Create the suite
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homepage", "Home");
            }
            User currentUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            QuestionSuite newQuestionSuite = new QuestionSuite()
            {
                Code = GenerateNewCode(),
                Name = name,
                Owner = currentUser,
                Questions = Dal.Instance.GetQuestionsById(questionIds),
                TimeLimit = Int32.Parse(timeLimit)
            };

            //Update db with new suite
            Dal.Instance.CreateQuestionSuite(newQuestionSuite);

            QuestionSuiteViewModel qsvm = new QuestionSuiteViewModel()
            {
                PreviouslyCreatedQuestionSuiteCode = newQuestionSuite.Code,
                UserQuestionSuites = Dal.Instance.GetQuestionSuiteByUser(currentUser)
            };
            //Return to the main page and add a field to copy the suite's ID
            return View("Index", qsvm);
        }

        [HttpPost]
        public ActionResult Delete(int hiddenSuiteId)
        {
            Dal.Instance.DeleteQuestionSuiteById(hiddenSuiteId);
            return RedirectToAction("Index");
        }

            /// <summary>
            /// Generate a new 6 character code at random using all upper case letters and numbers, then checks that the code don't already exist.
            /// </summary>
            /// <returns>The new code generated</returns>
            private String GenerateNewCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code;
            do
            {
                code = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

            } while (Dal.Instance.GetQuestionSuiteByCode(code) != null);
            return code;

        }

    }
}