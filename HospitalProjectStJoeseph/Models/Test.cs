
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models
{
    public class Test
    {

        public int TestID { get; set; }
        public string test_category { get; set; }
        public DateTime test_date { get; set; }

    }
    public class TestDto
    {
        public int TestID { get; set; }
        public string test_category { get; set; }
        public DateTime test_date { get; set; }
    }

}
