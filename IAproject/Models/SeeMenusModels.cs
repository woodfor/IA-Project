using System;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace IAproject.Models
{
    public class SeeMenu
    {
        [Display(Name = "Your Calories")]
        public double Calories { get; set; }
        
    }
}
