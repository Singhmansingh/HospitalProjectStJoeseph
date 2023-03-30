using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProjectStJoeseph.Models
{
    public class Requisition
    {

        [Key]
        public int RequisitionID { get; set; }
        public string test_result { get; set; }
        public DateTime record_date { get; set; }



        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }


        [ForeignKey("Physician")]
        public int physician_id { get; set; }
        public virtual Physician Physician { get; set; }

        [ForeignKey("Clinic")]
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }

        [ForeignKey("Test")]
        public int TestID { get; set; }
        public virtual Test Test { get; set; }



    }
    public class RequisitionDto
    {
        public int RequisitionID { get; set; }
        public string test_result { get; set; }
        public DateTime record_date { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int physician_id { get; set; }
        public string physician_name { get; set; }
        public int ClinicId { get; set; }
        public string ClinicName { get; set; }
        public int TestID { get; set; }
        public string test_category { get; set; }
        public DateTime test_date { get; set; }


    }

}
