using sheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sheduler.ViewModels
{
    public class General_reports
    {
        public List<Response> Response { get; set; }
     
        public List<Inquiry> Inquiry { get; set; }
        public List<UserLocation> UserLocation { get; set; }
        public List<Student> students { get; set; }
        public List<Event> events { get; set; }
    }
}