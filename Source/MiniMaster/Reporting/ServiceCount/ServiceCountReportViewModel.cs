using MiniMaster.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Reporting.ServiceCount
{
    public class ServiceCountReportViewModel : INotifyPropertyChanged
    {
        public ServiceCountReportViewModel()
        {
            this.PropertyChanged += ServiceCountReportViewModel_PropertyChanged;
            this.ReportFromDate = DateTime.Today;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportFromDate)));
        }

        private void ServiceCountReportViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
                var count = Workspace.CurrentData.Services
                                        .Where(s => s.DateAndTime >= ReportFromDate)
                                        .Count(s => Workspace.CurrentData.ServiceJobs.Any(x => x.ServiceId == s.Id && x.AcolyteId == acolyteId));
                gridSource.Add(new GridSourceItem { Name = acolyte.Name + " " + acolyte.Firstname, NumberOfServices = count });
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
            public int NumberOfServices { get; set; }
        }

    }
}
