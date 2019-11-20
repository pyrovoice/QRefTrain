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
                Dal.Instance.CreateLog(new Log(LogLevel.ERROR, e.Message, Dal.Instance.GetDBTime(), ResultToValidate.User.Id));
            }
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
            Exam currentExam = Dal.Instance.GetOngoingExamByUsername(ResultToValidate.User.Name);
            if (currentExam.StartDate == null)
            {
                throw new ValidationExamException("No start date defined for the exam of ID: " + currentExam.Id);
            }
            if (IsExamTimeOverLimit(currentExam))
            {
                throw new ValidationExamException("Time over limit for the exam of ID: " + currentExam.Id);
            }
        }

        private bool IsExamTimeOverLimit(Exam exam)
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
            if (ResultToValidate.ResultType == ResultType.Exam)
            {
                return true;
            }
            return false;
        }


        private void ResolveQuizResult()
        {
            if (ResultToValidate.ResultType == ResultType.Training)
            {
                return;
            }
            if (ResultToValidate.ResultType == ResultType.Exam)
            {
                ResolveExam();
            }
        }

        private void ResolveExam()
        {
            if (ResultToValidate.QuestionSuite != null)
            {
                Helper.MailingHelper.SendMailToQuestionSuiteOwner(ResultToValidate);
            }
            Dal.Instance.CreateResult(ResultToValidate);
            Dal.Instance.CloseExamByUsername(ResultToValidate.User.Name);
        }
    }
}