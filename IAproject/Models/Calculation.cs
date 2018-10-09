using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IAproject.Models
{
    public class Calculation
    {
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Display(Name = "Weight")]
        public double Weight { get; set; }
        [Display(Name = "Height")]
        public double Height { get; set; }
        [Display(Name = "Gender")]
        public Gender PersonalGender { get; set; }
        [Display(Name = "Activity")]
        public Activity Activity { get; set; }
        [Display(Name = "Your calories")]
        public double CalResult { get; set; }
    }
    public enum Gender
    {
        Male,Female
    }

    public enum Activity
    {
        BMR,
        Sedentary,
        Lightly,
        Moderately,
        Very_Active,
        Extra_Active
    }
}
