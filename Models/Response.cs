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
    
    public partial class Response
    {
        public int ResponseId { get; set; }
        public int Inquirery_id { get; set; }
        public string Response1 { get; set; }
        public System.DateTime DatetimeOfReply { get; set; }
        public Nullable<bool> status { get; set; }
    
        public virtual Inquiry Inquiry { get; set; }
    }
}
