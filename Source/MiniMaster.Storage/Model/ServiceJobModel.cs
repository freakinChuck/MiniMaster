using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Storage.Model
{
    public class ServiceJobModel
    {
        internal ServiceJobModel()
        {
        }

        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string JobId { get; set; }
        public string AcolyteId { get; set; }

        public static ServiceJobModel CreateNewServiceJob(string serviceId, string jobId)
        {
            var model = new ServiceJobModel();
            model.Id = Guid.NewGuid().ToString();
            model.ServiceId = serviceId;
            model.JobId = jobId;
            Workspace.CurrentData.ServiceJobs.Add(model);
            return model;
        }
    }
}
