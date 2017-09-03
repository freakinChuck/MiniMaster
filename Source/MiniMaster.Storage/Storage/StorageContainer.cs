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
            Acolytes = new List<AcolyteModel>();
            Jobs = new List<JobModel>();
            ServiceTemplateGroups = new List<ServiceTemplateGroupModel>();
            ServiceTemplates = new List<ServiceTemplateModel>();
        }
        public List<AcolyteModel> Acolytes { get; set; }
        public List<JobModel> Jobs { get; set; }
        public List<ServiceTemplateGroupModel> ServiceTemplateGroups { get; set; }
        public List<ServiceTemplateModel> ServiceTemplates { get; set; }
    }
}
