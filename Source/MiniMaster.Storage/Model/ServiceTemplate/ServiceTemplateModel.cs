using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MiniMaster.Storage.Model.ServiceTemplate
{
    public class ServiceTemplateModel
    {
        internal ServiceTemplateModel()
        {
            Jobs = new List<string>();
        }

        public string Id { get; set; }
        public string GroupId { get; set; }
        public DayOfWeek Day { get; set; }

        private TimeSpan time;
        [XmlIgnore]
        public TimeSpan Time { get { return time; } set { time = value; } }
        public long TimeTicks
        {
            get { return time.Ticks; }
            set { time = new TimeSpan(value); }
        }

        public List<string> Jobs { get; set; }

        public static ServiceTemplateModel CreateNewServiceTemplate(string groupId)
        {
            var model = new ServiceTemplateModel();
            model.Id = Guid.NewGuid().ToString();
            model.GroupId = groupId;
            Workspace.CurrentData.ServiceTemplates.Add(model);
            return model;
        }
    }
}
