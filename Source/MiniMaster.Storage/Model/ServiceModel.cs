using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Storage.Model
{
    public class ServiceModel
    {
        internal ServiceModel()
        {
        }

        public string Id { get; set; }
        public bool OnlyOlderAcolytes { get; set; }
        public DateTime DateAndTime { get; set; }

        public string Text { get; set; }
        public string Comment { get; set; }

        public ServiceJobModel CreateNewServiceJob(string jobId)
        {
            return ServiceJobModel.CreateNewServiceJob(this.Id, jobId);
        }
        public static ServiceModel CreateNewService()
        {
            var model = new ServiceModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Services.Add(model);
            return model;
        }
    }
}
