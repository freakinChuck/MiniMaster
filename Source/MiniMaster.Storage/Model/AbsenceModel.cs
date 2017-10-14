using System;
using System.Collections.Generic;
using System.Text;

namespace MiniMaster.Storage.Model
{
    public class AbsenceModel
    {
        internal AbsenceModel()
        {

        }
        public string Id { get; set; }
        public string AcolyteId { get; set; }
        public DateTime DateAndTime { get; set; }
        public bool WholeDay { get; set; }

        public static AbsenceModel CreateNewAbsence(string acolyteId)
        {
            var model = new AbsenceModel();
            model.Id = Guid.NewGuid().ToString();
            model.AcolyteId = acolyteId;
            Workspace.CurrentData.Absences.Add(model);
            return model;
        }
    }
}
