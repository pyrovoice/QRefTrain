using System;
using QRefTrain3.CustomException;
using QRefTrain3.Models;
using QRefTrain3.ViewModel;

namespace QRefTrain3.Helper
{
    public class QuizResolver
    {
        // 30 seconds are added as a security, in case the loading time was too high
        private static readonly int EXAM_TIME_LIMIT_SECURITY = 30000;
        protected Result ResultToValidate { get; set; }

        public QuizResolver(Result result)
        {
            ResultToValidate = result;
        }

        public void ValidateAndResolveQuizResult()
        {
            try
            {
                ValidateResult();
            }
            catch (ValidationExamException e)
            {
                Dal.Instance.CreateLog(new Log(LogLevel.ERROR, LogType.QUIZ_FINISHED, e.Message, Dal.Instance.GetDBTime(), ResultToValidate.User));
            }
            ResolveQuizResult();
        }


        private void ValidateResult()
        {
            if (IsResultOfficial())
            {
                ValidateTimeLimit();
            }
        }

        private void ValidateTimeLimit()
        {
            Quiz currentExam = Dal.Instance.GetOngoingExamByUsername(ResultToValidate.User.Name);
            if (currentExam.StartDate == null)
            {
                throw new ValidationExamException("No start date defined for the exam of ID: " + currentExam.Id);
            }
            if (IsExamTimeOverLimit(currentExam))
            {
                throw new ValidationExamException("Time over limit for the exam of ID: " + currentExam.Id);
            }
        }

        private bool IsExamTimeOverLimit(Quiz exam)
        {
            int timeTakenForExamMillisecond = (ResultToValidate.DateTime - exam.StartDate).Milliseconds;
            if (timeTakenForExamMillisecond > exam.Suite.TimeLimit + EXAM_TIME_LIMIT_SECURITY)
            {
                return true;
            }
            return false;
        }

        private bool IsResultOfficial()
        {
            if (ResultToValidate.QuizType == QuizType.Exam)
            {
                return true;
            }
            return false;
        }


        private void ResolveQuizResult()
        {
            if (ResultToValidate.QuizType == QuizType.Training)
            {
                ResolveTraining();
            }
            if (ResultToValidate.QuizType == QuizType.Exam)
            {
                ResolveExam();
            }
        }

        private void ResolveTraining()
        {
            Dal.Instance.CreateResult(ResultToValidate);
        }

        private void ResolveExam()
        {
            if (ResultToValidate.QuestionSuite.Owner != null)
            {
                Helper.MailingHelper.SendMailToQuestionSuiteOwner(ResultToValidate);
            }
            Dal.Instance.CreateResult(ResultToValidate);
            Dal.Instance.CloseExamByUsername(ResultToValidate.User.Name);
        }
    }
}