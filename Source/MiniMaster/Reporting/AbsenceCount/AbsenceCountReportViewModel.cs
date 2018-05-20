using MiniMaster.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Reporting.AbsenceCount
{
    public class AbsenceCountReportViewModel : INotifyPropertyChanged
    {
        public AbsenceCountReportViewModel()
        {
            this.PropertyChanged += AbsenceCountReportViewModel_PropertyChanged;
            this.ReportFromDate = DateTime.Today;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportFromDate)));
        }

        private void AbsenceCountReportViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReportFromDate))
            {
                ReloadGridSource();
            }
        }

        private void ReloadGridSource()
        {
            List<GridSourceItem> gridSource = new List<GridSourceItem>();
            var allAcolytes = Workspace.CurrentData.Acolytes.OrderBy(x => x.Name).ThenBy(x => x.FamilyKey).ThenBy(x => x.Firstname);

            foreach (var acolyte in allAcolytes)
            {
                var acolyteId = acolyte.Id;
                var count = Workspace.CurrentData.Absences
                                        .Count(s => s.DateAndTime >= ReportFromDate && s.AcolyteId == acolyteId);
                gridSource.Add(new GridSourceItem { Name = acolyte.Name + " " + acolyte.Firstname, NumberOfAbsences = count });
            }

            this.GridSource = gridSource;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GridSource)));
        }

        private DateTime reportFromDate;

        public DateTime ReportFromDate
        {
            get { return reportFromDate; }
            set
            {
                reportFromDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportFromDate)));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public List<GridSourceItem> GridSource { get; set; }

        public class GridSourceItem
        {
            public string Name { get; set; }
            public int NumberOfAbsences { get; set; }
        }

    }
}
