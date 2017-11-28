using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaster.Storage.Model
{
    public class ContinousAbsenceModel
    {
        internal ContinousAbsenceModel()
        {

        }
        public string Id { get; set; }
        public string AcolyteId { get; set; }
        public DayOfWeek Day { get; set; }
        public String Time { get; set; }

        public static ContinousAbsenceModel CreateNewContinousAbsence(string acolyteId)
        {
            var model = new ContinousAbsenceModel();
            model.Id = Guid.NewGuid().ToString();
            model.AcolyteId = acolyteId;
            Workspace.CurrentData.ContinousAbsences.Add(model);
            return model;
        }
    }
}
