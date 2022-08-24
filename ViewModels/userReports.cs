using sheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sheduler.ViewModels
{
    public class userReports
    {
        public List<Response> Response { get; set; }
        public List<Event> Event { get; set; }
        public List<Inquiry> Inquiry { get; set; }
        public List<UserLocation> UserLocation { get; set; }
    }
}