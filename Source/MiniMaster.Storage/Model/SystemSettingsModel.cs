using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Storage.Model
{
    public class SystemSettingsModel
    {
        public bool ShowPastServices { get; set; }
        public bool ShowPastAbsences { get; set; }
        public bool ShowServicesWithoutJobsInAbsenceWindow { get; set; }
        public bool ShowPastServicesInAbsenceWindow { get; set; }
    }
}
