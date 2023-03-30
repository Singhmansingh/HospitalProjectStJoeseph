using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models.ViewModels
{
    public class DetailsClinic
    {
        public ClinicDto SelectedClinic { get; set; }

        public IEnumerable<ServiceDto> ProvidedServices { get; set; }
        public IEnumerable<ServiceDto> UnprovidedServices { get; set; }


    }
}