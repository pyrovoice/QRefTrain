using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class HomeViewModel
    {
        public bool IsUserOngoingExam { get; set; }

        public HomeViewModel(bool v)
        {
            this.IsUserOngoingExam = v;
        }

        
    }
}