using System.ComponentModel.DataAnnotations;

namespace HospitalProjectStJoeseph.Models
{
    public class Physician
    {
        [Key]
        public int physician_id { get; set; }

        public string physician_name { get; set; }

        public string specialty { get; set; }

        public string phone { get; set; }

        public string email { get; set; }
    }
}
