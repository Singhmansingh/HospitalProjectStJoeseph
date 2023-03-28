using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public DateTime ServiceTime { get; set; } 
        
        public ICollection<Clinic> Clinic { get; set; }
    }

    public class ServiceDto
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; }

        public DateTime ServiceTime { get; set; }
    }

}