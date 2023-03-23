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

        public TimeSpan ClinicTime { get; set; }

        [ForeignKey("Service")]

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }
        public string ServiceName { get; set; }
    }
}