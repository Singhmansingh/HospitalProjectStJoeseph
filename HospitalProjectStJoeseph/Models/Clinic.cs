using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models
{
    public class Clinic
    {
        [Key]

        public int ClinicId { get; set; }

        public string ClinicName { get; set; }

        public string ClinicDescription { get; set; }

        public TimeSpan ClinicTime { get; set; }
    }
}