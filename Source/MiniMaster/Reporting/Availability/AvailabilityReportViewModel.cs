using MiniMaster.RessourceScheduling;
using MiniMaster.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Reporting.Availability
{
    public class AvailabilityReportViewModel : INotifyPropertyChanged
    {
        public AvailabilityReportViewModel()
        {
            this.PropertyChanged += AvailabilityReportViewModel_PropertyChanged;
            this.ReportDate = DateTime.Today;
            this.TimeText = "09:00";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportDate)));
        }

        private void AvailabilityReportViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReportDateAndTime))
            {
                ReloadGridSource();
            }
        }

        private void ReloadGridSource()
        {

            var serviceTime = ReportDateAndTime;

            var gridSource = new RessourceScheduleManager().GetPossibleAcolytesForService(serviceTime).Select(x => new GridSourceItem { Name = x.Name + " " + x.Firstname }).ToList();


            this.GridSource = gridSource;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GridSource)));
        }

        private DateTime reportFromDate;

        public DateTime ReportDate
        {
            get { return reportFromDate; }
            set
            {
                reportFromDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportDate)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportDateAndTime)));
            }
        }

        public DateTime ReportDateAndTime
        {
            get
            {
                try
                {
                    var span = TimeSpan.Parse(TimeText);
                    return ReportDate.Add(span);
                }
                catch { return ReportDate; }
            }
        }

        private string timeText;

        public string TimeText
        {
            get { return timeText; }
            set
            {
                timeText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeText)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReportDateAndTime)));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public List<GridSourceItem> GridSource { get; set; }

        public class GridSourceItem
        {
            public string Name { get; set; }
        }

    }
}
