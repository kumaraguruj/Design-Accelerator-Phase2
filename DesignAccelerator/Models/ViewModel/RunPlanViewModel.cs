using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignAccelerator.Models.ViewModel
{
    public class RunPlanViewModel: MappingViewModel
    {
        public int daId { get; set; }
        public int TransactionSeq { get; set; }
    }
}