using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Storage.Model
{
    public class JobModel
    {
        internal JobModel()
        {

        }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Comment { get; set; }
        public int Order { get; set; }

        public static JobModel CreateNewJob()
        {
            var model = new JobModel();
            model.Id = Guid.NewGuid().ToString();
            model.Order = (Workspace.CurrentData.Jobs.Max(x => (int?)x.Order) ?? 0) +1;
            Workspace.CurrentData.Jobs.Add(model);
            return model;
        }
    }
}
