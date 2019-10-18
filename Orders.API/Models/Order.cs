using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.API.Models {
    public class Order {
        public long Id { get; set; }
        public long PetId { get; set; }
        public int Quantity { get; set; }
        public DateTime ShipDate { get; set; }
        //NOTE: the example API used a string and did not enforce an enum value (ie any string value is accepted), 
        //      so I'm simply using as a string as well. This should probably be enhanced to properly use an enum.
        public string Status { get; set; }
        public bool Complete { get; set; }
    }
}
