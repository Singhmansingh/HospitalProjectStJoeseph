using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models.ViewModels
{
    public class DetailsService
    {
        
        public ServiceDto SelectedService { get; set; }
        public IEnumerable<ClinicDto> ProvidedClinic { get; set; }
    }
}