using System;

namespace Orders.API.Models {
    public class Order {
        public long Id { get; set; }
        public long PetId { get; set; }
        public int Quantity { get; set; }
        public DateTime ShipDate { get; set; }
        //TODO: the example API used a string and did not enforce an enum value (ie any string value is accepted), 
        //      so I'm simply using as a string as well. This should probably be enhanced to enforce enum values.
        public string Status { get; set; }
        public bool Complete { get; set; }
    }
}
