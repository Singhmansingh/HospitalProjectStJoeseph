using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models
{
    public class BestWish
    {
        [Key]
        public int BestWishId { get; set; }
        public string BestWishSender { get; set; }
        public string BestWishMessage { get; set; }
        public bool BestWishIsRead { get; set; }
        public DateTime BestWishSendDate { get; set; }
        public string BestWishSenderEmail { get; set; }
        public string BestWishSenderPhone { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }

    }
}