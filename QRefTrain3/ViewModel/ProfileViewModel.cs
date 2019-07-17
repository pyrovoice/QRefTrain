using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class ProfileViewModel
    {
        public List<ResultViewModel> Results { get; set; }
        public int UserRank { get; set; }

        public ProfileViewModel(List<ResultViewModel> results, int userRank)
        {
            this.Results = results;
            this.UserRank = userRank;
        }
    }
}