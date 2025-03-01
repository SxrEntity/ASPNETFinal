using System.Collections.Generic;

namespace SportsPro.Models{

    public class IncidentViewModel{

        public Incident Incident {get; set;}
        public string Action {get; set;}
        
        public IEnumerable<Customer> Customers {get; set;}
        public IEnumerable<Product> Products {get; set;}
        public IEnumerable<Technician> Technicians {get; set;}
    }
}