﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyDosa_dbEntities1 : DbContext
    {
        public MyDosa_dbEntities1()
            : base("name=MyDosa_dbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Calender> Calenders { get; set; }
        public virtual DbSet<Campus_Branches> Campus_Branches { get; set; }
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Inquiry> Inquiries { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<semester> semesters { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<UserLocation> UserLocations { get; set; }
        public virtual DbSet<Userrole> Userroles { get; set; }
        public virtual DbSet<Year> Years { get; set; }
    }
}