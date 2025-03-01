using System.Collections.Generic;

namespace SportsPro.Models{

    public class IncidentListViewModel{

        public string Filter {get; set;}
        public IEnumerable<Incident> Incidents {get; set;}
    }
}