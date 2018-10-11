using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IAproject.Models
{
    public class Calculation
    {
        [Required(ErrorMessage = "Please enter Your age.")]
        [Display(Name = "Age")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Please enter Your Weight.")]
        [Display(Name = "Weight")]
        public double Weight { get; set; }
        [Required(ErrorMessage = "Please enter Your Height.")]
        [Display(Name = "Height")]
        public double Height { get; set; }
        [Required(ErrorMessage = "Please select Your Gender to see correct answer.")]
        [Display(Name = "Gender")]
        public Gender PersonalGender { get; set; }
        [Required(ErrorMessage = "Please select Your Activity  to see correct answer.")]
        [Display(Name = "Activity")]
        public Activity Activity { get; set; }
        [Required(ErrorMessage = "Please enter Your calories.")]
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
