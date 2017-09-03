using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Storage.Model.ServiceTemplate
{
    public class ServiceTemplateGroupModel
    {
        internal ServiceTemplateGroupModel()
        {

        }

        public string Id { get; set; }
        public string Text { get; set; }
        public bool Default { get; set; }

        public static ServiceTemplateGroupModel CreateNewServiceTemplateGroup()
        {
            var model = new ServiceTemplateGroupModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.ServiceTemplateGroups.Add(model);
            return model;
        }
        public ServiceTemplateModel CreateNewServiceTemplate()
        {
            return ServiceTemplateModel.CreateNewServiceTemplate(this.Id);
        }
    }
}
