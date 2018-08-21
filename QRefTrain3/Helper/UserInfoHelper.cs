using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Helper
{

    public enum Rank
    {
        Bronze, Silver, Gold, Diamond
    }

    public class UserInfoHelper
    {
        /// <summary>
        /// Calculate the user's rank by accessing his official results.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Bronze if 60%+ average and 10+ result, Silver if 80%+ average and 10+ results, Gold if 90%+ average and 20+ results, Diamond if 98%+ average and 30+ results.</returns>
        public static Rank? GetUserRank(User user)
        {
            if (!user.IsEmailConfirmed) { return null; }
            var rs = Dal.Instance.GetResultByUser(user);
            for (int i = rs.Count - 1; i >= 0; i--)
            {
                if (rs[i].User != user || rs[i].ResultType != ResultType.Exam)
                {
                    rs.RemoveAt(i);
                }
            }
            return GetUserRank(user, rs);
        }

        private static Rank? GetUserRank(User user, List<Result> results)
        {
            float totalPossibleAnswers = 0;
            float totalGoodAnswers = 0;
            foreach (Result r in results)
            {
                totalPossibleAnswers += r.QuestionsAsked.Count();
                totalGoodAnswers += r.GetNumberGoodAnswers();
            }
            // Get a %age of the ratio
            float average = totalPossibleAnswers / totalGoodAnswers * 100;

            // Return the rank
            if (results.Count >= 30 && average >= 98)
            {
                return Rank.Diamond;
            }
            else if (results.Count >= 20 && average >= 90)
            {
                return Rank.Gold;
            }
            else if (results.Count >= 10 && average >= 80)
            {
                return Rank.Silver;
            }
            else if (results.Count >= 10 && average >= 60)
            {
                return Rank.Bronze;
            }
            return null;
        }
    }
}