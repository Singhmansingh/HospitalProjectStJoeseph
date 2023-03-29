using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalProjectStJoeseph.Models;

namespace HospitalProjectStJoeseph.Models
{
    public class Appointment
    {
        [Key]
        public int appointmentId { get; set; }

        [ForeignKey("Patient")]
        public int patientId { get; set; }
        public virtual Patient Patient { get; set; }

        [ForeignKey("Physician")]
        public int physicianId { get; set; }
        public virtual Physician Physician { get; set; }

        public DateTime appointment_start { get; set; }

        public DateTime appointment_end { get; set; }

        [ForeignKey("Service")]
        public int servicesId { get; set; }
        public virtual Service Service { get; set; }
    }
}
