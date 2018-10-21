using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAproject.Models;
namespace IAproject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index([Bind(Include = "Age,PersonalGender,Height,Weight,Activity,CalResult")] Calculation cal)
        {
            
            IList<string> calorie = new List<string>();
            double BMR=0;
            double rate = 1.2;
            double exercise = 0.17;
            if (!(cal.Age == 0) && !(cal.Height == 0) && !(cal.Height == 0))
            {
                if (cal.PersonalGender == Gender.Male)
                {
                    BMR = 10 * cal.Weight + 6.25 * cal.Height - 5 * cal.Age + 5;
                }
                else if (cal.PersonalGender == Gender.Female)
                {
                    BMR = 10 * cal.Weight + 6.25 * cal.Height - 5 * cal.Age - 161;
                }
                if (cal.Activity == Activity.BMR) // Show the calculation result.
                {
                    calorie.Add("you need" + BMR + "Calories/day to maintain your weight.");
                    calorie.Add("you need" + (BMR - 500) + "Calories/day to lose 0.5 kg per week.");
                    calorie.Add("you need" + (BMR - 1000) + "Calories/day to lose 1 kg per week.");
                    calorie.Add("you need" + (BMR + 500) + "Calories/day to gain 0.5 kg per week.");
                    calorie.Add("you need" + (BMR + 1000) + "Calories/day to gain 1 kg per week.");
                }
                if (cal.Activity == Activity.Sedentary)
                {
                    double tmp = BMR * rate;
                    calorie.Add("you need" + tmp + "Calories/day to maintain your weight.");
                    calorie.Add("you need" + (tmp - 500) + "Calories/day to lose 0.5 kg per week.");
                    calorie.Add("you need" + (tmp - 1000) + "Calories/day to lose 1 kg per week.");
                    calorie.Add("you need" + (tmp + 500) + "Calories/day to gain 0.5 kg per week.");
                    calorie.Add("you need" + (tmp + 1000) + "Calories/day to gain 1 kg per week.");
                }
                if (cal.Activity == Activity.Lightly)
                {
                    double tmp = BMR * (rate + exercise);
                    calorie.Add("you need" + tmp + "Calories/day to maintain your weight.");
                    calorie.Add("you need" + (tmp - 500) + "Calories/day to lose 0.5 kg per week.");
                    calorie.Add("you need" + (tmp - 1000) + "Calories/day to lose 1 kg per week.");
                    calorie.Add("you need" + (tmp + 500) + "Calories/day to gain 0.5 kg per week.");
                    calorie.Add("you need" + (tmp + 1000) + "Calories/day to gain 1 kg per week.");
                }
                if (cal.Activity == Activity.Moderately)
                {
                    double tmp = BMR * (rate + exercise * 2);
                    calorie.Add("you need" + tmp + "Calories/day to maintain your weight.");
                    calorie.Add("you need" + (tmp - 500) + "Calories/day to lose 0.5 kg per week.");
                    calorie.Add("you need" + (tmp - 1000) + "Calories/day to lose 1 kg per week.");
                    calorie.Add("you need" + (tmp + 500) + "Calories/day to gain 0.5 kg per week.");
                    calorie.Add("you need" + (tmp + 1000) + "Calories/day to gain 1 kg per week.");
                }
                if (cal.Activity == Activity.Very_Active)
                {
                    double tmp = BMR * (rate + exercise * 3);
                    calorie.Add("you need" + tmp + "Calories/day to maintain your weight.");
                    calorie.Add("you need" + (tmp - 500) + "Calories/day to lose 0.5 kg per week.");
                    calorie.Add("you need" + (tmp - 1000) + "Calories/day to lose 1 kg per week.");
                    calorie.Add("you need" + (tmp + 500) + "Calories/day to gain 0.5 kg per week.");
                    calorie.Add("you need" + (tmp + 1000) + "Calories/day to gain 1 kg per week.");
                }
                if (cal.Activity == Activity.Extra_Active)
                {
                    double tmp = BMR * (rate + exercise * 4);
                    calorie.Add("you need" + tmp + "Calories/day to maintain your weight.");
                    calorie.Add("you need" + (tmp - 500) + "Calories/day to lose 0.5 kg per week.");
                    calorie.Add("you need" + (tmp - 1000) + "Calories/day to lose 1 kg per week.");
                    calorie.Add("you need" + (tmp + 500) + "Calories/day to gain 0.5 kg per week.");
                    calorie.Add("you need" + (tmp + 1000) + "Calories/day to gain 1 kg per week.");
                }
            }
            else
            {
                
            }
            ViewData["Calculation"] = calorie;
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        

    }
}