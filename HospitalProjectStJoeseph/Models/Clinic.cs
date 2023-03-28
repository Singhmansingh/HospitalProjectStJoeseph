using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalProjectStJoeseph.Models
{
    public class Clinic
    {
        [Key]

        public int ClinicId { get; set; }

        public string ClinicName { get; set; }

        public string ClinicDescription { get; set; }

        public DateTime ClinicTime { get; set; }

        public ICollection<Service> Services { get; set; }
    }


        public class ClinicDto
    {
        public int ClinicId { get; set;}
        public string ClinicName { get; set;}
        public string ClinicDescription { get; set; }
        public DateTime ClinicTime { get; set; }

        public int ServiceId { get; set; } 
        public string ServiceName { get; set; }
    }
}