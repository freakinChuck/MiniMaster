using MiniMaster._Helper;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniMaster.Acolyte
{
    public class CreateAbsenceViewModel : INotifyPropertyChanged
    {
        public CreateAbsenceViewModel()
        {
            LoadServices();
            this.SingleAbsenceDateAndTime = DateTime.Today;
            this.RangeAbsenceFromDate = DateTime.Today;
            this.RangeAbsenceUntilDate = DateTime.Today;
            this.PropertyChanged += CreateAbsenceViewModel_PropertyChanged;
        }

        private void CreateAbsenceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SingleAbsenceWholeDay))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotSingleAbsenceWholeDay)));
            }
        }

        public Action CloseAction { get; set; }

        public string AcolyteId { get; internal set; }

        #region SingleAbsence

        public DateTime SingleAbsenceDateAndTime { get; set; }

        private bool singleAbsenceWholeDay;
        public bool SingleAbsenceWholeDay
        {
            get { return singleAbsenceWholeDay; }
            set {
                singleAbsenceWholeDay = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SingleAbsenceWholeDay)));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotSingleAbsenceWholeDay)));
            }
        }

        public bool NotSingleAbsenceWholeDay => !SingleAbsenceWholeDay;
        public DateTime SingleAbsenceDate
        {
            get { return SingleAbsenceDateAndTime.Date; }
            set
            {
                var time = SingleAbsenceDateAndTime.TimeOfDay;
                this.SingleAbsenceDateAndTime = value.Add(time);
            }
        }
        public TimeSpan SingleAbsenceTime
        {
            get { return SingleAbsenceDateAndTime.TimeOfDay; }
            set
            {
                var date = SingleAbsenceDateAndTime.Date;
                SingleAbsenceDateAndTime = date.Add(value);
            }
        }

        public ICommand AddSingleAbsenceCommand
        {
            get { return new BindingCommand(x => AddSingleAbsence()); }
        }

        private void AddSingleAbsence()
        {
            var newAbsence = AbsenceModel.CreateNewAbsence(this.AcolyteId);
            newAbsence.DateAndTime = SingleAbsenceWholeDay ? SingleAbsenceDate : SingleAbsenceDateAndTime;
            newAbsence.WholeDay = SingleAbsenceWholeDay;
            this.CloseAction?.Invoke();
        }

        #endregion

        #region AbsenceByRange

        public DateTime RangeAbsenceFromDate { get; set; }
        public DateTime RangeAbsenceUntilDate { get; set; }

        public ICommand AbsenceByRangeCommand
        {
            get { return new BindingCommand(x => AddAbsenceByRange()); }
        }

        private void AddAbsenceByRange()
        {
            for (DateTime absenceDate = RangeAbsenceFromDate; absenceDate <= RangeAbsenceUntilDate; absenceDate = absenceDate.AddDays(1))
            {
                var newAbsence = AbsenceModel.CreateNewAbsence(this.AcolyteId);
                newAbsence.DateAndTime = absenceDate;
                newAbsence.WholeDay = true;
            }
            this.CloseAction?.Invoke();
        }

        #endregion


        #region AbsenceByService

        private void LoadServices()
        {
            bool showEmpty = Workspace.CurrentData.Settings.ShowServicesWithoutJobsInAbsenceWindow;
            bool showPast = Workspace.CurrentData.Settings.ShowPastServicesInAbsenceWindow;
            AllServicesForAbsences = new BindingList<AbsenceForServiceViewModel>(Workspace.CurrentData.Services
                .Where(x => showPast || x.DateAndTime > DateTime.Today)
                .Where(x => showEmpty|| Workspace.CurrentData.ServiceJobs.Any(j => j.ServiceId == x.Id))
                .Select(x =>
                {
                    return new AbsenceForServiceViewModel
                    {
                        ServiceId = x.Id,
                        ServiceDateAndTime = x.DateAndTime
                    };
                })
                .OrderByDescending(x => x.ServiceDateAndTime)
                .ToList());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllServicesForAbsences)));
        }

        public BindingList<AbsenceForServiceViewModel> AllServicesForAbsences { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AbsenceByServiceCommand
        {
            get { return new BindingCommand(x => AbsenceByService()); }
        }


        private void AbsenceByService()
        {
            foreach (var selectedAbsence in AllServicesForAbsences.Where(x => x.HasAbsenceForServiceSelected))
            {
                var newAbsence = AbsenceModel.CreateNewAbsence(this.AcolyteId);
                newAbsence.DateAndTime = selectedAbsence.ServiceDateAndTime;
                newAbsence.WholeDay = false;
            }
            this.CloseAction?.Invoke();
        }



        public class AbsenceForServiceViewModel
        {
            public string ServiceId { get; set; }
            public DateTime ServiceDateAndTime { get; set; }
            public bool HasAbsenceForServiceSelected { get; set; }
            public string ServiceDateAndTimeString
            {
                get
                {
                    return this.ServiceDateAndTime.ToString("dd.MM.yyyy HH.mm");
                }
            }

        }

        #endregion

    }
}
