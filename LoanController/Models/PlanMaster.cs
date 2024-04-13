using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanController.Models
{
    public class PlanMaster
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public int Tenure { get; set; }
        public float ROI { get; set; }
    }
}