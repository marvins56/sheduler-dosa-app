//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sheduler.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Userrole
    {
        public int Roleid { get; set; }
        public string AccessNo { get; set; }
        public int userroleid { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual Student Student { get; set; }
    }
}
