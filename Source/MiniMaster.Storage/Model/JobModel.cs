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

        public static JobModel CreateNewJob()
        {
            var model = new JobModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Jobs.Add(model);
            return model;
        }
    }
}
