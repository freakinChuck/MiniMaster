using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaster.Storage.Model
{
    public class AcolyteModel
    {
        internal AcolyteModel()
        {

        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public DateTime? Entry { get; set; }

        public string FamilyKey { get; set; }

        public static AcolyteModel CreateNewAcolyte()
        {
            var model = new AcolyteModel();
            model.Id = Guid.NewGuid().ToString();
            Workspace.CurrentData.Acolytes.Add(model);
            return model;
        }
    }
}
