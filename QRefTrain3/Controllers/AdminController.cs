using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;

namespace QRefTrain3.Controllers
{
    public class AdminController : BaseController
    {

        private readonly string filepath = "C:\\Users\\maxim\\Desktop\\data.csv";
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "QRefTrain";

        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homepage", "Home");
            }
            else
            {
                User u = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
                if (u.UserRole != UserRole.Admin)
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            /*
            User user = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            if (user.UserRole != UserRole.Admin)
            {
                Dal.Instance.CreateLog(new Log()
                {
                    LogText = "User tried to access admin page without right. User : " + user.Name + ", Right : " + user.UserRole,
                    LogTime = DateTime.Now,
                    UserId = user.Id

                });
                return RedirectToAction("Homepage", "Home");
            }*/
            return View("Index", Dal.Instance.getAllLogs());
        }

        [HttpPost]
        public ActionResult Export()
        {
            string test = "";
            string delimiter = ";";
            foreach (Question q in Dal.Instance.getAllQuestions())
            {
                test += q.QuestionText + delimiter;
                Boolean toremove = false;
                foreach (Answer a in q.Answers)
                {
                    if (a.IsTrue)
                    {
                        toremove = true;
                        test += a.Answertext + "\\N";
                    }
                }
                if (toremove)
                {
                    test = test.Remove(test.Length - 2);
                    toremove = false;
                }
                test += delimiter;
                foreach (Answer a in q.Answers)
                {
                    if (!a.IsTrue)
                    {
                        toremove = true;
                        test += a.Answertext + "\\N";
                    }
                }
                if (toremove)
                {
                    test = test.Remove(test.Length - 2);
                    toremove = false;
                }
                test += delimiter;
                test += q.AnswerExplanation + delimiter + q.NationalGoverningBodies.Replace(";", "-") + delimiter + q.GifName + delimiter + q.Subject.ToString() + delimiter + q.Id;
                test += "\n";
            }

            System.IO.File.WriteAllText(filepath, test);

            return View("Index");
        }

        [HttpPost]
        public ActionResult Import()
        {
            var lines = System.IO.File.ReadAllLines(filepath);
            List<Question> questions = new List<Question>();
            foreach (string line in lines)
            {
                Question q = new Question();
                string[] data = line.Split(';');
                q.QuestionText = data[0];
                q.AnswerExplanation = data[1];
                q.Subject = (QuestionSubject)Enum.Parse(typeof(QuestionSubject), data[2]);
                q.GifName = data[3];
                q.NationalGoverningBodies = data[4];
                q.Answers = new List<Answer>();
                for (int i = 5; i < data.Count() - 1; i += 2)
                {
                    Answer a = new Answer()
                    {
                        Answertext = data[i],
                        IsTrue = data[i + 1].Equals("True")
                    };
                    q.Answers.Add(a);
                }
                Dal.Instance.CreateQuestion(q);
            }

            return RedirectToAction("QuestionListing");
        }

        public ActionResult QuestionListing()
        {
            var questions = Dal.Instance.getAllQuestions();
            foreach (Question q in questions)
            {
                q.Answers.Sort((val1, val2) => val1.CompareTo(val2));
            }
            return View("QuestionsListing", questions);
        }
        /*
        [HttpPost]
        public ActionResult AddQuestion(string BaseName, bool isNoPenalty, bool isTurnOver, bool isBlueCard, bool isYellowCard, bool isRedCard, bool isBackToHoops,
            bool isNoPenaltyTrue, bool isTurnOverTrue, bool isBlueCardTrue, bool isYellowCardTrue, bool isRedCardTrue, bool isBackToHoopsTrue,
            bool isAnswer1, bool isAnswer2, bool isAnswer3, bool isAnswer4, bool isAnswer5, bool isAnswer6,
            bool isAnswerTrue1, bool isAnswerTrue2, bool isAnswerTrue3, bool isAnswerTrue4, bool isAnswerTrue5, bool isAnswerTrue6,
            string Video, string QuestionSubject, List<string> NGBs)
        {
            BaseName = "Question" + BaseName;
            List<Answer> answers = new List<Answer>();
            if (isAnswer1) { answers.Add(new Answer(BaseName + "Answer1", isAnswerTrue1)); }
            if (isAnswer2) { answers.Add(new Answer(BaseName + "Answer2", isAnswerTrue2)); }
            if (isAnswer3) { answers.Add(new Answer(BaseName + "Answer3", isAnswerTrue3)); }
            if (isAnswer4) { answers.Add(new Answer(BaseName + "Answer4", isAnswerTrue4)); }
            if (isAnswer5) { answers.Add(new Answer(BaseName + "Answer5", isAnswerTrue5)); }
            if (isAnswer6) { answers.Add(new Answer(BaseName + "Answer6", isAnswerTrue6)); }
            if (isNoPenalty)
            {
                answers.Add(new Answer("NoPenalty", isNoPenaltyTrue));
            }
            if (isTurnOver)
            {
                answers.Add(new Answer("Turnover", isTurnOverTrue));
            }
            if (isBlueCard)
            {
                answers.Add(new Answer("BlueCard", isBlueCardTrue));
            }
            if (isYellowCard)
            {
                answers.Add(new Answer("YellowCard", isYellowCardTrue));
            }
            if (isRedCard)
            {
                answers.Add(new Answer("RedCard", isRedCardTrue));
            }
            if (isBackToHoops)
            {
                answers.Add(new Answer("backToHoops", isBackToHoopsTrue));
            }

            Question questionToAdd = new Question((Models.QuestionSubject)Enum.Parse(typeof(Models.QuestionSubject), QuestionSubject),
                Video, BaseName + "Text", answers, BaseName + "Explanation", NGBs.ToArray());
            Dal.Instance.CreateQuestion(questionToAdd);
            return RedirectToAction("Index");
        }*/


        [HttpPost]
        public ActionResult ImportFromGoogleDrive()
        {
            UserCredential credential;
            using (var stream =
                new FileStream(Server.MapPath("\\Elements\\credentials.json"), FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            String spreadsheetId = "1opDpgHI6D0-zhaR33GLi027xwDtiJClj_XaiBZ2004w";
            Spreadsheet spreadsheet = service.Spreadsheets.Get(spreadsheetId).Execute();
            List<Question> importedQuestions = new List<Question>();
            foreach (Sheet sheet in spreadsheet.Sheets)
            {
                String range = sheet.Properties.Title + "!A2:G";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                        service.Spreadsheets.Values.Get(spreadsheetId, range);
                IList<IList<Object>> values = request.Execute().Values;
                //Check we have data
                if (values != null && values.Count > 0)
                {
                    //For each question (one per row)
                    foreach (var row in values)
                    {
                        Question newQuestion = new Question();
                        List<Answer> answers = new List<Answer>();
                        Enum.TryParse(sheet.Properties.Title, out QuestionSubject subject);
                        for (int i = 0; i < row.Count; i++)
                        {
                            switch (i)
                            {
                                // Public Id
                                case 0:
                                    newQuestion.PublicId = subject + "-" + row[i];
                                    break;
                                // Question Text
                                case 1:
                                    newQuestion.QuestionText = row[i] + "";
                                    break;
                                //Good answers
                                case 2:
                                    String[] allGoodAnswers = row[i].ToString().Split(new string[] { "\n" }, StringSplitOptions.None);
                                    for (int goodAnswerCounter = 0; goodAnswerCounter < allGoodAnswers.Length; goodAnswerCounter++)
                                    {
                                        if (!String.IsNullOrEmpty(allGoodAnswers[goodAnswerCounter]))
                                        {
                                            answers.Add(new Answer(allGoodAnswers[goodAnswerCounter], true));
                                        }
                                    }
                                    break;
                                //Bad Answers
                                case 3:
                                    String[] allBadAnswers = row[i].ToString().Split(new string[] { "\n" }, StringSplitOptions.None);
                                    for (int goodAnswerCounter = 0; goodAnswerCounter < allBadAnswers.Length; goodAnswerCounter++)
                                    {
                                        //When adding "Cards" to bad answers, it means that we want to offer the user all card penalty possible as bad answers, except the one that were already selected as good answers
                                        //For each card color possible, we check that this answer does not already exists in the good answers and if not, we add it to the bad answers
                                        if (allBadAnswers[goodAnswerCounter].Equals("Cards"))
                                        {
                                            if (answers.Find(u => u.Answertext.Equals("BlueCard")) == null)
                                            {
                                                answers.Add(new Answer("BlueCard", false));
                                            }
                                            if (answers.Find(u => u.Answertext.Equals("YellowCard")) == null)
                                            {
                                                answers.Add(new Answer("YellowCard", false));
                                            }
                                            if (answers.Find(u => u.Answertext.Equals("RedCard")) == null)
                                            {
                                                answers.Add(new Answer("RedCard", false));
                                            }
                                        }else if (!String.IsNullOrEmpty(allBadAnswers[goodAnswerCounter]))
                                        {
                                            answers.Add(new Answer(allBadAnswers[goodAnswerCounter], false));
                                        }
                                    }
                                    break;
                                //Answer explanation
                                case 4:
                                    newQuestion.AnswerExplanation = row[i] + "";
                                    break;
                                //NGB
                                case 5:
                                    newQuestion.NationalGoverningBodies = row[i] + "";
                                    break;
                                //Link
                                case 6:
                                    if (!String.IsNullOrEmpty(row[i] + ""))
                                    {
                                        newQuestion.GifName = row[i] + "";
                                    }
                                    break;
                            }
                        }
                        newQuestion.Answers = answers;
                        newQuestion.Subject = subject;
                        importedQuestions.Add(newQuestion);
                    }
                }
            }
            // We now have a list with all questions we try to import.
            List<Question> dbQuestions = Dal.Instance.getAllQuestionsExceptRetired();
            // For each question, try to find an equal question in the database. If you can, nothing to do here so we can remove both questions from the list.
            for (int questionCounter = importedQuestions.Count - 1; questionCounter >= 0; questionCounter--)
            {
                foreach (Question dbQ in dbQuestions)
                {
                    if (importedQuestions[questionCounter].IsSimilar(dbQ))
                    {
                        importedQuestions.RemoveAt(questionCounter);
                        dbQuestions.Remove(dbQ);
                        break;
                    }
                }
            }
            // Each question left in the question imported list is a new question that we should add to the database 
            foreach (Question q in importedQuestions)
            {
                Dal.Instance.CreateQuestion(q);
            }
            // Each question left in the database questions list is a question that should not be used anymore, so we can flag them as Retired
            foreach (Question q in dbQuestions)
            {
                Dal.Instance.RetireQuestion(q);
            }

            return RedirectToAction("Index");
        }
    }
}
