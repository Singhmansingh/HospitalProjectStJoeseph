using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models
{
    public class Requisition
    {

        public int RequisitionID { get; set; }
        public int patient_id { get; set; }
        public int physician_id { get; set; }
        public int clinic_id { get; set; }
        public int test_id { get; set; }
        public DateTime record_date { get; set; }
        public string test_result { get; set; }


    }
    public class RequisitionDto
    {
        public int RequisitionID { get; set; }
        public int patient_id { get; set; }
        public int physician_id { get; set; }
        public int clinic_id { get; set; }
        public int test_id { get; set; }
        public DateTime record_date { get; set; }
        public string test_result { get; set; }

    }

}
