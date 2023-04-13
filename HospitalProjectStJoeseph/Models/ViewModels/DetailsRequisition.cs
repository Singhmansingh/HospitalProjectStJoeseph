using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectStJoeseph.Models.ViewModels
{
    public class DetailsRequisition
    {
        //the Requisition itself that we want to display
        public RequisitionDto SelectedRequisition { get; set; }

        // Patients related to the Requisitions
        public PatientDto RelatedPatient { get; set; }
    }
}

