using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNumber { get; set; }
        public string PatientPhysicalAddress { get; set; }
        public string PatientEmailAddress { get; set; }
    }
}