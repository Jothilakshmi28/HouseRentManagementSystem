using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementSystem.ViewModel
{
    public class PropertyDetails
    {
        public string Location { get; set; }
        public string BHK_type { get; set; }
        public int Id { get; set; }
        public string House_type { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int Rent { get; set; }
        public int SquareFeet { get; set; }

        public int SellerID { get; set; }

        public byte[] Images { get; set; }

    }
}