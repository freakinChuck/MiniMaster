using MiniMaster.Storage.Model;
using MiniMaster.Storage.Model.ServiceTemplate;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaster.Storage.Storage
{
    public class StorageContainer
    {
        public StorageContainer()
        {
            Settings = new SystemSettingsModel();

            Acolytes = new List<AcolyteModel>();
            Jobs = new List<JobModel>();
            ServiceTemplateGroups = new List<ServiceTemplateGroupModel>();
            ServiceTemplates = new List<ServiceTemplateModel>();
            Services = new List<ServiceModel>();
            ServiceJobs = new List<ServiceJobModel>();
            Absences = new List<AbsenceModel>();
            ContinousAbsences = new List<ContinousAbsenceModel>();
        }

        public SystemSettingsModel Settings { get; set; }
        
        public List<AcolyteModel> Acolytes { get; set; }
        public List<JobModel> Jobs { get; set; }
        public List<ServiceTemplateGroupModel> ServiceTemplateGroups { get; set; }
        public List<ServiceTemplateModel> ServiceTemplates { get; set; }
        public List<ServiceModel> Services { get; set; }
        public List<ServiceJobModel> ServiceJobs { get; set; }
        public List<AbsenceModel> Absences { get; set; }
        public List<ContinousAbsenceModel> ContinousAbsences { get; set; }
    }
}
