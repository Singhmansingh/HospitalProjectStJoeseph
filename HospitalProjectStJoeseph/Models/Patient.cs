using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public bool PatientIsRegistered { get; set; }
    }

    public class PatientDto
    {
        public Patient Patient { get; set; }
        public List<BestWish> BestWishes { get; set; }

    }

    public class UserPatient
    {
        [Key]
        public int UserPatientId { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}