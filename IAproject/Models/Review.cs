//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IAproject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public string ReviewNum { get; set; }
        public string ReviewText { get; set; }
        public decimal Stars { get; set; }
        public string MenuID { get; set; }
    
        public virtual Menu Menu { get; set; }
    }
}
