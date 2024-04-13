using LoanController.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoanController.Controllers
{
    public class LoanController : Controller
    {
        public ActionResult Index()
        {
            // Load PlanMaster data
            string query = "SELECT * FROM PlanMaster";
            DataTable dt = DBHelper.ExecuteQuery(query);
            ViewBag.PlanList = dt;
            return View();
        }

        [HttpPost]
        public ActionResult CalculateEMI(int tenure, float roi, float loanAmount)
        {
            float emi = (loanAmount + (loanAmount * (roi / 100))) / tenure;
            return Json(new { emi });
        }

        [HttpPost]
        public ActionResult GenerateSchedule(int planId, int tenure, DateTime loanDate, float loanAmount)
        {
            DateTime dueDate = loanDate.AddMonths(1);
            float roi;
            string query = $"SELECT ROI FROM PlanMaster WHERE PlanID = {planId}";
            using (DataTable dt = DBHelper.ExecuteQuery(query))
            {
                roi = Convert.ToSingle(dt.Rows[0]["ROI"]);
            }

            float emi = (loanAmount + (loanAmount * (roi / 100))) / tenure;

            for (int i = 0; i < tenure; i++)
            {
                query = $"INSERT INTO EMISchedule (PlanID, DueDate, EMIAmount) VALUES ({planId}, '{dueDate.ToString("yyyy-MM-dd")}', {emi})";
                DBHelper.ExecuteNonQuery(query);
                dueDate = dueDate.AddMonths(1);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetPlanDetails(int planId)
        {
            // Fetch plan details from database
            string query = $"SELECT * FROM PlanMaster WHERE PlanID = {planId}";
            DataTable dt = DBHelper.ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                var planDetails = new
                {
                    Tenure = dt.Rows[0]["Tenure"],
                    ROI = dt.Rows[0]["ROI"]
                };
                return Json(planDetails, JsonRequestBehavior.AllowGet);
            }

            return Json(null);
        }
    }

}