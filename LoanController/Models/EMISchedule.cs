using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanController.Models
{
    public class EMISchedule
    {
        public int ScheduleID { get; set; }
        public int PlanID { get; set; }
        public DateTime DueDate { get; set; }
        public float EMIAmount { get; set; }
    }
}