//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HouseRentManagementSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database1Entities1 : DbContext
    {
        public Database1Entities1()
            : base("name=Database1Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Prop> Props { get; set; }
        public virtual DbSet<Renter> Renters { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<Request_Details> Request_Details { get; set; }
        public virtual DbSet<Sold_Property> Sold_Property { get; set; }
        public virtual DbSet<city> cities { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<BHK_type> BHK_type { get; set; }
        public virtual DbSet<PriceRange> PriceRanges { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
    }
}
