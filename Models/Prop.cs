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
    using System.Collections.Generic;
    
    public partial class Prop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prop()
        {
            this.Images1 = new List<Image>();
            this.Request_Details = new HashSet<Request_Details>();
            this.Sold_Property = new HashSet<Sold_Property>();
        }
    
        public string BHK_type { get; set; }
        public int Id { get; set; }
        public string House_type { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public Nullable<decimal> Rent { get; set; }
        public Nullable<int> SellerID { get; set; }
        public string Availability { get; set; } 
        public Nullable<int> SquareFeet { get; set; }
        public string PropertyName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IList<Image> Images1 { get; set; }
        public virtual Renter Renter { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Request_Details> Request_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sold_Property> Sold_Property { get; set; }
    }
}
