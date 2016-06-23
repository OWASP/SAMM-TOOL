using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OwaspSAMM.DAL;

namespace OwaspSAMM.Web.Models
{
    public class BusinessUnitSummaryVM
    {
        public UserData BUOwner { get; set; }
        public List<string> BUList { get; set; }
        public List<Assessment> BUAssessments { get; set; }
    }
}